using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Indicates that this property should not be enumerated by the built-in methods that enumerate Lazinator properties.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DoNotEnumerateAttribute : Attribute
    {
        public DoNotEnumerateAttribute()
        {
        }
    }
}