using CountedTree.Updating;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CountedTree.PendingChanges
{
    public partial class PendingChangesOverTime<TKey> : IPendingChangesOverTime<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {

        public PendingChangesOverTime()
        {
            SubmittedChanges = new Queue<PendingChangesAtTime<TKey>>();
            RedundancyAvoider = new RedundancyAvoider();
        }

        public PendingChangesOverTime(long rootID) : this()
        {
            RootID = rootID;
        }

        public bool AddPendingChangesAtTime(PendingChangesAtTime<TKey> pendingChanges, long versionNumber)
        {
            bool isRedundant = RedundancyAvoider.IsRedundant(new Guid(), versionNumber);
            if (isRedundant)
                return false;
            if (pendingChanges.SubmissionTime <= NextTimeAvailable)
                pendingChanges = new PendingChangesAtTime<TKey>((DateTime) NextTimeAvailable + TimeSpan.FromTicks(1), pendingChanges.PendingChanges); // could happen as a result of nonsynchronized clocks or b/c two sets happen within a tick. We want each set to have a different time so that when we request the next set, we get only that set, not two sets.
            if (!SubmittedChanges.Any())
                NextTimeAvailable = pendingChanges.SubmissionTime;
            SubmittedChanges.Enqueue(pendingChanges);
            return true;
            
        }

        public void RemovePendingChanges(DateTime atOrBeforeTime)
        {
            while (SubmittedChanges.Any() && SubmittedChanges.Peek().SubmissionTime <= atOrBeforeTime)
                SubmittedChanges.Dequeue();
            if (SubmittedChanges.Any())
                NextTimeAvailable = SubmittedChanges.First().SubmissionTime;
            else
                NextTimeAvailable = null;
        }

        public PendingChangesCollection<TKey> GetPendingChangesAsOfTime(DateTime asOfTime)
        {
            var pcc = new PendingChangesCollection<TKey>(SubmittedChanges.TakeWhile(x => x.SubmissionTime <= asOfTime).Select(x => x.PendingChanges.AsEnumerable()));
            return pcc;
        }

        public PendingChangesCollection<TKey> GetAllPendingChanges()
        {
            var pcc = new PendingChangesCollection<TKey>(SubmittedChanges.Select(x => x.PendingChanges.AsEnumerable()));
            return pcc;
        }

        public PendingChangesCollection<TKey> GetNextSetOfPendingChanges()
        {
            if (NextTimeAvailable != null)
                return GetPendingChangesAsOfTime((DateTime)NextTimeAvailable);
            else
                return null;
        }

        public IEnumerable<PendingChangesAtTime<TKey>> GetAllPendingChangesGroupedByTime()
        {
            return SubmittedChanges;
        }

        public void RemoveFirstSetOfPendingChanges()
        {
            if (NextTimeAvailable != null)
                RemovePendingChanges((DateTime) NextTimeAvailable);
        }
    }
}
