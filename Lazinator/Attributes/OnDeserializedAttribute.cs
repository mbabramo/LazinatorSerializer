using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Provides code that should run after a property is deserialized.
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
