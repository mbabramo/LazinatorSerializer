using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorAnalyzer.AttributeClones
{
    /// <summary>
    /// Provides code that should be inserted at the top of the Lazinator class or struct.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
    public class CloneInsertCodeAttribute : Attribute
    {
        public string CodeToInsert { get; set; }

        public CloneInsertCodeAttribute(string codeToInsert)
        {
            throw new Exception("DEBUGQQQ");
            CodeToInsert = codeToInsert;
        }
    }
}
