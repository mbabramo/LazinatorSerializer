using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Used to indicate that a class or struct should not be handled by automatically matching constructor parameters to properties. Thus, if the class does not implement ILazinator, the user must define a custom converter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IgnoreRecordLikeAttribute : Attribute
    {
        public IgnoreRecordLikeAttribute()
        {
        }
    }
}