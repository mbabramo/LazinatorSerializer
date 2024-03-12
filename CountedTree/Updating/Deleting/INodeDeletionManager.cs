using Lazinator.Core;
using System;
using Lazinator.Attributes;
using System.Collections.Generic;

namespace CountedTree.Updating
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.NodeDeletionManager)]
    public interface INodeDeletionManager<TKey> where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        bool StoreUintSets { get; set; }
        TimeSpan MinimumRetentionTime { get; set; }
        /// <summary>
        /// The deletion plans queue contains information about what to delete at particular times. A DeletionPlan may be for an entire tree or for some set of nodes being replaced by updated nodes in the tree. When we delete from the latest version of the tree, we want what we delete to be available for some time, so we don't delete it right away.
        /// </summary>
        Queue<DeletionPlan> DeletionPlans { get; set; }
        /// <summary>
        /// Some nodes cannot be deleted because later nodes inherit their UintSet storage from them. So, we defer deletion of a node until the inheriting node is deleted.
        /// </summary>
        NodeDeferralPlan DeferralPlan { get; set; }
        /// <summary>
        /// When deleting an entire tree, we do not delete the entire tree all at once, since that might be too slow. As a result, we maintain a stack to walk.
        /// </summary>
        Stack<NodeDeletionStatus> DeletionStack { get; set; }
    }
}