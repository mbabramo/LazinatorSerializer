using Lazinator.Core;
using System;
using Lazinator.Attributes;

namespace CountedTree.NodeBuffers
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.InternalNodeBuffer)]
    public interface IInternalNodeBuffer<TKey> : INodeBufferBase<TKey> where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
    {
    }
}