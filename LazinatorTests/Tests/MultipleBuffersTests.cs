using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using LazinatorTests.Examples;
using Lazinator.Core;
using Xunit;
using Lazinator.Wrappers;
using LazinatorTests.Examples.Hierarchy;
using LazinatorTests.Examples.Structs;
using LazinatorTests.Examples.NonAbstractGenerics;
using LazinatorCollections.Tuples;
using Lazinator.Buffers;
using LazinatorTests.Examples.ExampleHierarchy;
using System.Threading.Tasks;
using System.Diagnostics;
using LazinatorCollections.Tree;
using LazinatorTests.Utilities;
using System.Buffers;
using Lazinator.Persistence;

namespace LazinatorTests.Tests
{
    public class MultipleBuffersTests : SerializationDeserializationTestBase
    {
        [Fact]
        public void SplittableEntitiesWork()
        {
            Example e = GetTypicalExample();
            LazinatorMemory singleBufferResult = e.SerializeLazinator(new LazinatorSerializationOptions(IncludeChildrenMode.IncludeAllChildren, false, false, false));
            LazinatorMemory multipleBufferResult = e.SerializeLazinator(new LazinatorSerializationOptions(IncludeChildrenMode.IncludeAllChildren, false, false, false, 10));
            multipleBufferResult.MoreOwnedMemory.Count().Should().BeGreaterThan(0);
            LazinatorMemory consolidated = multipleBufferResult.GetConsolidatedMemory();
            consolidated.Matches(singleBufferResult.InitialMemory.Span).Should().BeTrue();

            Example e2 = new Example(consolidated);
            ExampleEqual(e, e2).Should().BeTrue();

            Example e3 = new Example(multipleBufferResult);
            ExampleEqual(e, e3).Should().BeTrue();

            Example e4 = new Example(multipleBufferResult);
            Example e5 = e4.CloneLazinatorTyped();
            ExampleEqual(e, e5).Should().BeTrue();
        }

        [Theory]
        [ClassData(typeof(BoolPermutations_4))]
        public async Task SplittableEntitiesSaveToBlobs(bool containedInSingleBlob, bool useFile, bool async, bool recreateBlobMemoryReference)
        {
            if (async)
                await SplittableEntitiesSavedHelper_Async(containedInSingleBlob, useFile, recreateBlobMemoryReference);
            else
                SplittableEntitiesSavedHelper(containedInSingleBlob, useFile, recreateBlobMemoryReference);
        }

        private void SplittableEntitiesSavedHelper(bool containedInSingleBlob, bool useFile, bool recreateIndex)
        {
            Example e = GetTypicalExample();
            LazinatorMemory multipleBufferResult = e.SerializeLazinator(new LazinatorSerializationOptions(IncludeChildrenMode.IncludeAllChildren, false, false, false, 10));

            // Write to one or more blobs
            IBlobManager blobManager = useFile ? new FileBlobManager() : new InMemoryBlobStorage();
            string fullPath = GetPathForIndexAndBlobs(useFile, false);
            if (fullPath == null)
                return;
            PersistentIndex index = new PersistentIndex(fullPath, blobManager, containedInSingleBlob);
            index.PersistLazinatorMemory(multipleBufferResult);
            // Note: Index reference is first var indexReference = memoryReferenceInBlobs[0];

            // Read from one or more blobs
            if (recreateIndex)
                index = PersistentIndex.ReadFromBlob(blobManager, fullPath, null, 0);
            var revisedMemory = index.GetLazinatorMemory();

            var e2 = new Example(revisedMemory);
            ExampleEqual(e, e2).Should().BeTrue();
        }

