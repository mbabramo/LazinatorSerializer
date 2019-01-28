using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Provides code that should run before or after a property's value is set.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class OnDeserializedAttribute : Attribute
    {
        public string CodeToInsert { get; set; }

        public OnDeserializedAttribute(string codeToInsert)
        {
            CodeToInsert = codeToInsert;
        }
    }
}
