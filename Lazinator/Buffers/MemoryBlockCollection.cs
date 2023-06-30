using Lazinator.Exceptions;
using Lazinator.Persistence;
using Lazinator.Support;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public partial class MemoryBlockCollection : IMemoryBlockCollection, IEnumerable<MemoryRange>
    {
        public IBlobManager BlobManager { get; set; }
        protected List<MemoryBlock> MemoryBlocks = new List<MemoryBlock>();
        protected Dictionary<MemoryBlockID, int> MemoryBlocksIndexFromBlockID = null;
        public long Length { get; private set; }
        public MemoryBlockID HighestMemoryBlockID { get; private set; }
        public MemoryBlockID GetNextMemoryBlockID() => HighestMemoryBlockID.Next();
        protected int NumMemoryBlocks => MemoryBlocks?.Count ?? 0;
        public virtual int NumMemoryRanges => NumMemoryBlocks;

        protected virtual int GetRangeLength(int rangeIndex) => MemoryBlocksLoadingInfo[rangeIndex].MemoryBlockLength;
        protected virtual int GetOffsetIntoBlockForRange(int rangeIndex) => 0;
        protected virtual int GetMemoryBlockIndexFromMemoryRangeIndex(int i) => i;

        #region Construction

        public MemoryBlockCollection()
        {

        }

        public MemoryBlockCollection(IEnumerable<MemoryBlock> memoryBlocks)
        {
            MemoryBlocks = memoryBlocks.ToList();
            HighestMemoryBlockID = MemoryBlocks.Any() ? new MemoryBlockID(MemoryBlocks.Max(x => x.MemoryBlockID.GetIntID())) : new MemoryBlockID(0);
            Length = MemoryBlocks.Sum(x => (long) x.Length);
            InitializeMemoryBlocksInformation();
            if (MemoryBlocks.Count != MemoryBlocksLoadingInfo.Count)
                throw new Exception("DEBUG");
        }

        public LazinatorMemory ToLazinatorMemory()
        {
            return new LazinatorMemory(this);
        }

        public virtual MemoryBlockCollection DeepCopy()
        {
            var collection = new MemoryBlockCollection(MemoryBlocks);
            return collection;
        }

        public virtual void AppendMemoryBlock(MemoryBlock memoryBlock)
        {
            if (MemoryBlocksLoadingInfo == null)
                MemoryBlocksLoadingInfo = new List<MemoryBlockLoadingInfo>();
            if (MemoryBlocksIndexFromBlockID == null)
                MemoryBlocksIndexFromBlockID = new Dictionary<MemoryBlockID, int>();
            MemoryBlocksLoadingInfo.Add(memoryBlock.LoadingInfo);
            MemoryBlocksIndexFromBlockID[memoryBlock.MemoryBlockID] = MemoryBlocksLoadingInfo.Count() - 1;
            MemoryBlocks.Add(memoryBlock);
            if (memoryBlock.MemoryBlockID > HighestMemoryBlockID)
                HighestMemoryBlockID = memoryBlock.MemoryBlockID;
            Length += (long)memoryBlock.Length;
        }

        public void SetBlocks(IEnumerable<MemoryBlock> blocks)
        {
            MemoryBlocks = new List<MemoryBlock>();
            MemoryBlocksIndexFromBlockID = new Dictionary<MemoryBlockID, int>();
            Length = 0;
            int i = 0;
            if (blocks == null)
            {
                HighestMemoryBlockID = new MemoryBlockID(-1);
                return;
            }
            foreach (var block in blocks)
            {
                AppendMemoryBlock(block);
            }
        }

        #endregion

        #region MemoryRanges

        // In MemoryBlockCollection, a range is always equal to a block. In an inherited class, that is not necessarily the case.

        public virtual MemoryRange MemoryRangeAtIndex(int i)
        {
            var block = MemoryBlockAtIndex(i);
            return new MemoryRange(block, new MemoryBlockSlice(0, block.Length));
        }

        public async virtual ValueTask<MemoryRange> MemoryRangeAtIndexAsync(int i)
        {
            var block = await MemoryBlockAtIndexAsync(i);
            return new MemoryRange(block, new MemoryBlockSlice(0, block.Length));
        }

        public virtual IEnumerable<MemoryRange> EnumerateMemoryRanges()
        {
            for (int i = 0; i < NumMemoryRanges; i++)
            {
                yield return MemoryRangeAtIndex(i);
            }
        }

        public IEnumerable<MemoryBlock> EnumerateMemoryBlocks()
        {
            if (MemoryBlocks != null)
                foreach (var block in MemoryBlocks)
                    yield return block;
        }

        public IEnumerator<MemoryRange> GetEnumerator()
        {
            foreach (var memoryRange in EnumerateMemoryRanges())
                yield return memoryRange;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int GetMemoryBlockIndexFromBlockID(MemoryBlockID memoryBlockID)
        {
            var d = GetMemoryBlockIndicesFromIDs();
            return d.ContainsKey(memoryBlockID) ? d[memoryBlockID] : -1;
        }

        public MemoryBlock GetMemoryBlockByBlockID(MemoryBlockID memoryBlockID)
        {
            var d = GetMemoryBlockIndicesFromIDs();
            if (!d.ContainsKey(memoryBlockID))
                return null;
            return MemoryBlockAtIndex(d[memoryBlockID]);
        }
        
        protected Dictionary<MemoryBlockID, int> GetMemoryBlockIndicesFromIDs()
        {
            if (MemoryBlocksIndexFromBlockID == null)
                InitializeMemoryBlocksInformation();
            return MemoryBlocksIndexFromBlockID;
        }

        public string ToStringByBlock()
        {
            return String.Join("; ", EnumerateMemoryBlocks().Select(x => x.ToString().Replace("\n", " ")));
        }

        #endregion

        #region Memory blocks loading information
        
        private void InitializeMemoryBlocksInformation()
        {
            Dictionary<MemoryBlockID, int> d = new Dictionary<MemoryBlockID, int>(); 
            MemoryBlocksLoadingInfo = new List<MemoryBlockLoadingInfo>();
            for (int i = 0; i < MemoryBlocks.Count; i++)
            {
                MemoryBlock memoryBlock = MemoryBlocks[i];
                MemoryBlockID blockID = memoryBlock.MemoryBlockID;
                if (!d.ContainsKey(blockID))
                    d[blockID] = i;
                MemoryBlocksLoadingInfo.Add(memoryBlock.LoadingInfo);
            }
            MemoryBlocksIndexFromBlockID = d;
        }

        private void UpdateLoadingOffset(MemoryBlockID memoryBlockID, long offset)
        {
            int i = GetMemoryBlockIndicesFromIDs()[memoryBlockID];
            MemoryBlocksLoadingInfo[i] = MemoryBlocksLoadingInfo[i].WithLoadingOffset(offset);
        }

        #endregion

        #region Memory block creation and loading

        public MemoryBlock MemoryBlockAtIndex(int i)
        {
            if (MemoryBlocks == null)
                MemoryBlocks = MemoryBlocksLoadingInfo.Select(x => (MemoryBlock)null).ToList();
            if (MemoryBlocks[i] == null)
                MemoryBlocks[i] = CreateMemoryBlockForIndex(i);
            var block = MemoryBlocks[i];
            return block;
        }

        public async ValueTask<MemoryBlock> MemoryBlockAtIndexAsync(int i)
        {
            if (MemoryBlocks == null)
                MemoryBlocks = MemoryBlocksLoadingInfo.Select(x => (MemoryBlock)null).ToList();
            if (MemoryBlocks[i] == null)
                MemoryBlocks[i] = await CreateMemoryBlockForIndexAsync(i);
            var block = MemoryBlocks[i];
            return block;
        }

        private MemoryBlock CreateMemoryBlockForIndex(int i)
        {
            string path = GetPathForIndex();
            var loadingInfo = MemoryBlocksLoadingInfo[i];
            ReadOnlyMemory<byte> memory = BlobManager.Read(path, loadingInfo.GetLoadingOffset(), loadingInfo.MemoryBlockLength);
            ReadOnlyBytes readOnlyBytes = new ReadOnlyBytes(memory);
            MemoryBlock block = new MemoryBlock(readOnlyBytes) { IsPersisted = true };
            return block;
        }

        private async ValueTask<MemoryBlock> CreateMemoryBlockForIndexAsync(int i)
        {
            string path = GetPathForIndex();
            var loadingInfo = MemoryBlocksLoadingInfo[i];
            ReadOnlyMemory<byte> memory = await BlobManager.ReadAsync(path, loadingInfo.GetLoadingOffset(), loadingInfo.MemoryBlockLength);
            ReadOnlyBytes readOnlyBytes = new ReadOnlyBytes(memory);
            MemoryBlock block = new MemoryBlock(readOnlyBytes) { IsPersisted = true };
            return block;
        }

        #endregion

        #region Slicing
            
        
        public virtual MemoryRangeReference GetMemoryRangeAtOffsetFromStartPosition(int initialMemoryRangeIndex, int furtherOffsetInMemoryBlock, long offsetFromInitialPosition)
        {
            (int finalMemoryRangeIndex, int finalFurtherOffset) = Offseter.MoveForward(NumMemoryRanges, r => GetRangeLength(r), initialMemoryRangeIndex, furtherOffsetInMemoryBlock, offsetFromInitialPosition);
            
            return new MemoryRangeReference(finalMemoryRangeIndex, finalFurtherOffset);
        }

        // DEBUG5 problem: We call this for 0, 30, 9 (meaning memory block index 0, location 30, length 9), which is the location of where the memory was. But now, we've been adding ranges when writing. So, we actually do need for at least this purpose for this to be MemoryBlocks, and then we need to be sure that is what is passed in. We need to see whther we actually want indexInInitialMemoryBlock, offsetInInitialMemoryBlock for all places where this is called.
        
        public IEnumerable<MemoryRangeByBlockIndex> EnumerateMemoryRangesByBlockIndex(int initialMemoryRangeIndex, int furtherOffsetInMemoryBlock, long length)
        {

            (int finalMemoryRangeIndex, int finalFurtherOffset) = Offseter.MoveForward(NumMemoryRanges, r => GetRangeLength(r), initialMemoryRangeIndex, furtherOffsetInMemoryBlock, length);
            for (int r = initialMemoryRangeIndex; r <= finalMemoryRangeIndex; r++)
            {
                int rangeLength = GetRangeLength(r);
                if (rangeLength > 0)
                {
                    int startingPositionRelativeToRange = (r == initialMemoryRangeIndex) ? furtherOffsetInMemoryBlock : 0;
                    int endingPositionRelativeToRange = (r == finalMemoryRangeIndex) ? finalFurtherOffset : rangeLength - 1;
                    if (startingPositionRelativeToRange < endingPositionRelativeToRange)
                    {
                        int memoryBlockIndex = GetMemoryBlockIndexFromMemoryRangeIndex(r);
                        int initialOffsetInMemoryBlock = GetOffsetIntoBlockForRange(r);
                        int revisedOffsetInMemoryBlock = startingPositionRelativeToRange + initialOffsetInMemoryBlock;
                        int lengthInMemoryRange = endingPositionRelativeToRange - startingPositionRelativeToRange;
                        yield return new MemoryRangeByBlockIndex(memoryBlockIndex, revisedOffsetInMemoryBlock, lengthInMemoryRange);
                    }
                }
            }
        }
        
        public IEnumerable<MemoryRangeByBlockID> EnumerateMemoryRangesByBlockID(int memoryRangeIndex, int furtherOffsetInMemoryBlock, long length)
        {
            foreach (MemoryRangeByBlockIndex indexAndSlice in EnumerateMemoryRangesByBlockIndex(memoryRangeIndex, furtherOffsetInMemoryBlock, length))
            {
                int memoryBlockIndex = indexAndSlice.MemoryBlockIndex;
                var memoryBlockID = GetMemoryBlockIDForMemoryRangeIndex(memoryBlockIndex);
                yield return new MemoryRangeByBlockID(memoryBlockID, indexAndSlice.OffsetIntoMemoryBlock, indexAndSlice.Length);
            }
        }

        private MemoryBlockID GetMemoryBlockIDForMemoryRangeIndex(int memoryRangeIndex)
        {
            return MemoryBlockAtIndex(GetMemoryBlockIndexFromMemoryRangeIndex(memoryRangeIndex)).MemoryBlockID;
        }

        public virtual IEnumerable<MemoryRange> EnumerateMemoryRanges(int indexInitialRange, int offsetInInitialRange, long length)
        {
            foreach (MemoryRangeByBlockIndex indexAndSlice in EnumerateMemoryRangesByBlockIndex(indexInitialRange, offsetInInitialRange, length))
            {
                MemoryRange memoryRange = MemoryRangeAtIndex(indexAndSlice.MemoryBlockIndex);
                yield return memoryRange;
            }
        }

        #endregion

        #region Persistence

        internal List<MemoryBlock> GetUnpersistedMemoryBlocks()
        {
            List<MemoryBlock> memoryBlocks = new List<MemoryBlock>();
            HashSet<MemoryBlockID> memoryBlockIDs = new HashSet<MemoryBlockID>();
            foreach (MemoryBlock memoryBlock in MemoryBlocks)
            {
                if (memoryBlock.IsPersisted)
                    continue;
                MemoryBlockID memoryBlockID = memoryBlock.MemoryBlockID;
                if (!memoryBlockIDs.Contains(memoryBlockID))
                {
                    memoryBlockIDs.Add(memoryBlockID);
                    memoryBlocks.Add(memoryBlock);
                }
            }
            return memoryBlocks;
        }

        public void PersistMemoryBlocks(bool isInitialVersion)
        {
            var memoryBlocksToPersist = GetUnpersistedMemoryBlocks();

            long offset = 0;
            string pathForSingleBlob = ContainedInSingleBlob ? GetPathForMemoryBlock(new MemoryBlockID(0)) : null;
            if (ContainedInSingleBlob)
            {
                if (isInitialVersion)
                {
                    if (BlobManager.Exists(pathForSingleBlob))
                        BlobManager.Delete(pathForSingleBlob);
                    offset = 0;
                }
                else
                    offset = BlobManager.GetLength(pathForSingleBlob);
                BlobManager.OpenForWriting(pathForSingleBlob);
            }
            foreach (var memoryBlockToPersist in memoryBlocksToPersist)
            {
                memoryBlockToPersist.LoadMemory();
                MemoryBlockID memoryBlockID = memoryBlockToPersist.MemoryBlockID;
                string path = ContainedInSingleBlob ? pathForSingleBlob : GetPathForMemoryBlock(memoryBlockID); 
                if (ContainedInSingleBlob)
                {
                    BlobManager.Append(path, memoryBlockToPersist.MemoryAsLoaded.ReadOnlyMemory);
                    UpdateLoadingOffset(memoryBlockToPersist.MemoryBlockID, offset);
                    offset += memoryBlockToPersist.LoadingInfo.MemoryBlockLength;
                }
                else
                    BlobManager.Write(path, memoryBlockToPersist.MemoryAsLoaded.ReadOnlyMemory);
                memoryBlockToPersist.IsPersisted = true;
                OnMemoryBlockPersisted(memoryBlockID);
            }
            if (ContainedInSingleBlob)
                BlobManager.CloseAfterWriting(pathForSingleBlob);
        }

        public async ValueTask PersistMemoryBlocksAsync(bool isInitialVersion)
        {
            var memoryBlocksToPersist = GetUnpersistedMemoryBlocks();

            long offset = 0;
            string pathForSingleBlob = ContainedInSingleBlob ? GetPathForMemoryBlock(new MemoryBlockID(0)) : null;
            if (ContainedInSingleBlob)
            {
                if (isInitialVersion)
                {
                    if (BlobManager.Exists(pathForSingleBlob))
                        BlobManager.Delete(pathForSingleBlob);
                    offset = 0;
                }
                else
                    offset = BlobManager.GetLength(pathForSingleBlob);
                BlobManager.OpenForWriting(pathForSingleBlob);
            }
            foreach (var memoryBlockToPersist in memoryBlocksToPersist)
            {
                await memoryBlockToPersist.LoadMemoryAsync();
                MemoryBlockID memoryBlockID = memoryBlockToPersist.MemoryBlockID;
                string path = ContainedInSingleBlob ? pathForSingleBlob : GetPathForMemoryBlock(memoryBlockID);
                if (ContainedInSingleBlob)
                {
                    await BlobManager.AppendAsync(path, memoryBlockToPersist.MemoryAsLoaded.ReadOnlyMemory);
                    UpdateLoadingOffset(memoryBlockToPersist.MemoryBlockID, offset);
                    offset += memoryBlockToPersist.LoadingInfo.MemoryBlockLength;
                }
                else
                    BlobManager.Write(path, memoryBlockToPersist.MemoryAsLoaded.ReadOnlyMemory);
                memoryBlockToPersist.IsPersisted = true;
                OnMemoryBlockPersisted(memoryBlockID);
            }
            if (ContainedInSingleBlob)
                BlobManager.CloseAfterWriting(pathForSingleBlob);
        }

        public virtual void OnMemoryBlockPersisted(MemoryBlockID memoryBlockID)
        {

        }

        #endregion

        #region Path

        public virtual string GetPathForIndex() => GetPathHelper(BaseBlobPath, null, " Index");
        public virtual string GetPathForMemoryBlock(MemoryBlockID memoryBlockID) => GetPathHelper(BaseBlobPath, null, ContainedInSingleBlob ? " AllBlocks" : (" Block " + memoryBlockID.ToString()));

        public static string GetPathForIndex(string baseBlobPath, IEnumerable<int> forkInformation, int versionNumber) => GetPathHelper(baseBlobPath, forkInformation, " Index " + versionNumber.ToString());

        internal static string GetPathHelper(string baseBlobPath, IEnumerable<int> forkInformation, string identifyingInformation)
        {
            string pathOnly = Path.GetDirectoryName(baseBlobPath);
            if (pathOnly != null && pathOnly.Length > 0)
                pathOnly += "\\";
            string withoutExtension = Path.GetFileNameWithoutExtension(baseBlobPath);
            string forkInformationString = forkInformation == null || !forkInformation.Any() ? "" : " " + String.Join(",", forkInformation);
            string combined = pathOnly + withoutExtension + forkInformationString + identifyingInformation + Path.GetExtension(baseBlobPath);
            return combined;
        }

        #endregion
    }
}
