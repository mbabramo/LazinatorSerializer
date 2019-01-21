using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Location
{
    public struct AfterContainerLocation : IContainerLocation
    {
        public bool IsBeforeContainer => false;

        public bool IsAfterContainer => true;

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
