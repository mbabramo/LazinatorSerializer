using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Indicates that this property will be included unless IncludeChildrenMode is ExcludeAllChildren.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IncludableChildAttribute : Attribute
    {
        public IncludableChildAttribute()
        {
        }
    }
}