        private async Task SplittableEntitiesSavedHelper_Async(bool containedInSingleBlob, bool useFile, bool recreateIndex)
        {
            Example e = GetTypicalExample();
            LazinatorMemory multipleBufferResult = await e.SerializeLazinatorAsync(new LazinatorSerializationOptions(IncludeChildrenMode.IncludeAllChildren, false, false, false, 10));

            // Write to one or more blobs
            IBlobManager blobManager = useFile ? new FileBlobManager() : new InMemoryBlobStorage();
            string fullPath = GetPathForIndexAndBlobs(useFile, false);
            if (fullPath == null)
                return;
            PersistentIndex index = new PersistentIndex(fullPath, blobManager, containedInSingleBlob);
            await index.PersistLazinatorMemoryAsync(multipleBufferResult);
            // Note: Index reference is first var indexReference = memoryReferenceInBlobs[0];

            // Read from one or more blobs
            if (recreateIndex)
                index = await PersistentIndex.ReadFromBlobAsync(blobManager, fullPath, null, 0);
            var revisedMemory = await index.GetLazinatorMemoryAsync();

            Example e2 = new Example(revisedMemory);
            ExampleEqual(e, e2).Should().BeTrue();
        }

        private SortedList<double, string> RegularSortedList;
        private List<double> MainListValues => RegularSortedList.Select(x => x.Key).ToList();
        private void AddToMainList(double d) => RegularSortedList.Add(d, "");
        private void DeleteFromMainList(double d) => RegularSortedList.Remove(d);

        private LazinatorBinaryTree<WDouble> BinaryTree;
        private List<double> BinaryTreeValues => BinaryTree.Select(x => x.WrappedValue).ToList();
        private void AddToBinaryTree(double d) => BinaryTree.Add(d);
        private void DeleteFromBinaryTree(double d) => BinaryTree.Remove(d);
        private int Count = 0;

        private void RandomAddition(Random r)
        {
            double d = r.NextDouble();
            AddToMainList(d);
            AddToBinaryTree(d);
            Count++;
        }

        private void RandomDeletion(Random r)
        {
            if (Count > 0)
            {
                int i = r.Next(Count);
                var key = RegularSortedList.Keys[i];
                DeleteFromMainList(key);
                DeleteFromBinaryTree(key);
                Count--;
            }
        }

        private void Initialize()
        {
            RegularSortedList = new SortedList<double, string>();
            BinaryTree = new LazinatorBinaryTree<WDouble>();
            Count = 0;
        }

        private void ConfirmSame()
        {
            if (!MainListValues.SequenceEqual(BinaryTreeValues))
                throw new Exception("Binary tree failed");
        }

        private void RandomChanges(int numChanges, Random r)
        {
            for (int i = 0; i < numChanges; i++)
            {
                if (r.NextDouble() < 0.30)
                    RandomDeletion(r);
                else
                    RandomAddition(r);
            }
        }

        private void MultipleRoundsOfRandomChanges(int numRounds, int numChangesFirstRound, int numChangesLaterRounds, Action postRoundAction)
        {
            Random r = new Random(0);
            Initialize();
            for (int i = 0; i < numRounds; i++)
            {
                RandomChanges(i == 0 ? numChangesFirstRound : numChangesLaterRounds, r);
                postRoundAction();
            }
            ConfirmSame();
        }

        private async Task MultipleRoundsOfRandomChangesAsync(int numRounds, int numChangesFirstRound, int numChangesLaterRounds, Func<ValueTask> postRoundAction)
        {
            Random r = new Random(0);
            Initialize();
            for (int i = 0; i < numRounds; i++)
            {
                RandomChanges(i == 0 ? numChangesFirstRound : numChangesLaterRounds, r);
                await postRoundAction();
            }
            ConfirmSame();
        }

        [Fact]
        public void BinaryTreeTest()
        {
            MultipleRoundsOfRandomChanges(5, 100, 100, () => { });
        }


        [Fact]
        public void BinaryTreeTest_Cloning()
        {
            MultipleRoundsOfRandomChanges(5, 100, 100, () => { BinaryTree = BinaryTree.CloneLazinatorTyped(); });
        }



        [Fact]
        public void BinaryTreeTest_RecreatingManually()
        {
            // This test both confirms that binary trees work without diff serialization and also helps show what the memory should look like
            // at each stage. This makes it easier to find problems if tests using diff serialization fail.
            List<PersistentIndex> indices = new List<PersistentIndex>();
            int round = 0;
            MultipleRoundsOfRandomChanges(3, 2, 1, () => // DEBUG -- try higher numbers
            {
                Debug.WriteLine($"Round {round}");
                LazinatorMemory lazinatorMemory = BinaryTree.SerializeLazinator(LazinatorSerializationOptions.Default);

                Debug.WriteLine($"Consolidated{round++}: " + lazinatorMemory.ToStringConsolidated());

                BinaryTree = new LazinatorBinaryTree<WDouble>(lazinatorMemory);
            });
        }

