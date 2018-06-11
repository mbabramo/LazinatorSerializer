using System;

namespace LazinatorCodeGen.AttributeClones
{
    /// <summary>
    /// Indicates that all Lazinator properties in a class implementing this interface should be treated as Autoclone properties.    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CloneAutocloneAllAttribute : Attribute
    {
        public CloneAutocloneAllAttribute()
        {
        }
    }
}
