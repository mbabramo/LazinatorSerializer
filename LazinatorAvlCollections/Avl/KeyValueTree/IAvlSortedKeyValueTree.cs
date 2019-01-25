using Lazinator.Core;
using Lazinator.Attributes;
using System;
using LazinatorCollections.Interfaces;
using LazinatorCollections;

namespace LazinatorAvlCollections.Avl.KeyValueTree
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IAvlSortedKeyValueTree)]
    public interface IAvlSortedKeyValueTree<TKey, TValue>
        where TKey : ILazinator
        where TValue : ILazinator
    {
    }
}