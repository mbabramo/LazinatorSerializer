using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Indicates that the Lazinator object should implement the ILazinatorAsync interface and include properties that access
    /// memory storage asynchronously. This is useful if the storage may be lazily loaded.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class CloneAsyncLazinatorMemoryAttribute : Attribute
    {
        public CloneAsyncLazinatorMemoryAttribute()
        {
        }
    }
}