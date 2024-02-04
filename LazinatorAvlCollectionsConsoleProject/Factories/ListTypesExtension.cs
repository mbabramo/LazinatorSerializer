namespace LazinatorAvlCollections.Factories
{
    public static class ListTypesExtension
    {
        public static bool IsMultilayer(this ListTypes type) => type is ListTypes.AvlListMultilayer or ListTypes.AvlSortedListMultilayer or ListTypes.AvlSortedListMultilayerWithDuplicates or ListTypes.AvlListMultilayer;

        public static bool IsSorted(this ListTypes type) => type is ListTypes.LazinatorSortedList or ListTypes.LazinatorSortedListWithDuplicates or ListTypes.LazinatorSortedLinkedList or ListTypes.LazinatorSortedLinkedListWithDuplicates or ListTypes.AvlSortedList or ListTypes.AvlSortedListWithDuplicates or ListTypes.AvlSortedListMultilayer or ListTypes.AvlSortedListMultilayerWithDuplicates or ListTypes.UnbalancedAvlSortedList;

        public static bool AllowsDuplicates(this ListTypes type) => type is ListTypes.LazinatorSortedListWithDuplicates or ListTypes.LazinatorSortedLinkedListWithDuplicates or ListTypes.AvlSortedListWithDuplicates or ListTypes.AvlSortedListMultilayerWithDuplicates;

        public static bool IsUnbalanced(this ListTypes type) => type is ListTypes.UnbalancedAvlList or ListTypes.UnbalancedAvlSortedList;
    }
}
