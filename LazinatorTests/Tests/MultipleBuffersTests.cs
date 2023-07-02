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
using Lazinator.Collections.Tuples;
using Lazinator.Buffers;
using LazinatorTests.Examples.ExampleHierarchy;
using System.Threading.Tasks;
using System.Diagnostics;
using Lazinator.Collections.Tree;
using LazinatorTests.Utilities;
using System.Buffers;
using Lazinator.Persistence;
using Lazinator.Collections;

namespace LazinatorTests.Tests
{
    public class MultipleBuffersTests : SerializationDeserializationTestBase
    {
        [Fact]
        public void MoveActiveToCompletedMemoryWorks()
        {
            BufferWriter w = new BufferWriter(1024);
            w.Write((byte)1);
            w.Write((byte)2);
            w.LazinatorMemory.EnumerateBytes().ToList().Should().Equal(new byte[] { 1, 2 });
            w.MoveActiveToCompletedMemory();
            w.Write((byte)3);
            w.Write((byte)4);
            w.LazinatorMemory.EnumerateBytes().ToList().Should().Equal(new byte[] { 1, 2, 3, 4 });
            w.Write((byte)5);
            w.Write((byte)6);
            w.LazinatorMemory.EnumerateBytes().ToList().Should().Equal(new byte[] { 1, 2, 3, 4, 5, 6 });
            bool isMemoryRangeCollection = w.LazinatorMemory.MultipleMemoryBlocks is MemoryRangeCollection;
            isMemoryRangeCollection.Should().BeFalse();
        }

        [Fact]
        public void BufferWriterWithPreviousVersionWorks_MemoryBlockCollectionWithSingleBlock()
        {
            List<byte> GetBytesList(BufferWriter bw) => bw.LazinatorMemory.EnumerateBytes().ToList();
            
            BufferWriter w = new BufferWriter(1024);
            for (byte i = 0; i < 3; i++)
                w.Write(i);
            LazinatorMemory previousVersion = w.LazinatorMemory;
            BufferWriter w2 = new BufferWriter(1024, previousVersion);
            for (byte i = 10; i < 13; i++)
                w2.Write(i);
            GetBytesList(w2).Should().Equal(new byte[] { 10, 11, 12 });
            GetBytesList(w2).Should().Equal(new byte[] { 10, 11, 12 }); // enumeration should have no effect
            w2.InsertReferenceToPreviousVersion(0, 1, 2);
            GetBytesList(w2).Should().Equal(new byte[] { 10, 11, 12, 1, 2 });
            w2.InsertReferenceToPreviousVersion(0, 2, 1);
            GetBytesList(w2).Should().Equal(new byte[] { 10, 11, 12, 1, 2, 2 });
            w2.Write((byte) 20);
            var DEBUG = w2.LazinatorMemory;
            GetBytesList(w2).Should().Equal(new byte[] { 10, 11, 12, 1, 2, 2, 20 });
        }

        [Fact]
        public void SplittableEntitiesWork()
        {
            Example e = GetTypicalExample();
            LazinatorMemory singleBufferResult = e.SerializeLazinator(new LazinatorSerializationOptions(IncludeChildrenMode.IncludeAllChildren, false, false, false));
            LazinatorMemory multipleBufferResult = e.SerializeLazinator(new LazinatorSerializationOptions(IncludeChildrenMode.IncludeAllChildren, false, false, false, 10));
            multipleBufferResult.MultipleMemoryBlocks.Count().Should().BeGreaterThan(0);
            LazinatorMemory consolidated = new LazinatorMemory(multipleBufferResult.GetConsolidatedMemory());
            consolidated.Matches(singleBufferResult.InitialReadOnlyMemory.Span).Should().BeTrue();

            Example e2 = new Example(consolidated);
            ExampleEqual(e, e2).Should().BeTrue();

            Example e3 = new Example(multipleBufferResult);
            ExampleEqual(e, e3).Should().BeTrue();

            Example e4 = new Example(multipleBufferResult);
            Example e5 = e4.CloneLazinatorTyped();
            ExampleEqual(e, e5).Should().BeTrue();
        }

