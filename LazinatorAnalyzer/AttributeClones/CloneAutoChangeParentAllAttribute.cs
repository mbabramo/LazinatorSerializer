using System;

namespace LazinatorAnalyzer.AttributeClones
{
    /// <summary>
    /// Indicates that all Lazinator properties in a class implementing this interface should be treated as AutoChangeParent properties.    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CloneAutoChangeParentAllAttribute : Attribute
    {
        public CloneAutoChangeParentAllAttribute()
        {
        }
    }
}
