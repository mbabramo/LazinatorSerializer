namespace Lazinator.CodeGeneration
{
    public enum LazinatorSupportedCollectionType : byte
    {
        List,
        Memory,
        HashSet,
        Array,
        Dictionary,
        ReadOnlySpan,
        Queue,
        Stack,
        SortedDictionary,
        SortedList,
        LinkedList,
        SortedSet
    }
}