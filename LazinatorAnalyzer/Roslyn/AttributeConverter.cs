using LazinatorCodeGen.AttributeClones;
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
                case nameof(LazinatorAttribute):
                    var uniqueID = attributeData.GetAttributeConstructorValueByParameterName("uniqueID");
                    if (!(uniqueID is int))
                        throw new Exception("LazinatorAttribute uniqueID is improperly formatted");
                    var version = attributeData.GetAttributeConstructorValueByParameterName("version");
                    var autogenerate = attributeData.GetAttributeConstructorValueByParameterName("autogenerate");
                    if (version != null && autogenerate != null)
                    {
                        if (!(version is int))
                            throw new Exception("LazinatorAttribute version is improperly formatted");
                        if (!(autogenerate is bool))
                            throw new Exception("LazinatorAttribute autogenerate is improperly formatted");
                        return new LazinatorAttribute((int)uniqueID, (int)version, (bool)autogenerate);
                    }
                    else
                        return new LazinatorAttribute((int)uniqueID);
                case nameof(EliminatedWithVersionAttribute):
                    var version2 = attributeData.GetAttributeConstructorValueByParameterName("version");
                    if (!(version2 is int))
                        throw new Exception("EliminatedWithVersionAttribute version is improperly formatted");
                    return new EliminatedWithVersionAttribute((int) version2);
                case nameof(IntroducedWithVersionAttribute):
                    var version3 = attributeData.GetAttributeConstructorValueByParameterName("version");
                    if (!(version3 is int))
                        throw new Exception("IntroducedWithVersionAttribute version is improperly formatted");
                    return new IntroducedWithVersionAttribute((int)version3);
                case nameof(SetterAccessibilityAttribute):
                    var accessibility = attributeData.GetAttributeConstructorValueByParameterName("accessibility");
                    if (!(accessibility is string))
                        throw new Exception("SetterAccessibilityAttribute accessibility is improperly formatted");
                    return new SetterAccessibilityAttribute((string)accessibility);
                case nameof(UnofficiallyIncorporateInterfaceAttribute):
                    var otherInterfaceFullyQualifiedTypeName = attributeData.GetAttributeConstructorValueByParameterName("otherInterfaceFullyQualifiedTypeName");
                    if (!(otherInterfaceFullyQualifiedTypeName is string))
                        throw new Exception("UnofficiallyIncorporateInterfaceAttribute otherInterfaceType is improperly formatted");
                    var accessibility2 = attributeData.GetAttributeConstructorValueByParameterName("accessibility");
                    if (!(accessibility2 is string))
                        throw new Exception("UnofficiallyIncorporateInterfaceAttribute accessibility is improperly formatted");
                    return new UnofficiallyIncorporateInterfaceAttribute((string) otherInterfaceFullyQualifiedTypeName, (string)accessibility2);
                case nameof(ExcludableChildAttribute):
                    return new ExcludableChildAttribute();
                case nameof(IgnoreRecordLikeAttribute):
                    return new IgnoreRecordLikeAttribute();
                case nameof(NonexclusiveLazinatorAttribute):
                    return new NonexclusiveLazinatorAttribute();
                default:
                    return null;
            }
        }
    }
}
