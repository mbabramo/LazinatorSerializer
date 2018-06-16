using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Indicates that the ordinary GetHashCode method should be used in lieu of serializing when GetBinaryHashCode32 is called.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class NonbinaryHashAttribute : Attribute
    {
        public NonbinaryHashAttribute()
        {
        }

    }
}
