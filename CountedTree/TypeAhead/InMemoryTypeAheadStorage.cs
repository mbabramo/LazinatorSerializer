using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CountedTree.TypeAhead
{
    /// <summary>
    /// This maintains type-ahead nodes in memory and responds to type ahead requests.
    /// </summary>
    /// <typeparam name="R"></typeparam>
    public class InMemoryTypeAheadStorage<R> : ITypeAheadStorage<R> where R : ILazinator, IComparable, IComparable<R>, IEquatable<R>
    {
        Dictionary<Guid, TypeAheadNode<R>> Nodes = new Dictionary<Guid, TypeAheadNode<R>>();

        public int MinThreshold { get; set; } = 10;
        public int MaxThreshold { get; set; } = 15;

        private TypeAheadNode<R> GetNode(Guid nodeID)
        {
            if (Nodes.ContainsKey(nodeID))
                return Nodes[nodeID];
            return null;
        }

        private void AddNode(TypeAheadNode<R> node, Guid nodeID)
        {
            Nodes[nodeID] = node;
        }

        public Task<bool> AddTypeAheadItem(string stringToHere, Guid nodeID, char? nextChar, TypeAheadItem<R> typeAheadItem)
        {
            var node = GetNode(nodeID);
            if (node == null)
            {
                node = new TypeAheadNode<R>(stringToHere, MinThreshold, MaxThreshold);
                AddNode(node, nodeID);
            }
            return Task.FromResult(node.AddResult(nextChar, typeAheadItem));
        }

        public Task AddTypeAheadItemsFromChildren(Guid nodeID, List<TypeAheadItem<R>> typeAheadItems)
        {
            var node = GetNode(nodeID);
            node.AddResultsFromChildren(typeAheadItems);
            return Task.CompletedTask;
        }
        
        public Task<List<R>> GetSearchResults(Guid nodeID, int numToTake)
        {
            var node = GetNode(nodeID);
            if (node == null)
                return Task.FromResult(new List<R>());
            return Task.FromResult(node.GetResults(numToTake));
        }

        public Task<List<TypeAheadItem<R>>> GetTypeAheadItems(Guid nodeID, int numToTake, bool prioritizeExactMatches = false)
        {
            var node = GetNode(nodeID);
            if (node == null)
                return Task.FromResult(new List<TypeAheadItem<R>>());
            return Task.FromResult(node.GetTypeAheadItems(numToTake, prioritizeExactMatches));
        }

        public Task<MoreResultsRequest<R>> RemoveResult(Guid nodeID, R result)
        {
            var node = GetNode(nodeID);
            if (node == null)
                return null;
            //if (node.StringToHere == "B")
            //{
            //    System.Diagnostics.Debug.WriteLine("");
            //    System.Diagnostics.Debug.WriteLine("After removing " + result);
            //    foreach (var item in node.TypeAheadItems)
            //        System.Diagnostics.Debug.WriteLine(item);
            //    foreach (var exactMatch in node.ExactMatches)
            //        System.Diagnostics.Debug.WriteLine("Exact match: " + exactMatch);
            //    System.Diagnostics.Debug.WriteLine("");
            //}
            return Task.FromResult(node.RemoveResult(result));
        }


        public Task<TypeAheadItemsForParent<R>> GetItemsForParent(Guid nodeID, string searchString, TypeAheadItem<R> lowestItemOnParent, int maxNumResults)
        {
            var node = GetNode(nodeID);
            if (node == null)
                return Task.FromResult<TypeAheadItemsForParent<R>>(null);
            return Task.FromResult(node.GetItemsForParent(lowestItemOnParent, searchString, maxNumResults));
        }
    }
}
