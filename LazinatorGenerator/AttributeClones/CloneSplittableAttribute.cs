using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorGenerator.AttributeClones
{
    /// <summary>
    /// Indicates that Lazinator objects implementing this interface may be split over multiple buffers, if the threshold size for
    /// splitting is reached. If the SizeOfLength attribute designates a length of 8, the object is automatically splittable.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class CloneSplittableAttribute : Attribute
    {
        public CloneSplittableAttribute()
        {
        }

    }
}
