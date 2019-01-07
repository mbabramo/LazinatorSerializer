using Lazinator.Attributes;
using Lazinator.Core;
using System;

namespace Lazinator.Collections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlOldNode)]
    interface IAvlOldNode<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        AvlOldNode<TKey, TValue> Left { get; set; }
        AvlOldNode<TKey, TValue> Right { get; set; }
        TKey Key { get; set; }
        TValue Value { get; set; }
        long Count { get; set; }
        int Balance { get; set; }
    }
}
