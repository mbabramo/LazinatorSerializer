using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Indicates that information on the Lazinator version should not be included with a serialized item. This is recommended only for built-in Lazinator wrapper classes. Use of this on other classes may lead to incompatibility with later versions of Lazinator.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class ExcludeLazinatorVersionByteAttribute : Attribute
    {
        public ExcludeLazinatorVersionByteAttribute()
        {
        }

    }
}