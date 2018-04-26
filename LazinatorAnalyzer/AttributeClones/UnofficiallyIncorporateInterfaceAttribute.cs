using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorCodeGen.AttributeClones
{
    public class UnofficiallyIncorporateInterfaceAttribute : Attribute
    {
        public string OtherInterfaceFullyQualifiedTypeName { get; private set; }
        public string Accessibility { get; private set; }

        public UnofficiallyIncorporateInterfaceAttribute(string otherInterfaceFullyQualifiedTypeName, string accessibility)
        {
            OtherInterfaceFullyQualifiedTypeName = otherInterfaceFullyQualifiedTypeName;
            Accessibility = accessibility;
        }
    }
}
