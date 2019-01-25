using LazinatorCollections.Interfaces;
using LazinatorCollections.Location;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorCollections
{
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
