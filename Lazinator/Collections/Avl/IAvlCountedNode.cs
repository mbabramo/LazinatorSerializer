using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace Lazinator.Collections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlCountedNode)]
    public interface IAvlCountedNode<T> where T : ILazinator
    {
        long LeftCount { get; set; }
        long SelfCount { get; set; }
        long RightCount { get; set; }
    }
}