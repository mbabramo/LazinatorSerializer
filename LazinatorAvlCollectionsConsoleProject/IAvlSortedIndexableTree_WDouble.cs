using Lazinator.Attributes;
using Lazinator.Core;
using LazinatorAvlCollections.Avl.ValueTree;
using LazinatorAvlCollections;
using System;
using Lazinator.Wrappers;
using LazinatorAvlCollections.Avl;

namespace PerformanceProfiling
{
    [Lazinator(9003)]
    [SingleParent]
    [AsyncLazinatorMemory]
    public interface IAvlSortedIndexableTree_WDouble : IAvlIndexableTreeWithNodeType<WDouble, AvlCountedNode<WDouble>>
    {
    }
}