using Lazinator.Core;
using System;
using Lazinator.Attributes;

namespace CountedTree.Updating
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.NodeToDelete)]
    public interface INodeToDelete
    {
        [SetterAccessibility("private")]
        long NodeID { get; }
        [SetterAccessibility("private")]
        bool IsRoot { get; }

    }
}