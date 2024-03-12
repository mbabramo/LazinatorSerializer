using System;
using System.Linq;
using CountedTree.PendingChanges;
using Lazinator.Core;

namespace CountedTree.NodeBuffers
{
    public abstract partial class NodeBufferBase<TKey> : INodeBufferBase<TKey>, INodeBufferBaseMethods<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {

        public abstract PendingChangesCollection<TKey> PendingChangesAtNodeIndex(int index);

        public abstract int NumPendingChangesAtNodeIndex(int index);
        public abstract int NetItemChangeAtNodeIndex(int index);


        public int GetMaxPendingChanges()
        {
            return Cumulative.MaxPendingChanges;
        }

        public int GetNumPendingChanges()
        {
            return NumPendingChangesAtNodeIndex(0) + NumPendingChangesAboveNodeIndex(0);
        }

        public int GetNetItemChanges()
        {
            return NetItemChangeAtNodeIndex(0) + NetItemChangeAboveNodeIndex(0);
        }

        public abstract PendingChangesCollection<TKey> AllPendingChanges();

        public CumulativeBufferedChanges SetCumulativePendingChanges(int numNodes)
        {
            var cumulativePendingChangesAtIndexAscending = new int[numNodes];
            var cumulativePendingChangesAtIndexDescending = new int[numNodes];
            var cumulativeNetItemChangeAtIndexAscending = new int[numNodes];
            var cumulativeNetItemChangeAtIndexDescending = new int[numNodes];
            int cumPendingChanges = 0, cumNetItemChanges = 0;
            for (int i = 0; i < numNodes; i++)
            {
                cumPendingChanges += NumPendingChangesAtNodeIndex(i);
                cumNetItemChanges += NetItemChangeAtNodeIndex(i);
                cumulativePendingChangesAtIndexAscending[i] = cumPendingChanges;
                cumulativeNetItemChangeAtIndexAscending[i] = cumNetItemChanges;
            }
            cumPendingChanges = 0;
            cumNetItemChanges = 0;
            for (int i = numNodes - 1; i >= 0; i--)
            {
                cumPendingChanges += NumPendingChangesAtNodeIndex(i);
                cumNetItemChanges += NetItemChangeAtNodeIndex(i);
                cumulativePendingChangesAtIndexDescending[i] = cumPendingChanges;
                cumulativeNetItemChangeAtIndexDescending[i] = cumNetItemChanges;
            }
            var maxPendingChanges = Enumerable.Range(0, numNodes).Select(x => NumPendingChangesAtNodeIndex(x)).Max();
            CumulativeBufferedChanges c = new CumulativeBufferedChanges(cumulativePendingChangesAtIndexAscending, cumulativePendingChangesAtIndexDescending, cumulativeNetItemChangeAtIndexAscending, cumulativeNetItemChangeAtIndexDescending, maxPendingChanges);
            return c;
        }

        public int NetItemChangeAboveNodeIndex(int index)
        {
            return Cumulative.CumulativeNetItemChangeAtIndexDescending[index] - NetItemChangeAtNodeIndex(index);
        }
        public int NetItemChangeBelowNodeIndex(int index)
        {
            return Cumulative.CumulativeNetItemChangeAtIndexAscending[index] - NetItemChangeAtNodeIndex(index);
        }
        public int NumPendingChangesAboveNodeIndex(int index)
        {
            return Cumulative.CumulativePendingChangesAtIndexDescending[index] - NumPendingChangesAtNodeIndex(index);
        }
        public int NumPendingChangesBelowNodeIndex(int index)
        {
            return Cumulative.CumulativePendingChangesAtIndexAscending[index] - NumPendingChangesAtNodeIndex(index);
        }
    }
}
