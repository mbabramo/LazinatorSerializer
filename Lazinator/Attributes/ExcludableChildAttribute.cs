using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Indicates that if IncludeChildrenMode is set to ExcludeOnlyExcludableChildren, then this property should be excluded. Otherwise, it will be included.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ExcludableChildAttribute : Attribute
    {
        public ExcludableChildAttribute()
        {
        }
    }
}