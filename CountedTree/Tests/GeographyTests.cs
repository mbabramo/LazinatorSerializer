using System;
using System.Linq;
using System.Threading.Tasks;
using R8RUtilities;
using Xunit;
using System.Collections.Generic;
using CountedTree.Core;
using CountedTree.NodeResults;
using CountedTree.PendingChanges;
using CountedTree.Updating;
using FluentAssertions;
using CountedTree.QueryExecution;
using CountedTree.UintSets;
using Utility;
using Lazinator.Wrappers;

namespace CountedTree.Tests
{
    public partial class CountedTreeTests
    {
        
        [Fact]
        public void MortonEncodingWorks()
        {
            // check using arbitrary floats
            MortonEncodingWorksHelper(33.7241242342123F, 44.2423342342341F); 
            MortonEncodingWorksHelper(-1.234F, 84.3241F);
            MortonEncodingWorksHelper(56.4213F, -120F);
            MortonEncodingWorksHelper(-32.2341F, -178.2341F);
            // check extreme values and negative values
            MortonEncodingWorksHelper(-90F, -10F); 
            MortonEncodingWorksHelper(90F, 10F);
            MortonEncodingWorksHelper(-50F, -180F);
            MortonEncodingWorksHelper(-50F, 180F);
            MortonEncodingWorksHelper(-90F, 180F);
        }

        private static void MortonEncodingWorksHelper(float lat, float lon)
        {
            float lat2, lon2;
            UInt64 m = MortonEncoding.latlon2morton(lat, lon);
            MortonEncoding.morton2latlon(m, out lat2, out lon2);
            lat2.Should().BeApproximately(lat, 0.00001F);
            lon2.Should().BeApproximately(lon, 0.00001F);
        }

        [Fact]
        public void GetFromCenterAndRadiusWorks()
        {
            float lat = 23F, lon = -30F, radius = 20;
            var rect = BoundingBox.GetBoundingBox(new LatLonPoint(lat, lon), radius);
            rect.UpperLeft.ApproximateMilesTo(rect.UpperRight).Should().BeApproximately(radius * 2, 0.2F);
            rect.UpperLeft.ApproximateMilesTo(rect.LowerLeft).Should().BeApproximately(radius * 2, 0.2F);
            rect.UpperLeft.ApproximateMilesTo(new LatLonPoint(rect.Top, lon)).Should().BeApproximately(radius, 0.1F);
            rect.UpperLeft.ApproximateMilesTo(new LatLonPoint(lat, rect.Left)).Should().BeApproximately(radius, 0.1F);
        }

        [Fact]
        public void ProperMortonRangesProduceDistinctDescendants()
        {
            ProperMortonRangesProduceDistinctDescendants_Helper(new ProperMortonRange(0, 0), 0);
        }

        internal void ProperMortonRangesProduceDistinctDescendants_Helper(ProperMortonRange startRange, int recursionLevel)
        {
            var descendants64 = startRange.GetDescendants(2).ToList();
            descendants64.Distinct().Count().Should().Be(descendants64.Count());
            int numToFurtherExplore = 0;
            if (recursionLevel <= 1)
                numToFurtherExplore = 64;
            else if (recursionLevel <= 31)
                numToFurtherExplore = 1;
            foreach (var descendant in descendants64.Take(numToFurtherExplore))
                ProperMortonRangesProduceDistinctDescendants_Helper(descendant, recursionLevel + 1);
        }

