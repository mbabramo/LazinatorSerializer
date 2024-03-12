using Lazinator.Core;
using System;
using Lazinator.Attributes;

namespace CountedTree.Updating
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.DeletionPlan)]
    public interface IDeletionPlan
    {
        [SetterAccessibility("private")]
        public DateTime DeletionTime { get; }
    }
}