using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using R8RUtilities;
using CountedTree.UintSets;
using CountedTree.Core;
using Utility;
using Lazinator.Wrappers;

namespace CountedTree.Tests
{
    [Collection("CountedTreeTests")]
    public class UintSetTests
    {
        #region UintSet setup

        [Fact]
        public void BasicUintSetWorks()
        {
            const int maxNumItems = 1000;
            const int numChanges = 10000;
            UintSet u = new UintSet();
            bool[] a = new bool[maxNumItems];
            for (int i = 0; i < numChanges; i++)
            {
                uint j = (uint)RandomGenerator.GetRandom(0, maxNumItems - 1);
                bool b = RandomGenerator.GetRandomBool();
                if (b)
                    u.AddUint(j);
                else
                    u.RemoveUint(j);
                a[j] = b;
            }
            for (int i = 0; i < maxNumItems; i++)
                u.Contains((uint)i).Should().Be(a[i]);
            UintSet.GetCardinality(u.AsBitArray()).Should().Be(a.Where(x => x == true).Count());
        }

        [Fact]
        public void UintSetFromStringWorks()
        {
            string b = "0010010";
            UintSet u = new UintSet(b);
            for (int i = 0; i < b.Length; i++)
                u.Contains((uint)i).Should().Be(b[i] == '1');
        }

        [Fact]
        public void IndexWithinPresentItemsWorks()
        {
            string b = "00001010100";
            UintSet u = new UintSet(b);
            u.GetIndexWithinPresentItems(4).Should().Be(0);
            u.GetIndexWithinPresentItems(6).Should().Be(1);
            u.GetIndexWithinPresentItems(8).Should().Be(2);
        }

        #endregion

        #region UintSetLoc tests
        
        [Fact]
        public void UintSetLoc_ConstructFromItemsWorks()
        {
            List<KeyAndID<WFloat>?> splitValues = new List<KeyAndID<WFloat>?>()
            {
                new KeyAndID<WFloat>(1.5F, 999),
                new KeyAndID<WFloat>(3.0F, 999),
                new KeyAndID<WFloat>(4.5F, 999),
                new KeyAndID<WFloat>(6.0F, 999),
                new KeyAndID<WFloat>(7.5F, 999),
            };

            List<KeyAndID<WFloat>> items = new List<KeyAndID<WFloat>>()
            {
                new KeyAndID<WFloat>(1.1F, 0),
                new KeyAndID<WFloat>(1.5F, 2),
                new KeyAndID<WFloat>(4.7F, 5),
                new KeyAndID<WFloat>(7.8F, 7),
            };
            for (int i = 0; i < 2; i++)
            {
                if (i == 1)
                    splitValues.Add(null); // make sure the algorithm works whether we add null to the end or not
                var uintSetWithLoc = UintSetWithLoc.ConstructFromItems(items, splitValues, false, true);
                uintSetWithLoc.Set.AsEnumerable().ToList().Should().BeEquivalentTo(new uint[] { 0, 2, 5, 7 });
                uintSetWithLoc.Loc.UncompressedLocations.ToArray().Should().BeEquivalentTo(new byte[] { 0, 0, 3, 5 });
            }
        }

        [Fact]
        public void UintSetLoc_CountNumberOfFilteredItemsInEachChildWorks()
        {
            byte numChildren = 3;
            CountNumberOfFilteredItemsHelper(numChildren, new byte[] { 2, 0, 2, 2, 1, 0 }, new UintSet("111111"), null, new uint[] { 2, 1, 3 });
            CountNumberOfFilteredItemsHelper(numChildren, new byte[] { 2, 0, 2, 2, 1, 0 }, new UintSet("111111"), new UintSet("1"), new uint[] { 0, 0, 1 });
            CountNumberOfFilteredItemsHelper(numChildren, new byte[] { 2, 0, 2, 2, 1, 0 }, new UintSet("111111"), new UintSet("0111"), new uint[] { 1, 0, 2 });
            CountNumberOfFilteredItemsHelper(numChildren, new byte[] { 2, 0, 2, 2, 1, 0 }, new UintSet("1"), null, null);
            CountNumberOfFilteredItemsHelper(numChildren, new byte[] { 2, 0, 2, 2, 1, 0 }, new UintSet("0111"), null, null);
            CountNumberOfFilteredItemsHelper(numChildren, new byte[] { 2, 0, 2, 2, 1, 0 }, new UintSet("111111111111111111111"), new UintSet("1"), null);
        }

