using Lazinator.Attributes;
using System;

namespace CountedTree.Core
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.TreeInfo)]
    public interface ITreeInfo
    {
        public Guid TreeID { get; set; }
        public long CurrentRootID { get; set; }
        public uint NumValuesInTree { get; set; }
        public uint MaxDepth { get; set; }
        public int WorkNeeded { get; set; }
    }
}