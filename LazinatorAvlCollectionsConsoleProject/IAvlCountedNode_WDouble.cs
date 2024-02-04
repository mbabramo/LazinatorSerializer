using Lazinator.Attributes;
using Lazinator.Wrappers;
using LazinatorAvlCollections.Avl.ValueTree;

namespace PerformanceProfiling
{
    [Lazinator(9006)]
    [SingleParent]
    [AsyncLazinatorMemory]
    public interface IAvlCountedNode_WDouble : IAvlCountedNode<WDouble>
    {
    }
}