        private void CountNumberOfFilteredItemsHelper(byte numChildren, byte[] l, UintSet allItems, UintSet filter, uint[] expectedResult)
        {
            UintSetWithLoc u = new UintSets.UintSetWithLoc(allItems, new UintSetLoc(true, l));
            if (expectedResult == null)
                u.Invoking(y => y.CountForEachChild(numChildren, filter))
                    .Should().Throw<Exception>();
            else
            {
                uint[] result = u.CountForEachChild(numChildren, filter);
                result.Should().Equal(expectedResult);
            }
        }

        [Fact]
        public void UintSetLoc_AddItemsAtUintIndicesWorks()
        {
            UintSetLoc l = new UintSetLoc(true);
            UintSet before = new UintSet();
            List<WUInt32> indicesToAdd = new List<WUInt32>() { 1, 3 };
            List<byte> locationsToAdd = new List<byte>() { 2, 2 };
            UintSet after = new UintSet();
            after.AddUints(indicesToAdd);
            l = l.AddItemsAtUintIndices(before, indicesToAdd, locationsToAdd);
            l.UncompressedLocations.ToArray().Should().Equal(new List<byte> { 2, 2 });

            before = after.Clone();
            indicesToAdd = new List<WUInt32>() { 1, 8 };
            locationsToAdd = new List<byte>() { 3, 1 };
            after.AddUints(indicesToAdd);
            l = l.AddItemsAtUintIndices(before, indicesToAdd, locationsToAdd);
            l.UncompressedLocations.ToArray().Should().Equal(new List<byte> { 3, 2, 1 });

            before = after.Clone();
            indicesToAdd = new List<WUInt32>() { 11, 9 }; // note: not sorted
            locationsToAdd = new List<byte>() { 5, 4 };
            after.AddUints(indicesToAdd);
            l = l.AddItemsAtUintIndices(before, indicesToAdd, locationsToAdd);
            l.UncompressedLocations.ToArray().Should().Equal(new List<byte> { 3, 2, 1, 4, 5 });

            before = after.Clone();
            indicesToAdd = new List<WUInt32>() { 12, 12 }; // note: repetition
            locationsToAdd = new List<byte>() { 6, 7 };
            after.AddUints(indicesToAdd);
            l = l.AddItemsAtUintIndices(before, indicesToAdd, locationsToAdd);
            l.UncompressedLocations.ToArray().Should().Equal(new List<byte> { 3, 2, 1, 4, 5, 7 });
        }

        [Fact]
        public void UintSetLoc_RemoveItemsAtUintIndicesWorks()
        {
            UintSetLoc l = new UintSetLoc(true, new byte[] { 1, 5, 0, 5, 1 } );
            UintSet before = new UintSet("0001001011001000"); // 3, 6, 8, 9, 12
            List<WUInt32> indicesToRemove = new List<WUInt32>() { 1, 2, 3, 3, 4, 9, 13, 14, 14, 15 }; // get rid of indices 3 and 9 (the 0th and 3rd above) -- ignore the others
            l = l.RemoveItemsAtUintIndices(before, indicesToRemove);
            l.UncompressedLocations.ToArray().Should().Equal(new List<byte> { 5, 0, 1 });


            l = new UintSetLoc(true, new byte[] { 1, 5, 0, 5, 1 });
            before = new UintSet("0001001011001000");
            indicesToRemove = new List<WUInt32>() { 3, 6, 8, 9, 12 }; // remove everything
            l = l.RemoveItemsAtUintIndices(before, indicesToRemove);
            l.UncompressedLocations.ToArray().Should().Equal(new List<byte> { });
        }

