using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Indicates that the Lazinator interface to which it is attached corresponds to a Lazinator object that will never require more than 250 bytes to serialize. This allows the Lazinator object to be stored more compactly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class SmallLazinatorAttribute : Attribute
    {
        public SmallLazinatorAttribute()
        {
        }
        
    }
}