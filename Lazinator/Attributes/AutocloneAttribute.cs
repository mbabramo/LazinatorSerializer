using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Indicates that when this property is set to a Lazinator object, that object should always be cloned. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AutocloneAttribute : Attribute
    {
        public AutocloneAttribute()
        {
        }
    }
}