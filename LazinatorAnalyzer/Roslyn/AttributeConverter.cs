using LazinatorAnalyzer.AttributeClones;
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
                    if (version != null)
                    {
                        if (autogenerate == null)
                        {
                            if (!(version is int))
                                return null;
                            return new CloneLazinatorAttribute((int) uniqueID, (int) version);
                        }
                        else
                        {
                            if (!(version is int))
                                return null;
                            if (!(autogenerate is bool))
                                return null;
                            return new CloneLazinatorAttribute((int) uniqueID, (int) version, (bool) autogenerate);
                        }
                    }
                    else
                        return new CloneLazinatorAttribute((int)uniqueID);

                case "CustomNonlazinatorWriteAttribute":
                    var writeMethod = attributeData.GetAttributeConstructorValueByParameterName("writeMethod");
                    if (writeMethod != null && !(writeMethod is string))
                        return null;
                    return new CloneCustomNonlazinatorWriteAttribute((string)writeMethod);
                case "InsertAttributeAttribute":
                    var attributeText = attributeData.GetAttributeConstructorValueByParameterName("attributeText");
                    if (!(attributeText is string ))
                        return null;
                    return new CloneInsertAttributeAttribute((string)attributeText);
                case "FixedLengthLazinatorAttribute":
                    var fixedLength = attributeData.GetAttributeConstructorValueByParameterName("fixedLength");
                    if (!(fixedLength is int))
                        return null;
                    return new CloneFixedLengthLazinatorAttribute((int)fixedLength);
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
                case "DerivationKeywordAttribute":
                    var derivationKeyword = attributeData.GetAttributeConstructorValueByParameterName("derivationKeyword");
                    if (!(derivationKeyword is string))
                        return null;
                    return new CloneDerivationKeywordAttribute((string)derivationKeyword);
                case "UnofficiallyIncorporateInterfaceAttribute":
                    var otherInterfaceFullyQualifiedTypeName = attributeData.GetAttributeConstructorValueByParameterName("otherInterfaceFullyQualifiedTypeName");
                    if (!(otherInterfaceFullyQualifiedTypeName is string))
                        return null;
                    var accessibility2 = attributeData.GetAttributeConstructorValueByParameterName("accessibility");
                    if (!(accessibility2 is string))
                        return null;
                    return new CloneUnofficiallyIncorporateInterfaceAttribute((string) otherInterfaceFullyQualifiedTypeName, (string)accessibility2);
                case "AllowMovedAttribute":
                    return new CloneAllowMovedAttribute();
                case "AutoChangeParentAttribute":
                    return new CloneAutoChangeParentAttribute();
                case "AutoChangeParentAllAttribute":
                    return new CloneAutoChangeParentAllAttribute();
                case "AutogeneratedAttribute":
                    return new CloneAutogeneratedAttribute();
                case "FullyQualifyAttribute":
                    return new CloneFullyQualifyAttribute();
                case "ExcludableChildAttribute":
                    return new CloneExcludableChildAttribute();
                case "DoNotAutogenerateAttribute":
                    return new CloneDoNotAutogenerateAttribute();
                case "IncludableChildAttribute":
                    return new CloneIncludableChildAttribute();
                case "NonexclusiveLazinatorAttribute":
                    return new CloneNonexclusiveLazinatorAttribute();
                case "NonbinaryHashAttribute":
                    return new CloneNonbinaryHashAttribute();
                case "SmallLazinatorAttribute":
                    return new CloneSmallLazinatorAttribute();
                case "ExcludeLazinatorVersionByteAttribute":
                    return new CloneExcludeLazinatorVersionByteAttribute();
                default:
                    return null;
            }
        }
    }
}
