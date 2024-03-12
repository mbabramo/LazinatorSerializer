using CountedTree.PendingChanges;
using CountedTree.Node;
using CountedTree.NodeStorage;
using CountedTree.Core;
using R8RUtilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace CountedTree.Updating
{
    public static class TreeFactory 
    {


        public static async Task<TreeInfo> CreateLinearTree<TKey>(Guid treeID, TreeStructure treeStructure, TreeUpdateSettings treeUpdateSettings, KeyAndID<TKey>? first, KeyAndID<TKey>? last) where TKey : struct, ILazinator,
              IComparable,
              IComparable<TKey>,
              IEquatable<TKey>
        {
            CountedNode<TKey> rootNode = GetLinearRootNode(treeStructure, first, last);
            var treeInfo = await CreateTreeFromRootNode(treeID, rootNode);
            CreateTreeHistoryManager<TKey>(treeID, treeStructure, treeUpdateSettings, treeInfo);
            return treeInfo;
        }

        private static void CreateTreeHistoryManager<TKey>(Guid treeID, TreeStructure treeStructure, TreeUpdateSettings treeUpdateSettings, TreeInfo treeInfo) where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
        {
            var thm = new TreeHistoryManager<TKey>(treeInfo, treeUpdateSettings, treeStructure);
            StorageFactory.GetTreeHistoryManagerAccess().Set<TKey>(treeID, thm);
        }

        public static async Task<TreeInfo> CreateGeoTree(Guid treeID, TreeUpdateSettings treeUpdateSettings, TreeStructure treeStructure)
        {
            CountedLeafNodeGeo rootNode = GetGeoRootNode(treeStructure);
            var treeInfo = await CreateTreeFromRootNode(treeID, rootNode);
            CreateTreeHistoryManager<WUInt64>(treeID, treeStructure, treeUpdateSettings, treeInfo);
            return treeInfo;
        }

        public async static Task<TreeInfo> ReconstructTreeInfo<TKey>(Guid treeID, long currentRootID) where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
        {
            INodeStorage nodeStorage = StorageFactory.GetNodeStorage();
            PendingChangesOverTimeStorage pendingChangesStorage = StorageFactory.GetPendingChangesStorage();
            IDateTimeProvider dateTimeProvider = StorageFactory.GetDateTimeProvider();
            var rootNode = await nodeStorage.GetNode<TKey>(treeID, currentRootID);
            var treeInfo = new TreeInfo(treeID, currentRootID, rootNode.NodeInfo.NumSubtreeValues, rootNode.NodeInfo.MaxDepth, rootNode.NodeInfo.WorkNeeded);
            return treeInfo;
        }

        private static async Task<TreeInfo> CreateTreeFromRootNode<TKey>(Guid treeID, CountedNode<TKey> rootNode) where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
        {
            INodeStorage nodeStorage = StorageFactory.GetNodeStorage();
            PendingChangesOverTimeStorage pendingChangesStorage = StorageFactory.GetPendingChangesStorage();
            IDateTimeProvider dateTimeProvider = StorageFactory.GetDateTimeProvider();
            await nodeStorage.AddNode(treeID, rootNode, null); // no uint set for leaf node
            await pendingChangesStorage.InitializePendingChangesForNode<TKey>(treeID, rootNode.ID);
            await pendingChangesStorage.InitializePendingChangesForNode<TKey>(treeID, -1); // inactive items storage
            return new TreeInfo(treeID, rootNode.ID, rootNode.NodeInfo.NumSubtreeValues, rootNode.NodeInfo.MaxDepth, rootNode.NodeInfo.WorkNeeded);
        }


        private static CountedNode<TKey> GetLinearRootNode<TKey>(TreeStructure treeStructure, KeyAndID<TKey>? first, KeyAndID<TKey>? last) where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
        {
            return new CountedLeafNode<TKey>(new List<KeyAndID<TKey>>(), 0, 0, treeStructure, first, last);
        }

        private static CountedLeafNodeGeo GetGeoRootNode(TreeStructure treeStructure)
        {
            return new CountedLeafNodeGeo(new List<KeyAndID<WUInt64>>(), 0, 0, treeStructure, new KeyAndID<WUInt64>(ulong.MinValue, uint.MinValue), new KeyAndID<WUInt64>(ulong.MaxValue, uint.MaxValue));
        }
    }
}
