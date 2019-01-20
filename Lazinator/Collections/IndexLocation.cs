using Lazinator.Collections.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
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

        public bool IsAfterCollection => Index == Count;

        public IContainerLocation GetNextLocation()
        {
            if (Index == Count - 1)
                return null;
            return new IndexLocation(Index + 1, Count);
        }

        public IContainerLocation GetPreviousLocation()
        {
            if (Index == 0)
                return null;
            return new IndexLocation(Index - 1, Count);
        }
    }
}
