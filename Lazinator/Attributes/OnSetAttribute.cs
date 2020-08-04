using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Provides code that should run after a property's value is set.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class OnSetAttribute : Attribute
    {
        public string CodeBeforeSet { get; set; }
        public string CodeAfterSet { get; set; }

        public OnSetAttribute(string codeBeforeSet, string codeAfterSet)
        {
            CodeBeforeSet = codeBeforeSet;
            CodeAfterSet = codeAfterSet;
        }
    }
}