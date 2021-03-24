using Lazinator.Attributes;
using Lazinator.Core;
using System;

namespace LazinatorCollections.Tree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorBinaryTreeNode)]
    [Splittable]
    [AsyncLazinatorMemory]
    public interface ILazinatorBinaryTreeNode<T> where T : ILazinator, IComparable<T>
    {
        LazinatorBinaryTreeNode<T> LeftNode { get; set; }
        LazinatorBinaryTreeNode<T> RightNode { get; set; }
        T Data { get; set; }
    }
}