        [Fact]
        public void ProperMortonRangesWork()
        {
            ProperMortonRange full = new ProperMortonRange(0, 0);
            List<ProperMortonRange> expectedChildren = new List<ProperMortonRange>()
            {
                new ProperMortonRange(ConvertBinaryToUlong("0000000000000000 0000000000000000 0000000000000000 0000000000000000"), 1),
                new ProperMortonRange(ConvertBinaryToUlong("0100000000000000 0000000000000000 0000000000000000 0000000000000000"), 1),
                new ProperMortonRange(ConvertBinaryToUlong("1000000000000000 0000000000000000 0000000000000000 0000000000000000"), 1),
                new ProperMortonRange(ConvertBinaryToUlong("1100000000000000 0000000000000000 0000000000000000 0000000000000000"), 1),
            };
            List<ProperMortonRange> expectedGrandchildren = new List<ProperMortonRange>()
            {
                new ProperMortonRange(ConvertBinaryToUlong("0000000000000000 0000000000000000 0000000000000000 0000000000000000"), 2),
                new ProperMortonRange(ConvertBinaryToUlong("0001000000000000 0000000000000000 0000000000000000 0000000000000000"), 2),
                new ProperMortonRange(ConvertBinaryToUlong("0010000000000000 0000000000000000 0000000000000000 0000000000000000"), 2),
                new ProperMortonRange(ConvertBinaryToUlong("0011000000000000 0000000000000000 0000000000000000 0000000000000000"), 2),
                new ProperMortonRange(ConvertBinaryToUlong("0100000000000000 0000000000000000 0000000000000000 0000000000000000"), 2),
                new ProperMortonRange(ConvertBinaryToUlong("0101000000000000 0000000000000000 0000000000000000 0000000000000000"), 2),
                new ProperMortonRange(ConvertBinaryToUlong("0110000000000000 0000000000000000 0000000000000000 0000000000000000"), 2),
                new ProperMortonRange(ConvertBinaryToUlong("0111000000000000 0000000000000000 0000000000000000 0000000000000000"), 2),
                new ProperMortonRange(ConvertBinaryToUlong("1000000000000000 0000000000000000 0000000000000000 0000000000000000"), 2),
                new ProperMortonRange(ConvertBinaryToUlong("1001000000000000 0000000000000000 0000000000000000 0000000000000000"), 2),
                new ProperMortonRange(ConvertBinaryToUlong("1010000000000000 0000000000000000 0000000000000000 0000000000000000"), 2),
                new ProperMortonRange(ConvertBinaryToUlong("1011000000000000 0000000000000000 0000000000000000 0000000000000000"), 2),
                new ProperMortonRange(ConvertBinaryToUlong("1100000000000000 0000000000000000 0000000000000000 0000000000000000"), 2),
                new ProperMortonRange(ConvertBinaryToUlong("1101000000000000 0000000000000000 0000000000000000 0000000000000000"), 2),
                new ProperMortonRange(ConvertBinaryToUlong("1110000000000000 0000000000000000 0000000000000000 0000000000000000"), 2),
                new ProperMortonRange(ConvertBinaryToUlong("1111000000000000 0000000000000000 0000000000000000 0000000000000000"), 2),
            };
            full.GetChildren().Should().Equal(expectedChildren);
            full.GetDescendants(1).Should().Equal(expectedGrandchildren);
            full.GetDescendants(3).Distinct().Count().Should().Be(1 * 4 * 4 * 4 * 4); // we are skipping three generations and then getting the next generation, so it's 4^4 (256)
        }

        private static ulong ConvertBinaryToUlong(string binaryString)
        {
            return (ulong)(Convert.ToInt64(RemoveWhitespace(binaryString), 2));
        }

        private static string RemoveWhitespace(string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }

        [Fact]
        public void PrimeMeridianCutoffWorks()
        {
            float lon = -179.9F, lat = -30F, miles = 200;
            LatLonRectangle rect = BoundingBox.GetBoundingBox(new LatLonPoint(lat, lon), miles); 
            rect.CutOffAtPrimeMeridian.Should().BeTrue();
        }


        [Fact]
        public void PrimeMeridianDistanceWorks()
        {
            LatLonPoint p1 = new LatLonPoint(45F, -179.999F);
            LatLonPoint p2 = new LatLonPoint(45F, 179.999F);
            p1.ApproximateMilesTo(p2).Should().BeLessThan(10F);
        }

        [Fact]
        public void LatLonRectangleClosestPointWorks()
        {
            float top = 20F;
            float bottom = 0F;
            float left = 10F;
            float right = 30F;
            LatLonRectangle r = new LatLonRectangle(new LatLonPoint(top, left), new LatLonPoint(bottom, right));

            r.ClosestPointInOrOnRectangle(new LatLonPoint(top + 1, left - 1)).Equals(new LatLonPoint(top, left)).Should().BeTrue();
            r.ClosestPointInOrOnRectangle(new LatLonPoint(top - 1, left - 1)).Equals(new LatLonPoint(top - 1, left)).Should().BeTrue();
            r.ClosestPointInOrOnRectangle(new LatLonPoint(bottom - 1, left - 1)).Equals(new LatLonPoint(bottom, left)).Should().BeTrue();
            r.ClosestPointInOrOnRectangle(new LatLonPoint(top + 1, left + 1)).Equals(new LatLonPoint(top, left + 1)).Should().BeTrue();
            r.ClosestPointInOrOnRectangle(new LatLonPoint(top - 1, left + 1)).Equals(new LatLonPoint(top - 1, left + 1)).Should().BeTrue();
            r.ClosestPointInOrOnRectangle(new LatLonPoint(bottom - 1, left + 1)).Equals(new LatLonPoint(bottom, left + 1)).Should().BeTrue();
            r.ClosestPointInOrOnRectangle(new LatLonPoint(top + 1, right + 1)).Equals(new LatLonPoint(top, right)).Should().BeTrue();
            r.ClosestPointInOrOnRectangle(new LatLonPoint(top - 1, right + 1)).Equals(new LatLonPoint(top - 1, right)).Should().BeTrue();
            r.ClosestPointInOrOnRectangle(new LatLonPoint(bottom - 1, right + 1)).Equals(new LatLonPoint(bottom, right)).Should().BeTrue();
        }


