using Lazinator.Attributes;
using System;
using System.Collections.Generic;
using Lazinator.Buffers;

namespace Lazinator.Persistence
{
    [Lazinator((int) LazinatorCoreUniqueIDs.IPersistentIndex)]
    public interface IPersistentIndex : IMemoryRangeCollection
    {
        [SetterAccessibility("private")]
        int IndexVersion { get; }
        List<(int lastMemoryBlockIDBeforeFork, int forkNumber)> ForkInformation { get; set; }
        Memory<byte> MemoryBlockStatus { get; set; }
    }
}