using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorCollections.Location
{
    public struct BeforeContainerLocation : IContainerLocation
    {
        public bool IsBeforeContainer => true;

        public bool IsAfterContainer => false;

        public IContainerLocation GetNextLocation()
        {
            throw new NotImplementedException();
        }

        public IContainerLocation GetPreviousLocation()
        {
            throw new NotImplementedException();
        }
    }
}
