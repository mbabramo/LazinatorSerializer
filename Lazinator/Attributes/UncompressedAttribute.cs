using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Indicates that a property should not be compressed.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class UncompressedAttribute : Attribute
    {
        public UncompressedAttribute()
        {
        }
    }
}