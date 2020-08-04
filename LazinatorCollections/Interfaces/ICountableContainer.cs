using Lazinator.Attributes;
using Lazinator.Core;

namespace LazinatorCollections
{
    /// <summary>
    /// Tracks the number of items in a Lazinator container collection (using a long).
    /// </summary>
    [NonexclusiveLazinator((int) LazinatorCollectionUniqueIDs.ICountableContainer)]
    public interface ICountableContainer: ILazinator
    {
        long LongCount { get; }
    }
}
