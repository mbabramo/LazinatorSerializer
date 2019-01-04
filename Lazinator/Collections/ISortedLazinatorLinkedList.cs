using Lazinator.Core;
using Lazinator.Attributes;

namespace Lazinator.Collections
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ISortedLazinatorLinkedList)]
    public interface ISortedLazinatorLinkedList<T> where T : ILazinator
    {
        bool AllowDuplicates { get; set; }
    }
}