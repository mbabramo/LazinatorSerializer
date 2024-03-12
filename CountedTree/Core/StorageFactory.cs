using CountedTree.NodeStorage;
using CountedTree.PendingChanges;
using CountedTree.Updating;
using R8RUtilities;
using System;

namespace CountedTree.Core
{
    public static class StorageFactory
    {
        // Note: We don't have a <TKey> generic constraint on storage factory. The reason is that we have a Reset method, and we don't want to have to call Reset for every possible node type, since this code may not know every possible node type. More generally, it makes sense to think of "storage" as covering all node types and pending changes types. 

        // todo: we're going to choose our storage method via a config.json file, so we can use in memory storage or other remote forms of storage.

        static INodeStorage NodeStorage;
        static IBlob<Guid> BlobStorageForUintSet;
        static IBlob<Guid> BlobStorageForTreeHistoryManager;
        static IBlob<Guid> BlobStorageForPendingChangesStorage;
        static AbsoluteFakeDateTimeProvider DateTimeProvider;

        public static void Reset()
        {
            NodeStorage = new InMemoryNodeStorage();
            BlobStorageForUintSet = new InMemoryBlob<Guid>();
            BlobStorageForTreeHistoryManager = new InMemoryBlob<Guid>();
            BlobStorageForPendingChangesStorage = new InMemoryBlob<Guid>();
            DateTimeProvider = new AbsoluteFakeDateTimeProvider();
        }

        public static INodeStorage GetNodeStorage()
        {
            return NodeStorage;
        }

        public static PendingChangesOverTimeStorage GetPendingChangesStorage()
        {
            return new PendingChangesOverTimeStorage(BlobStorageForPendingChangesStorage);
        }

        public static ITreeHistoryManagerAccess GetTreeHistoryManagerAccess()
        {
            return new TreeHistoryManagerAccess(BlobStorageForTreeHistoryManager);
        }

        public static IBlob<Guid> GetUintSetStorage()
        {
            return BlobStorageForUintSet;
        }

        public static IDateTimeProvider GetDateTimeProvider()
        {
            return DateTimeProvider;
        }
    }
}
