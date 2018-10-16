using System;

namespace LazinatorAnalyzer.AttributeClones
{
    /// <summary>
    /// Indicates the relative order in which properties are to be written and read.
    /// All primitive properties (bytes, ints, strings, etc.) are ordered together before
    /// other properties, but the relative order is applied within each group.
    /// For classes, this applies within a particular level of the class hierarchy. 
    /// The default value in the absence of the attribute is 0. 
    /// Where multiple properties share the same value, the expected behavior is undefined, 
    /// properties are generally arranged alphabetically, but this is not guaranteed.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CloneRelativeOrderAttribute : Attribute
    {
        public int RelativeOrder { get; set; }

        public CloneRelativeOrderAttribute(int relativeOrder)
        {
            RelativeOrder = relativeOrder;
        }
    }
}
