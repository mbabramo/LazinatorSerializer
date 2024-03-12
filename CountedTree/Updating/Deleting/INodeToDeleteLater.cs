using Lazinator.Core;
using System;
using Lazinator.Attributes;

namespace CountedTree.Updating
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.NodeToDeleteLater)]
    public interface INodeToDeleteLater
    {
        [SetterAccessibility("private")]
        NodeToDelete NodeToDelete { get; }
        [SetterAccessibility("private")]
        long DeleteWhenThisDeleted { get; }

    }
}