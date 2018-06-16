using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorAnalyzer.AttributeClones
{
    /// <summary>
     /// Specifies a method to call to read and/or write a span instead of the default methods.
     /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class CloneCustomNonlazinatorWriteAttribute : Attribute
    {
        public string WriteMethod { get; set; }

        public CloneCustomNonlazinatorWriteAttribute(string writeMethod)
        {
            WriteMethod = writeMethod;
        }
    }
}
