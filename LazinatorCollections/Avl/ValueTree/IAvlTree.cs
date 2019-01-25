﻿using Lazinator.Core;
using Lazinator.Attributes;

namespace LazinatorCollections.Avl.ValueTree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlTree)]
    public interface IAvlTree<T> where T : ILazinator
    {
        string ToTreeString();
    }
}