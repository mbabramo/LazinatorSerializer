using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace Lazinator.Collections.Tree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IBinaryTree)]
    public interface IBinaryTree<T> where T : ILazinator
    {
        [SetterAccessibility("protected")]
        bool AllowDuplicates { get; }
        [SetterAccessibility("protected")]
        bool Unbalanced { get; }
        BinaryNode<T> Root { get; set; }
    }
}