using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace LazinatorCollections.Tree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IBinaryNode)]
    public interface IBinaryNode<T> where T : ILazinator
    {
        T Value { get; set; }
        BinaryNode<T> Left { get; set; }
        BinaryNode<T> Right { get; set; }
        [DoNotAutogenerate]
        BinaryNode<T> Parent { get; set; }
        BinaryNode<T> GetNextNode();
        BinaryNode<T> GetPreviousNode();
    }
}