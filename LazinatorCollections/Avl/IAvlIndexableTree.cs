using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace LazinatorCollections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlIndexableTree)]
    public interface IAvlIndexableTree<T> where T : ILazinator
    {
    }
}