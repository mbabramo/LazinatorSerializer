using Lazinator.Core;
using Lazinator.Attributes;

namespace LazinatorAvlCollections.Avl.BinaryTree
{
    /// <summary>
    /// An interface for a binary tree, which can be balanced or not, can have duplicates or not, and can cache the ends or not.
    /// </summary>
    /// <typeparam name="T">The type of value to be stored</typeparam>
    /// <typeparam name="N">The type of node to be used</typeparam>
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IBinaryTreeWithNodeType)]
    [SingleParent]
    [AsyncLazinatorMemory]
    public interface IBinaryTreeWithNodeType<T, N> where T : ILazinator where N : class, ILazinator, INode<T, N>, new()
    {
        T CachedFirst { get; set; }
        T CachedLast { get; set; }
        [SetterAccessibility("protected")]
        bool AllowDuplicates { get; }
        [SetterAccessibility("protected")]
        bool Unbalanced { get; }
        [SetterAccessibility("protected")]
        bool CacheEnds { get; }
        [OnPropertyAccessed("_Root?.OnRootAccessed();")]
        N Root { get; set; }
    }
}