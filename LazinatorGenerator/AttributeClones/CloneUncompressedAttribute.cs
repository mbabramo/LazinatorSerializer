using System;

namespace LazinatorGenerator.AttributeClones
{
    /// <summary>
    /// Indicates that a property should not be compressed.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CloneUncompressedAttribute : Attribute
    {
        public CloneUncompressedAttribute()
        {
        }
    }
}