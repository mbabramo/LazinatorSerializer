using System;

namespace LazinatorAnalyzer.AttributeClones
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