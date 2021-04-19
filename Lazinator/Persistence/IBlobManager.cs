using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lazinator.Buffers;

namespace Lazinator.Persistence
{
    public interface IBlobManager
    {
        IBlobMemoryAllocator MemoryAllocator { get; set; }
        ReadOnlyMemory<byte> ReadAll(string path) => Read(path, 0, (int)GetLength(path));
        async ValueTask<ReadOnlyMemory<byte>> ReadAllAsync(string path) => await ReadAsync(path, 0, (int)GetLength(path));
        ReadOnlyMemory<byte> Read(string path, long offset, int length);
        ValueTask<ReadOnlyMemory<byte>> ReadAsync(string path, long offset, int length);
        long GetLength(string path);
        void Write(string path, Memory<byte> bytes);
        ValueTask WriteAsync(string path, Memory<byte> bytes);
        void OpenForWriting(string path);
        void Append(string path, Memory<byte> bytes);
        ValueTask AppendAsync(string path, Memory<byte> bytes);
        void CloseAfterWriting(string path);
        void Delete(string path);
        bool Exists(string path);
    }
}
