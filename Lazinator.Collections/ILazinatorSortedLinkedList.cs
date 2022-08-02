using Lazinator.Core;
using Lazinator.Attributes;

namespace Lazinator.Collections
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorSortedLinkedList)]
    public interface ILazinatorSortedLinkedList<T> : ILazinatorLinkedList<T> where T : ILazinator
    {
    }
}