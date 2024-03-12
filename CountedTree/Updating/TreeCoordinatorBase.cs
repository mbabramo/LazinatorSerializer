using CountedTree.Node;
using CountedTree.NodeStorage;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountedTree.Updating
{
    public abstract class TreeCoordinatorBase<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {
        public long RootID;
        public INodeStorage NodeStorage;
        public TemporaryNodeStorage<TKey> TemporaryNodeStorage = new TemporaryNodeStorage<TKey>();
        public HashSet<long> NodesRequested = new HashSet<long>();


        public bool IsRequestedAlreadyOrAdd(long nodeID)
        {
            lock (NodesRequested)
            {
                if (NodesRequested.Contains(nodeID))
                    return true;
                NodesRequested.Add(nodeID);
                return false;
            }
        }

        public async Task<CountedNode<TKey>> GetInitialRootNode(Guid treeID)
        {
            await RequestNodesByID(treeID, new long[] { RootID }, null);
            return TemporaryNodeStorage.GetNode(RootID);
        }

        public async Task RequestNodesByID(Guid treeID, IEnumerable<long> nodeIDsToRequest, long? parentID)
        {
            var nodesNeeded = nodeIDsToRequest.Where(requestedNodeID => TemporaryNodeStorage.GetNode(requestedNodeID) == null).ToList();
            var nodesAlreadyLoading = nodesNeeded.Where(requestedNodeID => IsRequestedAlreadyOrAdd(requestedNodeID)).ToList();
            var nodesToLoad = nodesNeeded.Where(requestedNodeID => !nodesAlreadyLoading.Contains(requestedNodeID)).ToList();
            var nodeLoadTasks = nodesToLoad.Select(requestedNodeID => NodeStorage.GetNode<TKey>(treeID, requestedNodeID)).ToList();
            var requestedNodes = await Task.WhenAll(nodeLoadTasks);
            int i = 0;
            foreach (var requestedNode in requestedNodes)
            {
                if (requestedNode == null)
                    throw new Exception($"Internal error. Requested node {nodesToLoad[i]} not found.");
                await TemporaryNodeStorage.AddNewOrUnmutatedNodeToNodeStorage(requestedNode, parentID, false);
                i++;
            }
            // probably the nodes requested earlier are done, but we can't be sure.
            bool nodesAlreadyLoadingComplete = false;
            do
            {
                var nodesStillLoading = nodesAlreadyLoading.Where(requestedNodeID => TemporaryNodeStorage.GetNode(requestedNodeID) == null);
                nodesAlreadyLoadingComplete = !nodesStillLoading.Any();
                if (!nodesAlreadyLoadingComplete)
                    await Task.Delay(5);
            }
            while (!nodesAlreadyLoadingComplete);
        }
    }
}
