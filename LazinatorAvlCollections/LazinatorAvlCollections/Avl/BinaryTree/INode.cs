using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Collections.Location;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorAvlCollections.Avl.BinaryTree
{
    [NonexclusiveLazinator((int) LazinatorAvlCollectionUniqueIDs.INode)]
    public interface INode<T, N> where T : ILazinator where N : class, ILazinator, new()
    {
        T Value { get; set; }
        N Left { get; set; }
        N Right { get; set; }
        N Parent { get; set; }
        IContainerLocation GetLocation();
        N CreateNode(T value, N parent);
        [DoNotAutogenerate]
        N SomeChild { get; }
        void OnRootAccessed();
    }
}
