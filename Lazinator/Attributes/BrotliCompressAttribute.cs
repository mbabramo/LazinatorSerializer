using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Indicates that a string property should be Brotli compressed. This takes longer but will save space.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class BrotliCompressAttribute : Attribute
    {
        public BrotliCompressAttribute()
        {
        }
    }
}