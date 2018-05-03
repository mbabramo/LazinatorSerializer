using System;

namespace LazinatorCodeGen.AttributeClones
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CloneDerivationKeywordAttribute : Attribute
    {
        public string Choice { get; set; }

        public CloneDerivationKeywordAttribute(string accessibility)
        {
            Choice = accessibility;
        }
    }
}