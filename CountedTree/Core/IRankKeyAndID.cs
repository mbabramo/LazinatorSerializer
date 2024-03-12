using Lazinator.Core;
using System;
using Lazinator.Attributes;

namespace CountedTree.Core
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.RankKeyAndID)]
    public interface IRankKeyAndID<TKey> where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        [SetterAccessibility("private")]
        public uint Rank { get; }
        [SetterAccessibility("private")]
        public TKey Key { get; }
        [SetterAccessibility("private")]
        public uint ID { get; }
    }
}