using Lazinator.Core;
using System;
using Lazinator.Attributes;

namespace CountedTree.Node
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.IncludedIndices)]
    public interface IIncludedIndices
    {
        /// <summary>
        /// The expected index among all items in the superset of the first item from the superset included in this node. 
        /// </summary>
        uint FirstIndexInSuperset { get; set; }
        /// <summary>
        /// The expected index among all items in the superset of the last item from the superset included in this node. 
        /// </summary>
        uint LastIndexInSuperset { get; set; }
        /// <summary>
        /// The expected index among all items in the filtered set of the first item from the filtered set included in this node. 
        /// </summary>
        uint FirstIndexInFilteredSet { get; set; }
        /// <summary>
        /// The expected index among all items in the filtered set of the last item from the filtered set included in this node. 
        /// </summary>
        uint LastIndexInFilteredSet { get; set; }
    }
}