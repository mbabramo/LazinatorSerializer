using Lazinator.Attributes;
using Lazinator.Core;
using LazinatorAvlCollections.Avl.ValueTree;
using LazinatorAvlCollections;
using Lazinator.Wrappers;

namespace PerformanceProfiling
{
    [Lazinator((int)9000)]
    [SingleParent]
    [AsyncLazinatorMemory]
    interface IAvlNode_WDouble
    {
        WDouble Value { get; set; }
        AvlNode_WDouble Left { get; set; }
        AvlNode_WDouble Right { get; set; }

        [DoNotAutogenerate]
        AvlNode_WDouble Parent { get; set; }
        int Balance { get; set; }
    }
}