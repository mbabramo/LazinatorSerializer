﻿using System;
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

namespace LazinatorTests.Tests
{
    public class BinaryTreeTests
    {
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

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, true, false)]
        [InlineData(true, false, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, true)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        [InlineData(false, false, false)]
        public void BinaryTreeTest_ReloadingFromBlobs(bool useFile, bool containedInSingleBlob, bool recreateIndex)
        {
            MultipleRoundsOfRandomChanges(10, 10, 10, () => 
            { 
                LazinatorMemory multipleBufferResult = BinaryTree.SerializeLazinator(new LazinatorSerializationOptions(IncludeChildrenMode.IncludeAllChildren, false, false, false, 5));

                // Write to one or more blobs
                IBlobManager blobManager = useFile ? new FileBlobManager() : new InMemoryBlobStorage();
                string fullPath = GetPathForIndexAndBlobs(useFile);
                if (fullPath == null)
                    return;
                PersistentIndex index = new PersistentIndex(fullPath, blobManager, containedInSingleBlob);
                index.PersistLazinatorMemory(multipleBufferResult);

                if (recreateIndex)
                    index = PersistentIndex.ReadFromBlobWithIntPrefix(blobManager, fullPath);
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
                string fullPath = GetPathForIndexAndBlobs(useFile);
                if (fullPath == null)
                    return;
                PersistentIndex index = new PersistentIndex(fullPath, blobManager, containedInSingleBlob);
                await index.PersistLazinatorMemoryAsync(multipleBufferResult);

                if (recreateIndex)
                    index = await PersistentIndex.ReadFromBlobWithIntPrefixAsync(blobManager, fullPath);
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

            IBlobManager blobManager = useFile ? new global::Lazinator.Buffers.FileBlobManager() : new global::LazinatorTests.Utilities.InMemoryBlobStorage();
            string fullPath = GetPathForIndexAndBlobs(useFile);
            PersistentIndex index = new PersistentIndex(fullPath, blobManager, containedInSingleBlob);
            index.PersistLazinatorMemory(afterChange);

            if (recreateIndex)
                index = PersistentIndex.ReadFromBlobWithIntPrefix(blobManager, fullPath);
            var afterChangeReloaded = index.GetLazinatorMemory();
            var tree3 = new LazinatorBinaryTree<WByte>(afterChangeReloaded); // DEBUG -- what happens if we just do afterChange? Either it should work or we should throw. 
            tree3.Root.Data.Should().Be(2);

            // DEBUG -- should also have an async version of this

            // DEBUG -- next step is to change something where there is still some child that will remain untouched.
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
        public void BinaryTreeTest_DiffSerialization(bool useFile, bool containedInSingleBlob, bool recreateIndex)
        {
            MultipleRoundsOfRandomChanges(2 /* DEBUG ALL SETTINGS MUST CHANGE*/, 1, 1, () =>
            {
                LazinatorSerializationOptions options = new LazinatorSerializationOptions(IncludeChildrenMode.IncludeAllChildren, false, false, true, 20);
                LazinatorMemory multipleBufferResult = BinaryTree.SerializeLazinator(options);
                // DEBUG -- with these settings should be 67 bytes

                // Write to one or more blobs
                IBlobManager blobManager = useFile ? new global::Lazinator.Buffers.FileBlobManager() : new global::LazinatorTests.Utilities.InMemoryBlobStorage();
                string fullPath = GetPathForIndexAndBlobs(useFile);
                if (fullPath == null)
                    return;
                PersistentIndex index = new PersistentIndex(fullPath, blobManager, containedInSingleBlob);
                index.PersistLazinatorMemory(multipleBufferResult);

                if (recreateIndex)
                    index = PersistentIndex.ReadFromBlobWithIntPrefix(blobManager, fullPath);
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
        public async Task BinaryTreeTest_DiffSerializationAsync(bool useFile, bool containedInSingleBlob, bool recreateIndex)
        {
            throw new Exception();
        }

        private static string GetPathForIndexAndBlobs(bool useFile)
        {
            string path = @"C:\Users\Admin\Desktop\testfolder";
            if (useFile && !System.IO.Directory.Exists(path))
                return null; // ignore this error
            string fullPath = path + @"\binary-tree.fil";
            return fullPath;
        }
    }
}
