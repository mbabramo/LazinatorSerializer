using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Indicates that the Lazinator interface to which it is attached corresponds to a Lazinator object of fixed length. This allows the length to be omitted.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class FixedLengthLazinatorAttribute : Attribute
    {
        public int FixedLength { get; private set; }

        public FixedLengthLazinatorAttribute(int fixedLength)
        {
            FixedLength = fixedLength;
        }

    }
}