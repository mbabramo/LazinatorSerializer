using System;
using System.Collections.Generic;
using CountedTree.Core;
using CountedTree.PendingChanges;
using CountedTree.Queries;
using CountedTree.NodeResults;
using R8RUtilities;
using System.Threading.Tasks;
using Lazinator.Core;

namespace CountedTree.Node
{
    public abstract partial class CountedNode<TKey> : ICountedNode<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {
        
        /// <summary>
        /// The depth of this node. The tree root has Depth 0.
        /// </summary>
        public byte Depth => NodeInfo.Depth;

        /// <summary>
        /// The ID of this node
        /// </summary>
        public long ID => NodeInfo.NodeID;
        /// <summary>
        /// Returns true if this is a leaf node.
        /// </summary>
        public bool IsLeafNode => NodeInfo.MaxDepth == Depth;

        public CountedNode()
        {

        }

        public CountedNode(byte depth, TreeStructure treeStructure)
        {
            TreeStructure = treeStructure;
        }

        public abstract List<long> GetChildrenIDs();

        public abstract IEnumerable<long> GetNodesToFlushFrom(int minWork);

        public abstract IEnumerable<Tuple<long, bool>> GetNodesToFlushTo(int minWork);

        public abstract Task<List<CountedNode<TKey>>> FlushToNode(long nextIDToUse, PendingChangesCollection<TKey> changesToIncorporate);

        public abstract Task<NodeResultBase<TKey>> ProcessQuery(NodeQueryBase<TKey> request);

        public abstract Task SetUintSetStorage(IBlob<Guid> uintSetStorage);

        public virtual long? NodeForDescendantsToInheritFrom => null;
        public virtual long? NodeForDescendantDeltaSetsToBuildOn => null;

        public virtual Task DeleteUintSet()
        {
            return Task.CompletedTask;
        }

        public virtual Task MakeNotInherited(long idOfNodeNotToInheritFrom)
        {
            return Task.CompletedTask;
        }
    }
}
