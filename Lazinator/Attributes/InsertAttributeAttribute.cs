using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Provides text that should be added to the code-behind for the definition of the property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class InsertAttributeAttribute : Attribute
    {
        public string AttributeText { get; set; }

        public InsertAttributeAttribute(string attributeText)
        {
            AttributeText = attributeText;
        }
    }
}