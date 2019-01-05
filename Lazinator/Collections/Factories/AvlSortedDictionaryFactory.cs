using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public partial class AvlSortedDictionaryFactory<TKey, TValue> : IAvlSortedDictionaryFactory<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        public bool AllowDuplicateKeys => false;

        public ILazinatorOrderedKeyable<TKey, TValue> Create()
        {
            debug;
        }
    }
}