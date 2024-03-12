using Lazinator.Core;
using System;
using Lazinator.Attributes;

namespace CountedTree.Updating
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.LastClientAccess)]
    public interface ILastClientAccess
    {
        DateTime LastAccessTime { get; set; }
        long VersionNumber { get; set; }
    }
}