using Lazinator.Attributes;
using Lazinator.Core;
using System;

namespace Lazinator.Collections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlNode)]
    interface IAvlNode<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        AvlNode<TKey, TValue> Left { get; set; }
        AvlNode<TKey, TValue> Right { get; set; }
        TKey Key { get; set; }
        TValue Value { get; set; }
        long Count { get; set; }
        int Balance { get; set; }
    }
}
