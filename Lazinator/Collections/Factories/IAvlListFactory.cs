using Lazinator.Core;
using Lazinator.Attributes;

namespace Lazinator.Collections.Factories
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlListFactory)]
    public interface IAvlListFactory<T> where T : ILazinator
    {
    }
}