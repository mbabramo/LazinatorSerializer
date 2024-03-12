using Lazinator.Core;
using System;
using Lazinator.Attributes;

namespace CountedTree.NodeBuffers
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.CombinedInternalNodeBuffer)]
    public interface ICombinedInternalNodeBuffer<TKey> : INodeBufferBase<TKey> where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        INodeBufferBaseMethods<TKey> NodeBuffer {get; set;}
        INodeBufferBaseMethods<TKey> RequestBuffer {get; set;}
    }
}