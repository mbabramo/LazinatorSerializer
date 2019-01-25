using Lazinator.Core;
using Lazinator.Attributes;

namespace Lazinator.List
{
    [Lazinator((int)LazinatorCoreUniqueIDs.ILazinatorLinkedListNode)]
    public interface ILazinatorLinkedListNode<T> : ILazinator where T : ILazinator
    {
        T Value { get; set; }
        LazinatorLinkedListNode<T> NextNode { get; set; }
    }
}