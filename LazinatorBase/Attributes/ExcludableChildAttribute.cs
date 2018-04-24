using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Used to mark a field. This indicates that if IncludeChildrenMode is set to ExcludeOnlyExcludableChildren, then this field should be excluded. Otherwise, it will be included.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ExcludableChildAttribute : Attribute
    {
        public ExcludableChildAttribute()
        {
        }
    }
}