using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LazinatorCollections.Remote
{
    public static class RemoteManager<TKey, TValue> where TKey : ILazinator where TValue : class, ILazinator
    {
        public static Func<TKey, Task<TValue>> RemoteGetter;
        public static Func<Remote<TKey, TValue>, bool> RemoteStoreLocally;
        public static Func<TValue, TKey> RemoteKeyGenerator;
        public static Func<TKey, TValue, Task> RemoteSaver;
    }
}
