using Lazinator.Core;
using Lazinator.Attributes;

namespace LazinatorCollections.ListDerivatives
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorArray)]
    public interface ILazinatorArray<T> where T : ILazinator
    {
    }
}