using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LazinatorCollections.Remote
{
    public static class RemoteManager<TKey> where TKey : ILazinator
    {
        public static Func<TKey, Task<ILazinator>> RemoteGetter;
        public static Func<IRemote, bool> RemoteStoreLocally;
        public static Func<ILazinator, TKey> RemoteKeyGenerator;
        public static Func<TKey, ILazinator, Task> RemoteSaver;
    }

    public static class RemoteManager<TKey, TValue> where TKey : ILazinator where TValue : class, ILazinator
    {
        private static Func<TKey, Task<TValue>> _RemoteGetter;
        private static Func<Remote<TKey, TValue>, bool> _RemoteStoreLocally;
        private static Func<TValue, TKey> _RemoteKeyGenerator;
        private static Func<TKey, TValue, Task> _RemoteSaver;

        public static Func<TKey, Task<TValue>> RemoteGetter
        {
            get => _RemoteGetter ?? (key => RemoteManager<TKey>.RemoteGetter(key).ContinueWith(t => t.Result as TValue));
            set => _RemoteGetter = value;
        }
        public static Func<Remote<TKey, TValue>, bool> RemoteStoreLocally
        {
            get => _RemoteStoreLocally ?? (r => RemoteManager<TKey>.RemoteStoreLocally == null ? true : RemoteManager<TKey>.RemoteStoreLocally(r));
            set => _RemoteStoreLocally = value;
        }
        public static Func<TValue, TKey> RemoteKeyGenerator
        {
            get => _RemoteKeyGenerator ?? (v => RemoteManager<TKey>.RemoteKeyGenerator(v));
            set => _RemoteKeyGenerator = value;
        }
        public static Func<TKey, TValue, Task> RemoteSaver
        {
            get => _RemoteSaver ?? ((k, v) => RemoteManager<TKey>.RemoteSaver(k, v));
            set => _RemoteSaver = value;
        }
    }
}
