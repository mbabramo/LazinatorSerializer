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
            if (memoryBlockID == 15)
            {
                var DEBUG = 0;
            }
            PreTruncationLength = preTruncationLength;
        }
    }
}
