using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Lazinator.Exceptions;

namespace Lazinator.Buffers
{
    /// <summary>
    /// Saves or loads memory references in consecutively numbered files. The initial file contains information on all the other files, so that these
    /// can be asynchronously loaded if necessary.
    /// </summary>
    public class MemoryReferenceInFile : MemoryReference
    {
        string PathForFile;
        bool IsIndexFile => Path.GetFileNameWithoutExtension(PathForFile).EndsWith("-0");
        bool LengthDetermined;
        public bool Persisted { get; private set; }

        /// <summary>
        /// Creates a reference to the first of a set of existing files. This should be followed by a call to GetAdditionalReferences(Async).
        /// </summary>
        /// <param name="path"></param>
        public MemoryReferenceInFile(string path)
        {
            PathForFile = path;
            Persisted = true;
            if (!IsIndexFile)
                throw new ArgumentException("To use this overload to MemoryReferenceInFile, pass the name of a file ending in -0.");
        }

        /// <summary>
        /// Creates a reference to an existing file. This is called internally by GetAdditionalReferences(Async), after a call to MemoryReferenceInFile.
        /// </summary>
        /// <param name="path">The path, including a number referring to the specific file</param>
        /// <param name="length"></param>
        public MemoryReferenceInFile(string path, int length)
        {
            PathForFile = path;
            Length = length;
            LengthDetermined = true;
            Persisted = true;
        }

        /// <summary>
        /// Creates a new file to serve as a memory reference.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="referencedMemory"></param>
        /// <param name="versionOfReferencedMemory"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        public MemoryReferenceInFile(string path, IMemoryOwner<byte> referencedMemory, int versionOfReferencedMemory, int startIndex, int length) : base(referencedMemory, versionOfReferencedMemory, startIndex, length)
        {
            PathForFile = path;
            LengthDetermined = true;
            Persisted = false;
        }

        private int Offset = 0;

        private int _Length;
        public override int Length 
        { 
            get 
            {
                if (!LengthDetermined)
                {
                    FileInfo fi = new FileInfo(PathForFile);
                    long longLength = fi.Length;
                    if (longLength > int.MaxValue)
                        ThrowHelper.ThrowTooLargeException(int.MaxValue);
                    _Length = (int)longLength;
                    LengthDetermined = true;
                }
                return _Length;
            } 
            set => _Length = value; 
        }

        public async ValueTask PersistIfNecessary()
        {
            if (Persisted)
                return;
            using FileStream fs = File.OpenWrite(PathForFile);
            await fs.WriteAsync(ReferencedMemory.Memory);
        }

        public async override ValueTask LoadMemoryAsync()
        {
            using FileStream fs = File.OpenRead(PathForFile);
            byte[] target = new byte[Length];
            if (Offset != 0)
                fs.Seek(Offset, SeekOrigin.Begin);
            await fs.ReadAsync(target);
            ReferencedMemory = new SimpleMemoryOwner<byte>(target);
        }

        public override ValueTask ConsiderUnloadMemoryAsync()
        {
            ReferencedMemory = null;
            return ValueTask.CompletedTask;
        }

        #region Index file

        /// <summary>
        /// Uses information in an initial MemoryReferenceInFile to generate information on other MemoryReferenceInFiles. 
        /// This should be called immediately after the constructor. 
        /// </summary>
        /// <returns></returns>
        public async Task<List<MemoryReferenceInFile>> GetAdditionalReferencesAsync()
        {
            List<int> fileLengths = new List<int>();
            using FileStream reader = File.Open(PathForFile, FileMode.Open);
            byte[] intHolder = new byte[4];
            await reader.ReadAsync(intHolder, 0, 4);
            int numBytesRead = 0;
            int numItems = ReadUncompressedPrimitives.ToInt32(intHolder, ref numBytesRead);
            Offset = 4 + numItems * 4;
            for (int i = 1; i <= numItems; i++)
            {
                await reader.ReadAsync(intHolder, 4 * i, 4);
                int fileLength = ReadUncompressedPrimitives.ToInt32(intHolder, ref numBytesRead);
                fileLengths.Add(fileLength);
            }
            List<MemoryReferenceInFile> memoryReferences = GetMemoryReferences(fileLengths);
            return memoryReferences;
        }

        /// <summary>
        /// Uses information in an initial MemoryReferenceInFile to generate information on other MemoryReferenceInFiles. 
        /// This should be called immediately after the constructor. 
        /// </summary>
        /// <returns></returns>
        public List<MemoryReferenceInFile> GetAdditionalReferences()
        {
            List<int> fileLengths = new List<int>();
            using FileStream reader = File.Open(PathForFile, FileMode.Open);
            byte[] intHolder = new byte[4];
            reader.Read(intHolder, 0, 4);
            int numBytesRead = 0;
            int numItems = ReadUncompressedPrimitives.ToInt32(intHolder, ref numBytesRead);
            Offset = 4 + numItems * 4;
            for (int i = 1; i <= numItems; i++)
            {
                reader.Read(intHolder, 4 * i, 4);
                int fileLength = ReadUncompressedPrimitives.ToInt32(intHolder, ref numBytesRead);
                fileLengths.Add(fileLength);
            }
            List<MemoryReferenceInFile> memoryReferences = GetMemoryReferences(fileLengths);
            return memoryReferences;
        }

        private List<MemoryReferenceInFile> GetMemoryReferences(List<int> fileLengths)
        {
            List<MemoryReferenceInFile> memoryReferences = new List<MemoryReferenceInFile>();
            for (int i = 1; i <= fileLengths.Count; i++)
            {
                int length = fileLengths[i];
                string withExtension = GetPathWithNumber(PathForFile, i);
                memoryReferences.Add(new MemoryReferenceInFile(withExtension, fileLengths[i - 1]));
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
