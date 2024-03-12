using System;
using System.Threading.Tasks;
using CountedTree.Node;
using CountedTree.NodeResults;
using CountedTree.Queries;
using Lazinator.Core;
using R8RUtilities;

namespace CountedTree.NodeStorage
{
    public abstract class BaseNodeStorage : INodeStorage
    {
        public abstract Task AddNode<TKey>(Guid treeID, CountedNode<TKey> node, IBlob<Guid> uintSetStorage) where TKey : struct, ILazinator, IComparable, IComparable<TKey>,  IEquatable<TKey>;
        public abstract Task DeleteNode<TKey>(Guid treeID, long nodeID, IBlob<Guid> uintSetStorage) where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>;
        public abstract Task<CountedNode<TKey>> GetNode<TKey>(Guid treeID, long nodeID) where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>;
        public abstract Task<NodeResultBase<TKey>> ProcessQuery<TKey>(Guid treeID, long nodeID, NodeQueryBase<TKey> query) where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>;

        public static async Task StoreUintSet<TKey>(CountedNode<TKey> node, IBlob<Guid> uintSetStorage) where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
        {
            if (node is CountedInternalNode<TKey> && uintSetStorage != null && node.TreeStructure.StoreUintSets)
            { 
                // When initially creating nodes, we usually save UintSets into a temporary storage, so that we don't commit intermediate nodes' UintSets to permanent storage. But now we have the opportunity to set the permanent storage for this node.
                var c = (CountedInternalNode<TKey>)node;
                if (c.UintSetStorage == uintSetStorage)
                    return; // we've already saved it in permanent storage
                else
                    await c.SetUintSetStorage(uintSetStorage); // this will detect the change in storage (e.g., from temporary in memory to permanent) and persist it
            }
        }

        public async Task DeleteUintSet<TKey>(Guid treeID, long nodeID, IBlob<Guid> uintSetStorage) where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
        {
            if (uintSetStorage != null)
            {
                var node = await GetNode<TKey>(treeID, nodeID);
                if (node == null)
                    throw new Exception("Internal exception. Tried to delete uintset from nonexistent node.");
                await node.SetUintSetStorage(uintSetStorage);
                if (node is CountedInternalNode<TKey>)
                {
                    var c = (CountedInternalNode<TKey>)node;
                    await c.DeleteUintSet();
                }
            }
        }
    }
}
