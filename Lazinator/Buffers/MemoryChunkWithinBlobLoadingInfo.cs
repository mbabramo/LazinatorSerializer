using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public class MemoryChunkWithinBlobLoadingInfo : MemoryChunkLoadingInfo, IMemoryChunkWithinBlobLoadingInfo
    {
        public long OffsetWhenLoading { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