        [Theory]
        [ClassData(typeof(BoolPermutations_3))]
        public void BinaryTreeTest_ReloadingFromBlobs(bool useFile, bool containedInSingleBlob, bool recreateIndex)
        {
            MultipleRoundsOfRandomChanges(10, 10, 10, () =>
            {
                LazinatorMemory multipleBufferResult = BinaryTree.SerializeLazinator(new LazinatorSerializationOptions(IncludeChildrenMode.IncludeAllChildren, false, false, false, 5));

                // Write to one or more blobs
                IBlobManager blobManager = useFile ? new FileBlobManager() : new InMemoryBlobStorage();
                string fullPath = GetPathForIndexAndBlobs(useFile, true);
                if (fullPath == null)
                    return;
                PersistentIndex index = new PersistentIndex(fullPath, blobManager, containedInSingleBlob);
                index.PersistLazinatorMemory(multipleBufferResult);

                if (recreateIndex)
                    index = PersistentIndex.ReadFromBlob(blobManager, fullPath, null, 0);
                var revisedMemory = index.GetLazinatorMemory();
                BinaryTree = new LazinatorBinaryTree<WDouble>(revisedMemory);
            });
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, true, false)]
        [InlineData(true, false, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, true)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        [InlineData(false, false, false)]
        public async Task BinaryTreeTest_ReloadingFromBlobsAsync(bool useFile, bool containedInSingleBlob, bool recreateIndex)
        {
            await MultipleRoundsOfRandomChangesAsync(10, 10, 10, async () =>
            {
                LazinatorMemory multipleBufferResult = BinaryTree.SerializeLazinator(new LazinatorSerializationOptions(IncludeChildrenMode.IncludeAllChildren, false, false, false, 5));

                // Write to one or more blobs
                IBlobManager blobManager = useFile ? new FileBlobManager() : new InMemoryBlobStorage();
                string fullPath = GetPathForIndexAndBlobs(useFile, true);
                if (fullPath == null)
                    return;
                PersistentIndex index = new PersistentIndex(fullPath, blobManager, containedInSingleBlob);
                await index.PersistLazinatorMemoryAsync(multipleBufferResult);

                if (recreateIndex)
                    index = await PersistentIndex.ReadFromBlobAsync(blobManager, fullPath, null, 0);
                var revisedMemory = await index.GetLazinatorMemoryAsync();
                BinaryTree = new LazinatorBinaryTree<WDouble>(revisedMemory);
            });
        }


        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, true, false)]
        [InlineData(true, false, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, true)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        [InlineData(false, false, false)]
        public void BinaryTreeTest_DiffSerialization_RewritingAll(bool useFile, bool containedInSingleBlob, bool recreateIndex)
        {
            // Here, we test making a change that in fact requires rewriting the entire Lazinator object. The change will be to the 
            // only node's Data, and the node has no Left or Right, so no references to the original data can be made. 

            var tree1 = new LazinatorBinaryTree<WByte>();
            tree1.Add(1);
            LazinatorMemory initialResult = tree1.SerializeLazinator(LazinatorSerializationOptions.Default);

            var tree2 = new LazinatorBinaryTree<WByte>(initialResult);
            tree2.Root.Data = 2;
            LazinatorSerializationOptions options = new LazinatorSerializationOptions(IncludeChildrenMode.IncludeAllChildren, false, false, true, 20);
            LazinatorMemory afterChange = tree2.SerializeLazinator(options);

            IBlobManager blobManager = useFile ? new FileBlobManager() : new global::LazinatorTests.Utilities.InMemoryBlobStorage();
            string fullPath = GetPathForIndexAndBlobs(useFile, true);
            PersistentIndex index = new PersistentIndex(fullPath, blobManager, containedInSingleBlob);
            index.PersistLazinatorMemory(afterChange);

            if (recreateIndex)
                index = PersistentIndex.ReadFromBlob(blobManager, fullPath, null, 0);
            var afterChangeReloaded = index.GetLazinatorMemory();
            var tree3 = new LazinatorBinaryTree<WByte>(afterChangeReloaded);
            tree3.Root.Data.WrappedValue.Should().Be((byte)2);

            var tree4 = new LazinatorBinaryTree<WByte>(afterChange);
            tree4.Root.Data.WrappedValue.Should().Be((byte)2);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, true, false)]
        [InlineData(true, false, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, true)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        [InlineData(false, false, false)]
        public async Task BinaryTreeTest_DiffSerialization_RewritingAll_Async(bool useFile, bool containedInSingleBlob, bool recreateIndex)
        {
            // Here, we test making a change that in fact requires rewriting the entire Lazinator object. The change will be to the 
            // only node's Data, and the node has no Left or Right, so no references to the original data can be made. 

            var tree1 = new LazinatorBinaryTree<WByte>();
            tree1.Add(1);
            LazinatorMemory initialResult = await tree1.SerializeLazinatorAsync(LazinatorSerializationOptions.Default);

            var tree2 = new LazinatorBinaryTree<WByte>(initialResult);
            tree2.Root.Data = 2;
            LazinatorSerializationOptions options = new LazinatorSerializationOptions(IncludeChildrenMode.IncludeAllChildren, false, false, true, 20);
            LazinatorMemory afterChange = await tree2.SerializeLazinatorAsync(options);

            IBlobManager blobManager = useFile ? new FileBlobManager() : new global::LazinatorTests.Utilities.InMemoryBlobStorage();
            string fullPath = GetPathForIndexAndBlobs(useFile, true);
            PersistentIndex index = new PersistentIndex(fullPath, blobManager, containedInSingleBlob);
            await index.PersistLazinatorMemoryAsync(afterChange);

            if (recreateIndex)
                index = PersistentIndex.ReadFromBlob(blobManager, fullPath, null, 0);
            var afterChangeReloaded = await index.GetLazinatorMemoryAsync();
            var tree3 = new LazinatorBinaryTree<WByte>(afterChangeReloaded);
            tree3.Root.Data.WrappedValue.Should().Be((byte)2);

            var tree4 = new LazinatorBinaryTree<WByte>(afterChange);
            tree4.Root.Data.WrappedValue.Should().Be((byte)2);
        }


        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, true, false)]
        [InlineData(true, false, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, true)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        [InlineData(false, false, false)]
        public void BinaryTreeTest_DiffSerialization_KeepingSomeInformation(bool useFile, bool containedInSingleBlob, bool recreateIndex)
        {
            // Here, we test making a change that in fact requires rewriting the entire Lazinator object. The change will be to the 
            // only node's Data, and the node has no Left or Right, so no references to the original data can be made. 

            var tree1 = new LazinatorBinaryTree<WByte>();
            tree1.Add(1);
            tree1.Add(3); // right node to above
            LazinatorMemory initialResult = tree1.SerializeLazinator(LazinatorSerializationOptions.Default);

            var tree2 = new LazinatorBinaryTree<WByte>(initialResult);
            tree2.Root.Data = 2; // should now be 2 and 3
            LazinatorSerializationOptions options = new LazinatorSerializationOptions(IncludeChildrenMode.IncludeAllChildren, false, false, true, 20);
            LazinatorMemory afterChange = tree2.SerializeLazinator(options);

            IBlobManager blobManager = useFile ? new FileBlobManager() : new InMemoryBlobStorage();
            string fullPath = GetPathForIndexAndBlobs(useFile, true);
            PersistentIndex index = new PersistentIndex(fullPath, blobManager, containedInSingleBlob);
            index.PersistLazinatorMemory(afterChange);

            if (recreateIndex)
                index = PersistentIndex.ReadFromBlob(blobManager, fullPath, null, 0);
            var afterChangeReloaded = index.GetLazinatorMemory();
            var tree3 = new LazinatorBinaryTree<WByte>(afterChangeReloaded);
            tree3.Root.Data.WrappedValue.Should().Be((byte)2);
            tree3.Root.RightNode.Data.WrappedValue.Should().Be((byte)3);


            var tree4 = new LazinatorBinaryTree<WByte>(afterChange);
            tree4.Root.Data.WrappedValue.Should().Be((byte)2);
            tree4.Root.RightNode.Data.WrappedValue.Should().Be((byte)3);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, true, false)]
        [InlineData(true, false, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, true)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        [InlineData(false, false, false)]
        public async Task BinaryTreeTest_DiffSerialization_KeepingSomeInformation_Async(bool useFile, bool containedInSingleBlob, bool recreateIndex)
        {
            // Here, we test making a change that in fact requires rewriting the entire Lazinator object. The change will be to the 
            // only node's Data, and the node has no Left or Right, so no references to the original data can be made. 

            var tree1 = new LazinatorBinaryTree<WByte>();
            tree1.Add(1);
            tree1.Add(3); // right node to above
            LazinatorMemory initialResult = await tree1.SerializeLazinatorAsync(LazinatorSerializationOptions.Default);

            var tree2 = new LazinatorBinaryTree<WByte>(initialResult);
            tree2.Root.Data = 2; // should now be 2 and 3
            LazinatorSerializationOptions options = new LazinatorSerializationOptions(IncludeChildrenMode.IncludeAllChildren, false, false, true, 20);
            LazinatorMemory afterChange = await tree2.SerializeLazinatorAsync(options);

            IBlobManager blobManager = useFile ? new FileBlobManager() : new InMemoryBlobStorage();
            string fullPath = GetPathForIndexAndBlobs(useFile, true);
            PersistentIndex index = new PersistentIndex(fullPath, blobManager, containedInSingleBlob);
            await index.PersistLazinatorMemoryAsync(afterChange);

            if (recreateIndex)
                index = await PersistentIndex.ReadFromBlobAsync(blobManager, fullPath, null, 0);
            var afterChangeReloaded = await index.GetLazinatorMemoryAsync();
            var tree3 = new LazinatorBinaryTree<WByte>(afterChangeReloaded);
            tree3.Root.Data.WrappedValue.Should().Be((byte)2);
            tree3.Root.RightNode.Data.WrappedValue.Should().Be((byte)3);


            var tree4 = new LazinatorBinaryTree<WByte>(afterChange);
            tree4.Root.Data.WrappedValue.Should().Be((byte)2);
            tree4.Root.RightNode.Data.WrappedValue.Should().Be((byte)3);
        }

        [Theory]
        [ClassData(typeof(BoolPermutations_3))]
        public void PersistentIndexTest(bool useFile, bool containedInSingleBlob, bool recreateIndex)
        {
            List<byte> fullSequence = new List<byte>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            IMemoryOwner<byte> initialBytes = new SimpleMemoryOwner<byte>(new byte[3] { 1, 2, 3 });
            IMemoryOwner<byte> nextBytes = new SimpleMemoryOwner<byte>(new byte[3] { 4, 5, 6 });
            MemoryChunk nextBytesAsChunk = new MemoryChunk(nextBytes, new MemoryChunkReference(1, 0, 3), false);
            IMemoryOwner<byte> lastBytes = new SimpleMemoryOwner<byte>(new byte[4] { 7, 8, 9, 10 });
            MemoryChunk lastBytesAsChunk = new MemoryChunk(lastBytes, new MemoryChunkReference(2, 0, 4), false);
            LazinatorMemory initialMemory = new LazinatorMemory(initialBytes);
            LazinatorMemory memory1 = initialMemory.WithAppendedChunk(nextBytesAsChunk).WithAppendedChunk(lastBytesAsChunk);

            IBlobManager blobManager = useFile ? new FileBlobManager() : new InMemoryBlobStorage();
            string fullPath = GetPathForIndexAndBlobs(useFile, true);
            if (fullPath == null)
                return;
            PersistentIndex index = new PersistentIndex(fullPath, blobManager, containedInSingleBlob);
            index.PersistLazinatorMemory(memory1);

            if (recreateIndex)
                index = PersistentIndex.ReadFromBlob(blobManager, fullPath, null, 0);
            var memory2 = index.GetLazinatorMemory();
            var memory2List = memory2.GetConsolidatedMemory().ToArray().ToList();
            memory2List.SequenceEqual(fullSequence).Should().BeTrue();

            BinaryBufferWriter writer = new BinaryBufferWriter(0, memory2);
            writer.Write((byte)11);
            writer.Write((byte)12);
            writer.InsertReferenceToCompletedMemory(2, 1, 2); // 8, 9
            writer.InsertReferenceToCompletedMemory(0, 0, 3); // 1, 2, 3
            writer.InsertReferenceToCompletedMemory(1, 1, 1); // 5
            writer.InsertReferenceToCompletedMemory(2, 0, 2); // 7, 8
            writer.Write((byte)13);
            var memory3 = writer.LazinatorMemory;
            var memory3List = memory3.GetConsolidatedMemory().ToArray().ToList();
            List<byte> expected = new List<byte>() { 11, 12, 8, 9, 1, 2, 3, 5, 7, 8, 13 };
            memory3List.SequenceEqual(expected).Should().BeTrue();

        }

        [Theory]
        [ClassData(typeof(BoolPermutations_5))]
        public void BinaryTreeTest_DiffSerialization(bool useFile, bool containedInSingleBlob, bool recreateIndex, bool alwaysCopyPreviousBuffer_ProducingFakeDiff, bool useConsolidatedMemory)
        {
            List<PersistentIndex> indices = new List<PersistentIndex>();
            int round = 0;
            MultipleRoundsOfRandomChanges(3, 2, 1, () => // DEBUG -- try higher numbers
            {
                Debug.WriteLine($"Round {round}");

                LazinatorSerializationOptions options = alwaysCopyPreviousBuffer_ProducingFakeDiff ?  new LazinatorSerializationOptions(IncludeChildrenMode.IncludeAllChildren, false, false, true, int.MaxValue, int.MaxValue) : new LazinatorSerializationOptions(IncludeChildrenMode.IncludeAllChildren, false, false, true, 20, 5); // DEBUG
                LazinatorMemory multipleBufferResult = BinaryTree.SerializeLazinator(options);

                // Write to one or more blobs
                IBlobManager blobManager = useFile ? new FileBlobManager() : new InMemoryBlobStorage();
                string fullPath = GetPathForIndexAndBlobs(useFile, true);
                if (fullPath == null)
                    return;
                var index = (indices == null || !indices.Any()) ? new PersistentIndex(fullPath, blobManager, containedInSingleBlob) : new PersistentIndex(indices.Last());
                index.PersistLazinatorMemory(multipleBufferResult);
                indices.Add(index);

                if (recreateIndex)
                    index = PersistentIndex.ReadFromBlob(blobManager, fullPath, null, 0);
                var revisedMemory = index.GetLazinatorMemory();
                if (useConsolidatedMemory)
                {
                    var consolidatedMemory = revisedMemory.GetConsolidatedMemory();
                    revisedMemory = new LazinatorMemory(consolidatedMemory);
                    indices.RemoveAt(0); // with consolidated memory, we're not using diff serialization
                }

                Debug.WriteLine(revisedMemory.ToStringByChunk());
                Debug.WriteLine($"Consolidated{round}: " + revisedMemory.ToStringConsolidated());

                BinaryTree = new LazinatorBinaryTree<WDouble>(revisedMemory);
                round++;
            });
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, true, false)]
        [InlineData(true, false, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, true)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        [InlineData(false, false, false)]
        public async Task BinaryTreeTest_DiffSerializationAsync(bool useFile, bool containedInSingleBlob, bool recreateIndex)
        {
            throw new Exception();
        }

        // DEBUG -- we also need to have an index updated over multiple rounds and try at different times.

        private static string GetPathForIndexAndBlobs(bool useFile, bool binaryTree)
        {
            string path = @"C:\Users\Admin\Desktop\testfolder";
            if (useFile && !System.IO.Directory.Exists(path))
                return null; // ignore this error
            string fullPath = path + (binaryTree ? @"\binary-tree.fil" : @"\example.fil");
            return fullPath;
        }
    }
}
