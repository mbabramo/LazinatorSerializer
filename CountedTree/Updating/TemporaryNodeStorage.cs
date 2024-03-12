using CountedTree.Node;
using CountedTree.NodeStorage;
using Lazinator.Core;
using R8RUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountedTree.Updating
{
    public class TemporaryNodeStorage<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {
        #region Temporary node storage 

        InMemoryNodeStorage InMemoryStorage = new InMemoryNodeStorage();
        Dictionary<long, long> ReplacementNodeIDs = new Dictionary<long, long>();
        Dictionary<long, long?> Parents = new Dictionary<long, long?>();
        HashSet<long> EmptyNodes = new HashSet<long>();

        Guid FakeTreeID = new Guid(); // use a blank tree ID since this is intended for just one tree

        public CountedNode<TKey> GetNode(long nodeID)
        {
            return InMemoryStorage.GetNode<TKey>(FakeTreeID, nodeID).Result; // InMemoryStorage is not really async, so using .Result will not block
        }

        public CountedInternalNode<TKey> GetParentOfNode(CountedNode<TKey> node)
        {
            long? parentID = Parents[node.ID];
            if (parentID == null)
                return null;
            return (CountedInternalNode<TKey>)GetNodeOrReplacement((long)parentID);
        }

        public async Task AddNewOrUnmutatedNodeToNodeStorage(CountedNode<TKey> node, long? parentID, bool isEmpty)
        {
            if (GetNode(node.ID) == null)
            {
                await InMemoryStorage.AddNode(FakeTreeID, node, null);
                lock (InMemoryStorage)
                    Parents.Add(node.ID, parentID);
                if (isEmpty)
                    EmptyNodes.Add(node.ID);
            }
        }

        public async Task AddTransformedNodeToNodeStorage(CountedNode<TKey> node, long previousID, bool setChildParentsToThisNode = false)
        {
            if (ReplacementNodeIDs.ContainsKey(previousID))
                throw new Exception("Can only add one transformation of a node.");
            lock (InMemoryStorage)
                ReplacementNodeIDs[previousID] = node.ID;
            if (GetNode(node.ID) != null)
                throw new Exception("A node with this ID was already added.");
            await InMemoryStorage.AddNode(FakeTreeID, node, null);
            long? parentOfPreviousVersionOfThisNode = Parents[previousID];
            long? currentVersionOfThatParent = parentOfPreviousVersionOfThisNode == null ? null : (long?)GetUltimateReplacementID((long)parentOfPreviousVersionOfThisNode);
            // Note that when adding a bunch of nodes at once, the last of these will be the highest, but the current version of its parent may still be a node with a lower ID, because we haven't replaced the ancestor yet
            lock (InMemoryStorage)
            {
                Parents.Add(node.ID, currentVersionOfThatParent); // look to find latest version of the parent}
                if (setChildParentsToThisNode)
                    if (node is CountedInternalNode<TKey>)
                    {
                        CountedInternalNode<TKey> cn = (CountedInternalNode<TKey>)node;
                        foreach (var child in cn.ChildNodeInfos.Select(x => x.NodeID))
                            Parents[child] = node.ID;
                    }
            }
        }

        public async Task<List<NodeToDelete>> PrepareReplacedNodesForDeletion(long originalRootID, IBlob<Guid> temporaryUintSetStorage)
        {
            List<NodeToDelete> toDelete = new List<NodeToDelete>();
            List<long> allNodes = null;
            lock (InMemoryStorage)
                allNodes = InMemoryStorage.AsEnumerable<TKey>().Select(x => x.ID).ToList();
            foreach (long id in allNodes)
            {
                var ultimateReplacementID = GetUltimateReplacementID(id);
                bool isBeingReplaced = ultimateReplacementID != id;
                bool preexisting = id <= originalRootID;
                if (isBeingReplaced) 
                {
                    if (preexisting) // This is a preexisting node that now needs to be marked for deletion, because it's being replaced. It may be that it's being inherited, but then we'll note that we're deferring deletion when we add the later version to permanent storage.
                    {
                        if (EmptyNodes.Contains(id))
                            EmptyNodes.Remove(id); // we don't need to keep track of this anymore, but we don't need to delete it either, because it was never created.
                        else
                            toDelete.Add(new NodeToDelete(id, id == originalRootID));
                    }
                    else
                    {
                        // this is a temporary node that we no longer need any more. We don't want the successor to this node to inherit from it.
                        // We don't need to include this in nodesToBeDeleted, because it's not in permanent storage.
                        var ultimateReplacement = await InMemoryStorage.GetNode<TKey>(FakeTreeID, ultimateReplacementID);
                        await ultimateReplacement.SetUintSetStorage(temporaryUintSetStorage); // we need temporaryUintSetStorage since the node we're inheriting from may be there. 
                        await ultimateReplacement.MakeNotInherited(id);
                    }
                }
                if (isBeingReplaced || preexisting)
                    await InMemoryStorage.DeleteNode<TKey>(FakeTreeID, id, null); // if it's merely preexisting, we already have it in the main node storage, so we should get rid of it from temporary node storage.
            }
            return toDelete;
        }

        public long GetUltimateReplacementID(long id)
        {
            if (!ReplacementNodeIDs.ContainsKey(id))
                return id;
            return GetUltimateReplacementID(ReplacementNodeIDs[id]);
        }

        public int Count()
        {
            lock (InMemoryStorage)
            {
                return InMemoryStorage.AsEnumerable<TKey>().Count(); // can't enumerate directly because of lock
            }
        }

        public List<CountedNode<TKey>> ToList()
        {
            lock (InMemoryStorage)
            {
                return InMemoryStorage.AsEnumerable<TKey>().ToList(); // can't enumerate directly because of lock
            }
        }

        internal CountedNode<TKey> GetNodeOrReplacement(long id)
        {
            return GetNode(GetUltimateReplacementID(id));
        }

        public IEnumerable<CountedNode<TKey>> GetNodesOrReplacements(IEnumerable<CountedNode<TKey>> nodes)
        {
            lock (InMemoryStorage)
            {
                foreach (long nodeID in nodes.Select(x => GetUltimateReplacementID(x.ID)))
                    yield return GetNode(nodeID);
            }
        }

        #endregion
    }
}
