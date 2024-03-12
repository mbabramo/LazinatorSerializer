using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CountedTree.Node;
using CountedTree.NodeResults;
using CountedTree.Queries;
using Lazinator.Core;
using R8RUtilities;
using Utility;

namespace CountedTree.NodeStorage
{
    public class InMemoryNodeStorage : BaseNodeStorage, INodeStorage
    {
        public Dictionary<Guid, object> NodeStorage = new Dictionary<Guid, object>();

        private Guid GetFullNodeID(Guid treeID, long nodeID)
        {
            return MD5HashGenerator.GetDeterministicGuid(new Tuple<Guid, long>(treeID, nodeID));
        }

        public override async Task AddNode<TKey>(Guid treeID, CountedNode<TKey> node, IBlob<Guid> uintSetStorage)
        {
            lock (NodeStorage)
            {
                Guid fullID = GetFullNodeID(treeID, node.ID);
                if (NodeStorage.ContainsKey(fullID))
                    throw new Exception("Node already exists. It can be replaced only by a node with a new ID.");
                NodeStorage[fullID] = node;
            }
            await StoreUintSet(node, uintSetStorage);
        }

        public override async Task DeleteNode<TKey>(Guid treeID, long nodeID, IBlob<Guid> uintSetStorage)
        {
            await DeleteUintSet<TKey>(treeID, nodeID, uintSetStorage);
            Guid fullID = GetFullNodeID(treeID, nodeID);
            lock (NodeStorage)
            {
                if (NodeStorage.ContainsKey(fullID))
                    NodeStorage.Remove(fullID);
            }
        }

        public override Task<CountedNode<TKey>> GetNode<TKey>(Guid treeID, long nodeID)
        {
            Guid fullID = GetFullNodeID(treeID, nodeID);
            lock (NodeStorage)
            {
                if (NodeStorage.ContainsKey(fullID))
                    return Task.FromResult((CountedNode<TKey>) NodeStorage[fullID]);
                return Task.FromResult<CountedNode<TKey>>(null);
            }
        }

        public IEnumerable<CountedNode<TKey>> AsEnumerable<TKey>() where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
        {
            lock (NodeStorage)
            {
                return NodeStorage.Values.Select(x => (CountedNode<TKey>)x);
            }
        }

        public override async Task<NodeResultBase<TKey>> ProcessQuery<TKey>(Guid treeID, long nodeID, NodeQueryBase<TKey> query)
        {
            // NOTE: This implementation of INodeStorage copies the node here by calling GetNode. But other implementations should NOT do this. The goal should be to bring the query to the node, rather than the node to the query.
            var node = await GetNode<TKey>(treeID, nodeID);
            var result = await node.ProcessQuery(query);
            return result;
        }
    }
}
