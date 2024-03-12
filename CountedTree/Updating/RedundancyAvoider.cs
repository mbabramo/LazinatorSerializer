using CountedTree.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CountedTree.Updating
{

    public partial class RedundancyAvoider : IRedundancyAvoider
    {
        public const int MinHoursBeforeEliminatingClient = 24 * 14;

        public RedundancyAvoider()
        {
            LastAccessDictionary = new Dictionary<Guid, LastClientAccess>();
        }

        public bool IsRedundant(Guid clientID, long versionNumber)
        {
            if (LastAccessDictionary.ContainsKey(clientID))
            {
                LastClientAccess lastAccess = LastAccessDictionary[clientID];
                if (versionNumber <= lastAccess.VersionNumber)
                    return true;
            }
            UpdateLastAccessTime(clientID, versionNumber);
            EliminateOutdated();
            return false;
        }

        private void UpdateLastAccessTime(Guid clientID, long versionNumber)
        {
            LastAccessDictionary[clientID] = new LastClientAccess() { LastAccessTime = StorageFactory.GetDateTimeProvider().Now, VersionNumber = versionNumber };
        }

        public void EliminateOutdated()
        {
            // If a client hasn't sent any new information in a long time, then we can safely assume that we won't get anything redundant from the client.
            DateTime now = StorageFactory.GetDateTimeProvider().Now;
            DateTime eliminateClientsBefore = now - TimeSpan.FromHours(MinHoursBeforeEliminatingClient);
            foreach (var item in LastAccessDictionary.AsEnumerable().ToList())
            {
                if (item.Value.LastAccessTime < eliminateClientsBefore)
                    LastAccessDictionary.Remove(item.Key);
            }
        }
    }
}
