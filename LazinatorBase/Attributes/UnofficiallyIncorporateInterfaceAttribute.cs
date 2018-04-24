using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Attributes
{
    public class UnofficiallyIncorporateInterfaceAttribute : Attribute
    {
        public string OtherInterfaceFullyQualifiedTypeName { get; private set; }
        public Accessibility Accessibility { get; private set; }

        public UnofficiallyIncorporateInterfaceAttribute(string otherInterfaceFullyQualifiedTypeName, Accessibility accessibility)
        {
            OtherInterfaceFullyQualifiedTypeName = otherInterfaceFullyQualifiedTypeName;
            Accessibility = accessibility;
        }
    }
}
