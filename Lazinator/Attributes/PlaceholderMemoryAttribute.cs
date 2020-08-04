using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Specifies a method to call to read and/or write a non-Lazinator object instead of the default methods. This is used internally by LazinatorList.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class PlaceholderMemoryAttribute : Attribute
    {
        public string WriteMethod { get; set; }

        public PlaceholderMemoryAttribute(string writeMethod)
        {
            WriteMethod = writeMethod;
        }
    }
}