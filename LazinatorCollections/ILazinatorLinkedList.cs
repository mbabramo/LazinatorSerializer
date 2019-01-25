using Lazinator.Core;
using Lazinator.Attributes;

namespace LazinatorCollections
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorLinkedList)]
    public interface ILazinatorLinkedList<T> where T : ILazinator
    {
        int Count { get; set; }
        LazinatorLinkedListNode<T> FirstNode { get; set; }
        [SetterAccessibility("protected")]
        bool AllowDuplicates { get; }
    }
}