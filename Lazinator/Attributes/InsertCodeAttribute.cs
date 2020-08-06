using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Provides code that should be inserted at the top of the Lazinator class or struct.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
    public class InsertCodeAttribute : Attribute
    {
        public string CodeToInsert { get; set; }

        public InsertCodeAttribute(string codeToInsert)
        {
            throw new Exception("DEBUGQQQ");
            CodeToInsert = codeToInsert;
        }
    }
}