using Lazinator.Attributes;
using LazinatorCollections.Interfaces;
using LazinatorCollections.Location;
using Lazinator.Core;
using System;

namespace LazinatorCollections
{
    /// <summary>
    /// An interface for a container that can report changes up to its parent
    /// </summary>
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IMultilevelReporter)]
    public interface IMultilevelReporter
    {
        [DoNotAutogenerate]
        IMultilevelReportReceiver MultilevelReporterParent { get; set; }
    }
}