        [Fact]
        public void SplittableEntitiesWork_SmallestTree()
        {
            BinaryTree = new LazinatorBinaryTree<WDouble>();
            BinaryTree.Add(0);
            BinaryTree.Add(1);
            LazinatorMemory singleBufferResult = BinaryTree.SerializeLazinator(new LazinatorSerializationOptions(IncludeChildrenMode.IncludeAllChildren, false, false, false));
            LazinatorMemory multipleBufferResult = BinaryTree.SerializeLazinator(new LazinatorSerializationOptions(IncludeChildrenMode.IncludeAllChildren, false, false, false, 1));
            multipleBufferResult.MultipleMemoryBlocks.Count().Should().BeGreaterThan(0);
            LazinatorMemory multipleConsolidated = new LazinatorMemory(multipleBufferResult.GetConsolidatedMemory());
            string mString = multipleConsolidated.ToString();
            string sString = singleBufferResult.ToString();
            mString.Should().Be(sString);
            multipleConsolidated.Matches(singleBufferResult.InitialReadOnlyMemory.Span).Should().BeTrue();
        }

        [Fact]
        public void SplittableEntitiesWork_SingleSplit()
        {
            var node = new LazinatorBinaryTreeNode<WByte>() { Data = 1 };
            // node.LeftNode = new LazinatorBinaryTreeNode<WByte>() { Data = 0 };
            node.RightNode = new LazinatorBinaryTreeNode<WByte>() { Data = 2 };
            LazinatorMemory singleBufferResult = node.SerializeLazinator(new LazinatorSerializationOptions(IncludeChildrenMode.IncludeAllChildren, false, false, false));
            LazinatorMemory multipleBufferResult = node.SerializeLazinator(new LazinatorSerializationOptions(IncludeChildrenMode.IncludeAllChildren, false, false, false, 1));
            multipleBufferResult.MultipleMemoryBlocks.Count().Should().BeGreaterThan(0);
            LazinatorMemory multipleConsolidated = new LazinatorMemory(multipleBufferResult.GetConsolidatedMemory());
            string mString = multipleConsolidated.ToString();
            string sString = singleBufferResult.ToString();
            mString.Should().Be(sString);
            multipleConsolidated.Matches(singleBufferResult.InitialReadOnlyMemory.Span).Should().BeTrue();
        }


        [Theory]
        [ClassData(typeof(BoolPermutations_5))]
        public async Task SplittableEntitiesSaveToBlobs(bool containedInSingleBlob, bool useFile, bool async, bool recreateBlobMemoryReference, bool poolMemory)
        {
            if (async)
                await SplittableEntitiesSavedHelper_Async(containedInSingleBlob, useFile, recreateBlobMemoryReference, poolMemory);
            else
                SplittableEntitiesSavedHelper(containedInSingleBlob, useFile, recreateBlobMemoryReference, poolMemory);
        }

        private void SplittableEntitiesSavedHelper(bool containedInSingleBlob, bool useFile, bool recreateIndex, bool poolMemory)
        {
            Example e = GetTypicalExample();
            LazinatorMemory multipleBufferResult = e.SerializeLazinator(new LazinatorSerializationOptions(IncludeChildrenMode.IncludeAllChildren, false, false, false, 10));

            // Write to one or more blobs
            IBlobManager blobManager = GetBlobManager(useFile, poolMemory);
            string fullPath = GetPathForIndexAndBlobs(useFile, false);
            if (fullPath == null)
                return;
            PersistentIndex index = new PersistentIndex(fullPath, blobManager, containedInSingleBlob);
            index.PersistLazinatorMemory(multipleBufferResult);
            // Note: Index reference is first var indexReference = memoryReferenceInBlobs[0];

            // Read from one or more blobs
            if (recreateIndex)
                index = PersistentIndex.ReadFromBlob(blobManager, fullPath, null, index.IndexVersion);
            var revisedMemory = index.GetLazinatorMemory();

            var e2 = new Example(revisedMemory);
            ExampleEqual(e, e2).Should().BeTrue();
            if (poolMemory)
                blobManager.MemoryAllocator.FreeMemory(fullPath);
        }

