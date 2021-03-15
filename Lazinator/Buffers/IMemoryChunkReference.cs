using Lazinator.Attributes;

namespace Lazinator.Buffers
{
    [Lazinator((int)LazinatorCoreUniqueIDs.IMemoryChunkReference)]
    public interface IMemoryChunkReference
    {
        int MemoryChunkID { get; set; }
        int Offset { get; set; }
        int Length { get; set; }
    }
}