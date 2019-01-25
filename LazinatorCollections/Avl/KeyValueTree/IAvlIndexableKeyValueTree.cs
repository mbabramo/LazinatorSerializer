using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace LazinatorCollections.Avl.KeyValueTree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlIndexableKeyValueTree)]
    internal interface IAvlIndexableKeyValueTree<TKey, TValue>
        where TKey : ILazinator
        where TValue : ILazinator
    {
    }
}