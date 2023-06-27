using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public partial class MemoryBlockInsetLoadingInfo : MemoryBlockLoadingInfo, IMemoryBlockInsetLoadingInfo
    {
        public MemoryBlockInsetLoadingInfo(MemoryBlockID memoryBlockID, int preTruncationLength, long loadingOffsetWithinMultiBlockBlob) : base(memoryBlockID, preTruncationLength)
        {
            LoadingOffset = loadingOffsetWithinMultiBlockBlob;
        }

        public override long GetLoadingOffset() => LoadingOffset;
    }
}
