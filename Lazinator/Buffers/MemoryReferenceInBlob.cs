using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Lazinator.Exceptions;
using System.Reflection.Metadata;

namespace Lazinator.Buffers
{
    /// <summary>
    /// Saves or loads memory references referring either to parts of a single file or to consecutively numbered files. The initial file contains information on all the other components, so that these can be asynchronously loaded if necessary.
    /// </summary>
    public class MemoryReferenceInBlob : MemoryReference
    {
        string BlobPath;
        IBlobManager BlobManager;
        bool IsIndexFile => Path.GetFileNameWithoutExtension(BlobPath).EndsWith("-0");

        private long Offset = 0;

        /// <summary>
        /// Creates a reference to the index file (which may or may not contain all of the remaining data). This should be followed by a call to GetAdditionalReferences(Async).
        /// </summary>
        /// <param name="path"></param>
        public MemoryReferenceInBlob(string path, IBlobManager blobManager)
        {
            BlobPath = path;
            BlobManager = blobManager;
        }

        /// <summary>
        /// Creates a reference to an existing file. This is called internally by GetAdditionalReferences(Async), after a call to MemoryReferenceInFile.
        /// </summary>
        /// <param name="path">The path, including a number referring to the specific file</param>
        /// <param name="length"></param>
        public MemoryReferenceInBlob(string path, IBlobManager blobManager, int length, long offset, int referencedMemoryVersion)
        {
            BlobPath = path;
            BlobManager = blobManager;
            Length = length;
            Offset = offset;
            ReferencedMemoryVersion = referencedMemoryVersion;
        }

        #region Memory loading and unloading

        public async override ValueTask LoadMemoryAsync()
        {
            Memory<byte> bytes = await BlobManager.ReadAsync(BlobPath, Offset, Length);
            ReferencedMemory = new SimpleMemoryOwner<byte>(bytes);
        }

        public override ValueTask ConsiderUnloadMemoryAsync()
        {
            ReferencedMemory = null;
            return ValueTask.CompletedTask;
        }

        #endregion

        #region Index file management

        /// <summary>
        /// Uses information in an initial MemoryReferenceInFile to generate information on other MemoryReferenceInFiles. 
        /// This should be called immediately after the constructor. 
        /// </summary>
        /// <returns></returns>
        public async Task<List<MemoryReferenceInBlob>> GetAdditionalReferencesAsync(bool sameFile)
        {
            Memory<byte> intHolder = await BlobManager.ReadAsync(BlobPath, 0, 4);
            int numBytesRead = 0;
            int numItems = ReadUncompressedPrimitives.ToInt32(intHolder.Span, ref numBytesRead);
            Memory<byte> bytesForLengths = await BlobManager.ReadAsync(BlobPath, 4, numItems * 4);
            return CompleteGetAdditionalReferences(sameFile, numItems, bytesForLengths);
        }

        /// <summary>
        /// Uses information in an initial MemoryReferenceInFile to generate information on other MemoryReferenceInFiles. 
        /// This should be called immediately after the constructor. 
        /// </summary>
        /// <returns></returns>
        public List<MemoryReferenceInBlob> GetAdditionalReferences(bool sameFile)
        {
            Memory<byte> intHolder = BlobManager.Read(BlobPath, 0, 4);
            int numBytesRead = 0;
            int numItems = ReadUncompressedPrimitives.ToInt32(intHolder.Span, ref numBytesRead);
            Memory<byte> bytesForLengths = BlobManager.Read(BlobPath, 4, numItems * 4);
            return CompleteGetAdditionalReferences(sameFile, numItems, bytesForLengths);
        }

        private List<MemoryReferenceInBlob> CompleteGetAdditionalReferences(bool sameFile, int numItems, Memory<byte> bytesForLengths)
        {
            List<int> fileLengths = GetFileLengths(bytesForLengths, numItems);
            List<MemoryReferenceInBlob> memoryReferences = GetMemoryReferences(fileLengths, sameFile);
            if (sameFile)
                Offset = 4 + numItems * 4;
            return memoryReferences;
        }

        private List<int> GetFileLengths(Memory<byte> bytesForLengths, int numItems)
        {
            ReadOnlySpan<byte> spanForLengths = bytesForLengths.Span; 
            List<int> fileLengths = new List<int>();
            int numBytesRead = 0;
            for (int i = 1; i <= numItems; i++)
            {
                int fileLength = ReadUncompressedPrimitives.ToInt32(spanForLengths.Slice(numBytesRead), ref numBytesRead);
                fileLengths.Add(fileLength);
            }
            return fileLengths;
        }

        private List<MemoryReferenceInBlob> GetMemoryReferences(List<int> fileLengths, bool sameFile)
        {
            List<MemoryReferenceInBlob> memoryReferences = new List<MemoryReferenceInBlob>();
            long numBytesProcessed = Offset;
            for (int i = 1; i <= fileLengths.Count; i++)
            {
                int length = fileLengths[i];
                string referencePath = sameFile ? BlobPath : GetPathWithNumber(BlobPath, i);
                memoryReferences.Add(new MemoryReferenceInBlob(referencePath, BlobManager, fileLengths[i - 1], sameFile ? numBytesProcessed : 0, i));
                numBytesProcessed += length;
            }
            return memoryReferences;
        }

        public static string GetPathWithNumber(string originalPath, int i)
        {
            string withoutExtension = Path.GetFileNameWithoutExtension(originalPath);
            string withoutNumber = withoutExtension.EndsWith("-0") ? withoutExtension.Substring(0, withoutExtension.Length - 2) : withoutExtension;
            string withNumber = withoutNumber + "-" + i.ToString();
            string withExtension = withNumber + Path.GetExtension(originalPath);
            return withExtension;
        }

        #endregion
    }
}