        private async Task<TreeInfo> SetupGeoTree(TreeStructure ts, TreeUpdateSettings tu)
        {
            Guid treeID = RandomGenerator.GetGuid();
            var geoTree = await TreeFactory.CreateGeoTree(treeID, tu, ts);
            return geoTree;
        }

        private ulong GetRandomLocation()
        {
            if (RandomGenerator.GetRandomBool())
                return (ulong) RandomGenerator.GetRandomLong();
            // lat, lon, miles away
            List<Tuple<float, float>> somePlaces = new List<Tuple<float, float>>
            {
                new Tuple<float, float>(41.87440F, -87.63940F), // chicago
                new Tuple<float, float>(39.10190F, -84.50970F), // cincinnati
                new Tuple<float, float>(55.71670F, 12.45F), // copenhagen
                new Tuple<float, float>(-36.76670F, -73.0500F), // concepcion
                new Tuple<float, float>(-12.18330F, 96.83330F), // cocos island
                new Tuple<float, float>(-79.9F, 50.0F), // near south pole
                new Tuple<float, float>(79.9F, 50.0F), // near north pole
                new Tuple<float, float>(25.5F, -179.9F), // near prime meridian
                new Tuple<float, float>(25.5F, 179.9F), // near prime meridian
            };
            Tuple<float, float> randomPlace = RandomGenerator.PickRandom(somePlaces);
            float miles = 50;
            var randomPlacePoint = new LatLonPoint(randomPlace.Item1, randomPlace.Item2);
            LatLonRectangle rect = BoundingBox.GetBoundingBox(randomPlacePoint, miles);
            while (true)
            {
                LatLonPoint p = new LatLonPoint(RandomGenerator.GetRandom(rect.Bottom, rect.Top), RandomGenerator.GetRandom(rect.Left, rect.Right));
                if (p.ApproximateMilesTo(randomPlacePoint) < miles)
                    return MortonEncoding.latlonpoint2morton(p);
            }
        }


        public static async Task<List<GeoResult>> DoQueryGeo(TreeInfo treeInfo, uint? take, ulong mortonCenterPoint, float miles)
        {
            DateTime asOfTime = DateTime.Now;
            TreeQueryExecutorGeo coordinator = new TreeQueryExecutorGeo(treeInfo, asOfTime, take, QueryResultType.KeysIDsAndDistance, mortonCenterPoint, miles, null);
            StorageFactory.GetDateTimeProvider().SleepOrSkipTime(1);
            var results = (await coordinator.GetGeoResults()).Select(x => (GeoResult)x).ToList();
            return results;
        }


        public static async Task<UintSet> LoadGeoFilter(TreeInfo treeInfo, ulong mortonCenterPoint, float miles)
        {
            DateTime asOfTime = DateTime.Now;
            TreeQueryExecutorGeo coordinator = new TreeQueryExecutorGeo(treeInfo, asOfTime, null, QueryResultType.IDsAsBitSet, mortonCenterPoint, miles, null);
            StorageFactory.GetDateTimeProvider().SleepOrSkipTime(1);
            var results = await coordinator.GetAllResultsAsUintSet();
            return results;
        }


        [Fact]
        public async Task CanQueryEmptyGeoTree()
        {
            await CanQueryGeoTree_Helper(0, false);
        }

        [Fact]
        public async Task CanQueryGeoTree()
        {
            await CanQueryGeoTree_Helper(5000, false);
        }

