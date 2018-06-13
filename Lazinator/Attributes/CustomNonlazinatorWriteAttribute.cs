using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Specifies a method to call to read and/or write a span instead of the default methods.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class CustomNonlazinatorWriteAttribute : Attribute
    {
        public string WriteMethod { get; set; }

        public CustomNonlazinatorWriteAttribute(string writeMethod)
        {
            WriteMethod = writeMethod;
        }
    }
}