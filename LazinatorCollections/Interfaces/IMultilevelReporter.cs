using Lazinator.Attributes;
using LazinatorCollections.Interfaces;
using LazinatorCollections.Location;
using Lazinator.Core;
using System;

namespace LazinatorCollections
{
    /// <summary>
    /// A nonexclusive Lazinator interface for a container that can report changes up to its parent
    /// </summary>
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IMultilevelReporter)]
    public interface IMultilevelReporter
    {
        [DoNotAutogenerate]
        IMultilevelReportReceiver MultilevelReporterParent { get; set; }
    }
}
