using Lazinator.Core;
using Lazinator.Attributes;
using System;
using System.Collections.Generic;
using Lazinator.Wrappers;
using Lazinator.Collections.Tuples;

namespace Lazinator.Collections.Factories
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlDictionaryFactory)]
    public interface IAvlDictionaryFactory<TKey, TValue>
        where TKey : ILazinator, IComparable<TKey>
        where TValue : ILazinator
    {
        bool AllowDuplicates { get; set; }
        ILazinatorOrderedKeyableFactory<WUint, LazinatorTuple<TKey, TValue>> OrderedKeyableFactory { get; set; }
    }
}