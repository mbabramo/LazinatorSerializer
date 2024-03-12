using System;

namespace CountedTree.Updating
{
    public partial class TreeUpdateSettings : ITreeUpdateSettings
    {

        public TreeUpdateSettings(int minWorkThreshold, int maxSimultaneousUpdateNodes, int maxRequestBufferSize, TimeSpan minimumRetentionTime)
        {
            MinWorkThreshold = minWorkThreshold;
            MaxSimultaneousUpdateNodes = maxSimultaneousUpdateNodes;
            MaxRequestBufferSize = maxRequestBufferSize;
            MinimumRetentionTime = minimumRetentionTime;
        }
    }
}