        private async Task SplittableEntitiesSavedHelper_Async(bool containedInSingleBlob, bool useFile, bool recreateIndex, bool poolMemory)
        {
            Example e = GetTypicalExample();
            LazinatorMemory multipleBufferResult = await e.SerializeLazinatorAsync(new LazinatorSerializationOptions(IncludeChildrenMode.IncludeAllChildren, false, false, false, 10));

            // Write to one or more blobs
            IBlobManager blobManager = GetBlobManager(useFile, poolMemory);
            string fullPath = GetPathForIndexAndBlobs(useFile, false);
            if (fullPath == null)
                return;
            PersistentIndex index = new PersistentIndex(fullPath, blobManager, containedInSingleBlob);
            await index.PersistLazinatorMemoryAsync(multipleBufferResult);
            // Note: Index reference is first var indexReference = memoryReferenceInBlobs[0];

            // Read from one or more blobs
            if (recreateIndex)
                index = await PersistentIndex.ReadFromBlobAsync(blobManager, fullPath, null, index.IndexVersion);
            var revisedMemory = await index.GetLazinatorMemoryAsync();

            Example e2 = new Example(revisedMemory);
            ExampleEqual(e, e2).Should().BeTrue();
            if (poolMemory)
                blobManager.MemoryAllocator.FreeMemory(fullPath);
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
            MultipleRoundsOfRandomChanges(10, 10, 5, () => 
            {
                Debug.WriteLine($"Round {round}");
                LazinatorMemory lazinatorMemory = BinaryTree.SerializeLazinator(LazinatorSerializationOptions.Default);

                Debug.WriteLine($"Consolidated{round++}: " + lazinatorMemory.ToStringConsolidated());

                BinaryTree = new LazinatorBinaryTree<WDouble>(lazinatorMemory);
            });
        }

        [Theory]
        [ClassData(typeof(BoolPermutations_4))]
        public void BinaryTreeTest_ReloadingFromBlobs(bool useFile, bool containedInSingleBlob, bool recreateIndex, bool poolMemory)
        {
            int round = 0;
            IBlobManager blobManager = GetBlobManager(useFile, poolMemory);
            string fullPath = GetPathForIndexAndBlobs(useFile, true);
            if (fullPath == null)
                return;
            MultipleRoundsOfRandomChanges(10, 10, 10, () => 
            {
                LazinatorMemory multipleBufferResult = BinaryTree.SerializeLazinator(new LazinatorSerializationOptions(IncludeChildrenMode.IncludeAllChildren, false, false, false, 5));

                // Write to one or more blobs
                PersistentIndex index = new PersistentIndex(fullPath, blobManager, containedInSingleBlob); 
                index.PersistLazinatorMemory(multipleBufferResult);

                if (recreateIndex)
                    index = PersistentIndex.ReadFromBlob(blobManager, fullPath, null, index.IndexVersion);
                var revisedMemory = index.GetLazinatorMemory();

                BinaryTree = new LazinatorBinaryTree<WDouble>(revisedMemory);
                round++;
            });
            if (poolMemory)
                blobManager.MemoryAllocator.FreeMemory(fullPath);
        }

        [Theory]
        [ClassData(typeof(BoolPermutations_4))]
        public async Task BinaryTreeTest_ReloadingFromBlobsAsync(bool useFile, bool containedInSingleBlob, bool recreateIndex, bool poolMemory)
        {
            IBlobManager blobManager = GetBlobManager(useFile, poolMemory);
            string fullPath = GetPathForIndexAndBlobs(useFile, true);
            if (fullPath == null)
                return;
            await MultipleRoundsOfRandomChangesAsync(10, 10, 10, async () =>
            {
                LazinatorMemory multipleBufferResult = BinaryTree.SerializeLazinator(new LazinatorSerializationOptions(IncludeChildrenMode.IncludeAllChildren, false, false, false, 5));

                // Write to one or more blobs
                PersistentIndex index = new PersistentIndex(fullPath, blobManager, containedInSingleBlob);
                await index.PersistLazinatorMemoryAsync(multipleBufferResult);

                if (recreateIndex)
                    index = await PersistentIndex.ReadFromBlobAsync(blobManager, fullPath, null, index.IndexVersion);
                var revisedMemory = await index.GetLazinatorMemoryAsync();
                BinaryTree = new LazinatorBinaryTree<WDouble>(revisedMemory);
            });
            if (poolMemory)
                blobManager.MemoryAllocator.FreeMemory(fullPath);
        }


        [Theory]
        [ClassData(typeof(BoolPermutations_4))]
        public void BinaryTreeTest_DiffSerialization_RewritingAll(bool useFile, bool containedInSingleBlob, bool recreateIndex, bool poolMemory)
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

            IBlobManager blobManager = GetBlobManager(useFile, poolMemory);
            string fullPath = GetPathForIndexAndBlobs(useFile, true);
            if (fullPath == null)
                return;
            PersistentIndex index = new PersistentIndex(fullPath, blobManager, containedInSingleBlob);
            index.PersistLazinatorMemory(afterChange);

