using Lazinator.Core;
using Lazinator.Attributes;

namespace LazinatorCollections.Basic.Basic
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorSortedLinkedList)]
    public interface ILazinatorSortedLinkedList<T> where T : ILazinator
    {
    }
}