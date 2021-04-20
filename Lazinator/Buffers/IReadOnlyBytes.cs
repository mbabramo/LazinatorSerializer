using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public interface IReadOnlyBytes
    {
        ReadOnlyMemory<byte> ReadOnlyMemory { get;  }
        void Dispose();
    }
}
