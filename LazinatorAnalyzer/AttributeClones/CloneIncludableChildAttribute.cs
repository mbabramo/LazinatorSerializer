using System;

namespace LazinatorCodeGen.AttributeClones
{
    /// <summary>
    /// Indicates that this property will be included unless IncludeChildrenMode is ExcludeAllChildren.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CloneIncludableChildAttribute : Attribute
    {
        public CloneIncludableChildAttribute()
        {
        }
    }
}