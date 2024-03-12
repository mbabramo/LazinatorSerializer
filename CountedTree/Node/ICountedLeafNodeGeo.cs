using Lazinator.Attributes;
using Lazinator.Wrappers;

namespace CountedTree.Node
{
    [Lazinator((int) CountedTreeLazinatorUniqueIDs.CountedLeafNodeGeo)]
    public interface ICountedLeafNodeGeo : ICountedLeafNode<WUInt64>
    {
    }
}