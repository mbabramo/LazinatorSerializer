using System;

namespace LazinatorAnalyzer.AttributeClones
{
    /// <summary>
    /// Used to mark a field. This indicates that if IncludeChildrenMode is set to ExcludeOnlyExcludableChildren, then this field should be excluded.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CloneExcludableChildAttribute : Attribute
    {
        public CloneExcludableChildAttribute()
        {
        }
    }
}