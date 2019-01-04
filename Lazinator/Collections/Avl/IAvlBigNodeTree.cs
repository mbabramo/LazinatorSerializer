
using Lazinator.Attributes;
using Lazinator.Collections.Tuples;
using Lazinator.Core;
using System;

namespace Lazinator.Collections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlBigNodeTree)]
    interface IAvlBigNodeTree<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        AvlTree<LazinatorKeyValue<TKey, TValue>, AvlBigNodeContents<TKey, TValue>> UnderlyingTree { get; set; }
        bool AllowMultipleValuesPerKey { get; set; }
        int MaxItemsPerNode { get; set; }
    }
}
