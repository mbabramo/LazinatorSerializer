using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace Lazinator.Collections.Avl.ListTree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlWeightedNode)]
    internal interface IAvlListNode<T> where T : ILazinator
    {
        long LeftAggregate { get; set; }
        long SelfAggregate { get; set; }
        long RightAggregate { get; set; }
    }
}