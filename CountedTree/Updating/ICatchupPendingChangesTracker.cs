using Lazinator.Core;
using System;
using Lazinator.Attributes;

namespace CountedTree.Updating
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.CatchupPendingChangesTracker)]
    public interface ICatchupPendingChangesTracker<TKey> where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        /// <summary>
        /// The last set of buffered pending changes to have been deleted. We delete only after having added.
        /// </summary>
        long LastCatchupBufferedPendingChangesIDDeleted { get; set; }
        /// <summary>
        /// The last set of buffered pending changes to be added into the tree.
        /// </summary>
        long LastCatchupBufferedPendingChangesIDAdded  { get; set; }
        /// <summary>
        /// The ID of the current catchup buffered pending changes.
        /// </summary>
        long CurrentCatchupBufferedPendingChangesID { get; set; }
        /// <summary>
        /// The number of pending changes in the current catchup buffer.
        /// </summary>
        public int NumPendingChangesInCurrentCatchupBuffer { get; set; }
    }
}