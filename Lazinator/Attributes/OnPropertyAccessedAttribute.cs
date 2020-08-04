using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Provides code that should run whenever a property value is accessed.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class OnPropertyAccessedAttribute : Attribute
    {
        public string CodeToInsert { get; set; }

        public OnPropertyAccessedAttribute(string codeToInsert)
        {
            CodeToInsert = codeToInsert;
        }
    }
}
