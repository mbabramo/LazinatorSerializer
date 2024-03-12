using System;
using System.Collections.Generic;
using System.Linq;
using CountedTree.Node;
using CountedTree.PendingChanges;
using Lazinator.Core;

namespace CountedTree.NodeBuffers
{
    public partial class CombinedInternalNodeBuffer<TKey> : NodeBufferBase<TKey>, ICombinedInternalNodeBuffer<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {

        public CombinedInternalNodeBuffer(CountedInternalNode<TKey> node, PendingChangesCollection<TKey> pendingChanges)
        {
            NodeBuffer = node.MainBuffer;
            RequestBuffer = new InternalNodeBuffer<TKey>(node.TreeStructure.NumChildrenPerInternalNode, node.SplitValue, pendingChanges);
            Cumulative = SetCumulativePendingChanges(node.TreeStructure.NumChildrenPerInternalNode);
        }

        public CombinedInternalNodeBuffer(CountedInternalNode<TKey> node, INodeBufferBaseMethods<TKey> pendingChangesBuffer)
        {
            NodeBuffer = node.MainBuffer;
            RequestBuffer = pendingChangesBuffer;
            Cumulative = SetCumulativePendingChanges(node.TreeStructure.NumChildrenPerInternalNode);
        }

        IEnumerable<INodeBufferBaseMethods<TKey>> GetBuffers()
        {
            yield return NodeBuffer;
            yield return RequestBuffer;
        }

        public override PendingChangesCollection<TKey> PendingChangesAtNodeIndex(int index)
        {
            return new PendingChangesCollection<TKey>(GetBuffers().Select(x => x.PendingChangesAtNodeIndex(index).AsEnumerable()));
        }

        public override int NumPendingChangesAtNodeIndex(int index)
        {
            return GetBuffers().Sum(b => b.NumPendingChangesAtNodeIndex(index));
        }

        public override int NetItemChangeAtNodeIndex(int index)
        {
            return GetBuffers().Sum(b => b.NetItemChangeAtNodeIndex(index));
        }

        public override PendingChangesCollection<TKey> AllPendingChanges()
        {
            var enumerables = GetBuffers().Select(x => x.AllPendingChanges());
            return new PendingChangesCollection<TKey>(enumerables); // note that request buffer is second, so it will replace earlier-in-time main buffer
        }

    }
}
