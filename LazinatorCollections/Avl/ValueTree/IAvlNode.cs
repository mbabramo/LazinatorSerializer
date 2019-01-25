using Lazinator.Attributes;
using Lazinator.Core;
using System;

namespace LazinatorCollections.Avl.ValueTree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlNode)]
    interface IAvlNode<T> where T: ILazinator
    {
        int Balance { get; set; }
    }
}
