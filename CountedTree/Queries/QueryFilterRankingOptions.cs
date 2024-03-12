namespace CountedTree.Queries
{
    public enum QueryFilterRankingOptions : byte
    {
        /// <summary>
        /// Ranks returned when querying should reflect the ranking of returned items among all items in the tree, including those not within the filtered. For example, even if filtering a Restaurant rankings to Chinese restaurants, we would return indices relative to all rankings (so only the top restaurant overall would have a ranking of zero).
        /// </summary>
        RankWithinAllItems,
        /// <summary>
        /// Ranks returned when querying should reflect the ranking of returned items among a superset of the filter, which ordinarily will be less than all items. For example, if filtering a Restaurant rankings to Chinese restaurants in Virginia, we might have define a superset of all Chinese restaurants or all Virginia restaurants or all restaurants in Virginia, Maryland, or DC. Rankings will be relative to this superset.
        /// </summary>
        RankWithinSupersetOfItems,
        /// <summary>
        /// Ranks returned when querying should reflect the ranks of the items within the queried items. For example, if filtering a Restaurant rankings to Chinese restaurants, the ranks returned would be the ranks among Chinese restaurants, ignoring non-Chinese restaurants. 
        /// </summary>
        RankWithinFilter,
    }
}
