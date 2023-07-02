using Lazinator.Attributes;

namespace Lazinator.Buffers
{
    [Lazinator((int)LazinatorCoreUniqueIDs.IMemoryBlockLoadingInfo)]
    public interface IMemoryBlockLoadingInfo
    {
        // DEBUG -- change to MemoryBlock MemoryBlockID. Will need to change autogenerate code method so that it modifies config to allow read only structs to be automatically used.
        int MemoryBlockIntID { get; set; }
        int MemoryBlockLength { get; set; }
    }
}