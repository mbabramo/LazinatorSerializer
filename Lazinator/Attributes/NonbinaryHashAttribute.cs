using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Indicates that the ordinary GetHashCode method should be used in lieu of serializing when GetBinaryHashCode32 is called. This
    /// is useful when for many classes, binary hashes are used, but it is more efficient with some classes to use ordinary GetHashCode.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class NonbinaryHashAttribute : Attribute
    {
        public NonbinaryHashAttribute()
        {
        }

    }
}
