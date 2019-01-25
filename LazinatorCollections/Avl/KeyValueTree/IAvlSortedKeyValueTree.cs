using Lazinator.Core;
using Lazinator.Attributes;
using System;
using LazinatorCollections.Interfaces;

namespace LazinatorCollections.Avl.KeyValueTree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlSortedKeyValueTree)]
    public interface IAvlSortedKeyValueTree<TKey, TValue>
        where TKey : ILazinator
        where TValue : ILazinator
    {
    }
}