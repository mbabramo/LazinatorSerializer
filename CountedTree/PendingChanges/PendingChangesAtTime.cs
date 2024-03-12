using Lazinator.Core;
using System;

namespace CountedTree.PendingChanges
{
    public partial class PendingChangesAtTime<TKey> : IPendingChangesAtTime<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {

        public PendingChangesAtTime(DateTime submissionTime, PendingChangesCollection<TKey> pendingChanges)
        {
            SubmissionTime = submissionTime;
            PendingChanges = pendingChanges;
        }
    }
}
