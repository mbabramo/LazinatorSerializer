using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public interface IBlobManager
    {
        Memory<byte> Read(string path, long offset, int length);
        ValueTask<Memory<byte>> ReadAsync(string path, long offset, int length);
        long GetLength(string path);
        void Write(string path, Memory<byte> bytes);
        ValueTask WriteAsync(string path, Memory<byte> bytes);
        void Append(string path, Memory<byte> bytes);
        ValueTask AppendAsync(string path, Memory<byte> bytes);
    }
}
