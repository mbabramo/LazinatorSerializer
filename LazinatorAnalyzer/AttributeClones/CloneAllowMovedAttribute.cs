using System;

namespace LazinatorCodeGen.AttributeClones
{
    /// <summary>
    /// Indicates that this property can be set to a Lazinator object that already has a parent. This parent will be replaced. 
    /// This is dangerous because it can cause problems when serializing the original hierarchy.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CloneAllowMovedAttribute : Attribute
    {
        public CloneAllowMovedAttribute()
        {
        }
    }
}