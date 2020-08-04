using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Indicates that if IncludeChildrenMode is set to ExcludeOnlyExcludableChildren, then this property should be excluded. Otherwise, it will be included.
    /// This attribute thus allows for serialization of all of a hierarchy except specified children.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ExcludableChildAttribute : Attribute
    {
        public ExcludableChildAttribute()
        {
        }
    }
}