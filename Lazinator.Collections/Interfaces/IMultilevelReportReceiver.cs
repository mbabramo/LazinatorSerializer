using Lazinator.Attributes;
using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Location;
using Lazinator.Core;
using System;

namespace Lazinator.Collections
{
    /// <summary>
    /// A nonexclusive Lazinator interface for a container that can receive changes from a child container and can also report them up to its own parent.
    /// </summary>
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IMultilevelReportReceiver)]
    public interface IMultilevelReportReceiver : IMultilevelReporter
    {
        void EndItemChanged(bool isFirstItem, ILazinator revisedValue, IMultilevelReporter reporter);
        void EndItemRemoved(bool wasFirstItem, IMultilevelReporter reporter);
    }
}
