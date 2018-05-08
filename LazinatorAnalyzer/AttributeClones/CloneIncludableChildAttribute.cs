using System;

namespace LazinatorCodeGen.AttributeClones
{
    /// <summary>
    /// Used to mark a field. This indicates that if IncludeChildrenMode is set to IncludeOnlyIncludableChildren, then this field should be included.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CloneIncludableChildAttribute : Attribute
    {
        public CloneIncludableChildAttribute()
        {
        }
    }
}