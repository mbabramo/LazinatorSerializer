using Lazinator.Collections.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
    public partial class IndexLocation : IIndexLocation, IContainerLocation
    {
        public IndexLocation(long location, long count)
        {
            Location = location;
            Count = count;
        }

        public bool IsAfterCollection => Location == Count;

        public IContainerLocation GetNextLocation()
        {
            if (Location == Count - 1)
                return null;
            return new IndexLocation(Location + 1, Count);
        }

        public IContainerLocation GetPreviousLocation()
        {
            if (Location == 0)
                return null;
            return new IndexLocation(Location - 1, Count);
        }
    }
}
