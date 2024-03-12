using CountedTree.Core;
using CountedTree.NodeResults;
using CountedTree.Queries;
using Lazinator.Wrappers;
using R8RUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace CountedTree.Node
{
    public partial class CountedLeafNodeGeo : CountedLeafNode<WUInt64>, ICountedLeafNodeGeo
    {
        public CountedLeafNodeGeo(List<KeyAndID<WUInt64>> items, long id, byte depth, TreeStructure treeStructure, KeyAndID<WUInt64>? firstExclusive, KeyAndID<WUInt64>? lastInclusive) : base(items, id, depth, treeStructure, firstExclusive, lastInclusive)
        {
            if (TreeStructure.NumChildrenPerInternalNode != 4 && TreeStructure.NumChildrenPerInternalNode != 16 && TreeStructure.NumChildrenPerInternalNode != 64 && TreeStructure.NumChildrenPerInternalNode != 256)
                throw new Exception("Invalid number of children for a geo node"); // must be a power of 4
        }

        public CountedLeafNodeGeo(CountedLeafNode<WUInt64> node) : base(node.Items.ToList(), node.ID, node.Depth, node.TreeStructure, node.NodeInfo.FirstExclusive, node.NodeInfo.LastInclusive)
        {

        }

        public override Task<NodeResultBase<WUInt64>> ProcessQuery(NodeQueryBase<WUInt64> request)
        {
            return Task.FromResult((NodeResultBase<WUInt64>)ProcessGeoQuery(request as NodeQueryGeo));
        }

        // The basic flow of the code here is more or less the same as CountedLeafNode's query processing pipeline. We might be able to abstract common elements better, but there are some minor differences that make that hard (such as the absence of Skip in the geo query and the need to sort results by distance here). At least for now, we'll just live with the violation of the DRY principle.

        public NodeResultGeoBase ProcessGeoQuery(NodeQueryGeo request)
        {

            uint filteredMatches, supersetMatches;
            List<RankKeyAndID<WUInt64>> matches = MatchingIntegratedGeoItems(request, out filteredMatches, out supersetMatches);
            NodeResultGeoItems result = request.GetResultFromMatches(matches, filteredMatches, supersetMatches) as NodeResultGeoItems;
            return result as NodeResultGeoBase;
        }

        private List<RankKeyAndID<WUInt64>> MatchingIntegratedGeoItems(NodeQueryGeo request, out uint filteredMatches, out uint supersetMatches)
        {
            List<RankKeyAndID<WUInt64>> matches = new List<RankKeyAndID<WUInt64>>();
            // we could try to do something like CountedLeafNodeMatcher here, but it wouldn't do as much good, because we have to look at all the results anyway.
            List <KeyAndID<WUInt64>> integratedItems = request.PendingChanges.Integrate(Items, true, request.ItemIsPotentialMatch).ToList();
            AddToMatches(request, matches, integratedItems, out filteredMatches, out supersetMatches);
            var sorted = matches.OrderBy(x => MortonEncoding.morton2latlonpoint(x.Key).ApproximateMilesTo(request.QueryLatLonCenter)).ToList();
            return sorted;
        }

        private static void AddToMatches(NodeQueryGeo request, List<RankKeyAndID<WUInt64>> matches, List<KeyAndID<WUInt64>> allItems, out uint filteredMatches, out uint supersetMatches)
        {
            filteredMatches = 0;
            supersetMatches = 0;
            int indexNum = 0;
            foreach (var item in allItems)
            {
                bool supersetMatch = request.ItemMatches(item, (uint)indexNum, false);
                bool filteredMatch = supersetMatch && request.ItemMatches(item, (uint)indexNum, true);
                if (filteredMatch)
                {
                    matches.Add(new RankKeyAndID<WUInt64>(filteredMatches, item.Key, item.ID));
                    filteredMatches++;
                }
                if (supersetMatch)
                    supersetMatches++;
                if (filteredMatches == request.Take)
                    break;
                indexNum++;
            }
        }

        internal override bool TypeDividesEvenly()
        {
            return true;
        }

        internal override bool DivideEvenly(KeyAndID<WUInt64> firstExclusive, KeyAndID<WUInt64> lastInclusive, List<KeyAndID<WUInt64>> itemsWithPendingChanges, byte treeDepth)
        {
            if (MortonEncoding.GetMortonDepth(treeDepth, TreeStructure.NumChildrenPerInternalNode) + MortonEncoding.MortonTreeGenerationsToSkip(TreeStructure.NumChildrenPerInternalNode) + 1 > 31)
                return false;
            return base.DivideEvenly(firstExclusive, lastInclusive, itemsWithPendingChanges, treeDepth);
        }



        internal override IEnumerable<WUInt64> GetEvenlyDividedSplits(KeyAndID<WUInt64> firstExclusive, KeyAndID<WUInt64> lastInclusive, byte treeDepth, int numChildrenPerInternalNode)
        {
            ProperMortonRange mr = MortonEncoding.GetProperMortonRangeFollowingValue(firstExclusive.Key, treeDepth, TreeStructure.NumChildrenPerInternalNode); // this is the depth that we are starting at

            // We want to return the last value in each range except the last. So, four 4 items, we'd have the 25%, 50%, and 7% values as the splits.
            var descendants = mr.GetDescendants(MortonEncoding.MortonTreeGenerationsToSkip(TreeStructure.NumChildrenPerInternalNode));
            //Debug.WriteLine("");
            //Debug.WriteLine(mr);
            //foreach (var d in descendants)
            //    Debug.WriteLine(d);
            var endValues = descendants.Select(x => (WUInt64) x.EndValue()).ToList().ExceptLast();
            return endValues;
        }


        internal override CountedLeafNode<WUInt64> GetCountedLeafNode(long id, List<KeyAndID<WUInt64>> items, KeyAndID<WUInt64>? firstExclusive, KeyAndID<WUInt64>? lastInclusive, byte depth)
        {
            return new CountedLeafNodeGeo(items, id, depth, TreeStructure, firstExclusive, lastInclusive);
        }

        internal override CountedInternalNode<WUInt64> GetInPlaceReplacement_NoUintSetLoc(long newInternalNodeID, IEnumerable<CountedNode<WUInt64>> childrenNodes, byte depth)
        {
            var n = base.GetInPlaceReplacement_NoUintSetLoc(newInternalNodeID, childrenNodes, depth);
            return new CountedInternalNodeGeo(n);
        }

        internal async override Task<CountedInternalNode<WUInt64>> GetInPlaceReplacement_WithUintSetLoc(long nextIDToUseForChild, List<CountedNode<WUInt64>> childNodes, List<KeyAndID<WUInt64>> itemsToIncludeInUintSet, byte depth)
        {
            var n = await base.GetInPlaceReplacement_WithUintSetLoc(nextIDToUseForChild, childNodes, itemsToIncludeInUintSet, depth);
            return new CountedInternalNodeGeo(n);
        }
    }
}
