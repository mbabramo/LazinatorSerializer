using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace LazinatorAvlCollections.Avl
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IAvlIndexableTree)]
    public interface IAvlIndexableTree<T> where T : ILazinator
    {
    }
}