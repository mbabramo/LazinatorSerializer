using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SetterAccessibilityAttribute : Attribute
    {
        public Accessibility Choice { get; set; }

        public SetterAccessibilityAttribute(Accessibility accessibility)
        {
            Choice = accessibility;
        }
    }
}
