using Lazinator.Core;
using Lazinator.Attributes;
using System;
using System.Collections.Generic;
using Lazinator.Collections.Tuples;

namespace Lazinator.Collections.Factories
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlListNodeTreeFactory)]
    public interface IAvlListNodeTreeFactory<TKey, TValue> 
        where TKey : ILazinator, IComparable<TKey>
        where TValue : ILazinator
    {
        bool AllowDuplicates { get; set; }
        ILazinatorSortableFactory<LazinatorKeyValue<TKey, TValue>> SortableListFactory { get; set; }
        int MaxItemsPerInnerList { get; set; }
    }
}