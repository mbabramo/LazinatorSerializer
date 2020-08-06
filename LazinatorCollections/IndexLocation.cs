using LazinatorCollections.Location;

namespace LazinatorCollections
{
    /// <summary>
    /// A location within a Lazinator container, recording the index and the overall size of the container. The object can therefore
    /// report whether the location is after the container.
    /// </summary>
    public readonly struct IndexLocation : IContainerLocation
    {
        public readonly long Index;
        public readonly long Count;

        public IndexLocation(long index, long count)
        {
            Index = index;
            Count = count;
        }

        public bool IsBeforeContainer => Index < 0;
        public bool IsAfterContainer => Index == Count;

        public IContainerLocation GetNextLocation()
        {
            return new IndexLocation(Index + 1, Count);
        }

        public IContainerLocation GetPreviousLocation()
        {
            return new IndexLocation(Index - 1, Count);
        }
    }
}
