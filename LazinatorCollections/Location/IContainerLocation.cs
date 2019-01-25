namespace LazinatorCollections.Location
{
    public interface IContainerLocation
    {
        bool IsBeforeContainer { get; }
        bool IsAfterContainer { get; }
        IContainerLocation GetNextLocation();
        IContainerLocation GetPreviousLocation();
    }
}
