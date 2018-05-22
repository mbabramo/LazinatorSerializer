using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Indicates that this property's type should be fully qualified in the generated code-behind. Useful when a type has a name that otherwise would conflict.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FullyQualifyAttribute : Attribute
    {
        public FullyQualifyAttribute()
        {
        }
    }
}