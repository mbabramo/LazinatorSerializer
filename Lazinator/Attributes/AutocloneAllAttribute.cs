using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Indicates that all Lazinator properties in a class implementing this interface should be treated as Autoclone properties.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = true)]
    public class AutocloneAllAttribute : Attribute
    {
        public AutocloneAllAttribute()
        {
        }
    }
}