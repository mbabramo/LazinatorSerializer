using System;
using CountedTree.Core;
using Lazinator.Core;

namespace CountedTree.Node
{
    /// <summary>
    /// Information on this node and its descendants.
    /// </summary>
    public partial class NodeInfo<TKey> : INodeInfo<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {
 

        public NodeInfo(long nodeID, int numNodes, uint numSubtreeValues, int amountOfWorkNeeded, int maxWorkNeededInSubtree, byte depth, byte maxDepth, bool created, KeyAndID<TKey>? firstExclusive, KeyAndID<TKey>? lastInclusive)
        {
            NodeID = nodeID;
            NumNodes = numNodes;
            NumSubtreeValues = numSubtreeValues;
            WorkNeeded = amountOfWorkNeeded;
            MaxWorkNeededInSubtree = maxWorkNeededInSubtree;
            FirstExclusive = firstExclusive;
            LastInclusive = lastInclusive;
            Depth = depth;
            MaxDepth = maxDepth;
            Created = created;
        }

        public NodeInfo<TKey> CloneWithNewID(long nodeID)
        {
            return new NodeInfo<TKey>(nodeID, NumNodes, NumSubtreeValues, WorkNeeded, MaxWorkNeededInSubtree, Depth, MaxDepth, Created, FirstExclusive, LastInclusive);
        }

        public NodeInfo<TKey> Clone()
        {
            return new NodeInfo<TKey>(NodeID, NumNodes, NumSubtreeValues, WorkNeeded, MaxWorkNeededInSubtree, Depth, MaxDepth, Created, FirstExclusive, LastInclusive);
        }

        public override bool Equals(object obj)
        {
            NodeInfo<TKey> other = obj as NodeInfo<TKey>;
            if (other == null)
                return false;
            return NodeID == other.NodeID && Depth == other.Depth && MaxDepth == other.MaxDepth && NumSubtreeValues == other.NumSubtreeValues && WorkNeeded == other.WorkNeeded && MaxWorkNeededInSubtree == other.MaxWorkNeededInSubtree && FirstExclusive == other.FirstExclusive && LastInclusive == other.LastInclusive;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(NodeID, Depth, MaxDepth, NumSubtreeValues, WorkNeeded, MaxWorkNeededInSubtree, FirstExclusive, LastInclusive).GetHashCode();
        }

        public override string ToString()
        {
            return $"{NodeID} ({Depth} of {MaxDepth}): {FirstExclusive?.ToString() ?? "-Inf"}-{LastInclusive?.ToString() ?? "Inf"} ({NumSubtreeValues} values, {WorkNeeded} work here, {MaxWorkNeededInSubtree} max work)";
        }
    }
}
