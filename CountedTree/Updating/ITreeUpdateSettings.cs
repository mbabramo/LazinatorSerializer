using System;
using Lazinator.Attributes;

namespace CountedTree.Updating
{
    [Lazinator((int) CountedTreeLazinatorUniqueIDs.TreeUpdateSettings)]
    public interface ITreeUpdateSettings
    {
        int MinWorkThreshold { get; set; }
        int MaxSimultaneousUpdateNodes { get; set; }
        int MaxRequestBufferSize { get; set; }
        TimeSpan MinimumRetentionTime { get; set; }
    }
}