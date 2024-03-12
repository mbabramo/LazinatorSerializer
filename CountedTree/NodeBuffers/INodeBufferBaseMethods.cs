using Lazinator.Core;
using System;
using Lazinator.Attributes;
using CountedTree.PendingChanges;

namespace CountedTree.NodeBuffers
{
    [NonexclusiveLazinator((int)CountedTreeLazinatorUniqueIDs.NodeBufferBaseMethods)]
    public interface INodeBufferBaseMethods<TKey> : ILazinator where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {
        PendingChangesCollection<TKey> PendingChangesAtNodeIndex(int nodeIndex);
        PendingChangesCollection<TKey> AllPendingChanges();
        int GetNumPendingChanges();
        int GetNetItemChanges();
        int GetMaxPendingChanges();
        int NumPendingChangesAtNodeIndex(int nodeIndex);
        int NetItemChangeAtNodeIndex(int nodeIndex);
        int NumPendingChangesBelowNodeIndex(int nodeIndex);
        int NetItemChangeBelowNodeIndex(int nodeIndex);
        int NumPendingChangesAboveNodeIndex(int nodeIndex);
        int NetItemChangeAboveNodeIndex(int nodeIndex);
    }
}