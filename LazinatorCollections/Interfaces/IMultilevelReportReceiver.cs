using Lazinator.Attributes;
using LazinatorCollections.Interfaces;
using LazinatorCollections.Location;
using Lazinator.Core;
using System;

namespace LazinatorCollections
{
    /// <summary>
    /// An interface for a container that can receive changes from a child container and can also report them up to its own parent.
    /// </summary>
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IMultilevelReportReceiver)]
    public interface IMultilevelReportReceiver : IMultilevelReporter
    {
        void EndItemChanged(bool isFirstItem, ILazinator revisedValue, IMultilevelReporter reporter);
    }
}
