using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public interface IBlobReader
    {
        Memory<byte> Read(string path, long offset, int length);
        ValueTask<Memory<byte>> ReadAsync(string path, long offset, int length);
    }
}
