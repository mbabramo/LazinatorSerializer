using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorAnalyzer.AttributeClones
{
    /// <summary>
    /// Indicates that the Lazinator interface to which it is attached corresponds to a Lazinator object of fixed length. This allows the length to be omitted.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class CloneFixedLengthLazinatorAttribute : Attribute
    {
        public int FixedLength { get; private set; }

        public CloneFixedLengthLazinatorAttribute(int fixedLength)
        {
            FixedLength = fixedLength;
        }

    }
}
