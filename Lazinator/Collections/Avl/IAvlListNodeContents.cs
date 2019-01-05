using Lazinator.Core;
using Lazinator.Attributes;
using System;
using Lazinator.Collections.Tuples;

namespace Lazinator.Collections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlListNodeContents)]
    public interface IAvlListNodeContents<TKey, TValue>
        where TKey : ILazinator, IComparable<TKey>
        where TValue : ILazinator
    {
        ILazinatorSortable<LazinatorKeyValue<TKey, TValue>> Items { get; set; }
        int SelfItemsCount { get; set; }
        long LeftItemsCount { get; set; }
        long RightItemsCount { get; set; }
    }
}