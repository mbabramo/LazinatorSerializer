﻿using Lazinator.Attributes;
using LazinatorGenerator.AttributeClones;
using Microsoft.CodeAnalysis;
using System;
using System.Linq;

namespace LazinatorCodeGen.Roslyn
{
    public static class AttributeConverter
    {
        /// <summary>
        /// Return a clone of a Lazinator attribute based on the AttributeData produced by Roslyn.
        /// We use this approach because our generator code does not reference the Lazinator library.
        /// </summary>
        /// <param name="attributeData"></param>
        /// <returns></returns>
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

                case "PlaceholderMemoryAttribute":
                    var writeMethod = attributeData.GetAttributeConstructorValueByParameterName("writeMethod");
                    if (writeMethod != null && !(writeMethod is string))
                        return null;
                    return new ClonePlaceholderMemoryAttribute((string)writeMethod);
                case "NullableContextSettingAttribute":
                    var enabled = attributeData.GetAttributeConstructorValueByParameterName("enabled");
                    if (enabled != null && !(enabled is bool))
                        return null;
                    return new CloneNullableContextSettingAttribute((bool)enabled);
                case "ImplementsAttribute":
                    var implemented = attributeData.GetAttributeConstructorValuesByParameterName("implemented");
                    if (implemented != null && implemented.Any(x => !(x is string)))
                        return null;
                    return new CloneImplementsAttribute(implemented.Select(x => (string) x).ToArray());
                case "InsertAttributeAttribute":
                    var attributeText = attributeData.GetAttributeConstructorValueByParameterName("attributeText");
                    if (!(attributeText is string ))
                        return null;
                    return new CloneInsertAttributeAttribute((string)attributeText);
                case "InsertCodeAttribute":
                    var codeToInsert = attributeData.GetAttributeConstructorValueByParameterName("codeToInsert");
                    if (!(codeToInsert is string))
                        return null;
                    return new CloneInsertCodeAttribute((string)codeToInsert);
                case "OnDeserializedAttribute":
                    var codeToInsert2 = attributeData.GetAttributeConstructorValueByParameterName("codeToInsert");
                    if (!(codeToInsert2 is string))
                        return null;
                    return new CloneOnDeserializedAttribute((string)codeToInsert2);
                case "OnPropertyAccessedAttribute":
                    var codeToInsert3 = attributeData.GetAttributeConstructorValueByParameterName("codeToInsert");
                    if (!(codeToInsert3 is string))
                        return null;
                    return new CloneOnPropertyAccessedAttribute((string)codeToInsert3);
                case "OnSetAttribute":
                    var codeBeforeSet = attributeData.GetAttributeConstructorValueByParameterName("codeBeforeSet");
                    var codeAfterSet = attributeData.GetAttributeConstructorValueByParameterName("codeAfterSet");
                    if (codeBeforeSet != null && !(codeBeforeSet is string))
                        return null;
                    if (codeAfterSet != null && !(codeAfterSet is string))
                        return null;
                    return new CloneOnSetAttribute((string)codeBeforeSet, (string)codeAfterSet);
                case "SizeOfLengthAttribute":
                    var sizeOfLength = attributeData.GetAttributeConstructorValueByParameterName("sizeOfLength");
                    if (!(sizeOfLength is byte))
                        return null;
                    return new CloneSizeOfLengthAttribute((byte)sizeOfLength);
                case "SkipIfAttribute":
                    var skipCondition = attributeData.GetAttributeConstructorValueByParameterName("skipCondition");
                    if (!(skipCondition is string))
                        return null;
                    var initializeWhenSkipped = attributeData.GetAttributeConstructorValueByParameterName("initializeWhenSkipped");
                    if (initializeWhenSkipped != null && !(initializeWhenSkipped is string))
                        return null;
                    return new CloneSkipIfAttribute((string)skipCondition, (string)initializeWhenSkipped);
                case "RelativeOrderAttribute":
                    var relativeOrder = attributeData.GetAttributeConstructorValueByParameterName("relativeOrder");
                    if (!(relativeOrder is int))
                        return null;
                    return new CloneRelativeOrderAttribute((int)relativeOrder);
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
                case "AllowLazinatorInNonLazinatorAttribute":
                    return new CloneAllowLazinatorInNonLazinatorAttribute();
                case "AllowNonlazinatorOpenGenericsAttribute":
                    return new CloneAllowNonlazinatorOpenGenericsAttribute();
                case "AsyncLazinatorMemoryAttribute":
                    return new CloneAsyncLazinatorMemoryAttribute();
                case "AutogeneratedAttribute":
                    return new CloneAutogeneratedAttribute();
                case "DoNotAutogenerateAttribute":
                    return new CloneDoNotAutogenerateAttribute();
                case "DoNotEnumerateAttribute":
                    return new CloneDoNotEnumerateAttribute();
                case "EagerAttribute":
                    return new CloneEagerAttribute();
                case "ExcludableChildAttribute":
                    return new CloneExcludableChildAttribute();
                case "ExcludeLazinatorVersionByteAttribute":
                    return new CloneExcludeLazinatorVersionByteAttribute();
                case "FullyQualifyAttribute":
                    return new CloneFullyQualifyAttribute();
                case "GenerateRefStructAttribute":
                    return new CloneGenerateRefStructAttribute();
                case "IncludableChildAttribute":
                    return new CloneIncludableChildAttribute();
                case "IncludeRefPropertyAttribute":
                    return new CloneIncludeRefPropertyAttribute();
                case "NonbinaryHashAttribute":
                    return new CloneNonbinaryHashAttribute();
                case "NonexclusiveLazinatorAttribute":
                    return new CloneNonexclusiveLazinatorAttribute();
                case "SingleParentAttribute":
                    return new CloneSingleParentAttribute();
                case "SplittableAttribute":
                    return new CloneSplittableAttribute();
                case "UncompressedAttribute":
                    return new CloneUncompressedAttribute();
                default:
                    return null;
            }
        }
    }
}