        [Fact]
        public void UintSetLoc_GetLocationOfItemWorks()
        {
            UintSetLoc l = new UintSetLoc(true, new byte[] { 1, 5, 0, 5, 4 } );
            UintSet u = new UintSet("0001001011001000");
            l.GetLocationOfItem(u, 3).Should().Be(1);
            l.GetLocationOfItem(u, 6).Should().Be(5);
            l.GetLocationOfItem(u, 8).Should().Be(0);
            l.GetLocationOfItem(u, 9).Should().Be(5);
            l.GetLocationOfItem(u, 12).Should().Be(4);
        }

        [Fact]
        public void UintSetLocDeltas_IntegratingWorks()
        {
            UintSetLoc l = new UintSetLoc(true, new byte[] { 1, 5, 0, 5, 1 });
            UintSet before = new UintSet("0001001011001000");
            UintSetDeltasLoc deltas = new UintSetDeltasLoc(indicesToAdd: new List<WUInt32>() { 3, 4 }, locationsToAdd: new List<byte>() { 2, 3 }, indicesToRemove: new List<WUInt32>() { 9, 12 });
            UintSetWithLoc replacement = deltas.IntegrateIntoUintSetLoc(before, l);
            UintSet expectedReplacement = new UintSet("0001101010000000");
            UintSetLoc expectedUintSetLoc = new UintSetLoc(true, new byte[] { 2, 3, 5, 0 });
            replacement.Set.AsEnumerable().Should().Equal(expectedReplacement.AsEnumerable());
            replacement.Loc.UncompressedLocations.ToArray().Should().Equal(expectedUintSetLoc.UncompressedLocations.ToArray());
        }

        [Fact]
        public void UintSetLocDeltas_UpdatingWorks()
        {
            UintSetDeltasLoc deltas = new UintSetDeltasLoc(indicesToAdd: new List<WUInt32>() { 3, 4, 6, 7 }, locationsToAdd: new List<byte>() { 13, 14, 16, 17 }, indicesToRemove: new List<WUInt32>() { 8, 9, 10 });
            Dictionary<WUInt32, byte> itemsToAdd = new Dictionary<WUInt32, byte>()
            {
                { 3, 103 },
                { 9, 19 },
                { 12, 22 }
            };
            HashSet<WUInt32> itemsToRemove = new HashSet<WUInt32>() { 4, 7 };
            deltas = deltas.Update(itemsToRemove, itemsToAdd);
            var replacementToAdd = deltas.ToAdd.Keys.OrderBy(x => x).ToList();
            var replacementLocations = deltas.ToAdd.Values.OrderBy(x => x).ToList();
            var replacementToRemove = deltas.IndicesToRemove.OrderBy(x => x).ToList();
            var expectedToAdd = new List<WUInt32>() { 3, 6, 9, 12 };
            var expectedLocations = new List<byte>() { 16, 19, 22, 103 };
            var expectedToRemove = new List<WUInt32>() { 4, 7, 8, 10 }; // note that 4 both gets deleted from "to add" list and added to "to delete" list; meanwhile, 9 is being added, so it should no longer be removed

            replacementToAdd.Should().Equal(expectedToAdd.Select(x => x.WrappedValue));
            replacementLocations.Should().Equal(expectedLocations);
            replacementToRemove.Should().Equal(expectedToRemove.Select(x => x.WrappedValue));
        }

        #endregion

        #region Access and integration tests

        [Fact]
        public async Task WholeUintSetAccessWorks_WithLocations()
        {
            await WholeUintSetAccessWorks(true);
        }

        [Fact]
        public async Task WholeUintSetAccessWorks_WithoutLocations()
        {
            await WholeUintSetAccessWorks(false);
        }

