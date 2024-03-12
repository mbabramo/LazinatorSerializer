using Lazinator.Core;
using System;
using Lazinator.Attributes;

namespace CountedTree.NodeBuffers
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.NodeBufferBase)]
    public interface INodeBufferBase<TKey> where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        [SetterAccessibility("internal")]
        CumulativeBufferedChanges Cumulative { get; }
    }
}