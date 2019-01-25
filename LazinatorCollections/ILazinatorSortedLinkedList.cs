using Lazinator.Core;
using Lazinator.Attributes;

namespace LazinatorCollections
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorSortedLinkedList)]
    public interface ILazinatorSortedLinkedList<T> where T : ILazinator
    {
    }
}