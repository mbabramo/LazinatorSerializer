using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Indicates that a Lazinator object can serve as a generic type argument to a non-Lazinator collection, even if ordinarily prohibited as a result of a configuration setting.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AllowLazinatorInNonLazinatorAttribute : Attribute
    {
        public AllowLazinatorInNonLazinatorAttribute()
        {
        }
    }
}
