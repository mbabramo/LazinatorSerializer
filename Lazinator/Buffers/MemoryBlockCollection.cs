using Lazinator.Core;
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
        public long LengthOfMemoryBlocks { get; private set; }
        public MemoryBlockID HighestMemoryBlockID 
        { 
            get; 
            private set; 
        }
        public MemoryBlockID GetNextMemoryBlockID() => HighestMemoryBlockID.Next();
        protected int NumMemoryBlocks => MemoryBlocks?.Count ?? 0;
        public virtual int NumMemoryRanges => NumMemoryBlocks;
        public virtual long LengthReferenced => LengthOfMemoryBlocks;

        protected virtual int GetRangeLength(int rangeIndex) => MemoryBlocksLoadingInfo[rangeIndex].MemoryBlockLength;
        protected virtual int GetOffsetIntoBlockForRange(int rangeIndex) => 0;
        protected virtual int GetMemoryBlockIndexFromMemoryRangeIndex(int i) => i;

        #region Construction

        public MemoryBlockCollection()
        {

        }

        public MemoryBlockCollection(IEnumerable<MemoryBlock> memoryBlocks)
        {
            MemoryBlocks = memoryBlocks?.ToList();
            InitializeMemoryBlocksInformationFromMemoryBlocks();
            if (MemoryBlocksLoadingInfo != null && MemoryBlocksLoadingInfo.Any())
            {
                HighestMemoryBlockID = new MemoryBlockID(MemoryBlocksLoadingInfo.Max(x => x.MemoryBlockID.AsInt));
                LengthOfMemoryBlocks = MemoryBlocksLoadingInfo.Sum(x => (long)x.MemoryBlockLength);
            }
            else
            {
                HighestMemoryBlockID = new MemoryBlockID(0);
                LengthOfMemoryBlocks = 0;
            }
        }

        public LazinatorMemory ToLazinatorMemory()
        {
            return new LazinatorMemory(this);
        }

        public virtual MemoryBlockCollection DeepCopy()
        {
            var copy = this.CloneLazinatorTyped();
            CopyNonPersistedPropertiesTo(copy);
            return copy;
        }

        protected void CopyNonPersistedPropertiesTo(MemoryBlockCollection copy)
        {
            copy.MemoryBlocks = MemoryBlocks?.ToList();
            copy.BlobManager = BlobManager;
            copy.MemoryBlocksIndexFromBlockID = MemoryBlocksIndexFromBlockID.ToDictionary();
            copy.LengthOfMemoryBlocks = LengthOfMemoryBlocks;
            copy.HighestMemoryBlockID = HighestMemoryBlockID;
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
            LengthOfMemoryBlocks += (long)memoryBlock.Length;
        }

        public void SetBlocks(IEnumerable<MemoryBlock> blocks)
        {
            MemoryBlocks = new List<MemoryBlock>();
            MemoryBlocksIndexFromBlockID = new Dictionary<MemoryBlockID, int>();
            LengthOfMemoryBlocks = 0;
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
            if (block == null)
                return default;
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
                var result = MemoryRangeAtIndex(i);
                if (result.MemoryBlock != null)
                    yield return result;
            }
        }

        public IEnumerable<MemoryBlock> EnumerateMemoryBlocks()
        {
            InitializeMemoryBlocksInformationIfNecessary();
            if (MemoryBlocks != null)
                for (int i = 0; i < MemoryBlocks.Count; i++)
                    yield return MemoryBlockAtIndex(i);
        }

        public async IAsyncEnumerable<MemoryBlock> EnumerateMemoryBlocksAsync()
        {
            InitializeMemoryBlocksInformationIfNecessary();
            if (MemoryBlocks != null)
                for (int i = 0; i < MemoryBlocks.Count; i++)
                    yield return await MemoryBlockAtIndexAsync(i);
        }

        public void LoadAllMemory()
        {
            foreach (var memoryBlock in EnumerateMemoryBlocks())
            {
                // nothing to do but enumerate
            }
        }

        public async ValueTask LoadAllMemoryAsync()
        {
            await foreach (var memoryBlock in EnumerateMemoryBlocksAsync())
            {
                // nothing to do but enumerate
            }
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

        public virtual void ConsiderUnloadMemoryBlockAtMemoryRangeIndex(int memoryRangeIndex)
        {
            // For future use
        }
        public virtual ValueTask ConsiderUnloadMemoryBlockAtMemoryRangeIndexAsync(int memoryRangeIndex)
        {
            // For future use
            return ValueTask.CompletedTask;
        }

        public string ToStringByBlock()
        {
            return String.Join("; ", EnumerateMemoryBlocks().Select(x => x.ToString().Replace("\n", " >>> ")));
        }

        #endregion

        #region Memory blocks index and loading information

        protected void InitializeMemoryBlocksInformationIfNecessary()
        {
            if ((MemoryBlocksLoadingInfo != null && MemoryBlocksLoadingInfo.Count > 0) && (MemoryBlocks == null || MemoryBlocks.Count == 0))
                InitializeMemoryBlocksInformationFromLoadingInformation();
            else if (MemoryBlocksLoadingInfo == null)
                InitializeMemoryBlocksInformationFromMemoryBlocks();
            if (MemoryBlocksIndexFromBlockID == null)
                CreateMemoryBlocksIndexFromBlockID();
        }

        private void InitializeMemoryBlocksInformationFromLoadingInformation()
        {
            if ((MemoryBlocksLoadingInfo != null && MemoryBlocksLoadingInfo.Count > 0) && (MemoryBlocks == null || MemoryBlocks.Count == 0))
            {
                MemoryBlocks = MemoryBlocksLoadingInfo.Select(x => (MemoryBlock)null).ToList();
                CreateMemoryBlocksIndexFromBlockID();
            }
        }

        private void CreateMemoryBlocksIndexFromBlockID()
        {
            Dictionary<MemoryBlockID, int> d = new Dictionary<MemoryBlockID, int>();
            for (int i = 0; i < MemoryBlocksLoadingInfo.Count; i++)
            {
                MemoryBlockID blockID = MemoryBlocksLoadingInfo[i].MemoryBlockID;
                if (!d.ContainsKey(blockID))
                    d[blockID] = i;
            }
            MemoryBlocksIndexFromBlockID = d;
        }

        private void InitializeMemoryBlocksInformationFromMemoryBlocks()
        {
            MemoryBlocksLoadingInfo = new List<MemoryBlockLoadingInfo>();
            for (int i = 0; i < MemoryBlocks.Count; i++)
            {
                MemoryBlock memoryBlock = MemoryBlocks[i];
                MemoryBlockID blockID = memoryBlock.MemoryBlockID;
                MemoryBlocksLoadingInfo.Add(memoryBlock.LoadingInfo);
            }
            CreateMemoryBlocksIndexFromBlockID();
        }

        private void UpdateLoadingOffset(MemoryBlockID memoryBlockID, long offset)
        {
            int i = GetMemoryBlockIndicesFromIDs()[memoryBlockID];
            MemoryBlocksLoadingInfo[i] = MemoryBlocksLoadingInfo[i].WithLoadingOffset(offset);
        }
        protected Dictionary<MemoryBlockID, int> GetMemoryBlockIndicesFromIDs()
        {
            if (MemoryBlocksIndexFromBlockID == null)
            {
                InitializeMemoryBlocksInformationIfNecessary();
                if (MemoryBlocksIndexFromBlockID == null)
                    CreateMemoryBlocksIndexFromBlockID();
            }
            return MemoryBlocksIndexFromBlockID;
        }

        public int GetMemoryBlockIndexFromBlockID(MemoryBlockID memoryBlockID)
        {
            var d = GetMemoryBlockIndicesFromIDs();
            return d.ContainsKey(memoryBlockID) ? d[memoryBlockID] : -1;
        }

        public bool ContainsMemoryBlockID(MemoryBlockID memoryBlockID)
        {
            var d = GetMemoryBlockIndicesFromIDs();
            return d.ContainsKey(memoryBlockID);
        }

        public MemoryBlock GetMemoryBlockByBlockID(MemoryBlockID memoryBlockID)
        {
            var d = GetMemoryBlockIndicesFromIDs();
            if (!d.ContainsKey(memoryBlockID))
                return null;
            return MemoryBlockAtIndex(d[memoryBlockID]);
        }

        #endregion

        #region Memory block creation and loading

        public MemoryBlock MemoryBlockAtIndex(int i)
        {
            if (MemoryBlocks == null)
                InitializeMemoryBlocksInformationFromLoadingInformation(); 
            if (i >= MemoryBlocks.Count)
                return null;
            if (MemoryBlocks[i] == null)
                MemoryBlocks[i] = LoadMemoryBlockForIndex(i);
            var block = MemoryBlocks[i];
            return block;
        }

        public async ValueTask<MemoryBlock> MemoryBlockAtIndexAsync(int i)
        {
            if (MemoryBlocks == null)
                InitializeMemoryBlocksInformationFromLoadingInformation();
            if (MemoryBlocks[i] == null)
                MemoryBlocks[i] = await LoadMemoryBlockForIndexAsync(i);
            var block = MemoryBlocks[i];
            return block;
        }

        private MemoryBlock LoadMemoryBlockForIndex(int i)
        {
            var loadingInfo = MemoryBlocksLoadingInfo[i];
            string path = GetPathForMemoryBlock(loadingInfo.MemoryBlockID);
            ReadOnlyMemory<byte> memory = BlobManager.Read(path, loadingInfo.GetLoadingOffset(), loadingInfo.MemoryBlockLength);
            ReadOnlyBytes readOnlyBytes = new ReadOnlyBytes(memory);
            MemoryBlock block = new MemoryBlock(readOnlyBytes, loadingInfo, true);
            return block;
        }

        private async ValueTask<MemoryBlock> LoadMemoryBlockForIndexAsync(int i)
        {
            var loadingInfo = MemoryBlocksLoadingInfo[i];
            string path = GetPathForMemoryBlock(loadingInfo.MemoryBlockID);
            ReadOnlyMemory<byte> memory = await BlobManager.ReadAsync(path, loadingInfo.GetLoadingOffset(), loadingInfo.MemoryBlockLength);
            ReadOnlyBytes readOnlyBytes = new ReadOnlyBytes(memory);
            MemoryBlock block = new MemoryBlock(readOnlyBytes, loadingInfo, true);
            return block;
        }

        #endregion

        #region Slicing
            
        
        public virtual MemoryRangeReference GetMemoryRangeAtOffsetFromStartPosition(int initialMemoryRangeIndex, int furtherOffsetInMemoryBlock, long offsetFromInitialPosition)
        {
            (int finalMemoryRangeIndex, int finalFurtherOffset) = Offseter.MoveForward(NumMemoryRanges, r => GetRangeLength(r), initialMemoryRangeIndex, furtherOffsetInMemoryBlock, offsetFromInitialPosition);
            
            return new MemoryRangeReference(finalMemoryRangeIndex, finalFurtherOffset);
        }
        
        public IEnumerable<MemoryRangeByBlockIndex> EnumerateMemoryRangesWithBlockIndex(int initialMemoryRangeIndex, int furtherOffsetInMemoryBlock, long length)
        {
            (int finalMemoryRangeIndex, int finalFurtherOffset) = Offseter.MoveForward(NumMemoryRanges, r => GetRangeLength(r), initialMemoryRangeIndex, furtherOffsetInMemoryBlock, length);
            for (int r = initialMemoryRangeIndex; r <= finalMemoryRangeIndex; r++)
            {
                int rangeLength = GetRangeLength(r);
                if (rangeLength > 0)
                {
                    int startingPositionRelativeToRange = (r == initialMemoryRangeIndex) ? furtherOffsetInMemoryBlock : 0;
                    int endingPositionRelativeToRange = (r == finalMemoryRangeIndex) ? finalFurtherOffset : rangeLength;
                    int lengthInMemoryRange = endingPositionRelativeToRange - startingPositionRelativeToRange;
                    if (lengthInMemoryRange > 0)
                    {
                        int memoryBlockIndex = GetMemoryBlockIndexFromMemoryRangeIndex(r);
                        int initialOffsetInMemoryBlock = GetOffsetIntoBlockForRange(r);
                        int revisedOffsetInMemoryBlock = startingPositionRelativeToRange + initialOffsetInMemoryBlock;
                        yield return new MemoryRangeByBlockIndex(memoryBlockIndex, revisedOffsetInMemoryBlock, lengthInMemoryRange);
                    }
                }
            }
        }
        
        public IEnumerable<MemoryRangeByBlockID> EnumerateMemoryRangesWithBlockID(int memoryRangeIndex, int furtherOffsetInMemoryBlock, long length)
        {
            foreach (MemoryRangeByBlockIndex indexAndSlice in EnumerateMemoryRangesWithBlockIndex(memoryRangeIndex, furtherOffsetInMemoryBlock, length))
            {
                int memoryBlockIndex = indexAndSlice.MemoryBlockIndex;
                var memoryBlockID = MemoryBlockAtIndex(memoryBlockIndex).MemoryBlockID;
                yield return new MemoryRangeByBlockID(memoryBlockID, indexAndSlice.OffsetIntoMemoryBlock, indexAndSlice.Length);
            }
        }

        private MemoryBlockID GetMemoryBlockIDForMemoryRangeIndex(int memoryRangeIndex)
        {
            return MemoryBlockAtIndex(GetMemoryBlockIndexFromMemoryRangeIndex(memoryRangeIndex)).MemoryBlockID;
        }

        public virtual IEnumerable<MemoryRange> EnumerateMemoryRanges(int indexInitialRange, int offsetInInitialRange, long length)
        {
            foreach (MemoryRangeByBlockIndex indexAndSlice in EnumerateMemoryRangesWithBlockIndex(indexInitialRange, offsetInInitialRange, length))
            {
                var block = MemoryBlockAtIndex(indexAndSlice.MemoryBlockIndex);
                yield return new MemoryRange(block, new MemoryBlockSlice(indexAndSlice.OffsetIntoMemoryBlock, indexAndSlice.Length));
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
                if (memoryBlock == null)
                    continue; // not loaded
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
