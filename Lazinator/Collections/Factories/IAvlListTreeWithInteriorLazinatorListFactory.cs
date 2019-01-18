using Lazinator.Core;
using Lazinator.Attributes;

namespace Lazinator.Collections.Factories
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlTree)]
    public interface IAvlListTreeWithInteriorLazinatorListFactory<T> where T : ILazinator
    {
        int InteriorMaxCapacity { get; set; }
    }
}