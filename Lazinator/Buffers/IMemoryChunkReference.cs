using Lazinator.Attributes;

namespace Lazinator.Buffers
{
    [Lazinator((int)LazinatorCoreUniqueIDs.IMemoryChunkReference)]
    [FixedLengthLazinator(12)] // DEBUG -- maybe it shouldn't be fixed length -- instead just record the length of the list of memory chunk references
    public interface IMemoryChunkReference
    {
        [Uncompressed]
        int MemoryChunkID { get; set; }
        [Uncompressed]
        int IndexWithinMemoryChunk { get; set; }
        [Uncompressed]
        int Length { get; set; }
    }
}