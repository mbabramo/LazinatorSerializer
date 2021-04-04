using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Persistence
{
    public enum PersistentIndexMemoryChunkStatus : byte
    {
        NotYetUsed = 0,
        PreviouslyIncluded,
        NewlyIncluded,
        PreviouslyOmitted,
        NewlyOmitted,
    }
}
