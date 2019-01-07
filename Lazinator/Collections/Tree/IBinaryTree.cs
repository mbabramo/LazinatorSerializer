using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace Lazinator.Collections.Tree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IBinaryTree)]
    public interface IBinaryTree<T> where T : ILazinator
    {
        BinaryNode<T> Root { get; set; }
    }
}