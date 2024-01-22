﻿using Lazinator.Attributes;
using Lazinator.Core;

namespace LazinatorAvlCollections.Avl.ListTree
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IAvlSortedListTree)]
    public interface IAvlSortedListTree<T> : IAvlListTree<T> where T : ILazinator
    {
    }
}