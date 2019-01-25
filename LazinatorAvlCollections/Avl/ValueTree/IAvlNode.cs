using Lazinator.Attributes;
using Lazinator.Core;
using System;

namespace LazinatorAvlCollections.Avl.ValueTree
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IAvlNode)]
    interface IAvlNode<T> where T: ILazinator
    {
        int Balance { get; set; }
    }
}
