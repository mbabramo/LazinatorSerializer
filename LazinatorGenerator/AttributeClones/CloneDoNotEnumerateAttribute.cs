using System;

namespace LazinatorGenerator.AttributeClones
{
    /// <summary>
    /// Indicates that this property should not be enumerated by the built-in methods that enumerate properties.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CloneDoNotEnumerateAttribute : Attribute
    {
        public CloneDoNotEnumerateAttribute()
        {
        }
    }
}