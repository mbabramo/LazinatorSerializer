using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Indicates that Lazinator objects implementing this interface may be split over multiple buffers, if the threshold size for
    /// splitting is reached. If the SizeOfLength attribute designates a length of 8, the object is automatically splittable.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class SplittableAttribute : Attribute
    {
        public SplittableAttribute()
        {
        }

    }
}