            if (recreateIndex)
                index = PersistentIndex.ReadFromBlob(blobManager, fullPath, null, index.IndexVersion);
            var afterChangeReloaded = index.GetLazinatorMemory();
            var tree3 = new LazinatorBinaryTree<WByte>(afterChangeReloaded);
            tree3.Root.Data.WrappedValue.Should().Be((byte)2);

            var tree4 = new LazinatorBinaryTree<WByte>(afterChange);
            tree4.Root.Data.WrappedValue.Should().Be((byte)2);
            if (poolMemory)
                blobManager.MemoryAllocator.FreeMemory(fullPath);
        }

        [Theory]
        [ClassData(typeof(BoolPermutations_4))]
        public async Task BinaryTreeTest_DiffSerialization_RewritingAll_Async(bool useFile, bool containedInSingleBlob, bool recreateIndex, bool poolMemory)
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

            IBlobManager blobManager = GetBlobManager(useFile, poolMemory);
            string fullPath = GetPathForIndexAndBlobs(useFile, true);
            if (fullPath == null)
                return;
            PersistentIndex index = new PersistentIndex(fullPath, blobManager, containedInSingleBlob);
            await index.PersistLazinatorMemoryAsync(afterChange);

            if (recreateIndex)
                index = PersistentIndex.ReadFromBlob(blobManager, fullPath, null, index.IndexVersion);
            var afterChangeReloaded = await index.GetLazinatorMemoryAsync();
            var tree3 = new LazinatorBinaryTree<WByte>(afterChangeReloaded);
            tree3.Root.Data.WrappedValue.Should().Be((byte)2);

            var tree4 = new LazinatorBinaryTree<WByte>(afterChange);
            tree4.Root.Data.WrappedValue.Should().Be((byte)2);
            if (poolMemory)
                blobManager.MemoryAllocator.FreeMemory(fullPath);
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

            IBlobManager blobManager = useFile ? new FileBlobManager() : new InMemoryBlobManager();
            string fullPath = GetPathForIndexAndBlobs(useFile, true);
            PersistentIndex index = new PersistentIndex(fullPath, blobManager, containedInSingleBlob);
            index.PersistLazinatorMemory(afterChange);

            if (recreateIndex)
                index = PersistentIndex.ReadFromBlob(blobManager, fullPath, null, index.IndexVersion);
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

            IBlobManager blobManager = useFile ? new FileBlobManager() : new InMemoryBlobManager();
            string fullPath = GetPathForIndexAndBlobs(useFile, true);
            PersistentIndex index = new PersistentIndex(fullPath, blobManager, containedInSingleBlob);
            await index.PersistLazinatorMemoryAsync(afterChange);

            if (recreateIndex)
                index = await PersistentIndex.ReadFromBlobAsync(blobManager, fullPath, null, index.IndexVersion);
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
            var initialBytes = new ReadOnlyBytes(new byte[3] { 1, 2, 3 });
            var nextBytes = new ReadOnlyBytes(new byte[3] { 4, 5, 6 });
            MemoryBlock nextBytesAsBlock = new MemoryBlock(nextBytes, new MemoryBlockLoadingInfo(new MemoryBlockID(1), 3), false);
            var lastBytes = new ReadOnlyBytes(new byte[4] { 7, 8, 9, 10 });
            MemoryBlock lastBytesAsBlock = new MemoryBlock(lastBytes, new MemoryBlockLoadingInfo(new MemoryBlockID(2), 4), false);
            LazinatorMemory initialMemory = new LazinatorMemory(initialBytes);
            LazinatorMemory memory1 = initialMemory.WithAppendedBlock(nextBytesAsBlock).WithAppendedBlock(lastBytesAsBlock);

            IBlobManager blobManager = useFile ? new FileBlobManager() : new InMemoryBlobManager();
            string fullPath = GetPathForIndexAndBlobs(useFile, true);
            if (fullPath == null)
                return;
            PersistentIndex index = new PersistentIndex(fullPath, blobManager, containedInSingleBlob);
            index.PersistLazinatorMemory(memory1);

            if (recreateIndex)
                index = PersistentIndex.ReadFromBlob(blobManager, fullPath, null, index.IndexVersion);
            var memory2 = index.GetLazinatorMemory();
            var memory2List = memory2.GetConsolidatedMemory().ToArray().ToList();
            memory2List.SequenceEqual(fullSequence).Should().BeTrue();

