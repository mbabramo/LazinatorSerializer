using Lazinator.Core;
using Lazinator.Attributes;

namespace Lazinator.Collections
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorSortedLinkedList)]
    public interface ILazinatorSortedLinkedList<T> where T : ILazinator
    {
        bool AllowDuplicates { get; set; }
    }
}