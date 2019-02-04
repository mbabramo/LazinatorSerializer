using Lazinator.Core;
using Lazinator.Exceptions;
using Lazinator.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LazinatorCollections.Remote
{
    public partial class Remote<TKey, TValue> : IRemote<TKey, TValue> where TKey : ILazinator where TValue : class, ILazinator
    {
        [NonSerialized]
        private bool ValueLoaded = false;

        public async Task<TValue> GetValue()
        {
            if (!ValueLoaded && !StoreLocally)
            {
                Local = await RemoteManager<TKey, TValue>.RemoteGetter(Key);
            }
            ValueLoaded = true;
            return Local;
        }

        private void FreeRemoteStorage()
        {
            if (!StoreLocally)
            {
                ValueLoaded = false;
                Local = default;
            }
        }

        public async Task SaveValue(bool freeRemoteStorage)
        {
            if (ValueLoaded && (Local.IsDirty || Local.DescendantIsDirty || Local.HasChanged || Local.DescendantHasChanged))
            {
                Func<Remote<TKey, TValue>, bool> storeLocallyFunc = RemoteManager<TKey, TValue>.RemoteStoreLocally;
                if (storeLocallyFunc != null)
                    StoreLocally = storeLocallyFunc(this);
                if (!StoreLocally)
                {
                    Func<TValue, TKey> keyGenerator = RemoteManager<TKey, TValue>.RemoteKeyGenerator;
                    if (keyGenerator == null)
                    {
                        throw new LazinatorSerializationException($"Object of type {typeof(TValue)} with key type {typeof(TKey)} could not be set remotely, because no key generator was set up.");
                    }
                    else
                        Key = keyGenerator(Local);
                    await RemoteManager<TKey, TValue>.RemoteSaver(Key, Local);
                    Local = default;
                }
            }
            if (freeRemoteStorage)
                FreeRemoteStorage();
        }
    }
}
