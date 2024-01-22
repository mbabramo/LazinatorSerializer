using Lazinator.Attributes;
using Lazinator.Core;
using System;

namespace LazinatorAvlCollections.Avl.ValueTree
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IAvlNode)]
    interface IAvlNode<T> where T: ILazinator
    {
        T Value { get; set; }
        AvlNode<T> Left { get; set; }
        AvlNode<T> Right { get; set; }
        [DoNotAutogenerate]
        AvlNode<T> Parent { get; set; }
        int Balance { get; set; }
    }
}
