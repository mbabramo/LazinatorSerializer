namespace Lazinator.Collections.Location
{

    /// <summary>
    /// An interface for specifying the location of an item in the container and for getting the previous or next location.
    /// </summary>
    public interface IContainerLocation
    {
        bool IsBeforeContainer { get; }
        bool IsAfterContainer { get; }
        IContainerLocation GetNextLocation();
        IContainerLocation GetPreviousLocation();
    }
}
