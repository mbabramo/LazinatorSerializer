namespace Lazinator.Core
{
    /// <summary>
    /// Indicates which children should be serialized along with a parent Lazinator object. When a child is serialized, the mode will also apply to its children, recursively through the hierarchy. Children can be designated as excludable or includable with appropriate attributes. Any non-nullable reference type object will always be included, regardless of this setting. 
    /// </summary>
    public enum IncludeChildrenMode : byte
    {
        /// <summary>
        /// Includes all child objects. This should be chosen in ordinary circumstances. 
        /// </summary>
        IncludeAllChildren,
        /// <summary>
        /// Excludes all child objects. Only primitive properties (such as int and string) are included in the serialization.
        /// </summary>
        ExcludeAllChildren,
        /// <summary>
        /// Includes all children except those that include an ExcludableChildAttribute.
        /// </summary>
        ExcludeOnlyExcludableChildren,
        /// <summary>
        /// Excludes all children except those that include an IncludableChildAttribute.
        /// </summary>
        IncludeOnlyIncludableChildren
    }
}
