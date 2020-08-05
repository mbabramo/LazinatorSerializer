using System;

namespace LazinatorCollections.Location
{
    /// <summary>
    /// A container location specified to be before all items in the container
    /// </summary>
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
