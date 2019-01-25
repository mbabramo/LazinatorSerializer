using Lazinator.Core;
using Lazinator.Attributes;

namespace LazinatorCollections
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorLinkedListNode)]
    public interface ILazinatorLinkedListNode<T> : ILazinator where T : ILazinator
    {
        T Value { get; set; }
        LazinatorLinkedListNode<T> NextNode { get; set; }
    }
}