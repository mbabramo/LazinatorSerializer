using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorCodeGen.AttributeClones
{
    /// <summary>
    /// Indicates that information on the Lazinator version should not be included with a serialized item. This is recommended only for built-in Lazinator wrapper classes. Use of this on other classes may lead to incompatibility with later versions of Lazinator.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class CloneExcludeLazinatorVersionByteAttribute : Attribute
    {
        public CloneExcludeLazinatorVersionByteAttribute()
        {
        }

    }
}
