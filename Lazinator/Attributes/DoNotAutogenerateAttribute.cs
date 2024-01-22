using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Used to indicate that either a property in a Lazinator interface should not be automatically be generated in the code behind, 
    /// or that the entire code-behind should be left blank, leaving the responsibility for writing the code to the user.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public class DoNotAutogenerateAttribute : Attribute
    {
        public DoNotAutogenerateAttribute()
        {
        }
    }
}