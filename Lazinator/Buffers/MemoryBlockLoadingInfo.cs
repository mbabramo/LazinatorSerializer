using Lazinator.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public partial class MemoryBlockLoadingInfo : IMemoryBlockLoadingInfo
    {
        public MemoryBlockLoadingInfo(int memoryBlockID, int preTruncationLength)
        {
            MemoryBlockID = memoryBlockID;
            PreTruncationLength = preTruncationLength;
        }

        public MemoryBlockInsetLoadingInfo WithLoadingOffset(long loadingOffset) => new MemoryBlockInsetLoadingInfo(MemoryBlockID, PreTruncationLength, loadingOffset);

        public virtual long GetLoadingOffset() => 0;
    }
}
