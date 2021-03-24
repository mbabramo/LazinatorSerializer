using Lazinator.Attributes;
using Lazinator.Core;
using System;

namespace LazinatorCollections.Tree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorBinaryTree)]
    [Splittable]
    [AsyncLazinatorMemory]
    public interface ILazinatorBinaryTree<T> where T : ILazinator, IComparable<T>
    {
        LazinatorBinaryTreeNode<T> Root { get; set; }
    }
}