using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public partial class MemoryBlockInsetLoadingInfo : MemoryBlockLoadingInfo, IMemoryBlockInsetLoadingInfo
    {
        public MemoryBlockInsetLoadingInfo(MemoryBlockID memoryBlockID, int memoryBlockLength, long loadingOffsetWithinMultiBlockBlob) : base(memoryBlockID, memoryBlockLength)
        {
            LoadingOffset = loadingOffsetWithinMultiBlockBlob;
        }

        public override long GetLoadingOffset() => LoadingOffset;
    }
}
