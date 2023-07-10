using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorGenerator.AttributeClones
{
    /// <summary>
    /// Provides code that should run before or after a property's value is set.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CloneOnPropertyAccessedAttribute : Attribute
    {
        public string CodeToInsert { get; set; }

        public CloneOnPropertyAccessedAttribute(string codeToInsert)
        {
            CodeToInsert = codeToInsert;
        }
    }
}
