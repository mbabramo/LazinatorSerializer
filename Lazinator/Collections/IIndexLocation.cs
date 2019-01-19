using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace Lazinator.Collections
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IIndexLocation)]
    public interface IIndexLocation
    {
        /// <summary>
        /// The location where the item would be located.
        /// </summary>
        long Index { get; set; }
        /// <summary>
        /// The number of items in the collection.
        /// </summary>
        long Count { get; set; }
    }
}