            BufferWriter writer = new BufferWriter(0, memory2);
            writer.Write((byte)11);
            writer.Write((byte)12);
            writer.InsertReferenceToPreviousVersion(2, 1, 2); // 8, 9
            writer.InsertReferenceToPreviousVersion(0, 0, 3); // 1, 2, 3
            writer.InsertReferenceToPreviousVersion(1, 1, 1); // 5
            writer.InsertReferenceToPreviousVersion(2, 0, 2); // 7, 8
            writer.Write((byte)13);
            var memory3 = writer.LazinatorMemory;
            var memory3List = memory3.GetConsolidatedMemory().ToArray().ToList();
            List<byte> expected = new List<byte>() { 11, 12, 8, 9, 1, 2, 3, 5, 7, 8, 13 };
            memory3List.SequenceEqual(expected).Should().BeTrue();

        }

        [Theory]
        [ClassData(typeof(BoolPermutations_5))]
        public void BinaryTreeTest_DiffSerialization(bool useFile, bool containedInSingleBlob, bool recreateIndex, bool neverIncludeReferenceToPreviousBuffer, bool useConsolidatedMemory)
        {
            List<PersistentIndex> indices = new List<PersistentIndex>();
            IBlobManager blobManager = useFile ? new FileBlobManager() : new InMemoryBlobManager();
            string fullPath = GetPathForIndexAndBlobs(useFile, true);
            if (fullPath == null)
                return;
            int round = 0;
            int numRounds = 2; // DEBUG 10
            int numChangesFirstRound = 1; // DEBUG 10
            int numChangesSubsequentRounds = 1; // DEBUG 3
            MultipleRoundsOfRandomChanges(numRounds, numChangesFirstRound, numChangesSubsequentRounds, () =>
            {
                //Debug.WriteLine($"Round {round}");

                var DEBUG4 = BinaryTree.ToList();

                LazinatorSerializationOptions options = neverIncludeReferenceToPreviousBuffer ? new LazinatorSerializationOptions(IncludeChildrenMode.IncludeAllChildren, false, false, true, int.MaxValue, int.MaxValue) : new LazinatorSerializationOptions(IncludeChildrenMode.IncludeAllChildren, false, false, true, 20, 5);
                LazinatorMemory multipleBufferResult = BinaryTree.SerializeLazinator(options);
                //LazinatorSerializationOptions options = new LazinatorSerializationOptions(IncludeChildrenMode.IncludeAllChildren, false, false, false);
                //BinaryTree.SerializeLazinator();
                //LazinatorMemory multipleBufferResult = BinaryTree.SerializeLazinator(options);

                Debug.WriteLine("CURRENT STATE: " + String.Join(",", BinaryTree.ToList())); // DEBUG
                var DEBUG_ShouldBeCopy = new LazinatorBinaryTree<WDouble>(multipleBufferResult);
                var DEBUG_AsList = DEBUG_ShouldBeCopy.ToList();
                var DEBUG6 = BinaryTree.Root.LeftNode;
                var DEBUG7 = DEBUG_ShouldBeCopy.Root.LeftNode;

                // DEBUG
                Debug.WriteLine("Multiple buffer result:");
                Debug.WriteLine(multipleBufferResult.ToStringByBlock());
                Debug.WriteLine($"Consolidated{round}:\n" + multipleBufferResult.ToStringConsolidated());

                // Write to one or more blobs
                var index = (useConsolidatedMemory || indices == null || !indices.Any()) ? new PersistentIndex(fullPath, blobManager, containedInSingleBlob) : new PersistentIndex(indices.Last());
                index.PersistLazinatorMemory(multipleBufferResult);
                indices.Add(index);

                if (recreateIndex)
                    index = PersistentIndex.ReadFromBlob(blobManager, fullPath, null, index.IndexVersion);
                var revisedMemory = index.GetLazinatorMemory();
                if (useConsolidatedMemory)
                {
                    var consolidatedMemory = revisedMemory.GetConsolidatedMemory();
                    revisedMemory = new LazinatorMemory(consolidatedMemory);
                    if (round != numRounds - 1)
                    {
                        indices.RemoveAt(0); // with consolidated memory, we're not using diff serialization
                    }
                }

                // DEBUG
                Debug.WriteLine("Revised memory result:");
                Debug.WriteLine(revisedMemory.ToStringByBlock());
                Debug.WriteLine($"Consolidated{round}:\n" + revisedMemory.ToStringConsolidated());

                BinaryTree = new LazinatorBinaryTree<WDouble>(revisedMemory); // Note that revisedMemory is LazinatorMemory, containing potentially multiple blocks, and this is an acceptable way of initializing.
                round++;
            });
            DeleteBlocksAndIndex(useConsolidatedMemory, indices, blobManager);
        }

        private static void DeleteBlocksAndIndex(bool useConsolidatedMemory, List<PersistentIndex> indices, IBlobManager blobManager)
        {
            for (int i = 0; i < indices.Count(); i++)
            {
                var indexToDelete = indices[i];
                indexToDelete.Delete(PersistentIndexMemoryBlockStatus.NewlyOmitted, true);
                if (i == indices.Count() - 1)
                {
                    indexToDelete.Delete(PersistentIndexMemoryBlockStatus.PreviouslyIncluded, true);
                    indexToDelete.Delete(PersistentIndexMemoryBlockStatus.NewlyIncluded, true);
                }
                indexToDelete.DeleteIndex();
            }
            if (blobManager is InMemoryBlobManager inMemoryBlobManager && !useConsolidatedMemory) // if using consolidated memory, the memory blocks from the previous version will not be in the next, so we won't be deleting everything.
            {
                if (inMemoryBlobManager.Storage.Any())
                    throw new Exception("Blob storage should be empty.");
            }
        }

        [Theory]
        [ClassData(typeof(BoolPermutations_5))]
        public async Task BinaryTreeTest_DiffSerializationAsync(bool useFile, bool containedInSingleBlob, bool recreateIndex, bool neverIncludeReferenceToPreviousBuffer, bool useConsolidatedMemory)
        {
            List<PersistentIndex> indices = new List<PersistentIndex>();
            IBlobManager blobManager = useFile ? new FileBlobManager() : new InMemoryBlobManager();
            string fullPath = GetPathForIndexAndBlobs(useFile, true);
            if (fullPath == null)
                return;
            int round = 0;
            int numRounds = 10;
            await MultipleRoundsOfRandomChangesAsync(numRounds, 10, 3, async () => 
            {
                //Debug.WriteLine($"Round {round}");

                LazinatorSerializationOptions options = neverIncludeReferenceToPreviousBuffer ? new LazinatorSerializationOptions(IncludeChildrenMode.IncludeAllChildren, false, false, true, int.MaxValue, int.MaxValue) : new LazinatorSerializationOptions(IncludeChildrenMode.IncludeAllChildren, false, false, true, 20, 5);
                LazinatorMemory multipleBufferResult = await BinaryTree.SerializeLazinatorAsync(options);

                // Write to one or more blobs
                var index = (useConsolidatedMemory || indices == null || !indices.Any()) ? new PersistentIndex(fullPath, blobManager, containedInSingleBlob) : new PersistentIndex(indices.Last());
                await index.PersistLazinatorMemoryAsync(multipleBufferResult);
                indices.Add(index);

                if (recreateIndex)
                    index = await PersistentIndex.ReadFromBlobAsync(blobManager, fullPath, null, index.IndexVersion);
                var revisedMemory = index.GetLazinatorMemory();
                if (useConsolidatedMemory)
                {
                    var consolidatedMemory = await revisedMemory.GetConsolidatedMemoryAsync();
                    revisedMemory = new LazinatorMemory(consolidatedMemory);
                    if (round != numRounds - 1)
                    {
                        indices.RemoveAt(0); // with consolidated memory, we're not using diff serialization
                    }
                }

                //Debug.WriteLine(revisedMemory.ToStringByBlock());
                //Debug.WriteLine($"Consolidated{round}: " + revisedMemory.ToStringConsolidated());

                BinaryTree = new LazinatorBinaryTree<WDouble>(revisedMemory);
                round++;
            });
            DeleteBlocksAndIndex(useConsolidatedMemory, indices, blobManager); // note that there is no DeleteAsync, so we use the same methods here
        }

        [Fact]
        public void PersistentIndexVersionsWorkSimultaneously()
        {
            IBlobManager blobManager = new InMemoryBlobManager();
            LazinatorSerializationOptions options = new LazinatorSerializationOptions(IncludeChildrenMode.IncludeAllChildren, false, false, true, 20, 5);
            BinaryTree = new LazinatorBinaryTree<WDouble>();
            BinaryTree.Add(1.0);
            BinaryTree.Add(2.0);
            LazinatorMemory serialized0 = BinaryTree.SerializeLazinator(options);
            PersistentIndex index0 = new PersistentIndex("simultest", blobManager, false);
            index0.PersistLazinatorMemory(serialized0);

            var loadedFrom0 = index0.GetLazinatorMemory();
            BinaryTree = new LazinatorBinaryTree<WDouble>(loadedFrom0);
            BinaryTree.Add(3.0);
            LazinatorMemory serialized1 = BinaryTree.SerializeLazinator(options);
            PersistentIndex index1 = new PersistentIndex(index0);
            index1.PersistLazinatorMemory(serialized1);

            // reload both indices
            index0 = PersistentIndex.ReadFromBlob(blobManager, "simultest", null, 0);
            index1 = PersistentIndex.ReadFromBlob(blobManager, "simultest", null, 1);
            var tree0Reloaded = new LazinatorBinaryTree<WDouble>(index0.GetLazinatorMemory());
            var tree1Reloaded = new LazinatorBinaryTree<WDouble>(index1.GetLazinatorMemory());
            tree0Reloaded.Count().Should().Be(2);
            tree1Reloaded.Count().Should().Be(3);
        }

        internal class ForkPlan : List<ForkPlan>
        {
            public List<int> ForkNumbers;
            public List<double> DataToAdd;
            public ForkPlan Parent;

            public ForkPlan(ForkPlan parent, List<int> forkNumbers, List<double> dataToAdd)
            {
                this.ForkNumbers = forkNumbers;
                this.DataToAdd = dataToAdd;
                this.Parent = parent;
            }

            public List<double> OverallData()
            {
                List<double> overall = new List<double>();
                if (Parent != null)
                    overall.AddRange(Parent.OverallData());
                overall.AddRange(DataToAdd);
                return overall;
            }

        }

        [Fact]
        public void PersistentIndexForking()
        {
            InMemoryBlobManager blobManager = new InMemoryBlobManager();
            LazinatorSerializationOptions options = new LazinatorSerializationOptions(IncludeChildrenMode.IncludeAllChildren, false, false, true, 20, 5);

            // let's make a plan of what data should be added to a list of doubles at each point of our persistent index fork
            ForkPlan root = new ForkPlan(null, null, new List<double> { 1.0, 2.0 });
            ForkPlan child1 = new ForkPlan(root, new List<int> { 1 }, new List<double>() { 3.0 });
            ForkPlan child2 = new ForkPlan(root, new List<int> { 2 }, new List<double>() { 4.0, 5.0 });
            ForkPlan child3 = new ForkPlan(root, new List<int> { 3 }, new List<double>() { 6.0, 7.0 });
            ForkPlan child2_1 = new ForkPlan(child2, new List<int> { 2, 1 }, new List<double>() { 8.0, 9.0 });
            ForkPlan child2_2 = new ForkPlan(child2, new List<int> { 2, 2 }, new List<double>() { 8.0, 9.0, 10.0 });
            ForkPlan child2_2_1 = new ForkPlan(child2_2, new List<int> { 2, 2, 1 }, new List<double>() { 11.0 });


            void PersistAccordingToPlan(PersistentIndex index, ForkPlan plan)
            {
                if (index.PreviousPersistentIndex == null)
                {
                    BinaryTree = new LazinatorBinaryTree<WDouble>();
                }
                else
                {
                    var loadedFromPrevious = index.PreviousPersistentIndex.GetLazinatorMemory();
                    BinaryTree = new LazinatorBinaryTree<WDouble>(loadedFromPrevious);
                }
                foreach (var item in plan.DataToAdd)
                    BinaryTree.Add(item);
                LazinatorMemory serialized = BinaryTree.SerializeLazinator(options);
                index.PersistLazinatorMemory(serialized);
            }

            PersistentIndex index_root = new PersistentIndex("forktest", blobManager, false);
            PersistAccordingToPlan(index_root, root);
            PersistentIndex index_child1 = new PersistentIndex(index_root, 1);
            PersistAccordingToPlan(index_child1, child1);
            PersistentIndex index_child2 = new PersistentIndex(index_root, 2);
            PersistAccordingToPlan(index_child2, child2);
            PersistentIndex index_child3 = new PersistentIndex(index_root, 3);
            PersistAccordingToPlan(index_child3, child3);
            PersistentIndex index_child2_1 = new PersistentIndex(index_child2, 1);
            PersistAccordingToPlan(index_child2_1, child2_1);
            PersistentIndex index_child2_2 = new PersistentIndex(index_child2, 2);
            PersistAccordingToPlan(index_child2_2, child2_2);
            PersistentIndex index_child2_2_1 = new PersistentIndex(index_child2_2, 1);
            PersistAccordingToPlan(index_child2_2_1, child2_2_1);

            void VerifyProperPersistence(PersistentIndex index, ForkPlan plan)
            {
                var loaded = index.GetLazinatorMemory();
                BinaryTree = new LazinatorBinaryTree<WDouble>(loaded);
                var treeContents = BinaryTree.Select(x => (double)x).ToList();
                var expectedContents = plan.OverallData().ToList();
                treeContents.SequenceEqual(expectedContents).Should().BeTrue();
            }

            // verify contents
            VerifyProperPersistence(index_root, root);
            VerifyProperPersistence(index_child1, child1);
            VerifyProperPersistence(index_child2, child2);
            VerifyProperPersistence(index_child3, child3);
            VerifyProperPersistence(index_child2_1, child2_1);
            VerifyProperPersistence(index_child2_2, child2_2);
            VerifyProperPersistence(index_child2_2_1, child2_2_1);

            // now verify contents after progressive deletion. When an index has forks from it, we will only delete things that we know are safe to delete.
            // This illustrates the proper approach. When we are done with a version from which there are forks, we can delete newly omitted memory blocks (not relevant for root), plus the index itself. When we are done with a version that has no forks, but is itself forked from something else, we can delete previously included, newly omitted, and newly included, but not before the last fork. If it's the last one, though, we make it so that all previously included blocks are deleted, including those preceding the last fork.

            index_root.DeleteIndex();
            VerifyProperPersistence(index_child1, child1);
            index_child1.Delete(PersistentIndexMemoryBlockStatus.PreviouslyIncluded, false);
            index_child1.Delete(PersistentIndexMemoryBlockStatus.NewlyOmitted, false);
            index_child1.Delete(PersistentIndexMemoryBlockStatus.NewlyIncluded, false);
            index_child1.DeleteIndex();
            VerifyProperPersistence(index_child2, child2);
            index_child2.Delete(PersistentIndexMemoryBlockStatus.NewlyOmitted, true); // only newly omitted, because there are forks
            index_child2.DeleteIndex();
            VerifyProperPersistence(index_child3, child3);
            index_child3.Delete(PersistentIndexMemoryBlockStatus.PreviouslyIncluded, false);
            index_child3.Delete(PersistentIndexMemoryBlockStatus.NewlyOmitted, false);
            index_child3.Delete(PersistentIndexMemoryBlockStatus.NewlyIncluded, false);
            index_child3.DeleteIndex();
            VerifyProperPersistence(index_child2_1, child2_1);
            index_child2_1.Delete(PersistentIndexMemoryBlockStatus.PreviouslyIncluded, false);
            index_child2_1.Delete(PersistentIndexMemoryBlockStatus.NewlyOmitted, false);
            index_child2_1.Delete(PersistentIndexMemoryBlockStatus.NewlyIncluded, false);
            index_child2_1.DeleteIndex();
            VerifyProperPersistence(index_child2_2, child2_2);
            index_child2_2.Delete(PersistentIndexMemoryBlockStatus.NewlyOmitted, true); // only newly omitted, because there are forks
            index_child2_2.DeleteIndex();
            VerifyProperPersistence(index_child2_2_1, child2_2_1);
            index_child2_2_1.Delete(PersistentIndexMemoryBlockStatus.PreviouslyIncluded, true);
            index_child2_2_1.Delete(PersistentIndexMemoryBlockStatus.NewlyOmitted, false);
            index_child2_2_1.Delete(PersistentIndexMemoryBlockStatus.NewlyIncluded, false);
            index_child2_2_1.DeleteIndex();

            blobManager.Storage.Any().Should().BeFalse();

        }

        private static string GetPathForIndexAndBlobs(bool useFile, bool binaryTree)
        {
            string path = @"C:\Users\Admin\Desktop\testfolder";
            if (useFile && !System.IO.Directory.Exists(path))
                return null; // ignore this error
            string fullPath = path + (binaryTree ? @"\binary-tree.fil" : @"\example.fil");
            return fullPath;
        }

        private static IBlobManager GetBlobManager(bool useFile, bool poolMemory)
        {
            IBlobManager manager = useFile ? new FileBlobManager() : new InMemoryBlobManager();
            if (poolMemory)
                manager.MemoryAllocator = new PooledBlobMemoryAllocator();
            return manager;
        }
    }
}
