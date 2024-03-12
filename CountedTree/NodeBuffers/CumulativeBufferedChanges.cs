namespace CountedTree.NodeBuffers
{
    public partial class CumulativeBufferedChanges : ICumulativeBufferedChanges
    {

        public CumulativeBufferedChanges(int[] cumulativePendingChangesAtIndexAscending, int[] cumulativePendingChangesAtIndexDescending, int[] cumulativeNetItemChangeAtIndexAscending, int[] cumulativeNetItemChangeAtIndexDescending, int maxPendingChanges)
        {
            CumulativePendingChangesAtIndexAscending = cumulativePendingChangesAtIndexAscending;
            CumulativePendingChangesAtIndexDescending = cumulativePendingChangesAtIndexDescending;
            CumulativeNetItemChangeAtIndexAscending = cumulativeNetItemChangeAtIndexAscending;
            CumulativeNetItemChangeAtIndexDescending = cumulativeNetItemChangeAtIndexDescending;
            MaxPendingChanges = maxPendingChanges;
        }
    }
}
