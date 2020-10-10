using System;

namespace LazinatorAnalyzer.AttributeClones
{
    /// <summary>
    /// Indicates that the Lazinator interface to which it is attached corresponds to a Lazinator object that will never require more than 255 bytes to serialize. This allows the Lazinator object to be stored more compactly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class CloneSingleByteLengthAttribute : Attribute
    {
        public CloneSingleByteLengthAttribute()
        {
        }
        
    }
}