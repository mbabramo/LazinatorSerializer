using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Collections.Location;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorAvlCollections.Avl.BinaryTree
{
    /// <summary>
    /// The most basic node type explicitly defining the node type as a generic parameter. Another node type can implement this type defining itself as the generic parameter
    /// </summary>
    /// <typeparam name="T">The value type</typeparam>
    /// <typeparam name="N">The children and parent types (should be same as this type)</typeparam>
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
