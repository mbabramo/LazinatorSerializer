using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;

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

    /// <summary>
    /// The functionality for IAvlNode. This is separated so that alternative implementations can implement these without automatically generated Lazinator properties. 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public interface IAvlNodeEquivalent<TKey, TValue> : ILazinator where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        AvlNode<TKey, TValue> Left { get; set; }
        AvlNode<TKey, TValue> Right { get; set; }
        AvlNode<TKey, TValue> Parent { get; set; }
        KeyValuePair<TKey, TValue> KeyValuePair { get; }
        TKey Key { get; set; }
        TValue Value { get; set; }
        long Count { get; set; }
        int Balance { get; set; }
        bool NodeVisitedDuringChange { get; set; }
        long LeftCount { get; }
        long RightCount { get; }
    }
}
