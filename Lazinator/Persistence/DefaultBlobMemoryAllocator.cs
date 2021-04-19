using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Persistence
{
    public class DefaultBlobMemoryAllocator : IBlobMemoryAllocator
    {
        public Memory<byte> Allocate(string forPath, long offset, int length)
        {
            return new byte[length];
        }

        public void FreeMemory(string forPath)
        {
            // do nothing -- all allocated memory will be garbage collected
        }
    }
}
