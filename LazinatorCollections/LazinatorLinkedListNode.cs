using Lazinator.Core;

namespace LazinatorCollections
{
    public partial class LazinatorLinkedListNode<T> : ILazinatorLinkedListNode<T> where T : ILazinator
    {
        public LazinatorLinkedListNode()
        {

        }

        public LazinatorLinkedListNode(T item)
        {
            Value = item;
        }
    }
}
