using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace Lazinator.Collections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlBigNodeContents)]
    public interface IAvlBigNodeContents<TKey, TValue>
        where TKey : ILazinator, IComparable<TKey>
        where TValue : ILazinator
    {
        SortedLazinatorList<LazinatorKeyValue<TKey, TValue>> Items { get; set; }
        int SelfItemsCount { get; set; }
        long LeftItemsCount { get; set; }
        long RightItemsCount { get; set; }
    }
}