using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace Lazinator.Collections.Tree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IBinaryTree)]
    public interface IBinaryTree<T> where T : ILazinator
    {
        bool AllowDuplicates { get; set; }
        bool Unbalanced { get; set; }
        BinaryNode<T> Root { get; set; }
    }
}