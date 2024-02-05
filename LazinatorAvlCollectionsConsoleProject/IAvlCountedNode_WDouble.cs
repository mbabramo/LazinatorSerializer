using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Wrappers;
using LazinatorAvlCollections.Avl.ValueTree;

namespace PerformanceProfiling
{
    [Lazinator(9006)]
    [SingleParent]
    [AsyncLazinatorMemory]
    public interface IAvlCountedNode_WDouble : ILazinator
    {

        WDouble Value { get; set; }
        long LeftCount { get; set; }
        long RightCount { get; set; }
        [OnPropertyAccessed("OnChildNodeAccessed(_Left, true);")]
        AvlCountedNode_WDouble Left { get; set; }
        [OnPropertyAccessed("OnChildNodeAccessed(_Right, false);")]
        AvlCountedNode_WDouble Right { get; set; }
        [DoNotAutogenerate]
        AvlCountedNode_WDouble Parent { get; set; }

        int Balance { get; set; }
    }
}