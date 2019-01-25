using Lazinator.Core;
using Lazinator.Attributes;

namespace Lazinator.List
{
    [Lazinator((int)LazinatorCoreUniqueIDs.ILazinatorSortedLinkedList)]
    public interface ILazinatorSortedLinkedList<T> where T : ILazinator
    {
    }
}