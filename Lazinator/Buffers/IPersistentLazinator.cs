using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Buffers
{
    [NonexclusiveLazinator((int)LazinatorCoreUniqueIDs.IPersistentLazinator)]
    public interface IPersistentLazinator : ILazinator
    {
        LazinatorMemory GetLazinatorMemory();
        ValueTask<LazinatorMemory> GetLazinatorMemoryAsync();
        IPersistentLazinator PersistLazinatorMemory(LazinatorMemory lazinatorMemory);
        ValueTask<IPersistentLazinator> PersistLazinatorMemoryAsync(LazinatorMemory lazinatorMemory);
        void Delete(bool memoryDroppedFromPreviousIndex, bool memoryUsedInThisIndex, bool indexItself);
        ValueTask DeleteAsync(bool memoryDroppedFromPreviousIndex, bool memoryUsedInThisIndex, bool indexItself);
    }
}
