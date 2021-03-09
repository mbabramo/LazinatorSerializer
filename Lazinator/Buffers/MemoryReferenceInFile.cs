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
    public class MemoryReferenceInFile : MemoryReference
    {
        string PathForFile;
        IBlobReader BlobReader;
        bool IsIndexFile => Path.GetFileNameWithoutExtension(PathForFile).EndsWith("-0");

        private long Offset = 0;

        /// <summary>
        /// Creates a reference to the index file (which may or may not contain all of the remaining data). This should be followed by a call to GetAdditionalReferences(Async).
        /// </summary>
        /// <param name="path"></param>
        public MemoryReferenceInFile(string path, IBlobReader blobReader)
        {
            PathForFile = path;
            BlobReader = blobReader;
        }

        /// <summary>
        /// Creates a reference to an existing file. This is called internally by GetAdditionalReferences(Async), after a call to MemoryReferenceInFile.
        /// </summary>
        /// <param name="path">The path, including a number referring to the specific file</param>
        /// <param name="length"></param>
        public MemoryReferenceInFile(string path, IBlobReader blobReader, int length, long offset, int referencedMemoryVersion)
        {
            PathForFile = path;
            BlobReader = blobReader;
            Length = length;
            Offset = offset;
            ReferencedMemoryVersion = referencedMemoryVersion;
        }

        #region Memory loading and unloading

        public async override ValueTask LoadMemoryAsync()
        {
            Memory<byte> bytes = await BlobReader.ReadAsync(PathForFile, Offset, Length);
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
        public async Task<List<MemoryReferenceInFile>> GetAdditionalReferencesAsync(bool sameFile)
        {
            Memory<byte> intHolder = await BlobReader.ReadAsync(PathForFile, 0, 4);
            int numBytesRead = 0;
            int numItems = ReadUncompressedPrimitives.ToInt32(intHolder.Span, ref numBytesRead);
            Memory<byte> bytesForLengths = await BlobReader.ReadAsync(PathForFile, 4, numItems * 4);
            return CompleteGetAdditionalReferences(sameFile, numItems, bytesForLengths);
        }

        /// <summary>
        /// Uses information in an initial MemoryReferenceInFile to generate information on other MemoryReferenceInFiles. 
        /// This should be called immediately after the constructor. 
        /// </summary>
        /// <returns></returns>
        public List<MemoryReferenceInFile> GetAdditionalReferences(bool sameFile)
        {
            Memory<byte> intHolder = BlobReader.Read(PathForFile, 0, 4);
            int numBytesRead = 0;
            int numItems = ReadUncompressedPrimitives.ToInt32(intHolder.Span, ref numBytesRead);
            Memory<byte> bytesForLengths = BlobReader.Read(PathForFile, 4, numItems * 4);
            return CompleteGetAdditionalReferences(sameFile, numItems, bytesForLengths);
        }

        private List<MemoryReferenceInFile> CompleteGetAdditionalReferences(bool sameFile, int numItems, Memory<byte> bytesForLengths)
        {
            List<int> fileLengths = GetFileLengths(bytesForLengths, numItems);
            List<MemoryReferenceInFile> memoryReferences = GetMemoryReferences(fileLengths, sameFile);
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

        private List<MemoryReferenceInFile> GetMemoryReferences(List<int> fileLengths, bool sameFile)
        {
            List<MemoryReferenceInFile> memoryReferences = new List<MemoryReferenceInFile>();
            long numBytesProcessed = Offset;
            for (int i = 1; i <= fileLengths.Count; i++)
            {
                int length = fileLengths[i];
                string referencePath = sameFile ? PathForFile : GetPathWithNumber(PathForFile, i);
                memoryReferences.Add(new MemoryReferenceInFile(referencePath, BlobReader, fileLengths[i - 1], sameFile ? numBytesProcessed : 0, i));
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
