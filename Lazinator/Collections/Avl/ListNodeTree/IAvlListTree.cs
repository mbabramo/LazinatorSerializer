using Lazinator.Core;
using Lazinator.Attributes;
using System;
using Lazinator.Collections.Avl.ValueTree;

namespace Lazinator.Collections.Avl.ListNodeTree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlListTree)]
    internal interface IAvlListTree<T> where T : ILazinator
    {
        AvlIndexableTree<ILazinatorListable<T>> UnderlyingTree { get; set; }
    }
}