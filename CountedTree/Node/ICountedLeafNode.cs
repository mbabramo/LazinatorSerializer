using CountedTree.ByteUtilities;
using CountedTree.Core;
using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Collections;
using System;

namespace CountedTree.Node
{
    [Lazinator((int) CountedTreeLazinatorUniqueIDs.CountedLeafNode)]
    public interface ICountedLeafNode<TKey>: ICountedNode<TKey> where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        [SetterAccessibility("private")]
        LazinatorList<KeyAndID<TKey>> Items { get; }
    }
}