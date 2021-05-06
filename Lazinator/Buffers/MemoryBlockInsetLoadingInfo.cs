using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public partial class MemoryBlockInsetLoadingInfo : MemoryBlockLoadingInfo, IMemoryBlockInsetLoadingInfo
    {
        public MemoryBlockInsetLoadingInfo(int memoryBlockID, int preTruncationLength, long loadingOffset) : base(memoryBlockID, preTruncationLength)
        {
            LoadingOffset = loadingOffset;
        }

        public override long GetLoadingOffset() => LoadingOffset;
    }
}
