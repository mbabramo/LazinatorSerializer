using System;

namespace LazinatorGenerator.AttributeClones
{
    /// <summary>
    /// Indicates that an additional property should be created allowing ref access to the underlying property, which must be a primitive. For example, if applying this to a property char MyChar, then a ref char MyChar_Ref property will be created. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CloneIncludeRefPropertyAttribute : Attribute
    {
        public CloneIncludeRefPropertyAttribute()
        {
        }
    }
}