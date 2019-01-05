using Lazinator.Core;
using Lazinator.Attributes;
using System;
using System.Collections.Generic;

namespace Lazinator.Collections.Factories
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlSortedDictionaryFactory)]
    public interface IAvlSortedDictionaryFactory<TKey, TValue>
        where TKey : ILazinator, IComparable<TKey>
        where TValue : ILazinator
    {
        ILazinatorOrderedKeyableFactory<TKey, TValue> OrderedKeyableFactory { get; set; }
    }
}