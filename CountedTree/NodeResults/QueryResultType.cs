namespace CountedTree.NodeResults
{
    /// <summary>
    /// The type of information sought in the query. Note that a query to a particular node may not produce this result type, since it may produce a result indicating that further queries are needed.
    /// </summary>
    public enum QueryResultType : byte
    {
        /// <summary>
        /// The query should return a bit set, showing all items to be included but not in any particular order. Note that Skip/Take may be ignored when this is set.
        /// </summary>
        IDsAsBitSet,
        /// <summary>
        /// The query should return IDs of matching items (in order).
        /// </summary>
        IDsOnly,
        /// <summary>
        /// The query should return IDs and ranks within the query filter.
        /// </summary>
        IDsAndRanks,
        /// <summary>
        /// The query should return Keys of matching items. This is useful when calculating a distribution.
        /// </summary>
        KeysOnly,
        /// <summary>
        /// The query should return IDs and Keys.
        /// </summary>
        KeysAndIDs,
        /// <summary>
        /// The geographical query, which must be of a geography tree and thus be sorted by distance, should return keys, IDs and distance.
        /// </summary>
        KeysIDsAndDistance,
    }
}
