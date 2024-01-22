using Lazinator.Core;
using Lazinator.Attributes;

namespace LazinatorAvlCollections.Avl.BinaryTree
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IBinaryTreeWithNodeType)]
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