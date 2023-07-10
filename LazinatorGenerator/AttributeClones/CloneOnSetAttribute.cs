using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorGenerator.AttributeClones
{
    /// <summary>
    /// Provides code that should run before or after a property's value is set.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CloneOnSetAttribute : Attribute
    {
        public string CodeBeforeSet { get; set; }
        public string CodeAfterSet { get; set; }

        public CloneOnSetAttribute(string codeBeforeSet, string codeAfterSet)
        {
            CodeBeforeSet = codeBeforeSet;
            CodeAfterSet = codeAfterSet;
        }
    }
}
