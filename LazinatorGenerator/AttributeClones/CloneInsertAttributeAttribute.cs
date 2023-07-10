using System;

namespace LazinatorGenerator.AttributeClones
{
    /// <summary>
    /// Provides text that should be added to the code-behind for the definition of the property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class CloneInsertAttributeAttribute : Attribute
    {
        public string AttributeText { get; set; }

        public CloneInsertAttributeAttribute(string attributeText)
        {
            AttributeText = attributeText;
        }
    }
}