using Lazinator.Attributes;
using System;
using System.Collections.Generic;
using Lazinator.Buffers;

namespace Lazinator.Persistence
{
    [Lazinator((int) LazinatorCoreUniqueIDs.IPersistentIndex)]
    public interface IPersistentIndex : IMemorySegmentCollection
    {
        [Uncompressed]
        string BaseBlobPath { get; set; }
        [SetterAccessibility("private")]
        int IndexVersion { get; }
        List<(int lastMemoryChunkBeforeFork, int forkNumber)> ForkInformation { get; set; }
        Memory<byte> MemoryChunkStatus { get; set; }
        bool ContainedInSingleBlob { get; set; }
        bool IsPersisted { get; set; }
    }
}