using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Used to indicate that a property in a Lazinator interface should not be automatically be generated in the code behind, leaving the responsibility for generating the code to the user.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DoNotAutogenerateAttribute : Attribute
    {
        public DoNotAutogenerateAttribute()
        {
        }
    }
}