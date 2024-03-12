using System;
using System.Linq;
using R8RUtilities;
using Xunit;
using System.Collections.Generic;
using CountedTree.Core;
using CountedTree.Node;
using CountedTree.NodeBuffers;
using CountedTree.NodeResults;
using CountedTree.PendingChanges;
using CountedTree.Queries;
using FluentAssertions;
using System.Threading.Tasks;
using CountedTree.UintSets;
using System.Diagnostics;
using Utility;
using Lazinator.Wrappers;

namespace CountedTree.Tests
{
    public partial class CountedTreeTests
    {
        [Fact]
        public void PendingChangesSimplify()
        {
            PendingChange<WFloat>[] initialList = new PendingChange<WFloat>[]
            {
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.4F, 6), delete: true), // (0) 
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.8F, 1), delete: true), // (1) 
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.8F, 1), delete: true), // (2) redundant with (1)
                new PendingChange<WFloat>(new KeyAndID<WFloat>(3.5F, 2), delete: false), // (3)
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.8F, 1), delete: false), // (4)  
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.9F, 1), delete: false), // (5)
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.8F, 5), delete: true), // (6) out of ID order
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.8F, 3), delete: true), // (7) 
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.9F, 1), delete: false), // (8) redundant with (5)
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.1F, 2), delete: false), // (9)
                new PendingChange<WFloat>(new KeyAndID<WFloat>(8.9F, 6), delete: false), // (10) 

            };
            PendingChange<WFloat>[] expectedSimplification = new PendingChange<WFloat>[]
            {
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.1F, 2), delete: false), // (9)
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.4F, 6), delete: true), // (0) 
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.8F, 3), delete: true), // (7) 
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.8F, 5), delete: true), // (6) 
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.9F, 1), delete: false), // (5) 
                new PendingChange<WFloat>(new KeyAndID<WFloat>(8.9F, 6), delete: false), // (10) 
            };
            initialList.Simplify().Should().Equal(expectedSimplification);
        }

        [Fact]
        public void PendingChangesCombineAdditionAndDeletion()
        {
            PendingChange<WFloat>[] initialList = new PendingChange<WFloat>[]
            {
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.9F, 1), delete: false)
            };
            PendingChange<WFloat>[] supplementalList = new PendingChange<WFloat>[]
            {
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.9F, 1), delete: true)
            };
            PendingChange<WFloat>[] expectedList = new PendingChange<WFloat>[]
            {
            };
            VerifyPendingChangesCombine(initialList, supplementalList, expectedList);
        }

        [Fact]
        public void PendingChangesCombineAdditionAndMove()
        {
            PendingChange<WFloat>[] initialList = new PendingChange<WFloat>[]
            {
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.9F, 1), delete: false)
            };
            PendingChange<WFloat>[] supplementalList = new PendingChange<WFloat>[]
            {
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.9F, 1), delete: true),
                new PendingChange<WFloat>(new KeyAndID<WFloat>(3.0F, 1), delete: false),
            };
            PendingChange<WFloat>[] expectedList = new PendingChange<WFloat>[]
            {
                new PendingChange<WFloat>(new KeyAndID<WFloat>(3.0F, 1), delete: false),
            };
            VerifyPendingChangesCombine(initialList, supplementalList, expectedList); 
        }

        [Fact]
        public void PendingChangesCombineAdditionAndAddition()
        {
            // Something has gone wrong if this happens, since a deletion is omitted. But we infer the deletion and keep going. 
            PendingChange<WFloat>[] initialList = new PendingChange<WFloat>[]
            {
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.9F, 1), delete: false)
            };
            PendingChange<WFloat>[] supplementalList = new PendingChange<WFloat>[]
            {
                new PendingChange<WFloat>(new KeyAndID<WFloat>(3.0F, 1), delete: false)
            };
            PendingChange<WFloat>[] expectedList = new PendingChange<WFloat>[]
            {
                new PendingChange<WFloat>(new KeyAndID<WFloat>(3.0F, 1), delete: false)
            };
            VerifyPendingChangesCombine(initialList, supplementalList, expectedList);
        }

        [Fact]
        public void PendingChangesCombineDeletionAndAddition()
        {
            PendingChange<WFloat>[] initialList = new PendingChange<WFloat>[]
            {
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.8F, 1), delete: true)
            };
            PendingChange<WFloat>[] supplementalList = new PendingChange<WFloat>[]
            {
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.9F, 1), delete: false)
            };
            PendingChange<WFloat>[] expectedList = new PendingChange<WFloat>[]
            {
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.8F, 1), delete: true),
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.9F, 1), delete: false)
            };
            VerifyPendingChangesCombine(initialList, supplementalList, expectedList);
        }

        [Fact]
        public void PendingChangesCombineDeletionAndDeletion()
        {
            // redundant deletions
            PendingChange<WFloat>[] initialList = new PendingChange<WFloat>[]
            {
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.8F, 1), delete: true)
            };
            PendingChange<WFloat>[] supplementalList = new PendingChange<WFloat>[]
            {
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.8F, 1), delete: true)
            };
            PendingChange<WFloat>[] expectedList = new PendingChange<WFloat>[]
            {
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.8F, 1), delete: true)
            };
            VerifyPendingChangesCombine(initialList, supplementalList, expectedList);
        }

        [Fact]
        public void PendingChangesCombineDeletionAndMove()
        {
            // redundant deletions in the second list
            PendingChange<WFloat>[] initialList = new PendingChange<WFloat>[]
            {
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.8F, 1), delete: true)
            };
            PendingChange<WFloat>[] supplementalList = new PendingChange<WFloat>[]
            {
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.8F, 1), delete: true),
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.9F, 1), delete: false)
            };
            PendingChange<WFloat>[] expectedList = new PendingChange<WFloat>[]
            {
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.8F, 1), delete: true),
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.9F, 1), delete: false)
            };
            VerifyPendingChangesCombine(initialList, supplementalList, expectedList);
        }


        [Fact]
        public void PendingChangesCombineMoveAndAddition()
        {
            // deletion is omitted but should be inferred
            PendingChange<WFloat>[] initialList = new PendingChange<WFloat>[]
            {
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.8F, 1), delete: true),
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.9F, 1), delete: false)
            };
            PendingChange<WFloat>[] supplementalList = new PendingChange<WFloat>[]
            {
                new PendingChange<WFloat>(new KeyAndID<WFloat>(3.0F, 1), delete: false)
            };
            PendingChange<WFloat>[] expectedList = new PendingChange<WFloat>[]
            {
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.8F, 1), delete: true),
                new PendingChange<WFloat>(new KeyAndID<WFloat>(3.0F, 1), delete: false)
            };
            VerifyPendingChangesCombine(initialList, supplementalList, expectedList);
        }

        [Fact]
        public void PendingChangesCombineMoveAndDeletion()
        {
            PendingChange<WFloat>[] initialList = new PendingChange<WFloat>[]
            {
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.8F, 1), delete: true),
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.9F, 1), delete: false)
            };
            PendingChange<WFloat>[] supplementalList = new PendingChange<WFloat>[]
            {
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.9F, 1), delete: true)
            };
            PendingChange<WFloat>[] expectedList = new PendingChange<WFloat>[]
            {
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.8F, 1), delete: true),
            };
            VerifyPendingChangesCombine(initialList, supplementalList, expectedList);
        }

        [Fact]
        public void PendingChangesCombineMoveAndMove()
        {
            // offsetting addition and deletion should be eliminated
            PendingChange<WFloat>[] initialList = new PendingChange<WFloat>[]
            {
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.8F, 1), delete: true),
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.9F, 1), delete: false)
            };
            PendingChange<WFloat>[] supplementalList = new PendingChange<WFloat>[]
            {
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.9F, 1), delete: true),
                new PendingChange<WFloat>(new KeyAndID<WFloat>(3.0F, 1), delete: false)
            };
            PendingChange<WFloat>[] expectedList = new PendingChange<WFloat>[]
            {
                new PendingChange<WFloat>(new KeyAndID<WFloat>(2.8F, 1), delete: true),
                new PendingChange<WFloat>(new KeyAndID<WFloat>(3.0F, 1), delete: false)
            };
            VerifyPendingChangesCombine(initialList, supplementalList, expectedList);
        }


        private static void VerifyPendingChangesCombine(PendingChange<WFloat>[] initialList, PendingChange<WFloat>[] supplementalList, PendingChange<WFloat>[] expectedList)
        {
            PendingChangesCollection<WFloat> pcc = new PendingChangesCollection<WFloat>(new List<PendingChange<WFloat>[]> { initialList, supplementalList });
            var resultOrdered = pcc.AsEnumerable().OrderByDateOfChange();
            resultOrdered.Should().Equal(expectedList);
        }

        [Fact]
        public static void CombinedInternalNodeBufferWorks()
        {
            var node = GetUnconnectedInternalNode_Alt();
            var originalPendingChanges = node.MainBuffer.AllPendingChanges();
            Debug.WriteLine("Original pending changes: " + originalPendingChanges.ToString()); 
            PendingChangesCollection<WFloat> additionalPendingChanges = new PendingChangesCollection<WFloat>(GetAdditionalPendingChanges_Alt(), true);
            Debug.WriteLine("Additional pending changes: " + additionalPendingChanges.ToString()); 
            var combinedBuffer = new CombinedInternalNodeBuffer<WFloat>(node, additionalPendingChanges);
            Debug.WriteLine("Node buffer: " + combinedBuffer.NodeBuffer.AllPendingChanges().ToString()); 
            Debug.WriteLine("Request buffer: " + combinedBuffer.RequestBuffer.AllPendingChanges().ToString()); 
            var mergedChanges = combinedBuffer.AllPendingChanges().AsEnumerable().ToList();
            Debug.WriteLine("Merged changes: " + String.Join(", ", mergedChanges)); 
            var expectedUnsorted = new PendingChange<WFloat>[] {
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(7.5F, 3), true), // no effect on index 3 -> 7.5D, 2.8A
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(2.8F, 3), false),
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(2.9F, 1), false), // 1 redundantly added -> 2.9A
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(6.0F, 2), true), // 2 redundantly deleted -> 6.0D
                        
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(7.5F, 13), true), // 13 deleted again -> 7.5D
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(2.9F, 11), false),  // 11 ignored -> 2.9A
                                                                                     // 12 added back at same value -> nothing
                        
                                                                                        // 23 deleted (ignored)
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(7.5F, 23), true), // 23 added at yet another value -> 7.5D, 42.1A
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(42.1F, 23), false), 
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(10.8F, 21), false), // 21 changed to different value (replacing earlier add) -> 10.8A
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(6.0F, 22), true), // 22 added back at different value -> 6.0D, 6.2A
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(6.2F, 22), false), 
                        
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(7.5F, 33), true), // 33 added at yet another value (missing intervening delete) -> 7.5D, 55.6A
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(55.6F, 33), false), 
                                                                                        // 31 deleted -> nothing
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(6.0F, 32), true)  // 32 ignored -> 6.0D
                    };
            var expectedSortedByKey = expectedUnsorted.OrderBy(x => x.Item.Key).ThenBy(x => x.Item.ID).ThenBy(x => !x.Delete).ToList();
            var expectedSortedByID = expectedSortedByKey.OrderBy(x => x.Item.ID).ThenBy(x => x.Item.Key).ThenBy(x => !x.Delete).ToList();
            var mergedSortedByKey = mergedChanges.OrderBy(x => x.Item.Key).ThenBy(x => x.Item.ID).ThenBy(x => !x.Delete).ToList(); // should have no effect
            var mergedSortedByID = mergedChanges.OrderBy(x => x.Item.ID).ThenBy(x => x.Item.Key).ThenBy(x => !x.Delete).ToList();
            for (int i = 0; i < expectedUnsorted.Length; i++)
            {
                mergedSortedByID[i].Equals(expectedSortedByID[i]).Should().BeTrue(); // should meet same as expectations
            }
            for (int i = 0; i < expectedUnsorted.Length; i++)
            {
                mergedSortedByKey[i].Equals(mergedChanges[i]).Should().BeTrue(); // should be sorted correctly
                mergedSortedByKey[i].Equals(expectedSortedByKey[i]).Should().BeTrue(); // should be sorted correctly
            }
            expectedUnsorted.Length.Should().Be(mergedSortedByKey.Count());
        }

        private static CountedInternalNode<WFloat> GetUnconnectedInternalNode_Alt()
        {
            int numNodesPerInternal = 4; // keep it simple for testing purposes
            int maxItemsForLeaf = 25;
            var splitValues = new KeyAndID<WFloat>[] {
                    new KeyAndID<WFloat>(3.0F, 20),
                    new KeyAndID<WFloat>(6.0F, 2),
                    new KeyAndID<WFloat>(9.0F, 60) };
            return new CountedInternalNode<WFloat>(
                new NodeInfo<WFloat>[] {
                    GetNodeInfo(2, 1, 19, null, splitValues[0]), // <= 3.0
                    GetNodeInfo(3, 5, 31, splitValues[0], splitValues[1]), // > 3.0, <= 6.0
                    GetNodeInfo(4, 1, 1, splitValues[1], splitValues[2]), // > 6.0, <= 9.0
                    GetNodeInfo(5, 5, 39, splitValues[2], null) },
                new PendingChangesCollection<WFloat>(
                    new PendingChange<WFloat>[] {
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(7.5F, 3), true), // 3 deleted
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(2.8F, 3), false), // 3 added at different value
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(2.9F, 1), false), // 1 added
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(6.0F, 2), true), // 2 deleted
                        
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(7.5F, 13), true), // 13 deleted
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(2.8F, 13), false), // 13 added at different value
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(2.9F, 11), false), // 11 added
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(6.0F, 12), true), // 12 deleted
                        
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(7.5F, 23), true), // 23 deleted
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(2.8F, 23), false), // 23 added at different value
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(2.9F, 21), false), // 21 added
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(6.0F, 22), true), // 22 deleted
                        
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(7.5F, 33), true), // 33 deleted
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(2.8F, 33), false), // 33 added at different value
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(2.9F, 31), false), // 31 added
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(6.0F, 32), true), // 32 deleted
                    }, true
                    ),
                1, 0, new TreeStructure(false, numNodesPerInternal, maxItemsForLeaf, false, false));
        }

        private static PendingChange<WFloat>[] GetAdditionalPendingChanges_Alt()
        {
            return new PendingChange<WFloat>[] {
                                                                                        // no effect on index 3 -> 7.5D, 2.8A
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(2.9F, 1), false), // 1 redundantly added -> 2.9A
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(6.0F, 2), true), // 2 redundantly deleted -> 6.0F
                        
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(2.8F, 13), true), // 13 deleted again -> 7.5D
                                                                                    // 11 ignored -> 2.9A
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(6.0F, 12), false), // 12 added back at same value -> nothing
                        
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(2.8F, 23), true), // 23 deleted (should be ignored)
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(42.1F, 23), false), // 23 added at yet another value -> 7.5D, 42.1A
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(10.8F, 21), false), // 21 changed to different value -> 10.8A
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(6.2F, 22), false), // 22 added back at different value -> 6.0D, 6.2A
                        
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(55.6F, 33), false), // 33 added at yet another value (missing intervening delete) -> 7.5D, 55.6A
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(2.9F, 31), true), // 31 deleted -> nothing
                                                                                        // 32 ignored -> 6.0FD
                    }; // > 9.0
        }


        [Fact]
        public static async Task GetChildrenIndicesWorks()
        {
            var node = await GetUnconnectedInternalNode(false);
            // This node has the following splits.
            //new KeyAndID<WFloat>(3.0F, 20),
            //new KeyAndID<WFloat>(6.0F, 2),
            //new KeyAndID<WFloat>(9.0F, 60) };
            node.GetChildrenIndices(new List<KeyAndID<WFloat>>()
            {
                new KeyAndID<WFloat>(2.0F, 18), // child 0
                new KeyAndID<WFloat>(3.0F, 19), // child 0
                new KeyAndID<WFloat>(3.0F, 20), // child 0
                new KeyAndID<WFloat>(6.0F, 3), // child 2
                new KeyAndID<WFloat>(7.0F, 4), // child 2
                new KeyAndID<WFloat>(10.0F, 25), // child 3
            }
            ).Should().Equal(new List<byte> { 0, 0, 0, 2, 2, 3 });

            node.GetChildrenIndices(new List<KeyAndID<WFloat>>()
            {
            }
            ).Should().Equal(new List<byte> { });

            node.GetChildrenIndices(new List<KeyAndID<WFloat>>()
            {
                new KeyAndID<WFloat>(2.0F, 18), // child 0
                new KeyAndID<WFloat>(3.0F, 19), // child 0
                new KeyAndID<WFloat>(3.0F, 20), // child 0
                new KeyAndID<WFloat>(6.0F, 3), // child 2
                new KeyAndID<WFloat>(7.0F, 4), // child 2
            }
            ).Should().Equal(new List<byte> { 0, 0, 0, 2, 2 });

            node.GetChildrenIndices(new List<KeyAndID<WFloat>>()
            {
                new KeyAndID<WFloat>(2.0F, 18), // child 0
                new KeyAndID<WFloat>(3.0F, 19), // child 0
                new KeyAndID<WFloat>(3.0F, 20), // child 0
                new KeyAndID<WFloat>(6.0F, 3), // child 2
                new KeyAndID<WFloat>(7.0F, 4), // child 2
                new KeyAndID<WFloat>(10.0F, 25), // child 3
                new KeyAndID<WFloat>(11.0F, 26), // child 3
            }
            ).Should().Equal(new List<byte> { 0, 0, 0, 2, 2, 3, 3 });

            node.GetChildrenIndices(new List<KeyAndID<WFloat>>()
            {
                new KeyAndID<WFloat>(6.0F, 3), // child 2
            }
            ).Should().Equal(new List<byte> { 2 });

        }

        [Fact]
        public static async Task GetIncludedIndicesWorks()
        {
            await GetIncludedIndicesWorks_Helper(AnomalyType.NoAnomaly, true, false);
            await GetIncludedIndicesWorks_Helper(AnomalyType.NoAnomaly, false, false);
            await GetIncludedIndicesWorks_Helper(AnomalyType.NoAnomaly, true, true);
            await GetIncludedIndicesWorks_Helper(AnomalyType.NoAnomaly, false, true);
            await GetIncludedIndicesWorks_Helper(AnomalyType.RedundantDeletion, true, false);
            await GetIncludedIndicesWorks_Helper(AnomalyType.RedundantDeletion, false, false);
            // redundant deletion must be adjusted for because it can result in negative number of expected nodes. RedundantInsertion doesn't have that effect, so we don't need to test for it with Internal nodes.
            // also, we don't test redundant deletion with filters, because filters require UintSetLocs, and when we have those, we don't expect anomalies.
        }

        internal static async Task GetIncludedIndicesWorks_Helper(AnomalyType anomalyType, bool ascending, bool useFilters)
        {
            NodeWithExpectations n = await GetNodeWithExpectations(anomalyType, ascending, useFilters, useFilters ? FilterOptions.FilterOutIDsMultiplesOf10 : FilterOptions.NoFiltersAsNull, useFilters ? FilterOptions.FilterOutIDsMultiplesOf5 : FilterOptions.NoFiltersAsNull);
            UintSet supersetFilter = useFilters ? GetFilter(FilterOptions.FilterOutIDsMultiplesOf10) : null;
            UintSet filteredFilter = useFilters ? GetFilter(FilterOptions.FilterOutIDsMultiplesOf5) : null;
            UintSetWithLoc nodeUintSetWithLoc = n.node.TreeStructure.StoreUintSetLocs ? await n.node.GetUintSetWithLoc() : null;

            var requestBuffer = new InternalNodeBuffer<WFloat>(n.node.TreeStructure.NumChildrenPerInternalNode, n.node.SplitValue, n.additionalPendingChanges);
            IEnumerable<Tuple<uint, uint>> supersetRange = n.node.GetIncludedIndicesForNodesStartingAtZero(requestBuffer, ascending, nodeUintSetWithLoc, supersetFilter).ToList();
            IEnumerable<Tuple<uint, uint>> filteredRange = supersetRange;
            if (useFilters)
                filteredRange = n.node.GetIncludedIndicesForNodesStartingAtZero(requestBuffer, ascending, nodeUintSetWithLoc, filteredFilter).ToList();
            IncludedIndices[] i = supersetRange.Zip(filteredRange, (s, f) => s == null || f == null ? null : new IncludedIndices(s.Item1, s.Item2, f.Item1, f.Item2)).ToArray();
            i.Should().Equal(n.includedIndices);
        }


        public static async Task<CountedInternalNode<WFloat>> GetUnconnectedInternalNode(bool includeUintSets)
        {
            bool splitRangeEvenly = false;
            int numNodesPerInternal = 5; // keep it simple for testing purposes
            int maxItemsForLeaf = 25;
            IBlob<Guid> uintSetStorage = null;
            UintSetWithLoc uintSetWithLoc = null;
            // with the buffered pending changes (but ignoring the additional pending changes), we will have 
            // 21 items from 0 to 3.0 including the buffered changes (2.8, 3) and (2.9, 1), 
            // 30 from 3.0 to 6.0 excluding (6.0, 2) (which is deleted in the buffer), 
            // 1 from 6.0 to 9.0, and 
            // 39 greater than 9.0 and less than 12.0, and
            // 0 greater than 12.0
            // We will add * or ** to those with IDs ending in 5 or 10, since those will be the only items in our subsets. We have (2,2), (3,3), (0,0), (4,3) of those in the node where each pair refers to IDs ending in 5 and IDs ending in 10. With additional pending changes, we will add one item in the last group that is a multiple of 10.
            List<KeyAndID<WFloat>> itemsIncludingThoseInBuffer = new List<KeyAndID<WFloat>>()
            {
                new KeyAndID<WFloat>(2.8F, 3), // in buffer
                new KeyAndID<WFloat>(2.9F, 1), // in buffer; Will be deleted in additional pending changes
                new KeyAndID<WFloat>(2.0F, 103),
                new KeyAndID<WFloat>(2.0F, 2104),
                new KeyAndID<WFloat>(2.1F, 105), // *
                new KeyAndID<WFloat>(2.23F, 106),
                new KeyAndID<WFloat>(2.43F, 107),
                new KeyAndID<WFloat>(2.35F, 108),
                new KeyAndID<WFloat>(2.42F, 109),
                new KeyAndID<WFloat>(2.07F, 1110), // **
                new KeyAndID<WFloat>(2.6F, 111),
                new KeyAndID<WFloat>(0.6F, 112),
                new KeyAndID<WFloat>(0.3F, 113),
                new KeyAndID<WFloat>(0.6F, 2114),
                new KeyAndID<WFloat>(0.7F, 1115), // *
                new KeyAndID<WFloat>(1.6F, 116),
                new KeyAndID<WFloat>(1.8F, 117),
                new KeyAndID<WFloat>(1.9F, 118),
                new KeyAndID<WFloat>(2.1F, 119),
                new KeyAndID<WFloat>(2.4F, 120), // **
                new KeyAndID<WFloat>(2.2F, 121),

                new KeyAndID<WFloat>(3.0F, 201),
                new KeyAndID<WFloat>(3.0F, 1202),
                new KeyAndID<WFloat>(3.0F, 203),
                new KeyAndID<WFloat>(3.01F, 204),
                new KeyAndID<WFloat>(3.04F, 205), // *
                new KeyAndID<WFloat>(3.06F, 206),
                new KeyAndID<WFloat>(3.07F, 207),
                new KeyAndID<WFloat>(3.02F, 208),
                new KeyAndID<WFloat>(3.1F, 209),
                new KeyAndID<WFloat>(3.11F, 210), // **
                new KeyAndID<WFloat>(3.12F, 211),
                new KeyAndID<WFloat>(3.13F, 212),
                new KeyAndID<WFloat>(3.14F, 1213),
                new KeyAndID<WFloat>(3.15F, 214),
                new KeyAndID<WFloat>(3.0F, 215), // *
                new KeyAndID<WFloat>(3.05F, 216),
                new KeyAndID<WFloat>(3.06F, 217),
                new KeyAndID<WFloat>(3.07F, 218),
                new KeyAndID<WFloat>(3.1F, 219),
                new KeyAndID<WFloat>(3.4F, 2220), // **
                new KeyAndID<WFloat>(3.2F, 221),
                new KeyAndID<WFloat>(3.14F, 222),
                new KeyAndID<WFloat>(3.15F, 223),
                new KeyAndID<WFloat>(3.0F, 224),
                new KeyAndID<WFloat>(3.05F, 225), // *
                new KeyAndID<WFloat>(3.06F, 226),
                new KeyAndID<WFloat>(3.07F, 227),
                new KeyAndID<WFloat>(3.1F, 228),
                new KeyAndID<WFloat>(3.4F, 229),
                new KeyAndID<WFloat>(3.2F, 230), // **
                // note that 6.0F, 2 is marked as deleted in pending changes

                new KeyAndID<WFloat>(6.2F, 301), // will be deleted in additional pending changes

                new KeyAndID<WFloat>(10.0F, 401),
                new KeyAndID<WFloat>(10.0F, 402),
                new KeyAndID<WFloat>(10.0F, 403),
                new KeyAndID<WFloat>(10.01F, 1404),
                new KeyAndID<WFloat>(10.04F, 405), // *
                new KeyAndID<WFloat>(10.06F, 406),
                new KeyAndID<WFloat>(10.07F, 407),
                new KeyAndID<WFloat>(10.02F, 408),
                new KeyAndID<WFloat>(10.1F, 409),
                new KeyAndID<WFloat>(10.11F, 410), // **
                new KeyAndID<WFloat>(10.12F, 2411),
                new KeyAndID<WFloat>(10.13F, 412),
                new KeyAndID<WFloat>(10.14F, 413),
                new KeyAndID<WFloat>(10.15F, 414),
                new KeyAndID<WFloat>(10.0F, 415), // *
                new KeyAndID<WFloat>(10.05F, 416),
                new KeyAndID<WFloat>(10.06F, 417),
                new KeyAndID<WFloat>(10.07F, 418),
                new KeyAndID<WFloat>(10.1F, 419),
                new KeyAndID<WFloat>(10.4F, 420), // **
                new KeyAndID<WFloat>(10.2F, 421),
                new KeyAndID<WFloat>(10.14F, 422),
                new KeyAndID<WFloat>(10.15F, 423),
                new KeyAndID<WFloat>(10.0F, 424),
                new KeyAndID<WFloat>(10.05F, 425), // *
                new KeyAndID<WFloat>(10.06F, 426),
                new KeyAndID<WFloat>(10.07F, 427),
                new KeyAndID<WFloat>(10.1F, 428),
                new KeyAndID<WFloat>(10.4F, 429),
                new KeyAndID<WFloat>(10.2F, 430), // **
                new KeyAndID<WFloat>(10.2F, 431),
                new KeyAndID<WFloat>(10.2F, 432),
                new KeyAndID<WFloat>(10.2F, 433),
                new KeyAndID<WFloat>(10.2F, 434),
                new KeyAndID<WFloat>(10.2F, 435), // *
                new KeyAndID<WFloat>(10.2F, 436),
                new KeyAndID<WFloat>(10.2F, 437),
                new KeyAndID<WFloat>(10.2F, 438),
                new KeyAndID<WFloat>(10.2F, 439),
                // Note: 10.0F, 11 will be added in additional pending changes

                // Nothing in next group -- but one will be added in additional pending changes
            };
            var splitValues = new KeyAndID<WFloat>?[] { // note that the IDs here are not all that relevant
                    new KeyAndID<WFloat>(3.0F, 20),
                    new KeyAndID<WFloat>(6.0F, 2),
                    new KeyAndID<WFloat>(9.0F, 60),
                    new KeyAndID<WFloat>(12.0F, 999)};
            if (includeUintSets)
            {
                uintSetWithLoc = UintSetWithLoc.ConstructFromItems(itemsIncludingThoseInBuffer, splitValues, false, true);
                uintSetStorage = new InMemoryBlob<Guid>();
            }
            return await CountedInternalNode<WFloat>.GetCountedInternalNodeWithUintSet(
                new NodeInfo<WFloat>[] {
                    GetNodeInfo(2, 1, 19, null, splitValues[0]), // <= 3.0
                    GetNodeInfo(3, 5, 31, splitValues[0], splitValues[1]), // > 3.0, <= 6.0
                    GetNodeInfo(4, 1, 1, splitValues[1], splitValues[2]), // > 6.0, <= 9.0
                    GetNodeInfo(5, 5, 39, splitValues[2], splitValues[3]),
                    GetNodeInfo(6, 1, 0, splitValues[3], null)
                    },
                new PendingChangesCollection<WFloat>(
                    new PendingChange<WFloat>[] {
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(2.8F, 3), false), // add an item (node index 0 -> 20)
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(2.9F, 1), false), // add another (node index 0 -> 21)
                        new PendingChange<WFloat>(new KeyAndID<WFloat>(6.0F, 2), true), // subtract an item on edge based on value and ID (node index 1 -> 30)
                    }, true
                    ),
                1, 0, new TreeStructure(splitRangeEvenly, numNodesPerInternal, maxItemsForLeaf, includeUintSets, includeUintSets),
                uintSetStorage, uintSetWithLoc);
        }

        private static PendingChangesCollection<WFloat> GetAdditionalPendingChanges()
        {
            var additionalPendingChanges = new PendingChange<WFloat>[] {
                    new PendingChange<WFloat>(new KeyAndID<WFloat>(2.9F, 1), true), // delete item added above (node index 0 -> 20)
                    new PendingChange<WFloat>(new KeyAndID<WFloat>(6.2F, 301), true), // delete an item (node index 2 -> 0)
                    new PendingChange<WFloat>(new KeyAndID<WFloat>(10.0F, 11), false), // add an item (node index 3 -> 40)
                    new PendingChange<WFloat>(new KeyAndID<WFloat>(13.0F, 177), false), // add an item (node index 4 -> 1)
                };
            return new PendingChangesCollection<WFloat>(additionalPendingChanges, true);
        }

        private static bool[] GetChildNodeIsCreated()
        {
            // the last child node for our unconnected node is not created
            return new bool[] { true, true, true, true, false };
        }

        private static Tuple<uint, uint>[] GetExpectedIndexRange(bool ascending, FilterOptions filterChoice)
        {
            const int subnodes = 5;
            // The number of items in the nodes, including pending changes and additional pending changes.
            uint[] origItems = new uint[subnodes] { 19, 31, 1, 39, 0 };
            int[] deltas = new int[subnodes] { 2, -1, 0, 0, 0 };
            int[] additional_deltas = new int[subnodes] { -1, 0, -1, 1, 1 };
            uint[] itemsInNodes = new uint[subnodes] { 20, 30, 0, 40, 1 }; // sum of above
            // Including additional pending changes, we will filter out (2, 3, 0, 3, 0) items in the respective groups that are multiples of 10, and that are multiples of 5 : (4, 6, 0, 7, 0).
            // Cumulatively, this means for ascending 10: (2, 5, 5, 8, 8) and 5: (4, 10, 10, 17, 17), and 10: (3, 3, 6, 8, 8) and 5: (7, 7, 13, 17, 17) descending.
            uint[] filteredOut = new uint[subnodes];
            if (filterChoice == FilterOptions.FilterOutIDsMultiplesOf10)
                filteredOut = new uint[subnodes] { 2, 3, 0, 3, 0 };
            else if (filterChoice == FilterOptions.FilterOutIDsMultiplesOf5)
                filteredOut = new uint[subnodes] { 4, 6, 0, 7, 0 };
            else
                filteredOut = new uint[subnodes] { 0, 0, 0, 0, 0 };

            List<Tuple<uint, uint>> expectedRanges = new List<Tuple<uint, uint>>();
            uint totalSoFar = 0;
            for (int i = 0; i < subnodes; i++)
            {
                int nodeIndex = ascending ? i : subnodes - i - 1;
                uint numItemsBeforeFiltering = itemsInNodes[nodeIndex];
                uint numItemsAfterFiltering = numItemsBeforeFiltering - filteredOut[nodeIndex];
                if (numItemsAfterFiltering > 0)
                    expectedRanges.Add(new Tuple<uint, uint>(totalSoFar, totalSoFar + numItemsAfterFiltering - 1));
                else
                    expectedRanges.Add(null);
                totalSoFar += numItemsAfterFiltering;
            }

            return expectedRanges.ToArray();
        }

        [Fact]
        public static async Task NumItemsInSubtreeWorks()
        {
            AnomalyType anomalyType = AnomalyType.NoAnomaly; // we will check our adjustments for this by looking for index ranges
            var nodeAndPendingChanges = await GetUnconnectedNodeAndAdditionalPendingChanges(anomalyType, false);
            var combinedBuffer = new CombinedInternalNodeBuffer<WFloat>(nodeAndPendingChanges.node, nodeAndPendingChanges.additionalPendingChanges);
            Tuple<int, uint, uint, uint>[] expectedItemsAtNode = new Tuple<int, uint, uint, uint>[] // index, items at, cumulative below, cumulative above
            {
                new Tuple<int, uint, uint, uint>(0, 20, 0, 71),
                new Tuple<int, uint, uint, uint>(1, 30, 20, 41),
                new Tuple<int, uint, uint, uint>(2, 0, 50, 41),
                new Tuple<int, uint, uint, uint>(3, 40, 50, 1),
                new Tuple<int, uint, uint, uint>(4, 1, 90, 0),
            };

            foreach (var expectation in expectedItemsAtNode)
            {
                nodeAndPendingChanges.node.Cumulator.NumItemsInSubtreeIncludingBuffer(combinedBuffer, expectation.Item1).Should().Be(expectation.Item2);
                nodeAndPendingChanges.node.Cumulator.CumulativeItemsInSubtreeBelowIndexIncludingBuffer(combinedBuffer, expectation.Item1).Should().Be(expectation.Item3);
                nodeAndPendingChanges.node.Cumulator.CumulativeItemsInSubtreeAboveIndexIncludingBuffer(combinedBuffer, expectation.Item1).Should().Be(expectation.Item4);
            }
        }
        public class NodeAndPendingChanges
        {
            public CountedInternalNode<WFloat> node;
            public PendingChangesCollection<WFloat> additionalPendingChanges;
        }

        public class NodeWithExpectations : NodeAndPendingChanges
        {
            public IncludedIndices[] includedIndices;
        }

        private static async Task<NodeAndPendingChanges> GetUnconnectedNodeAndAdditionalPendingChanges(AnomalyType anomalyType, bool includeUintSet)
        {
            NodeAndPendingChanges n = new NodeAndPendingChanges();
            n.node = await GetUnconnectedInternalNode(includeUintSet);
            n.additionalPendingChanges = GetAdditionalPendingChanges();
            if (anomalyType == AnomalyType.RedundantDeletion)
                n.additionalPendingChanges.AddPendingChanges(new PendingChange<WFloat>[] { new PendingChange<WFloat>(RedundantDeletionItem, true) }, true);
            // not testing redundant insertion in Internal, since it only should affect leaf behavior
            return n;
        }

        public enum FilterOptions : byte
        {
            NoFiltersAsNull,
            NoFiltersWithUintSet,
            FilterOutIDsMultiplesOf5,
            FilterOutIDsMultiplesOf10
        }

        public static UintSet GetFilter(FilterOptions choice)
        {
            if (choice == FilterOptions.NoFiltersAsNull)
                return null;
            UintSet u = new UintSet();
            for (int i = 0; i < 2500; i++)
                if (i % 5 != 0)
                    u.AddUint((uint)i);
                else
                {
                    bool filterOut = (choice == FilterOptions.FilterOutIDsMultiplesOf5 && i % 5 == 0) || (choice == FilterOptions.FilterOutIDsMultiplesOf10 && i % 10 == 0);
                    if (!filterOut)
                        u.AddUint((uint)i);
                }
            return u;
        }

        public static QueryFilter GetQueryFilter(bool useSupersetFilter, bool useFilteredSetFilter)
        {
            FilterOptions supersetFilterOption, filteredSetFilterOption;
            UintSet supersetFilter, filteredSetFilter;
            GetFilters(useSupersetFilter, useFilteredSetFilter, out supersetFilterOption, out filteredSetFilterOption, out supersetFilter, out filteredSetFilter);
            return GetQueryFilter(supersetFilter, filteredSetFilter);
        }

        private static void GetFilters(bool useSupersetFilter, bool useFilteredSetFilter, out FilterOptions supersetFilterOption, out FilterOptions filteredSetFilterOption, out UintSet supersetFilter, out UintSet filteredSetFilter)
        {
            supersetFilterOption = useSupersetFilter ? FilterOptions.FilterOutIDsMultiplesOf10 : FilterOptions.NoFiltersAsNull;
            filteredSetFilterOption = useFilteredSetFilter ? FilterOptions.FilterOutIDsMultiplesOf5 : FilterOptions.NoFiltersAsNull;
            supersetFilter = GetFilter(supersetFilterOption);
            filteredSetFilter = GetFilter(filteredSetFilterOption);
        }

        public static QueryFilter GetQueryFilter(UintSet supersetFilter, UintSet filteredSetFilter)
        {
            QueryFilter queryFilter = null;
            if (supersetFilter != null || filteredSetFilter != null)
                queryFilter = new QueryFilter(filteredSetFilter, supersetFilter == null ? QueryFilterRankingOptions.RankWithinAllItems : QueryFilterRankingOptions.RankWithinSupersetOfItems, supersetFilter);
            return queryFilter;
        }

        private static async Task<NodeWithExpectations> GetNodeWithExpectations(AnomalyType anomalyType, bool ascending, bool useUintSetWithLoc, FilterOptions filterChoiceSuperset, FilterOptions filterChoiceFilteredset)
        {
            // the expected ranges account for the original node, including the main buffer, as well as the additional pending changes (which may include a redundant deletion), which will form the request buffer.
            var nodeAndPendingChanges = await GetUnconnectedNodeAndAdditionalPendingChanges(anomalyType, useUintSetWithLoc);
            var n = new NodeWithExpectations { node = nodeAndPendingChanges.node, additionalPendingChanges = nodeAndPendingChanges.additionalPendingChanges };
            Tuple<uint, uint>[] expectedIndexRangeSuperset = GetExpectedIndexRange(ascending, filterChoiceSuperset);
            Tuple<uint, uint>[] expectedIndexRangeFilteredSet = GetExpectedIndexRange(ascending, filterChoiceFilteredset);
            IncludedIndices[] includedIndices = expectedIndexRangeSuperset.Zip(expectedIndexRangeFilteredSet, (s, f) => s == null || f == null ? null : new IncludedIndices(s.Item1, s.Item2, f.Item1, f.Item2)).ToArray();
            n.includedIndices = includedIndices;
            return n;
        }

        [Fact]
        public static async Task InternalQueriesWork()
        {
            foreach (bool ascending in new bool[] { true, false })
                foreach (uint skip in new uint[] { 0, 10, 20, 30, 40, 50 })
                    foreach (uint? take in new uint?[] { null, 0, 5, 20, 100 })
                        foreach (float? min in new float?[] { null, 1.5F, 3.0F, 3.5F })
                            foreach (float? max in new float?[] { null, 5.5F, 6.0F, 6.8F })
                            {
                                bool[] useFiltersOptions = new bool[] { false };
                                if (min == null && max == null)
                                    useFiltersOptions = new bool[] { false, true }; // we can use filters only if min & max are null
                                foreach (bool useFilters in useFiltersOptions)
                                    await CheckInternalQuery_Helper(ascending, skip, take, min, max, true, useFilters); 
                            }
        }

        internal static async Task CheckInternalQuery_Helper(bool ascending, uint skip, uint? take, float? min, float? max, bool useUintSetWithLoc, bool useFilters)
        {
            if ((min != null || max != null) && useFilters)
                throw new Exception("Can't use filters with min/max specified, because value range queries do not use filters.");
            FilterOptions supersetFilterOption, filteredSetFilterOption;
            UintSet supersetFilter, filteredSetFilter;
            GetFilters(useFilters, useFilters, out supersetFilterOption, out filteredSetFilterOption, out supersetFilter, out filteredSetFilter);
            var n = await GetNodeWithExpectations(AnomalyType.NoAnomaly, ascending, useUintSetWithLoc, supersetFilterOption, filteredSetFilterOption);
            var nodeValueRanges = n.node.ChildNodeInfos.Select(x => new Tuple<float, float>(x.FirstExclusive?.Key ?? float.MinValue, x.LastInclusive?.Key ?? float.MaxValue)).ToArray();
            var childNodeIDs = n.node.ChildNodeInfos.Select(x => x.NodeID).ToArray();
            var nodePendingChanges = n.node.MainBuffer.AllPendingChanges(); // all the pending changes for this node (we'll take subset of these and additional changes for each child in GetExpectedFurtherQueries)
            var subsetOfAdditionalPendingChanges = new PendingChangesCollection<WFloat>(n.additionalPendingChanges.AsEnumerable(), true);
            var childNodesCreated = GetChildNodeIsCreated();
            var expectedFurtherQueries = GetExpectedFurtherQueries(nodePendingChanges, subsetOfAdditionalPendingChanges, childNodeIDs, n.includedIndices, childNodesCreated, nodeValueRanges, skip, take, min, max, ascending, supersetFilter, filteredSetFilter).ToList();
            NodeQueryLinearBase<WFloat> q = GetQueryForNode(ascending, true, skip, take, min, max, n.node.NodeInfo.NodeID, n.additionalPendingChanges, new IncludedIndices(n.includedIndices.First().FirstIndexInSuperset, n.includedIndices.Last().LastIndexInSuperset, n.includedIndices.First().FirstIndexInFilteredSet, n.includedIndices.Last().LastIndexInFilteredSet), supersetFilter, filteredSetFilter);
            if (q == null)
                expectedFurtherQueries.Count().Should().Be(0);
            else
            {
                var result = await n.node.ProcessQuery(q) as NodeResultLinearFurtherQueries<WFloat>;
                result.Should().NotBeNull();
                result.FurtherQueries.Count().Should().Be(expectedFurtherQueries.Count());
                for (int i = 0; i < result.FurtherQueries.Count(); i++)
                    result.FurtherQueries[i].Equals(expectedFurtherQueries[i]).Should().BeTrue();
            }
        }

        private static NodeQueryLinearBase<WFloat> GetQueryForNode(bool ascending, bool nodeExists, uint skip, uint? take, float? min, float? max, long nodeID, PendingChangesCollection<WFloat> additionalPendingChanges, IncludedIndices includedIndices, UintSet supersetFilter, UintSet filteredSetFilter)
        {
            var exectedNumItemsToReturn = includedIndices.LastIndexInFilteredSet - includedIndices.FirstIndexInFilteredSet + 1;
            if (take == 0 || exectedNumItemsToReturn <= skip)
                return null;
            NodeQueryLinearBase<WFloat> q;
            if (take > exectedNumItemsToReturn)
                take = exectedNumItemsToReturn;
            QueryFilter queryFilter = GetQueryFilter(supersetFilter, filteredSetFilter);
            if (supersetFilter != null)
            {
                var changesPassingSuperset = additionalPendingChanges.AsEnumerable().Where(x => supersetFilter.Contains(x.Item.ID));
                additionalPendingChanges = new PendingChangesCollection<WFloat>(changesPassingSuperset, true);
            }

            if (min == null && max == null)
                q = new NodeQueryIndexRange<WFloat>(nodeID, nodeExists, ascending, skip, take, includedIndices, additionalPendingChanges, QueryResultType.KeysAndIDs, queryFilter);
            else
                q = new NodeQueryValueRange<WFloat>(nodeID, nodeExists, ascending, skip, take, includedIndices, additionalPendingChanges, QueryResultType.KeysAndIDs, min == null ? null : (KeyAndID<WFloat>?) new KeyAndID<WFloat>((float)min, uint.MinValue), max == null ? null : (KeyAndID<WFloat>?) new KeyAndID<WFloat>((float)max, uint.MaxValue));
            return q;
        }

        private static PendingChangesCollection<WFloat> GetApplicablePendingChanges(PendingChangesCollection<WFloat> firstSet, PendingChangesCollection<WFloat> secondSet, float? minValue, float? maxValue)
        {
            var pcc = new PendingChangesCollection<WFloat>(new List<IEnumerable<PendingChange<WFloat>>>() { firstSet.AsEnumerable(), secondSet.AsEnumerable() });
            var some = pcc.AsEnumerable().Where(x => (minValue == null || x.Item.Key >= minValue) && (maxValue == null || x.Item.Key <= maxValue)).ToArray();
            return new PendingChangesCollection<WFloat>(some, false);
        }

        private static IEnumerable<NodeQueryLinearBase<WFloat>> GetExpectedFurtherQueries(PendingChangesCollection<WFloat> nodePendingChanges, PendingChangesCollection<WFloat> additionalPendingChanges, long[] childNodeIDs, IncludedIndices[] expectedIncludedIndices, bool[] nodeIsCreated, Tuple<float, float>[] nodeValueRanges, uint skip, uint? take, float? min, float? max, bool ascending, UintSet supersetFilter, UintSet filteredSetFilter)
        {
            var numNodes = expectedIncludedIndices.Length;
            var nodeRange = Enumerable.Range(0, numNodes).ToList();
            if (!ascending)
            {
                nodeRange.Reverse();
                expectedIncludedIndices = expectedIncludedIndices.Reverse().ToArray(); // we have expected index ranges in backwards order, but since we're going to go backward through the array, we need to shift it back to forwards order
            }
            int[] numItemsInNodeSuperset = expectedIncludedIndices.Select(x => x == null ? 0 : (int)(x.LastIndexInSuperset - x.FirstIndexInSuperset) + 1).ToArray();
            int[] numItemsInNodeFiltered = expectedIncludedIndices.Select(x => x == null ? 0 : (int)(x.LastIndexInFilteredSet - x.FirstIndexInFilteredSet) + 1).ToArray();
            Tuple<float, float> searchRange = new Tuple<float, float>(min ?? float.MinValue, max ?? float.MaxValue);
            bool[] nodeIsCompletelyInSearchRange = nodeValueRanges.Select(n => RangeOverlap<WFloat>.Item1IsCompletelyInItem2(n.Item1, n.Item2, searchRange.Item1, searchRange.Item2, true, false, false, false)).ToArray();
            bool[] nodeIsAtLeastPartlyInSearchRange = nodeValueRanges.Select(n => RangeOverlap<WFloat>.SomeOverlapExists(n.Item1, n.Item2, searchRange.Item1, searchRange.Item2, true, false, false, false)).ToArray();
            int[] minMatchesInNode = Enumerable.Range(0, numNodes).Select(n => nodeIsCompletelyInSearchRange[n] ? numItemsInNodeFiltered[n] : 0).ToArray();
            int[] maxMatchesInNodes = Enumerable.Range(0, numNodes).Select(n => nodeIsAtLeastPartlyInSearchRange[n] ? numItemsInNodeFiltered[n] : 0).ToArray();
            uint numDefinitivelyTakenSoFar = 0, numTriedToSkipSoFar = 0, indicesAlreadyProcessedSuperset = 0, indicesAlreadyProcessedFiltered = 0;
            foreach (int n in nodeRange)
            {
                if (expectedIncludedIndices[n] != null)
                {
                    uint numIndicesThisNodeSuperset = (uint)numItemsInNodeSuperset[n];
                    uint numIndicesThisNodeFiltered = (uint)numItemsInNodeFiltered[n];
                    if (maxMatchesInNodes[n] != 0 && (take == null || (numDefinitivelyTakenSoFar < (float)take)))
                    {
                        uint numToTryToSkipThisNode = 0;
                        uint? numToTryToTakeAfterSkipThisNode = null;
                        CalculateSkipAndTakeForNode(numItemsInNodeFiltered[n], skip - numTriedToSkipSoFar, take - numDefinitivelyTakenSoFar, minMatchesInNode[n], ref numTriedToSkipSoFar, ref numDefinitivelyTakenSoFar, out numToTryToSkipThisNode, out numToTryToTakeAfterSkipThisNode);
                        if (numToTryToTakeAfterSkipThisNode == null || numToTryToTakeAfterSkipThisNode > 0)
                        {
                            PendingChangesCollection<WFloat> pendingChangesForChild = GetApplicablePendingChanges(nodePendingChanges, additionalPendingChanges, nodeValueRanges[n].Item1, nodeValueRanges[n].Item2);
                            NodeQueryLinearBase<WFloat> q = GetQueryForNode(ascending, nodeIsCreated[n], (uint)numToTryToSkipThisNode, numToTryToTakeAfterSkipThisNode, min, max, childNodeIDs[n], pendingChangesForChild, new IncludedIndices(indicesAlreadyProcessedSuperset, indicesAlreadyProcessedSuperset + numIndicesThisNodeSuperset - 1, indicesAlreadyProcessedFiltered, indicesAlreadyProcessedFiltered + numIndicesThisNodeFiltered - 1), supersetFilter, filteredSetFilter);
                            if (q != null)
                                yield return q;
                        }
                    }
                    indicesAlreadyProcessedSuperset += numIndicesThisNodeSuperset;
                    indicesAlreadyProcessedFiltered += numIndicesThisNodeFiltered;
                }
            }
        }

        private static void CalculateSkipAndTakeForNode(int numItemsInThisNode, uint skipRemaining, uint? takeRemaining, int minMatchesInNode, ref uint numTriedToSkipSoFar, ref uint numDefinitivelyTakenSoFar, out uint numToTryToSkip, out uint? numToTryToTakeAfterSkip)
        {
            uint numToTryToTakeIgnoringSkip;
            if (takeRemaining == null)
                numToTryToTakeIgnoringSkip = (uint)numItemsInThisNode;
            else
                numToTryToTakeIgnoringSkip = Math.Min((uint)numItemsInThisNode, (uint)takeRemaining);
            numToTryToSkip = Math.Min((uint)numItemsInThisNode, skipRemaining);
            if (takeRemaining == null)
                numToTryToTakeAfterSkip = (uint)numItemsInThisNode - numToTryToSkip;
            else
                numToTryToTakeAfterSkip = Math.Min((uint)numItemsInThisNode - numToTryToSkip, (uint)takeRemaining);
            uint numDefinitivelySkipped = Math.Min(numToTryToSkip, (uint)minMatchesInNode);
            numTriedToSkipSoFar += (uint)numToTryToSkip;
            uint numDefinitivelyTaken = Math.Min((uint)numToTryToTakeAfterSkip, (uint)Math.Max(minMatchesInNode - (int)numToTryToSkip, 0));
            numDefinitivelyTakenSoFar += (uint)numDefinitivelyTaken;
            if (takeRemaining == null)
                numToTryToTakeAfterSkip = null;
        }

        static KeyAndID<WFloat> RedundantDeletionItem = new KeyAndID<WFloat>(7.5F, 999); // deleting something not really there


    }
}
