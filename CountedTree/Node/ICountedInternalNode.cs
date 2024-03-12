using CountedTree.NodeBuffers;
using Lazinator.Attributes;
using Lazinator.Core;
using System;

namespace CountedTree.Node
{
    [Lazinator((int) CountedTreeLazinatorUniqueIDs.CountedInternalNode)]
    public interface ICountedInternalNode<TKey> : ICountedNode<TKey> where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
    {

        /// <summary>
        /// Information on child nodes.
        /// </summary>
        NodeInfo<TKey>[] ChildNodeInfos { get; set; }
        /// <summary>
        /// Keeps track of the number of items below and including or above and including each node.
        /// </summary>
        CumulativeItemsCounter<TKey> Cumulator { get; set; }
        /// <summary>
        /// Storage within the node for items being buffered (not including pending changes)
        /// </summary>
        [SetterAccessibility("private")]
        INodeBufferBaseMethods<TKey> MainBuffer { get; }
        // uintset-related information
        long? InheritedMainSetNodeID { get; set; }
        long? InheritedDeltaSetsNodeID { get; set;  }
        bool UintSetInitialized { get; set; }
    }
}