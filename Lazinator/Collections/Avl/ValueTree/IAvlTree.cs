using Lazinator.Core;
using Lazinator.Attributes;
using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Location;
using Lazinator.Collections.Tree;

namespace Lazinator.Collections.Avl.ValueTree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlTree)]
    public interface IAvlTree<T> : ILazinator where T : ILazinator
    {
        string ToTreeString();
        [DoNotAutogenerate]
        AvlNode<T> AvlRoot { get; }
        IValueContainer<T> CreateNewWithSameSettings();
        void InsertAt(IContainerLocation location, T item);
        void RemoveNode(BinaryNode<T> node);
    }
}