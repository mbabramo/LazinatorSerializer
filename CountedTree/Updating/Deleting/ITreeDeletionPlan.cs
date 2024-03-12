using Lazinator.Core;
using System;
using Lazinator.Attributes;

namespace CountedTree.Updating
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.TreeDeletionPlan)]
    public interface ITreeDeletionPlan : IDeletionPlan
    {
        [SetterAccessibility("private")]
        long RootID { get; }

    }
}