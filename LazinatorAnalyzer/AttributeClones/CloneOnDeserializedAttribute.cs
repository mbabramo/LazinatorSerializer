using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorAnalyzer.AttributeClones
{
    /// <summary>
    /// Provides code that should run before or after a property's value is set.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CloneOnDeserializedAttribute : Attribute
    {
        public string CodeToInsert { get; set; }

        public CloneOnDeserializedAttribute(string codeToInsert)
        {
            CodeToInsert = codeToInsert;
        }
    }
}
