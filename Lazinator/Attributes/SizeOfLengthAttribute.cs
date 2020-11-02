using Lazinator.Support;
using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Allows specification of the maximum size of the 
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class SizeOfLengthAttribute : Attribute
    {
        public SizeOfLength SizeOfLength { get; set; }

        public SizeOfLengthAttribute(SizeOfLength sizeOfLength)
        {
            SizeOfLength = sizeOfLength;
        }
    }
}
