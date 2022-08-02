using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections
{
    /// <summary>
    /// Tracks the number of items in a Lazinator container (using a long).
    /// </summary>
    [NonexclusiveLazinator((int) LazinatorCollectionUniqueIDs.ICountableContainer)]
    public interface ICountableContainer: ILazinator
    {
        long LongCount { get; }
    }
}
