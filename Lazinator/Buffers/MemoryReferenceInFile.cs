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
    public class MemoryReferenceInFile : MemoryReference
    {
        string Path;
        bool LengthDetermined;
        public bool Persisted { get; private set; }

        public MemoryReferenceInFile(string path)
        {
            Path = path;
            Persisted = true;
        }

        public MemoryReferenceInFile(string path, int length)
        {
            Path = path;
            Length = length;
            LengthDetermined = true;
            Persisted = true;
        }

        public MemoryReferenceInFile(string path, IMemoryOwner<byte> referencedMemory, int versionOfReferencedMemory, int startIndex, int length) : base(referencedMemory, versionOfReferencedMemory, startIndex, length)
        {
            Path = path;
            LengthDetermined = true;
            Persisted = false;
        }

        private int _Length;
        public override int Length 
        { 
            get 
            {
                if (!LengthDetermined)
                {
                    FileInfo fi = new FileInfo(Path);
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
            using FileStream fs = File.OpenWrite(Path);
            await fs.WriteAsync(ReferencedMemory.Memory);
        }

        public async override ValueTask LoadMemoryAsync()
        {
            using FileStream fs = File.OpenRead(Path);
            byte[] target = new byte[Length];
            await fs.ReadAsync(target);
            ReferencedMemory = new SimpleMemoryOwner<byte>(target);
        }

        public override ValueTask ConsiderUnloadMemoryAsync()
        {
            ReferencedMemory = null;
            return ValueTask.CompletedTask;
        }
    }
}
