using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Persistence
{
    public interface IBlobMemoryAllocator
    {
        Memory<byte> Allocate(string forPath, long offset, int length);
        void FreeMemory(string forPath);
    }
}
