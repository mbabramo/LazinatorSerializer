using System;

namespace LazinatorGenerator.AttributeClones
{
    /// <summary>
    /// Indicates that non-Lazinator properties may be used as generic type arguments in a non-abstract class.
    /// This is dangerous because actually attempting to read and write properties of a non-Lazinator open generic type will not work.
    /// This may be useful when a generic type argument for a class is not serialized.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class CloneAllowNonlazinatorOpenGenericsAttribute : Attribute
    {
        public CloneAllowNonlazinatorOpenGenericsAttribute()
        {
        }
    }
}
