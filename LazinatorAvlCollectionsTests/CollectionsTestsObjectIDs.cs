namespace LazinatorTests.Examples
{
    // IMPORTANT: Even if a self-serializing class is deleted and is no longer represented in an data, it must keep its place in enums like this, because otherwise all other IDs will change. But you can rename it so that it's clear that it refers to a class that doesn't exist anymore.

    public enum CollectionsTestsObjectIDs
    {
        INonComparableWrapper = 2100,
        INonComparableWrapperString,
        IStructWithBadHashFunction,
    }
}
