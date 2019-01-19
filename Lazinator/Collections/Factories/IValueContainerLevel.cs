using Lazinator.Core;
using Lazinator.Attributes;

namespace Lazinator.Collections.Factories
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IValueContainerLevel)]
    public interface IValueContainerLevel
    {
        /// <summary>
        /// The type of value container to be used at this level.
        /// </summary>
        ValueContainerType ValueContainerType { get; set; }
        /// <summary>
        /// Whether the value container should be unbalanced, if that option is available for this type of container.
        /// </summary>
        bool Unbalanced { get; set; }
        /// <summary>
        /// Whether the value container should allow duplicate items, when a comparer is used to insert items into the value container in sorted order.
        /// </summary>
        bool AllowDuplicates { get; set; }
        /// <summary>
        /// The size at which this level should be split. If this level is an interior collection, then the interior collection is split once the interior collection exceeds this threshold. The size may represent the total number of items, if the value container is counted, or the approproximate depth, if it is not. 
        /// </summary>
        long SplitThreshold { get; set; }
    }
}