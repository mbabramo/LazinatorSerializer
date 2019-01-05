using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public class AvlSortedDictionaryWithDuplicatesFactory<TKey, TValue> : IAvlSortedDictionaryWithDuplicatesFactory<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        public bool AllowDuplicateKeys => false;
    }
}