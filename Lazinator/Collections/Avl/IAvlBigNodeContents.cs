using Lazinator.Core;
using Lazinator.Attributes;

namespace Lazinator.Collections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.AvlBigNodeContents)]
    public interface IAvlBigNodeContents<TKey, TValue>
        where TKey : ILazinator
        where TValue : ILazinator
    {
        SortedLazinatorList<LazinatorTuple<TKey, TValue>> Items { get; set; }
        int SelfItemsCount { get; set; }
        long LeftItemsCount { get; set; }
        long RightItemsCount { get; set; }
    }
}