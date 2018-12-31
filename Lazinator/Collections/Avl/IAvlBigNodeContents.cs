using Lazinator.Core;
using Lazinator.Attributes;

namespace Lazinator.Collections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.AvlBigNodeContents)]
    public interface IAvlBigNodeContents<TKey, TValue>
        where TKey : ILazinator
        where TValue : ILazinator
    {
        LazinatorList<LazinatorTuple<TKey, TValue>> Items { get; set; }
        long LeftCount { get; set; }
        long SelfCount { get; set; }
        long RightCount { get; set; }
    }
}