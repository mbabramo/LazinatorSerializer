using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Attributes
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = true)]
    public class UnofficiallyIncorporateInterfaceAttribute : Attribute
    {
        public string OtherInterfaceFullyQualifiedTypeName { get; private set; }
        public string Accessibility { get; private set; }

        /// <summary>
        /// Constructor for the UnofficiallyIncorporateInterfaceAttribute
        /// </summary>
        /// <param name="otherInterfaceFullyQualifiedTypeName">The fully qualified type name, including the namespace hierarchy and also an arity suffix for generic types (e.g., `1 or `2).</param>
        /// <param name="accessibility">The level of accessibility desired (e.g., private).</param>
        public UnofficiallyIncorporateInterfaceAttribute(string otherInterfaceFullyQualifiedTypeName, string accessibility)
        {
            OtherInterfaceFullyQualifiedTypeName = otherInterfaceFullyQualifiedTypeName;
            Accessibility = accessibility;
        }
    }
}
