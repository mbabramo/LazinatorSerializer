using Lazinator.Core;
using Lazinator.Exceptions;
using Lazinator.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Collections.Remote
{
    /// <summary>
    /// A key-value pair that may or may not store the value locally. Where the item is stored remotely, the RemoteManager can get used to get the value of the item.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public partial class Remote<TKey, TValue> : IRemote<TKey, TValue> where TKey : ILazinator where TValue : class, ILazinator
    {
        public bool ValueLoaded { get; set; } = false;

        public Remote()
        {
        }

        public Remote(TValue value)
        {
            SetValue(value);
        }

        /// <summary>
        /// Gets the item, using the remote manager if necessary. 
        /// </summary>
        /// <returns></returns>
        public async Task<TValue> GetValue()
        {
            if (!ValueLoaded && !StoreLocally)
            {
                var getter = RemoteManager<TKey, TValue>.RemoteGetter;
                if (getter == null)
                {
                    // If the remote getter is not initialized, we must store locally. We'll just return Local, which may be set to default/null but might already have been set.
                    StoreLocally = true;
                }
                else
                    Local = await getter(Key);
            }
            ValueLoaded = true;
            return Local;
        }

        /// <summary>
        /// Sets the locally stored value
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(TValue value)
        {
            Local = value;
            Local.IsDirty = true;
            ValueLoaded = true;
        }

        /// <summary>
        /// Frees the in memory storage of an item that is being stored remotely. If it is accessed again, it will be loaded from the remote source.
        /// </summary>
        private void FreeRemoteStorage()
        {
            if (!StoreLocally)
            {
                ValueLoaded = false;
                Local = default;
            }
        }

        /// <summary>
        /// Saves a value, storing it remotely if necessary.
        /// </summary>
        /// <param name="freeRemoteStorage">True if the memory for an item being stored remotely should be cleared after storage is accomplished</param>
        /// <returns></returns>
        public async Task SaveValue(bool freeRemoteStorage)
        {
            if (ValueLoaded && (Local.IsDirty || Local.DescendantIsDirty || Local.HasChanged || Local.DescendantHasChanged))
            {
                Func<Remote<TKey, TValue>, bool> storeLocallyFunc = RemoteManager<TKey, TValue>.RemoteStoreLocally;
                StoreLocally = storeLocallyFunc == null ? true : storeLocallyFunc(this);
                if (!StoreLocally)
                {
                    Key = GetKey();
                    await RemoteManager<TKey, TValue>.RemoteSaver(Key, Local);
                    Local = default;
                }
            }
            if (freeRemoteStorage)
                FreeRemoteStorage();
        }

        /// <summary>
        /// Obtains a key for an item using the RemoteManager's remote key generator.
        /// </summary>
        /// <returns>The key</returns>
        private TKey GetKey()
        {
            TKey key;
            Func<TValue, TKey> keyGenerator = RemoteManager<TKey, TValue>.RemoteKeyGenerator;
            if (keyGenerator == null)
            {
                throw new LazinatorSerializationException($"Object of type {typeof(TValue)} with key type {typeof(TKey)} could not be set remotely, because no key generator was set up.");
            }
            else
                key = keyGenerator(Local);
            return key;
        }
    }
}
