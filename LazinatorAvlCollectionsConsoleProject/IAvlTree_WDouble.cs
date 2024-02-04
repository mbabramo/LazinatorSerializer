using Lazinator.Attributes;
using Lazinator.Core;
using LazinatorAvlCollections.Avl.ValueTree;
using LazinatorAvlCollections;
using Lazinator.Wrappers;

namespace PerformanceProfiling
{
    [Lazinator((int)9001)]
    [SingleParent]
    [AsyncLazinatorMemory]
    public interface IAvlTree_WDouble : IAvlTreeWithNodeType<WDouble, AvlNode_WDouble> 
    {
    }
}