        internal async Task WholeUintSetAccessWorks(bool useLocations)
        {
            StorageFactory.Reset();
            IBlob<Guid> uintStorage = StorageFactory.GetUintSetStorage();
            // We'll set one uint set and then gradually transform it into another, and we'll make sure the final result is correct.
            Guid wholeSetContext = RandomGenerator.GetGuid();
            for (int i = 1; i <= 4; i++)
            {
                IUintSetAccess whole = new WholeUintSetAccess(wholeSetContext, useLocations, uintStorage);
                if (useLocations)
                    await whole.InitializeWithLoc(true);
                else
                    await whole.Initialize();
                if (i == 1)
                    await UintSetAccessWorks_Helper(4, 2, whole, useLocations);
                else if (i == 2)
                    await UintSetAccessWorks_Helper(20, 10, whole, useLocations);
                else if (i == 3)
                    await UintSetAccessWorks_Helper(1000, 10, whole, useLocations);
                else
                    await UintSetAccessWorks_Helper(1000, 100, whole, useLocations);
            }

        }


        [Fact]
        public async Task SplitUintSetAccessWorks_WithLocations()
        {
            //for (int i = 0; i < 10000; i++)
            //{
            //    RandomGenerator.Reset(i);
                await SplitUintSetAccessWorks(true);
            //}
        }

        [Fact]
        public async Task SplitUintSetAccessWorks_WithoutLocations()
        {
            await SplitUintSetAccessWorks(false);
        }

        internal async Task SplitUintSetAccessWorks(bool useLocations)
        {
            StorageFactory.Reset();
            IBlob<Guid> uintStorage = StorageFactory.GetUintSetStorage();
            // For split contexts, we'll start with one based on nothing special, then we'll build on that one, initially removing everything in it. We'll make sure our general algorithms work and also that the split context is unchanged.
            Guid splitSetContext1 = RandomGenerator.GetGuid();
            const int maxItemsInDeltaSet = 25;
            IUintSetAccess split1 = new SplitUintSetAccess(1, splitSetContext1, maxItemsInDeltaSet, useLocations, uintStorage);
            if (useLocations)
                await split1.InitializeWithLoc(true);
            else
                await split1.Initialize();
            await UintSetAccessWorks_Helper(20, 10, split1, useLocations);
            var uintSet1 = await split1.GetUintSet();
            var uintSet1Count = uintSet1.Count;

            Guid splitSetContext2 = RandomGenerator.GetGuid();
            IUintSetAccess split2 = new SplitUintSetAccess(2, splitSetContext2, splitSetContext1, splitSetContext1, true, true, maxItemsInDeltaSet, useLocations, uintStorage);
            await split2.Change(RandomGenerator.GetRandomBool(), uintSet1.AsEnumerable().ToList(), new List<WUInt32>(), new List<byte>()); // remove all items
            await UintSetAccessWorks_Helper(1000, 10, split2, useLocations); 
            var uintSet2 = await split2.GetUintSetWithLoc();
            var uintSet2Count = uintSet2.Set.Count;

            Guid splitSetContext3 = RandomGenerator.GetGuid();
            IUintSetAccess split3 = new SplitUintSetAccess(3, splitSetContext3, splitSetContext2, splitSetContext2, true, true, maxItemsInDeltaSet, useLocations, uintStorage);
            await split3.Change(RandomGenerator.GetRandomBool(), uintSet2.Set.AsEnumerable().ToList(), new List<WUInt32>(), new List<byte>());
            await UintSetAccessWorks_Helper(1000, 10, split3, useLocations);
            var uintSet3 = await split3.GetUintSet();

            // original sets should not have been changed
            uintSet1 = await split1.GetUintSet();
            uintSet1.Count.Should().Be(uintSet1Count);
            var uintSet2c = await split2.GetUintSet();
            uintSet2c.Count.Should().Be(uintSet2Count);
        }

