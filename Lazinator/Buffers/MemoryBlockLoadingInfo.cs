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
        public MemoryBlockLoadingInfo(MemoryBlockID memoryBlockID, int memoryBlockLength)
        {
            MemoryBlockIntID = memoryBlockID.AsInt;
            MemoryBlockLength = memoryBlockLength;
        }

        public MemoryBlockID MemoryBlockID => new MemoryBlockID(MemoryBlockIntID); 

        public MemoryBlockInsetLoadingInfo WithLoadingOffset(long loadingOffset) => new MemoryBlockInsetLoadingInfo(MemoryBlockID, MemoryBlockLength, loadingOffset);

        public virtual long GetLoadingOffset() => 0;
    }
}
