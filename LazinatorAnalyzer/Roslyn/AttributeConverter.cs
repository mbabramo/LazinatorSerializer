﻿using LazinatorCodeGen.AttributeClones;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorCodeGen.Roslyn
{
    public static class AttributeConverter
    {
        public static Attribute ConvertAttribute(AttributeData attributeData)
        {
            switch (attributeData.AttributeClass.Name)
            {
                case "LazinatorAttribute":
                    var uniqueID = attributeData.GetAttributeConstructorValueByParameterName("uniqueID");
                    if (!(uniqueID is int))
                        return null;
                    var version = attributeData.GetAttributeConstructorValueByParameterName("version");
                    var autogenerate = attributeData.GetAttributeConstructorValueByParameterName("autogenerate");
                    if (version != null && autogenerate != null)
                    {
                        if (!(version is int))
                            return null;
                        if (!(autogenerate is bool))
                            return null;
                        return new CloneLazinatorAttribute((int)uniqueID, (int)version, (bool)autogenerate);
                    }
                    else
                        return new CloneLazinatorAttribute((int)uniqueID);
                case "EliminatedWithVersionAttribute":
                    var version2 = attributeData.GetAttributeConstructorValueByParameterName("version");
                    if (!(version2 is int))
                        return null;
                    return new CloneEliminatedWithVersionAttribute((int) version2);
                case "IntroducedWithVersionAttribute":
                    var version3 = attributeData.GetAttributeConstructorValueByParameterName("version");
                    if (!(version3 is int))
                        return null;
                    return new CloneIntroducedWithVersionAttribute((int)version3);
                case "SetterAccessibilityAttribute":
                    var accessibility = attributeData.GetAttributeConstructorValueByParameterName("accessibility");
                    if (!(accessibility is string))
                        return null;
                    return new CloneSetterAccessibilityAttribute((string)accessibility);
                case "UnofficiallyIncorporateInterfaceAttribute":
                    var otherInterfaceFullyQualifiedTypeName = attributeData.GetAttributeConstructorValueByParameterName("otherInterfaceFullyQualifiedTypeName");
                    if (!(otherInterfaceFullyQualifiedTypeName is string))
                        return null;
                    var accessibility2 = attributeData.GetAttributeConstructorValueByParameterName("accessibility");
                    if (!(accessibility2 is string))
                        return null;
                    return new CloneUnofficiallyIncorporateInterfaceAttribute((string) otherInterfaceFullyQualifiedTypeName, (string)accessibility2);
                case "ExcludableChildAttribute":
                    return new CloneExcludableChildAttribute();
                case "IgnoreRecordLikeAttribute":
                    return new CloneIgnoreRecordLikeAttribute();
                case "NonexclusiveLazinatorAttribute":
                    return new CloneNonexclusiveLazinatorAttribute();
                default:
                    return null;
            }
        }
    }
}