        [Fact]
        public async Task CanQueryGeoTree_AllSameLocation()
        {
            await CanQueryGeoTree_Helper(5000, true);
        }

        internal async Task CanQueryGeoTree_Helper(int numToAdd, bool allSameLocation)
        {
            int maxItemsPerLeaf = 200;
            int numChildrenPerInternalNode = 64; // must be power of 4
            TreeStructure ts = new TreeStructure(true, numChildrenPerInternalNode, maxItemsPerLeaf, false, false);
            TreeUpdateSettings tus = new TreeUpdateSettings(10, 20, numToAdd + 1, TimeSpan.FromMinutes(30));
            var treeInfo = await SetupGeoTree(ts, tus);
            ulong randomLocation = GetRandomLocation();
            ITreeHistoryManager<WUInt64> thm = await StorageFactory.GetTreeHistoryManagerAccess().Get<WUInt64>(treeInfo.TreeID);
            PendingChange<WUInt64>[] additions =
                Enumerable.Range(0, numToAdd)
                .Select(x =>
                    new PendingChange<WUInt64>(
                        new KeyAndID<WUInt64>(
                            allSameLocation ? randomLocation : GetRandomLocation(),
                            (uint)x),
                        false)
                    )
                .ToArray();
            var items = additions.Select(x => x.Item).OrderBy(x => x).ToList();
            var itemsDescending = items.OrderByDescending(x => x).ToList();
            PendingChangesAtTime<WUInt64> pc = new PendingChangesAtTime<WUInt64>(StorageFactory.GetDateTimeProvider().Now, new PendingChangesCollection<WUInt64>(additions, true));
            treeInfo = await thm.AddPendingChanges(pc, new Guid(), 1);
            treeInfo = await thm.DoWorkRepeatedly(10);
            StorageFactory.GetDateTimeProvider().SleepOrSkipTime(1);

            const bool intensiveRandomizedTesting = false;
            const int numberQueriesToTest = (intensiveRandomizedTesting ? 100000 : 5);
            for (int i = 0; i < numberQueriesToTest; i++)
            {
                float miles = RandomGenerator.PickRandom(new float[] { 10F, 100F, 1000F, 15000F });
                ulong ranLoc = GetRandomLocation();
                var latLonPoint = MortonEncoding.morton2latlonpoint(ranLoc);
                var boundingBox = BoundingBox.GetBoundingBox(latLonPoint, miles);
                List<GeoResult> expected = GetExpectedResults(additions, miles, latLonPoint);
                await ConfirmGeoQuery(treeInfo, miles, ranLoc, expected);
                await ConfirmGeoFilter(treeInfo, miles, ranLoc, expected);
            }
        }

        private static async Task ConfirmGeoQuery(TreeInfo treeInfo, float miles, ulong ranLoc, List<GeoResult> expected)
        {
            var results = await DoQueryGeo(treeInfo, null, ranLoc, miles);
            for (int j = 0; j < results.Count(); j++)
                if (!results[j].Equals(expected[j]))
                    throw new Exception($"Failed match at {j}: {results[j]} {expected[j]}");
            results.Count().Should().Be(expected.Count());
        }

        private static async Task ConfirmGeoFilter(TreeInfo treeInfo, float miles, ulong ranLoc, List<GeoResult> expected)
        {
            var results = (await LoadGeoFilter(treeInfo, ranLoc, miles)).AsEnumerable().ToList();
            var expectedIDs = expected.Select(x => x.KeyAndID.ID).OrderBy(x => x).ToList();
            for (int j = 0; j < results.Count(); j++)
                if (!results[j].Equals(expectedIDs[j]))
                    throw new Exception($"Failed match at {j}: {results[j]} {expectedIDs[j]}");
            results.Count().Should().Be(expectedIDs.Count());
        }

        private static List<GeoResult> GetExpectedResults(PendingChange<WUInt64>[] additions, float miles, LatLonPoint latLonPoint)
        {
            return additions
                .Select(x => new { Point = MortonEncoding.morton2latlonpoint(x.Item.Key), KeyAndID = x.Item })
                .Select(x => new { Point = x.Point, Distance = x.Point.ApproximateMilesTo(latLonPoint), KeyAndID = x.KeyAndID })
                .Where(x => x.Distance <= miles)
                .OrderBy(x => x.Distance)
                .ThenBy(x => x.KeyAndID.ID)
                .Select(x => new GeoResult(x.KeyAndID, x.Distance))
                .ToList();
        }

    }
}
