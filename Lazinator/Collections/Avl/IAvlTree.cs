using Lazinator.Attributes;
using Lazinator.Core;
using System;

namespace Lazinator.Collections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlTree)]
    interface IAvlTree<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        AvlNode<TKey, TValue> Root { get; set; }
    }
}
