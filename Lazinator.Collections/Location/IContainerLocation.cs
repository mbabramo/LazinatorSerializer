namespace Lazinator.Collections.Location
{

    /// <summary>
    /// An interface for specifying the location of an item in the container and for getting the previous or next location.
    /// For example, this might be used to specify an index in (or after) a list, or it might be used to specify a location in
    /// a tree structure.
    /// </summary>
    public interface IContainerLocation
    {
        bool IsBeforeContainer { get; }
        bool IsAfterContainer { get; }
        IContainerLocation GetNextLocation();
        IContainerLocation GetPreviousLocation();
    }
}
