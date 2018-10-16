namespace Lazinator.Core
{
    /// <summary>
    /// Indication of which children should be serialized along with a parent Lazinator object. When a child is serialized, the mode will also apply to its children, recursively through the hierarchy.
    /// </summary>
    public enum IncludeChildrenMode : byte
    {
        IncludeAllChildren,
        ExcludeAllChildren,
        ExcludeOnlyExcludableChildren,
        IncludeOnlyIncludableChildren
    }
}
