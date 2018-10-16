using System;

namespace LazinatorAnalyzer.AttributeClones
{
    /// <summary>
    /// Indicates that the ordinary GetHashCode method should be used in lieu of serializing when GetBinaryHashCode32 is called.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class CloneNonbinaryHashAttribute : Attribute
    {
        public CloneNonbinaryHashAttribute()
        {
        }

    }
}
