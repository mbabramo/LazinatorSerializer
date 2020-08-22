using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Indicates that the Lazinator property modified by this attribute should be eagerly deserialized -- that is, that it should be immediately deserialized rather than
    /// deserialized only once called by user code.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EagerAttribute : Attribute
    {
        public EagerAttribute()
        {
        }
    }
}
