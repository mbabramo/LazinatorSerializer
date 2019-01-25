using Lazinator.Core;
using Lazinator.Attributes;

namespace LazinatorCollections
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorArray)]
    public interface ILazinatorArray<T> where T : ILazinator
    {
    }
}