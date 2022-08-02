using Lazinator.Attributes;
using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Location;
using Lazinator.Core;
using System;

namespace Lazinator.Collections
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
