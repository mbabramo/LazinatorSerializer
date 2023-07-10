using System;
using System.Diagnostics;

namespace LazinatorGenerator.AttributeClones
{
    /// <summary>
    /// Allows specification of the size of the length for objects of a particular type. This can be 1, 2, 4, or 8, and
    /// 0 can be set to indicate that the length is fixed and can be skipped. If a value other than these is specified, 4
    /// is used as a default, as occurs if the attribute is omitted.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class CloneSizeOfLengthAttribute : Attribute
    {
        public byte SizeOfLength { get; set; }

        public CloneSizeOfLengthAttribute(byte sizeOfLength)
        {
            if (sizeOfLength != 0 && sizeOfLength != 1 && sizeOfLength != 2 && sizeOfLength != 4 && sizeOfLength != 8)
                sizeOfLength = 4;
            SizeOfLength = sizeOfLength;
        }
    }
}
