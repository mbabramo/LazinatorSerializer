using Lazinator.Core;
using Lazinator.Attributes;
using System;
using System.Collections.Generic;

namespace Lazinator.Collections.Factories
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlSortedDictionaryWithDuplicatesFactory)]
    public interface IAvlSortedDictionaryWithDuplicatesFactory<TKey, TValue>
        where TKey : ILazinator, IComparable<TKey>
        where TValue : ILazinator
    {
    }
}