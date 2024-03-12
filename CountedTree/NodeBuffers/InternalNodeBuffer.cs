using System;
using System.Collections.Generic;
using CountedTree.Core;
using CountedTree.PendingChanges;
using Lazinator.Core;

namespace CountedTree.NodeBuffers
{
    public partial class InternalNodeBuffer<TKey> : NodeBufferBase<TKey>, IInternalNodeBuffer<TKey>  where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {

        internal PendingChangesCollection<TKey> PendingChanges { get; }
        internal int[] PendingChangesTracker { get; }
        internal int[] NetItemChangesTracker { get; }

        public InternalNodeBuffer(int numNodeReferences, Func<int, KeyAndID<TKey>?> splitValuesFn, PendingChangesCollection<TKey> pendingChanges)
        {
            PendingChanges = pendingChanges;
            PendingChangesTracker = new int[numNodeReferences];
            NetItemChangesTracker = new int[numNodeReferences];
            int splitValuesIndex = 0, pendingChangesIndex = 0;
            for (int nodeIndex = 0; nodeIndex < numNodeReferences - 1; nodeIndex++)
            {
                ProcessPendingChange(pendingChanges.PendingChanges, ref splitValuesIndex, ref pendingChangesIndex, splitValuesFn(nodeIndex));
            }
            ProcessPendingChange(pendingChanges.PendingChanges, ref splitValuesIndex, ref pendingChangesIndex, null); // process any pending changes beyond last split value
            Cumulative = base.SetCumulativePendingChanges(numNodeReferences);
        }


        public override PendingChangesCollection<TKey> AllPendingChanges()
        {
            return PendingChanges;
        }

        private void ProcessPendingChange(PendingChange<TKey>[] pendingChanges, ref int splitValuesIndex, ref int pendingChangesIndex, KeyAndID<TKey>? splitValue)
        {
            while (pendingChangesIndex < pendingChanges.Length && (splitValue == null || pendingChanges[pendingChangesIndex].Item <= splitValue))
            {
                PendingChangesTracker[splitValuesIndex]++;
                if (pendingChanges[pendingChangesIndex].Delete)
                    NetItemChangesTracker[splitValuesIndex]--;
                else
                    NetItemChangesTracker[splitValuesIndex]++;
                pendingChangesIndex++;
            }

            splitValuesIndex++;
        }

        public override PendingChangesCollection<TKey> PendingChangesAtNodeIndex(int index)
        {
            int indexInPendingChanges = NumPendingChangesBelowNodeIndex(index);
            List<PendingChange<TKey>> l = new List<PendingChange<TKey>>();
            for (int i = 0; i < NumPendingChangesAtNodeIndex(index); i++)
                l.Add(PendingChanges[indexInPendingChanges + i]);
            return new PendingChangesCollection<TKey>(l, false);
        }

        public override int NumPendingChangesAtNodeIndex(int index)
        {
            return PendingChangesTracker[index];
        }

        public override int NetItemChangeAtNodeIndex(int index)
        {
            return NetItemChangesTracker[index];
        }
    }
}
