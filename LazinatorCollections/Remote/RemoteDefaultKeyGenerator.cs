using Lazinator.Core;
using Lazinator.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorCollections.Remote
{
    public static class RemoteDefaultKeyGenerator<TKey> where TKey : ILazinator
    {
        private static Func<ILazinator, TKey> _Generator = null;

        public static Func<ILazinator, TKey> GetGenerator()
        {
            if (_Generator == null)
                RemoteDefaultKeyGenerator<WGuid>._Generator = l => l.GetBinaryHashCode128();
            return _Generator;
        }
    }
}
