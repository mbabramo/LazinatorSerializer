using Lazinator.Attributes;

namespace Lazinator.Buffers
{
    [Lazinator((int)LazinatorCoreUniqueIDs.IMemoryChunkReference)]
    public interface IMemoryChunkReference
    {
        int MemoryChunkID { get; set; }
        int IndexWithinMemoryChunk { get; set; }
        int NumBytes { get; set; }
    }
}