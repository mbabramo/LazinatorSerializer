using System;

namespace Lazinator.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DerivationKeywordAttribute : Attribute
    {
        public string Choice { get; set; }

        public DerivationKeywordAttribute(string accessibility)
        {
            Choice = accessibility;
        }
    }
}