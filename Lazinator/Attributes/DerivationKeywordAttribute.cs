using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Adds a derivation keyword (e.g., "virtual" or "override") to the corresponding Lazinator property. 
    /// This is useful when you plan to override the class with a non-Lazinator class or when a Lazinator class is derived
    /// from a non-Lazinator class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DerivationKeywordAttribute : Attribute
    {
        public string Choice { get; set; }

        public DerivationKeywordAttribute(string derivationKeyword)
        {
            Choice = derivationKeyword;
        }
    }
}