        private static async Task UintSetAccessWorks_Helper(int uintMax, uint numChangesAtOnce, IUintSetAccess access, bool useLocations)
        {
            StorageFactory.Reset();
            UintSet start = GetUintSet(uintMax);
            UintSetLoc startLoc = useLocations ? GetUintSetLoc(start) : null;
            UintSet goal = GetUintSet(uintMax);
            UintSetLoc goalLoc = useLocations ? GetUintSetLoc(goal) : null;
            var initial = await access.GetUintSetWithLoc();
            initial.Set.Count.Should().Be(0);
            UintSet.GetCardinality(initial.Set.AsBitArray()).Should().Be(0);
            if (useLocations)
                initial.Loc.UncompressedLocations.Length.Should().Be(0);
            if (useLocations)
                await access.InitializeWithLoc(new UintSetWithLoc(start, startLoc));
            else
                await access.Initialize(start);
            var changed = await access.GetUintSet();
            changed.AsEnumerable().Should().BeEquivalentTo(start.AsEnumerable());
            for (uint i = 0; i < uintMax; i += numChangesAtOnce) // we'll go all the way through the UintSet, thus changing all of the uintSetMax items that need changing, but we'll group the changes into batches of numChangesAtOnce. 
            {
                // Create lists of things to add and remove. We'll also put some extra things in these lists that we want to undo, just to check that undoing recent changes works.
                List<WUInt32> indexAdditions = new List<WUInt32>();
                List<byte> locationAdditions = useLocations ? new List<byte>() : null;
                List<WUInt32> indexRemovals = new List<WUInt32>();
                List<WUInt32> additions_to_undo = new List<WUInt32>();
                List<WUInt32> removals_to_undo = new List<WUInt32>();
                List<byte> removals_to_undo_locations = useLocations ? new List<byte>() : null;
                for (uint j = i; j < i + numChangesAtOnce; j++) // here's where we group
                {
                    if (goal.Contains(j) && !start.Contains(j))
                    {
                        indexAdditions.Add(j);
                        if (useLocations)
                            locationAdditions.Add(goalLoc.GetLocationOfItem(goal, j));
                    }
                    else if (start.Contains(j) && !goal.Contains(j))
                        indexRemovals.Add(j);
                    else if (useLocations && start.Contains(j) && goal.Contains(j) && startLoc.GetLocationOfItem(start, j) != goalLoc.GetLocationOfItem(goal, j))
                    { // change the location
                        indexAdditions.Add(j);
                        locationAdditions.Add(goalLoc.GetLocationOfItem(goal, j));
                    }
                    else if (!start.Contains(j) && RandomGenerator.GetRandomBool())
                    { // put in additions list and undo it after
                        indexAdditions.Add(j);
                        if (useLocations)
                            locationAdditions.Add((byte) RandomGenerator.GetRandom(0, 2)); // doesn't matter what location we add, since we are going to be deleting it next
                        additions_to_undo.Add(j);
                    }
                    else if (start.Contains(j) && RandomGenerator.GetRandomBool())
                    { // put in removals list and undo it after
                        indexRemovals.Add(j);
                        removals_to_undo.Add(j);
                        if (useLocations)
                            removals_to_undo_locations.Add(startLoc.GetLocationOfItem(start, j));
                    }
                }
                await access.Change(RandomGenerator.GetRandomBool(), indexRemovals, indexAdditions, locationAdditions);
                await access.Change(RandomGenerator.GetRandomBool(), additions_to_undo, removals_to_undo, removals_to_undo_locations); // reversed order, since we're undoing
            }
            UintSetWithLoc result = await access.GetUintSetWithLoc();
            result.Set.Count.Should().Be((uint) uintMax / 2);
            result.Set.AsEnumerable().Should().Equal(goal.AsEnumerable());
            if (useLocations)
            {
                result.Loc.UncompressedLocations.Length.Should().Be((int)uintMax / 2);
                result.Loc.UncompressedLocations.ToArray().AsEnumerable().Should().Equal(goalLoc.UncompressedLocations.ToArray().AsEnumerable());
            }
        }

        private static UintSet GetUintSet(int uintMax, int? numSet = null)
        {
            if (numSet == null)
                numSet = uintMax / 2;
            List<WUInt32> list = RandomGenerator.PickRandomInts(0, uintMax, (int) numSet).Select(x => (WUInt32)(uint)x).ToList();
            UintSet u = new UintSet(new HashSet<WUInt32>(list));
            return u;
        }

        private static UintSetLoc GetUintSetLoc(UintSet u)
        {
            return new UintSetLoc(true, u.AsEnumerable().Select(x => (byte)RandomGenerator.GetRandom(0, 2)).ToArray());
        }

