using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Attributes
{
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
