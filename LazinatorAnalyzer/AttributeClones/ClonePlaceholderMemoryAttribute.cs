using System;

namespace LazinatorAnalyzer.AttributeClones
{
    /// <summary>
    /// Specifies a method to call to read and/or write a span instead of the default methods.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ClonePlaceholderMemoryAttribute : Attribute
    {
        public string WriteMethod { get; set; }

        public ClonePlaceholderMemoryAttribute(string writeMethod)
        {
            WriteMethod = writeMethod;
        }
    }
}
