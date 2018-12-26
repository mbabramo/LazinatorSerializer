using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Used to indicate that, within the Lazinator class/struct, a child ref struct should be generated with the same basic functionality as the parent Lazinator object. Also, ToRefStruct and FromRefStruct methods will be generated. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class GenerateRefStructAttribute : Attribute
    {
        public GenerateRefStructAttribute()
        {
        }
    }
}