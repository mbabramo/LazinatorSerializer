using System;
using System.Threading.Tasks;
using CountedTree.Node;
using CountedTree.NodeResults;
using CountedTree.Queries;
using R8RUtilities;
using Utility;

namespace CountedTree.NodeStorage
{
    public class DelayedInMemoryNodeStorage : InMemoryNodeStorage
    {
        public async Task AddRandomDelay()
        {
            await Task.Delay(RandomGenerator.GetRandom(0, 10));
        }

        public override async Task AddNode<TKey>(Guid treeID, CountedNode<TKey> node, IBlob<Guid> uintSetStorage)
        {
            await AddRandomDelay();
            await base.AddNode(treeID, node, uintSetStorage);
        }

        public override async Task DeleteNode<TKey>(Guid treeID, long nodeID, IBlob<Guid> uintSetStorage)
        {
            await AddRandomDelay();
            await base.DeleteNode<TKey>(treeID, nodeID, uintSetStorage);
        }

        public override async Task<CountedNode<TKey>> GetNode<TKey>(Guid treeID, long nodeID) 
        {
            await AddRandomDelay();
            return await base.GetNode<TKey>(treeID, nodeID);
        }

        public override async Task<NodeResultBase<TKey>> ProcessQuery<TKey>(Guid treeID, long nodeID, NodeQueryBase<TKey> query)
        {
            await AddRandomDelay();
            return await base.ProcessQuery(treeID, nodeID, query);
        }
    }
}
