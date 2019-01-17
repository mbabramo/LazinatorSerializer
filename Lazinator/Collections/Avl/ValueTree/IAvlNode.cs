using Lazinator.Attributes;
using Lazinator.Core;
using System;

namespace Lazinator.Collections.Avl.ValueTree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlNode)]
    interface IAvlNode<T> where T: ILazinator
    {
        int Balance { get; set; }
    }
}
