using Lazinator.Attributes;
using Lazinator.Core;

namespace LazinatorAvlCollections.Avl.ValueTree
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IAvlIndexableTree)]
    [SingleParent]
    [AsyncLazinatorMemory]
    public interface IAvlIndexableTree<T> : IAvlIndexableTreeWithNodeType<T, AvlCountedNode<T>> 
        where T : ILazinator
    {
    }
}