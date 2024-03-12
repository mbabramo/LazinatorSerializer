using Lazinator.Attributes;
using Lazinator.Wrappers;

namespace CountedTree.Node
{
    [Lazinator((int) CountedTreeLazinatorUniqueIDs.CountedInternalNodeGeo)]
    public interface ICountedInternalNodeGeo : ICountedInternalNode<WUInt64>
    {
    }
}