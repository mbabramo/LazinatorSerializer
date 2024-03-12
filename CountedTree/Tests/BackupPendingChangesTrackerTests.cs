using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CountedTree.Core;
using CountedTree.PendingChanges;
using CountedTree.Updating;
using Xunit;
using FluentAssertions;
using Utility;
using Lazinator.Wrappers;

namespace CountedTree.Tests
{
    public partial class CountedTreeTests
    {
        private class VersionNumberTracker
        {
            public long VersionNumber = 0;
        }

        [Fact]
        public async Task CatchupPendingChangesTrackerWorks()
        {
            CatchupPendingChangesTracker<WFloat>.MaxCatchupBufferSize = 3;
            VersionNumberTracker version = new VersionNumberTracker();
            Guid treeID = RandomGenerator.GetGuid();
            var bp = new CatchupPendingChangesTracker<WFloat>();
            bp.CatchupPendingChangesStored.Should().Be(false);
            await bp.DeleteNoLongerNeededCatchupBufferedPendingChanges(treeID, false); // make sure this doesn't cause problems
            uint pcID = 0;
            pcID = await AddChange(treeID, bp, pcID, version);
            bp.NumPendingChangesInCurrentCatchupBuffer.Should().Be(1);
            bp.CatchupPendingChangesStored.Should().Be(true);
            pcID = await AddChange(treeID, bp, pcID, version);
            bp.NumPendingChangesInCurrentCatchupBuffer.Should().Be(2);
            // simulate a failure after items have been written to pending changes storage
            var bp2 = bp.Clone(); // if we have a failure, then the original object will be unchanged
            --version.VersionNumber; // lower the version number... 
            pcID = await AddChange(treeID, bp2, pcID, version); // ... so this should not affect bp and should not change our later calls to GetNextCatchupBufferToAddToPermanentStorage
            bp.NumPendingChangesInCurrentCatchupBuffer.Should().Be(2);
            pcID = await AddChange(treeID, bp, pcID, version);
            bp.NumPendingChangesInCurrentCatchupBuffer.Should().Be(3);
            pcID = await AddChange(treeID, bp, pcID, version);
            bp.NumPendingChangesInCurrentCatchupBuffer.Should().Be(1);
            pcID = await Add5Changes(treeID, bp, pcID, version);
            bp.NumPendingChangesInCurrentCatchupBuffer.Should().Be(5);
            var pcc = await bp.GetNextCatchupBufferToAddToPermanentStorage(treeID);
            pcc.Count.Should().Be(3);
            bp.CatchupPendingChangesStored.Should().Be(true);
            pcc = await bp.GetNextCatchupBufferToAddToPermanentStorage(treeID);
            pcc.Count.Should().Be(1);
            pcc = await bp.GetNextCatchupBufferToAddToPermanentStorage(treeID);
            pcc.Count.Should().Be(5);
        }

        private async Task<WUInt32> AddChange(Guid treeID, CatchupPendingChangesTracker<WFloat> bp, uint pcID, VersionNumberTracker versionNumber)
        {
            await bp.AddPendingChangesToCatchupBuffer(treeID, new PendingChangesAtTime<WFloat>(GetDateTimeProvider().Now, new PendingChangesCollection<WFloat>(new List<PendingChange<WFloat>>() { GetPendingChange(ref pcID) }, true)), ++versionNumber.VersionNumber);
            return pcID;
        }

        private async Task<WUInt32> Add5Changes(Guid treeID, CatchupPendingChangesTracker<WFloat> bp, uint pcID, VersionNumberTracker versionNumber)
        {
            await bp.AddPendingChangesToCatchupBuffer(treeID, new PendingChangesAtTime<WFloat>(StorageFactory.GetDateTimeProvider().Now, new PendingChangesCollection<WFloat>(new List<PendingChange<WFloat>>() { GetPendingChange(ref pcID), GetPendingChange(ref pcID), GetPendingChange(ref pcID), GetPendingChange(ref pcID), GetPendingChange(ref pcID) }, true)), ++versionNumber.VersionNumber);
            return pcID;
        }

        private PendingChange<WFloat> GetPendingChange(ref uint pcID)
        {
            var pc = new PendingChange<WFloat>(new KeyAndID<WFloat>(1.0F, pcID), false);
            pcID++;
            return pc;
        }
    }
}
