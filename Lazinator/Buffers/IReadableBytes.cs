using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public interface IReadableBytes : IMemoryAllocationInfo
    {
        ReadOnlyMemory<byte> ReadOnlyMemory { get; }
        IMemoryOwner<byte> MemoryOwner { get; }
        void Dispose();
    }
}
