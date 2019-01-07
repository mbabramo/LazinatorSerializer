using Lazinator.Core;
using Lazinator.Attributes;

namespace Lazinator.Collections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlTree)]
    public interface IAvlTree<T> where T : ILazinator
    {
        AvlNode<T> Root { get; set; }
    }
}