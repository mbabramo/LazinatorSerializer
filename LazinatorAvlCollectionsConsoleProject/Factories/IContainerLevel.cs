using Lazinator.Core;
using Lazinator.Attributes;

namespace LazinatorAvlCollections.Factories
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IContainerLevel)]
    public interface IContainerLevel
    {
        /// <summary>
        /// The type of container to be used at this level.
        /// </summary>
        ContainerType ContainerType { get; set; }
        /// <summary>
        /// Whether the value container should be unbalanced, if that option is available for this type of container.
        /// </summary>
        bool Unbalanced { get; set; }
        /// <summary>
        /// Whether the value container should allow duplicate items, when a comparer is used to insert items into the value container in sorted order.
        /// </summary>
        bool AllowDuplicates { get; set; }
        /// <summary>
        /// Whether the value container should, if supported, cache the first and last items of the collection.
        /// </summary>
        bool CacheEnds { get; set; }
        /// <summary>
        /// The size at which this level should be split. If this level is an inner collection, then the inner collection is split once the inner collection exceeds this threshold. The size should represent the number of items in the inner layer, not the total aggregated number of items.
        /// </summary>
        int SplitThreshold { get; set; }
    }
}