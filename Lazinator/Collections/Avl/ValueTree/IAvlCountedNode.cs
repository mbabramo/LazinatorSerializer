using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace Lazinator.Collections.Avl.ValueTree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlCountedNode)]
    public interface IAvlCountedNode<T> where T : ILazinator
    {
        long LeftCount { get; set; }
        [DoNotAutogenerate]
        long SelfCount { get; }
        long RightCount { get; set; }
    }
}