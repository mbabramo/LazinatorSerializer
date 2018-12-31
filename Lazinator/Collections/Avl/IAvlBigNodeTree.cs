
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.AvlBigNodeTree)]
    interface IAvlBigNodeTree<TKey, TValue> where TKey : ILazinator where TValue : ILazinator
    {
        // AvlTree<LazinatorTuple<TKey, TValue>, AvlBigNodeContents<TKey, TValue>> UnderlyingTree { get; set; }
    }
}
