namespace LazinatorCollections.Interfaces
{
    /// <summary>
    /// The location at which to find, insert, or remove an item, if duplicates are allowed. For insertions, an insertion will replace an existing item unless InsertBeforeFirst or InsertAfterLast is used.
    /// Those options should not be used for non-insertion operations.
    /// </summary>
    public enum MultivalueLocationOptions
    {
        Any,
        InsertBeforeFirst,
        First,
        Last,
        InsertAfterLast
    }
}
