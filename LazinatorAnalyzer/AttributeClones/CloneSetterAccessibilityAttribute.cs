using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorCodeGen.AttributeClones
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CloneSetterAccessibilityAttribute : Attribute
    {
        public string Choice { get; set; }

        public CloneSetterAccessibilityAttribute(string accessibility)
        {
            Choice = accessibility;
        }
    }
}
