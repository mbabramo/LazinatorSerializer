using Lazinator.Core;
using Lazinator.Attributes;

namespace LazinatorCollections.Tree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IBinaryTree)]
    public interface IBinaryTree<T> where T : ILazinator
    {
        T CachedFirst { get; set; }
        T CachedLast { get; set; }
        [SetterAccessibility("protected")]
        bool AllowDuplicates { get; }
        [SetterAccessibility("protected")]
        bool Unbalanced { get; }
        BinaryNode<T> Root { get; set; }
    }
}