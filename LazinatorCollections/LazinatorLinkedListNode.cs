using Lazinator.Core;

namespace LazinatorCollections
{
    /// <summary>
    /// A node in a LazinatorLinkedList
    /// </summary>
    /// <typeparam name="T">The type of item stored in the node</typeparam>
    public partial class LazinatorLinkedListNode<T> : ILazinatorLinkedListNode<T> where T : ILazinator
    {
        public LazinatorLinkedListNode(T item)
        {
            Value = item;
        }
    }
}
