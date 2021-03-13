using Lazinator.Attributes;

namespace Lazinator.Buffers
{
    [Lazinator((int)LazinatorCoreUniqueIDs.IMemoryChunkReference)]
    [FixedLengthLazinator(12)]
    public interface IMemoryChunkReference
    {
        int MemoryChunkID { get; set; }
        int IndexWithinMemoryChunk { get; set; }
        int Length { get; set; }
    }
}