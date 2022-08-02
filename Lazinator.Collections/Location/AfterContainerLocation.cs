using System;

namespace Lazinator.Collections.Location
{
    /// <summary>
    /// A container location specified to be after all items in the container
    /// </summary>
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
