using System.Linq;
using R8RUtilities;
using System.Collections.Generic;
using CountedTree.Core;
using CountedTree.Node;
using CountedTree.PendingChanges;
using Xunit;
using CountedTree.NodeStorage;
using Utility;
using Lazinator.Wrappers;

namespace CountedTree.Tests
{
    [Collection("CountedTreeTests")]
    public partial class CountedTreeTests
    {

        public CountedTreeTests()
        {
            StorageFactory.Reset();
        }

        public static IDateTimeProvider GetDateTimeProvider()
        {
            return StorageFactory.GetDateTimeProvider();
        }

        public static INodeStorage GetNodeStorage()
        {
            return StorageFactory.GetNodeStorage();
        }

        #region Node construction

        public enum AnomalyType
        {
            NoAnomaly,
            RedundantDeletion,
            RedundantInsertion
        }

        public static List<KeyAndID<WFloat>> GetSomeItems(int numItems, int startIndex = 0)
        {
            return Enumerable.Range(startIndex, numItems).Select(x => new KeyAndID<WFloat>((float)RandomGenerator.GetRandom(0, 100.0), (uint)x)).OrderBy(x => x).ToList();
        }

        public static List<KeyAndID<WFloat>> GetSomeItems(int numItems, List<KeyAndID<WFloat>> sourceList, int numToDeleteFromSource, int numToChangeInSource)
        {
            int numToNotDeleteInSource = sourceList.Count() - numToDeleteFromSource;
            var keep = RandomGenerator.PickRandomItems(sourceList, numToNotDeleteInSource).Select(x => x.Clone()).ToList();
            var itemsToChange = RandomGenerator.PickRandomInts(0, keep.Count(), numToChangeInSource);
            var change = RandomGenerator.PickRandomItems(keep, numToChangeInSource);
            foreach (var itemToChange in itemsToChange)
                keep[itemToChange] = keep[itemToChange].WithKey((float)RandomGenerator.GetRandom(0, 100.0));
            int numToAddAtEnd = numItems - numToNotDeleteInSource;
            keep.AddRange(GetSomeItems(numToAddAtEnd, (int)sourceList.Select(x => x.ID).Max() + 1));
            return keep.OrderBy(x => x).ToList();
        }

        public static PendingChangesCollection<WFloat> GetTransformativePendingChanges(List<KeyAndID<WFloat>> originalList, List<KeyAndID<WFloat>> replacementList, AnomalyType anomalyType = AnomalyType.NoAnomaly)
        {
            List<PendingChange<WFloat>> p = new List<PendingChange<WFloat>>();
            foreach (var item in originalList.Where(o => !replacementList.Any(r => r.Equals(o))))
                p.Add(new PendingChange<WFloat>(item, true));
            foreach (var item in replacementList.Where(r => !originalList.Any(o => r.Equals(o))))
                p.Add(new PendingChange<WFloat>(item, false));
            // Now add some offsetting changes.
            var itemToDeleteAndAddBack = RandomGenerator.PickRandom(replacementList);
            p.Add(new PendingChange<WFloat>(itemToDeleteAndAddBack.Clone(), true));
            p.Add(new PendingChange<WFloat>(itemToDeleteAndAddBack.Clone(), false));
            if (anomalyType == AnomalyType.RedundantDeletion)
                p.Add(new PendingChange<WFloat>(new KeyAndID<WFloat>(RandomGenerator.GetRandom(0, 100.0F), 200), true)); // deleting something that doesn't exist -- note that we use an ID that will be in our filter.
            else if (anomalyType == AnomalyType.RedundantInsertion)
            {
                var itemInOriginalAndReplacementList = originalList.First(x => x.ID % 10 == 0 && replacementList.Any(y => x.Equals(y))); // again, we'll do a redundant insertion of something that would be within the filter
                p.Add(new PendingChange<WFloat>(itemInOriginalAndReplacementList.Clone(), false)); // adding something that's already there
            }
            return new PendingChangesCollection<WFloat>(p, true);
        }

        internal static void GetOriginalAndReplacementItems(int numOriginalItems, int numToAddToOriginal, int numToDeleteFromOriginal, int numToChangeInOriginal, out List<KeyAndID<WFloat>> original, out List<KeyAndID<WFloat>> replacement)
        {
            original = GetSomeItems(numOriginalItems);
            replacement = GetSomeItems(numOriginalItems + numToAddToOriginal - numToDeleteFromOriginal, original, numToDeleteFromOriginal, numToChangeInOriginal);
        }

        internal static void GetOriginalAndReplacementItemsAndTransformativeChanges(int numOriginalItems, int numToAddToOriginal, int numToDeleteFromOriginal, int numToChangeInOriginal, out List<KeyAndID<WFloat>> original, out List<KeyAndID<WFloat>> replacement, out PendingChangesCollection<WFloat> changes)
        {
            GetOriginalAndReplacementItems(numOriginalItems, numToAddToOriginal, numToDeleteFromOriginal, numToChangeInOriginal, out original, out replacement);
            changes = GetTransformativePendingChanges(original, replacement);
        }


        internal static void GetOriginalAndReplacementItemsAndTransformativeChangesWithAnomaly(int numOriginalItems, int numToAddToOriginal, int numToDeleteFromOriginal, int numToChangeInOriginal, AnomalyType anomalyType, out List<KeyAndID<WFloat>> original, out List<KeyAndID<WFloat>> replacement, out PendingChangesCollection<WFloat> changes)
        {
            GetOriginalAndReplacementItems(numOriginalItems, numToAddToOriginal, numToDeleteFromOriginal, numToChangeInOriginal, out original, out replacement);
            changes = GetTransformativePendingChanges(original, replacement, anomalyType);
        }

        public static string ToListString<T>(IEnumerable<T> list)
        {
            return string.Join(", ", list.ToArray());
        }

        private static NodeInfo<WFloat> GetNodeInfo(uint nodeID, int numNodes, uint numSubtreeValues, KeyAndID<WFloat>? min, KeyAndID<WFloat>? max)
        {
            return new NodeInfo<WFloat>(nodeID, numNodes, numSubtreeValues, 0, 0, 0, 1, numSubtreeValues > 0, min, max);
        }





        #endregion

    }
}