        #endregion 

        #region Operations

        private enum UintSetOperation
        {
            Union,
            Intersection,
            Except
        }

        private UintSet GetOperationResult(UintSet s1, UintSet s2, UintSetOperation o)
        {
            List<WUInt32> l1 = s1.AsEnumerable().ToList();
            List<WUInt32> l2 = s2.AsEnumerable().ToList();
            List<WUInt32> r;
            switch (o)
            {
                case UintSetOperation.Union:
                    r = l1.Union(l2).ToList();
                    break;
                case UintSetOperation.Intersection:
                    r = l1.Intersect(l2).ToList();
                    break;
                case UintSetOperation.Except:
                    r = l1.Except(l2).ToList();
                    break;
                default:
                    throw new NotImplementedException();
            }
            UintSet uresult = new UintSet(new HashSet<WUInt32>(r));
            return uresult;
        }

        [Fact]
        public void UintSet_Operations()
        {
            // Because UintSet stores differently based on number of items, we try with a large and small number of items each operation.
            //UintSet_Operation(1000000, 800000, 300000, UintSetOperation.Union);
            //UintSet_Operation(1000000, 800000, 300000, UintSetOperation.Intersection);
            //UintSet_Operation(1000000, 800000, 300000, UintSetOperation.Except);
            UintSet_Operation(1000, 800, 10, UintSetOperation.Union);
            UintSet_Operation(1000, 800, 10, UintSetOperation.Intersection);
            UintSet_Operation(1000, 800, 10, UintSetOperation.Except);
            UintSet_Operation(1000, 800, 300, UintSetOperation.Union);
            UintSet_Operation(1000, 800, 300, UintSetOperation.Intersection);
            UintSet_Operation(1000, 800, 300, UintSetOperation.Except);
            UintSet_Operation(1000, 10, 5, UintSetOperation.Union);
            UintSet_Operation(1000, 10, 5, UintSetOperation.Intersection);
            UintSet_Operation(1000, 10, 5, UintSetOperation.Except);
        }

        private void UintSet_Operation(int length, int numItems1, int numItems2, UintSetOperation o)
        {
            UintSet set1 = GetUintSet(length, numItems1);
            UintSet set2 = GetUintSet(length, numItems2);
            UintSet expected = GetOperationResult(set1, set2, o);
            UintSet result;
            switch (o)
            {
                case UintSetOperation.Union:
                    result = set1.Union(set2);
                    break;
                case UintSetOperation.Intersection:
                    result = set1.Intersect(set2);
                    break;
                case UintSetOperation.Except:
                    result = set1.Except(set2);
                    break;
                default:
                    throw new NotImplementedException();
            }
            result.AsEnumerable().ToList().SequenceEqual(expected.AsEnumerable().ToList()).Should().BeTrue();
        }

        [Fact]
        public void RemoveUnsetBitsFromFilterWorks()
        {
            List<string> l = new List<string>()
            {
                "111,110,11",
                "111,011,011",
                "111,001,001",
                "111,111,111",
                "111,000,0",
                "011,001,010",
                "011,110,1",
                "011,000,0",
                "000,000,0",
                "000,111,0",
                "010,000,0",
                "010,001,0",
                "010,010,1",
                "010,100,0",
                "010,101,0",
                "010,111,1",
                "010,110,1",
                "101,000,0",
                "101,001,01",
                "101,010,0",
                "101,100,1",
                "101,101,11",
                "101,111,11",
                "101,110,1",
            };
            foreach (string s in l)
            {
                string[] split = s.Split(',');
                string main = split[0];
                string filter = split[1];
                string intendedResult = split[2];
                UintSet mainU = new UintSet(main);
                UintSet filterU = new UintSet(filter);
                UintSet resultU = mainU.RemoveUnsetBitsFromFilter(filterU);
                UintSet intendedResultU = new UintSet(intendedResult);
                resultU.IsEquivalentTo(intendedResultU).Should().BeTrue();
            }
        }

        #endregion
    }
}
