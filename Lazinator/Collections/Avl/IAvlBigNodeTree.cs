
using Lazinator.Attributes;
using Lazinator.Collections.Tuples;
using Lazinator.Core;
using System;

namespace Lazinator.Collections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlBigNodeTree)]
    interface IAvlListNodeTree<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        AvlTree<LazinatorKeyValue<TKey, TValue>, AvlListNodeContents<TKey, TValue>> UnderlyingTree { get; set; }
        int MaxItemsPerNode { get; set; }
    }
}
