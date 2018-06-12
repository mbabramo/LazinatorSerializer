using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Indicates that when this property is set to a Lazinator object, that object should always be cloned, and its parent class will be set appropriately. This will always be the behavior with a struct.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AutoChangeParentAttribute : Attribute
    {
        public AutoChangeParentAttribute()
        {
        }
    }
}