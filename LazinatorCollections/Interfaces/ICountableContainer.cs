using Lazinator.Attributes;
using Lazinator.Core;

namespace LazinatorCollections
{
    [NonexclusiveLazinator((int) LazinatorCollectionUniqueIDs.ICountableContainer)]
    public interface ICountableContainer: ILazinator
    {
        long LongCount { get; }
    }
}
