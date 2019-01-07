using Lazinator.Core;
using Lazinator.Attributes;
using Lazinator.Wrappers;

namespace Lazinator.Collections.Factories
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlListFactory)]
    public interface IAvlListFactory<T> where T : ILazinator
    {
        IIndexableContainerFactory<T> IndexableContainerFactory { get; set; }
    }
}