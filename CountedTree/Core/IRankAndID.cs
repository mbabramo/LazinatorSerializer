using Lazinator.Attributes;

namespace CountedTree.Core
{
    [Lazinator((int) CountedTreeLazinatorUniqueIDs.RankAndID)]
    public interface IRankAndID
    {
        [SetterAccessibility("private")]
        public uint Rank { get; }
        [SetterAccessibility("private")]
        public uint ID { get; }
    }
}