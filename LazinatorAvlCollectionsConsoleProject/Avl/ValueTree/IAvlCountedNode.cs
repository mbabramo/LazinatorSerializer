using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace LazinatorAvlCollections.Avl.ValueTree
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IAvlCountedNode)]
    public interface IAvlCountedNode<T> where T : ILazinator
    {
        T Value { get; set; }
        long LeftCount { get; set; }
        long RightCount { get; set; }
        [OnPropertyAccessed("OnChildNodeAccessed(_Left, true);")]
        AvlCountedNode<T> Left { get; set; }
        [OnPropertyAccessed("OnChildNodeAccessed(_Right, false);")]
        AvlCountedNode<T> Right { get; set; }
        [DoNotAutogenerate]
        AvlCountedNode<T> Parent { get; set; }

        int Balance { get; set; }
    }
}