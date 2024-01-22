using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Collections.Location;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorAvlCollections.Avl.BinaryTree
{
    [NonexclusiveLazinator((int)LazinatorAvlCollectionUniqueIDs.IUpdatableNode)]
    public interface IUpdatableNode<T, N> : INode<T, N> where T : ILazinator where N : class, ILazinator, new()
    {
        [DoNotAutogenerate]
        bool NodeVisitedDuringChange { get; set; }
        void UpdateFollowingTreeChange();
        [DoNotAutogenerate]
        int Balance { get; set; }
    }
}
