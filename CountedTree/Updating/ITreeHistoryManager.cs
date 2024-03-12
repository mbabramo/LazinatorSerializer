using System;
using System.Threading.Tasks;
using CountedTree.Core;
using CountedTree.Node;
using CountedTree.PendingChanges;
using CountedTree.Rebuild;
using Lazinator.Attributes;
using Lazinator.Core;

namespace CountedTree.Updating
{
    [Lazinator((int) CountedTreeLazinatorUniqueIDs.TreeHistoryManager)]
    public interface ITreeHistoryManager<TKey> where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
    {

        TreeInfo CurrentTreeInfo { get; set; }
        TreeUpdateSettings UpdateSettings { get; set; }
        TreeStructure TreeStructure { get; set; }

        /// <summary>
        /// The expected number of items in the tree, including the effect of the active pending changes. This could be inaccurate if there are redundant pending changes.
        /// </summary>
        uint ExpectedNumItemsInTree { get; set; }
        /// <summary>
        /// The expected net effect of active pending changes, which are submitted on queries as the request buffer.
        /// </summary>
        int NetPendingChangesInRequestBuffer { get; set; }
        /// <summary>
        /// The number of pending changes (additions and deletions currently stored in the request buffer).
        /// </summary>
        int NumPendingChangesInRequestBuffer { get; set; }
        /// <summary>
        /// The number of pending changes sets added so far.
        /// </summary>
        long NumPendingChangesSetsAdded { get; set; }
        int FlushingWorkNeeded { get; set; }
        DifferentWorkTypes? TypeOfWorkMostNeeded { get; set;  }
        bool TreeIsDeletedGoal { get; set; }
        bool TreeIsDeleted { get; set; }
        NodeDeletionManager<TKey> NodeDeletionManager { get; set; }
        CatchupPendingChangesTracker<TKey> CatchupPendingChangesTracker { get; set; }
        RedundancyAvoider RedundancyAvoider { get; set; }
        TreeRebuilder<TKey> Rebuilder { get; set; }

        // Primary methods to be access by client.
        Task<TreeInfo> AddPendingChanges(PendingChangesAtTime<TKey> pendingChangesAtTime, Guid clientID, long versionNumber);
        Task<TreeInfo> DoWorkRepeatedly(int workNeededGoal, Task doAfterEachRound = null);
        Task<TreeInfo> DoRoundOfWork(int workNeededGoal = 0);
        Task<TreeInfo> InitiateRebuildFromExternalSource(IRebuildSource<TKey> rebuildSource);

        // Tree deletion methods (also available to client)
        Task InitiateDeleteTreeAndHistory();
        Task ReinstateTree(CountedNode<TKey> newRootNode, TreeInfo treeInfo, TreeUpdateSettings updateSettings, TreeStructure treeStructure);

        // Primarily internal methods (included here for testing purposes)
        Task<ITreeHistoryManager<TKey>> Clone();
        Task<TreeInfo> AddPendingChangesFromStorageToTree();
        Task FlushTree(TreeUpdateSettings updateSettingsOverride = null);
        Task<CountedNode<TKey>> GetCurrentRoot();
        Task<int> GetDeletionWorkNeeded();
        Task<bool> CatchupPendingChangesAreStored();
        Task<bool> GetTreeIsDeleted();
        Task InitiateRebalancing();
        Task SetTreeToNewRoot(CountedNode<TKey> newRoot, bool copyPendingChanges);
        Task UpdateWorkNeeded();
        Task PrintTree(bool redundantChanges);
    }
}