using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Allows a setter for a property to be designated as "private", "protected", or "internal". The Lazinator interface may then omit "set", and an appropriate set will be created in the code-behind.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SetterAccessibilityAttribute : Attribute
    {
        public string Choice { get; set; }

        public SetterAccessibilityAttribute(string accessibility)
        {
            Choice = accessibility;
        }
    }
}
