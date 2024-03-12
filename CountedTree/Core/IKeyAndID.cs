using Lazinator.Attributes;
using Lazinator.Core;
using System;

namespace CountedTree.Core
{
    [Lazinator((int) CountedTreeLazinatorUniqueIDs.KeyAndID)]
    public interface IKeyAndID<TKey> where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        [SetterAccessibility("private")]
        public TKey Key { get; }
        [SetterAccessibility("private")]
        public uint ID { get; }
    }
}