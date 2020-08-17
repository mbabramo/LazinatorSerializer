using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using LazinatorAnalyzer.AttributeClones;
using LazinatorAnalyzer.Settings;
using LazinatorAnalyzer.Support;
using LazinatorCodeGen.Roslyn;
using Microsoft.CodeAnalysis;
using static LazinatorCodeGen.Roslyn.RoslynHelpers;

namespace Lazinator.CodeDescription
{
    public class PropertyDescription
    {
        #region Properties

        /* Type and object information */
        private ObjectDescription ContainingObjectDescription { get; set; }
        private bool ContainerIsClass => ContainingObjectDescription.ObjectType == LazinatorObjectType.Class && !ContainingObjectDescription.GeneratingRefStruct;
        private bool ContainerIsStruct => ContainingObjectDescription.ObjectType == LazinatorObjectType.Struct || ContainingObjectDescription.GeneratingRefStruct;
        private PropertyDescription ContainingPropertyDescription { get; set; }
        public PropertyDescription OutmostPropertyDescription => ContainingPropertyDescription?.OutmostPropertyDescription ?? this;
        private int UniqueIDForLazinatorType { get; set; }
        internal IPropertySymbol PropertySymbol { get; set; }
        private ITypeSymbol TypeSymbolIfNoProperty { get; set; }
        private ITypeSymbol Symbol => PropertySymbol != null ? (ITypeSymbol)PropertySymbol.Type : (ITypeSymbol)TypeSymbolIfNoProperty;
        internal bool GenericConstrainedToClass => Symbol is ITypeParameterSymbol typeParameterSymbol && typeParameterSymbol.HasReferenceTypeConstraint;
        internal bool GenericConstrainedToStruct => Symbol is ITypeParameterSymbol typeParameterSymbol && typeParameterSymbol.HasValueTypeConstraint;
        internal string DerivationKeyword { get; set; }
        private bool IsAbstract { get; set; }
        public NullableContext NullableContextSetting { get; set; }
        public bool NullableModeEnabled => NullableContextSetting.WarningsEnabled(); // TODO && NullableContextSetting.AnnotationsEnabled();
        public bool NullableModeInherited => NullableContextSetting.WarningsInherited(); // TODO annotations
        public string QuestionMarkIfNullableModeEnabled => NullableModeEnabled ? "?" : "";
        public string QuestionMarkIfNullableAndNullableModeEnabled => Nullable && NullableModeEnabled ? "?" : "";
        public string ILazinatorString => "ILazinator" + QuestionMarkIfNullableModeEnabled;
        public string ILazinatorStringWithItemSpecificNullability => "ILazinator" + QuestionMarkIfNullableAndNullableModeEnabled;
        internal bool Nullable { get; set; }
        internal bool SymbolEndsWithQuestionMark => Symbol.ToString().EndsWith("?");
        internal bool ReferenceTypeIsNullable => NullableModeEnabled ? SymbolEndsWithQuestionMark : true;
        internal string NullForgiveness => NullableModeEnabled ? "!" : "";
        private bool HasParameterlessConstructor => PropertySymbol.Type is INamedTypeSymbol namedTypeSymbol && namedTypeSymbol.InstanceConstructors.Any(y => !y.IsImplicitlyDeclared && !y.Parameters.Any());
        private bool IsInterface { get; set; }
        private int? ArrayRank { get; set; }
        internal bool IsDefinedInLowerLevelInterface { get; set; }
        internal bool IsLast { get; set; }
        private bool OmitLengthBecauseDefinitelyLast => (IsLast && ContainingObjectDescription.IsSealedOrStruct && ContainingObjectDescription.Version == -1);
        private string ChildSliceString => $"GetChildSlice(LazinatorMemoryStorage, {BackingFieldByteIndex}, {BackingFieldByteLength}{ChildSliceEndString})";
        private string ChildSliceEndString => $", {(OmitLengthBecauseDefinitelyLast ? "true" : "false")}, {(IsGuaranteedSmall ? "true" : "false")}, {(IsGuaranteedFixedLength ? $"{FixedLength}" : "null")}";
        internal string IncrementChildStartBySizeOfLength => OmitLengthBecauseDefinitelyLast || IsGuaranteedFixedLength ? "" : (IsGuaranteedSmall ? " + sizeof(byte)" : " + sizeof(int)");
        internal string DecrementTotalLengthBySizeOfLength => OmitLengthBecauseDefinitelyLast || IsGuaranteedFixedLength ? "" : (IsGuaranteedSmall ? " - sizeof(byte)" : " - sizeof(int)");

        /* Property type */
        internal LazinatorPropertyType PropertyType { get; set; }
        internal LazinatorSupportedCollectionType? SupportedCollectionType { get; set; }
        private LazinatorSupportedTupleType? SupportedTupleType { get; set; }
        internal bool IsPrimitive => PropertyType == LazinatorPropertyType.PrimitiveType || PropertyType == LazinatorPropertyType.PrimitiveTypeNullable;
        private bool IsClassOrInterface => PropertyType == LazinatorPropertyType.LazinatorClassOrInterface || PropertyType == LazinatorPropertyType.LazinatorNonnullableClassOrInterface || (PropertyType == LazinatorPropertyType.OpenGenericParameter && GenericConstrainedToClass);
        private bool IsLazinatorStruct => PropertyType == LazinatorPropertyType.LazinatorStruct || PropertyType == LazinatorPropertyType.LazinatorStructNullable || (PropertyType == LazinatorPropertyType.OpenGenericParameter && GenericConstrainedToStruct);
        internal bool IsDefinitelyStruct => IsLazinatorStruct ||
                                            (!Nullable && (
                                                (PropertyType == LazinatorPropertyType.NonLazinator && Symbol.IsValueType) ||
                                                (PropertyType == LazinatorPropertyType.SupportedTuple && (SupportedTupleType == LazinatorSupportedTupleType.ValueTuple || SupportedTupleType == LazinatorSupportedTupleType.KeyValuePair)) ||
                                                (PropertyType == LazinatorPropertyType.SupportedTuple && SupportedTupleType == LazinatorSupportedTupleType.RecordLikeType && Symbol.IsValueType) ||
                                                (IsMemoryOrSpan)
                                             ));
        internal bool IsSupportedTupleType => (PropertyType == LazinatorPropertyType.SupportedTuple && (SupportedTupleType == LazinatorSupportedTupleType.ValueTuple || SupportedTupleType == LazinatorSupportedTupleType.KeyValuePair)) ||
                                                (PropertyType == LazinatorPropertyType.SupportedTuple && SupportedTupleType == LazinatorSupportedTupleType.RecordLikeType && Symbol.IsValueType);
        internal bool IsPossiblyStruct => IsDefinitelyStruct ||
                                          PropertyType == LazinatorPropertyType.OpenGenericParameter;
        internal bool IsSimpleListOrArray => PropertyType == LazinatorPropertyType.SupportedCollection &&
                                             (SupportedCollectionType == LazinatorSupportedCollectionType.LinkedList ||
                                              SupportedCollectionType == LazinatorSupportedCollectionType.List ||
                                              (SupportedCollectionType == LazinatorSupportedCollectionType.Array && ArrayRank == 1));
        internal bool IsMemoryOrSpan => PropertyType == LazinatorPropertyType.SupportedCollection &&
                                        (SupportedCollectionType == LazinatorSupportedCollectionType.Memory ||
                                        SupportedCollectionType ==
                                        LazinatorSupportedCollectionType.ReadOnlyMemory ||
                                        SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlySpan);
        internal bool IsLazinator => PropertyType == LazinatorPropertyType.LazinatorClassOrInterface || PropertyType == LazinatorPropertyType.LazinatorNonnullableClassOrInterface || PropertyType == LazinatorPropertyType.LazinatorStruct || PropertyType == LazinatorPropertyType.LazinatorStructNullable || PropertyType == LazinatorPropertyType.OpenGenericParameter;
        internal bool IsSupportedCollectionOrTuple => PropertyType == LazinatorPropertyType.SupportedCollection || PropertyType == LazinatorPropertyType.SupportedTuple;

        internal bool IsSupportedCollectionReferenceType => PropertyType == LazinatorPropertyType.SupportedCollection && SupportedCollectionType != LazinatorSupportedCollectionType.Memory && SupportedCollectionType != LazinatorSupportedCollectionType.ReadOnlyMemory && SupportedCollectionType != LazinatorSupportedCollectionType.ReadOnlySpan;
        internal bool IsSupportedTupleReferenceType => PropertyType == LazinatorPropertyType.SupportedTuple && SupportedTupleType == LazinatorSupportedTupleType.Tuple;

        internal bool IsSupportedReferenceType => IsSupportedCollectionReferenceType || IsSupportedTupleReferenceType;
        internal bool IsSupportedValueType => IsSupportedCollectionOrTuple && !IsSupportedReferenceType;

        // DEBUG -- important: We must separate RecordLikeType into RecordLikeClass and RecordLikeStruct and update above accordingly (or maybe not).
        internal bool IsNonNullableReferenceType => !Nullable && (
            PropertyType == LazinatorPropertyType.LazinatorNonnullableClassOrInterface
            || IsSupportedReferenceType);
        internal bool IsNonNullableValueTypeWithNonNullableReferenceType => !Nullable && !IsNonNullableReferenceType && InnerProperties != null && InnerProperties.Any(x => x.IsNonNullableReferenceType); // note: this may actually not matter, since a value type can always be initialized with "default" without generating a compiler error, even if it contains a nonnullable type. In principle, we could still try to catch these in some circumstances and throw UnsetLazinator errors. But if the compiler doesn't give a warning, perhaps we ought not as well. Thus, we omit using this as a basis for NonNullableThatRequiresInitialization.
        internal bool NonNullableThatRequiresInitialization => IsNonNullableReferenceType; // || IsNonNullableValueTypeWithNonNullableReferenceType; (see above for explanation for why this is commented out.)
        internal bool NonNullableThatCanBeUninitialized => !Nullable && !NonNullableThatRequiresInitialization;
        public static bool UseNullableBackingFieldsForNonNullableReferenceTypes => false; // if TRUE, then we use a null backing field and add checks for PossibleUnsetException. If FALSE, then we don't do that, and instead we set the backing field in every constructor.
        internal bool AddQuestionMarkInBackingFieldForNonNullable => NullableModeEnabled && UseNullableBackingFieldsForNonNullableReferenceTypes && NonNullableThatRequiresInitialization;
        internal bool IsNonNullableWithNonNullableBackingField => NullableModeEnabled && !UseNullableBackingFieldsForNonNullableReferenceTypes && NonNullableThatRequiresInitialization;
        internal bool NullForgivenessInsteadOfPossibleUnsetException => NullableModeEnabled && !UseNullableBackingFieldsForNonNullableReferenceTypes && NonNullableThatRequiresInitialization;
        internal string BackingFieldStringOrContainedSpan(string propertyName) => (SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlySpan) ?
                    GetReadOnlySpanBackingFieldCast(propertyName) : (propertyName ?? BackingFieldString);
        internal string PossibleUnsetException => $"{IIF(AddQuestionMarkInBackingFieldForNonNullable, $" ?? throw new UnsetNonnullableLazinatorException()")}{IIF(NullForgivenessInsteadOfPossibleUnsetException, "!")}";
        internal string DefaultInitializationIfPossible(string defaultType) => $"{IIF(!AddQuestionMarkInBackingFieldForNonNullable, $" = default{IIF(defaultType != null, $"({defaultType})")}")}";
        string elseThrowString = $@"
                else 
                {{ 
                    throw new UnsetNonnullableLazinatorException(); 
                }}";
        internal string IfInitializationRequiredAddElseThrow => $"{IIF(AddQuestionMarkInBackingFieldForNonNullable, elseThrowString)}";
        internal string BackingFieldAccessWithPossibleException => $"{BackingFieldString}{PossibleUnsetException}";
        internal string BackingFieldStringOrContainedSpanWithPossibleException(string propertyName) => $"{BackingFieldStringOrContainedSpan(propertyName)}{PossibleUnsetException}";
        internal string BackingFieldWithPossibleValueDereference => $"{BackingFieldString}{IIF(PropertyType == LazinatorPropertyType.LazinatorStructNullable, $@".Value")}";
        internal string BackingFieldWithPossibleValueDereferenceWithPossibleException => $"{BackingFieldString}{IIF(PropertyType == LazinatorPropertyType.LazinatorStructNullable, $@".Value")}{PossibleUnsetException}";
        internal bool IsSupportedCollectionOrTupleOrNonLazinatorWithInterchangeType => IsSupportedCollectionOrTuple || (PropertyType == LazinatorPropertyType.NonLazinator && HasInterchangeType);
        internal bool IsNotPrimitiveOrOpenGeneric => PropertyType != LazinatorPropertyType.OpenGenericParameter && PropertyType != LazinatorPropertyType.PrimitiveType && PropertyType != LazinatorPropertyType.PrimitiveTypeNullable;
        internal bool IsNonLazinatorType => PropertyType == LazinatorPropertyType.NonLazinator || PropertyType == LazinatorPropertyType.SupportedCollection || PropertyType == LazinatorPropertyType.SupportedTuple;
        internal bool IsNonLazinatorTypeWithoutInterchange => PropertyType == LazinatorPropertyType.NonLazinator && !HasInterchangeType;
        internal string ConstructorInitialization => IIF(PropertyType != LazinatorPropertyType.LazinatorStruct && PropertyType != LazinatorPropertyType.LazinatorStructNullable && !NonSerializedIsStruct, "IncludeChildrenMode.IncludeAllChildren");

        /* Names */
        private bool UseFullyQualifiedNames => (Config?.UseFullyQualifiedNames ?? false) || HasFullyQualifyAttribute || Symbol.ContainingType != null;
        private string ShortTypeName => RegularizeTypeName(Symbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat), NullableModeEnabled && Nullable);
        private string ShortTypeNameWithoutNullableIndicator => WithoutNullableIndicator(ShortTypeName);
        internal string FullyQualifiedTypeName => Symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        private string FullyQualifiedNameWithoutNullableIndicator => WithoutNullableIndicator(FullyQualifiedTypeName);
        internal string AppropriatelyQualifiedTypeName => UseFullyQualifiedNames ? FullyQualifiedTypeName : ShortTypeName;

        public string DefaultExpression => PropertyType switch { LazinatorPropertyType.LazinatorStructNullable => "null", LazinatorPropertyType.LazinatorClassOrInterface => "null", LazinatorPropertyType.LazinatorNonnullableClassOrInterface => throw new NotImplementedException(), _ => $"default({AppropriatelyQualifiedTypeName})" };
        private string AppropriatelyQualifiedTypeNameWithoutNullableIndicator => UseFullyQualifiedNames ? FullyQualifiedNameWithoutNullableIndicator : ShortTypeNameWithoutNullableIndicator;

        private string AppropriatelyQualifiedTypeNameWithoutNullableIndicatorIfNonnullableReferenceType => PropertyType == LazinatorPropertyType.LazinatorNonnullableClassOrInterface ? AppropriatelyQualifiedTypeNameWithoutNullableIndicator : AppropriatelyQualifiedTypeName;

        internal string ShortTypeNameEncodable => Symbol.GetEncodableVersionOfIdentifier(false);
        private string ShortTypeNameEncodableWithoutNullable => (Symbol as INamedTypeSymbol).TypeArguments[0].GetEncodableVersionOfIdentifier(false);
        internal string FullyQualifiedTypeNameEncodable => Symbol.GetEncodableVersionOfIdentifier(true);
        private string FullyQualifiedTypeNameEncodableWithoutNullable => (Symbol as INamedTypeSymbol).TypeArguments[0].GetEncodableVersionOfIdentifier(true);
        internal string AppropriatelyQualifiedTypeNameEncodable => Symbol.GetEncodableVersionOfIdentifier(UseFullyQualifiedNames);
        private string AppropriatelyQualifiedTypeNameEncodableWithoutNullable => (Symbol as INamedTypeSymbol).TypeArguments[0].GetEncodableVersionOfIdentifier(UseFullyQualifiedNames);

        public string Namespace => Symbol.GetFullNamespace();
        private string WriteMethodName { get; set; }
        private string ReadMethodName { get; set; }
        internal string PropertyName { get; set; }
        internal string VersionOfPropertyNameForConstructorParameter => char.ToLower(PropertyName[0]) + PropertyName.Substring(1);
        internal string PropertyNameWithTypeNameForConstructorParameter => $"{AppropriatelyQualifiedTypeName} {VersionOfPropertyNameForConstructorParameter}";
        internal string AssignParameterToBackingField => $@"{BackingFieldString} = {VersionOfPropertyNameForConstructorParameter};
                            ";

        internal string BackingFieldString => $"_{PropertyName}";

        internal bool BackingAccessFieldIncluded => PlaceholderMemoryWriteMethod == null && !IsNonNullableWithNonNullableBackingField;
        internal string BackingAccessFieldName => $"_{PropertyName}_Accessed";
        internal string BackingFieldAccessedString => BackingAccessFieldIncluded ? BackingAccessFieldName : "true";
        internal string BackingFieldNotAccessedString => BackingAccessFieldIncluded ? $"!{BackingAccessFieldName}" : "false";
        internal string BackingDirtyFieldString => $"_{PropertyName}_Dirty";

        internal string BackingFieldByteIndex => $"_{PropertyName}_ByteIndex";
        internal string BackingFieldByteLength => $"_{PropertyName}_ByteLength";

        /* Enums */
        private string EnumEquivalentType { get; set; }
        private string EnumEquivalentCastToEquivalentType => EnumEquivalentType != null ? $"({EnumEquivalentType}) " : $"";
        private string EnumEquivalentCastToEnum => EnumEquivalentType != null ? $"({AppropriatelyQualifiedTypeName})" : $"";

        /* Inner properties */
        public List<PropertyDescription> InnerProperties { get; set; }
        private bool ContainsOpenGenericInnerProperty => InnerProperties != null && InnerProperties.Any(x => x.PropertyType == LazinatorPropertyType.OpenGenericParameter || x.ContainsOpenGenericInnerProperty);
        private bool ContainsLazinatorInnerProperty => InnerProperties != null && InnerProperties.Any(x => x.PropertyType == LazinatorPropertyType.LazinatorClassOrInterface || x.PropertyType == LazinatorPropertyType.LazinatorNonnullableClassOrInterface || x.ContainsLazinatorInnerProperty);
        internal string NullableStructValueAccessor => IIF(PropertyType == LazinatorPropertyType.LazinatorStructNullable, ".Value");

        /* Conversion */
        private string InterchangeTypeName { get; set; }
        private string InterchangeTypeNameWithoutNullabilityIndicator => WithoutNullableIndicator(InterchangeTypeName);
        private bool NonSerializedIsStruct { get; set; }
        private string DirectConverterTypeName { get; set; }
        internal string DirectConverterTypeNamePrefix => DirectConverterTypeName == "" || DirectConverterTypeName == null ? "" : DirectConverterTypeName + ".";
        private bool HasInterchangeType => InterchangeTypeName != null;

        /* Attributes */
        private IEnumerable<Attribute> UserAttributes => ContainingObjectDescription.Compilation.GetAttributes(PropertySymbol);
        internal int RelativeOrder { get; set; }
        private bool HasFullyQualifyAttribute => UserAttributes.OfType<CloneFullyQualifyAttribute>().Any();
        private IEnumerable<CloneInsertAttributeAttribute> InsertAttributes => UserAttributes.OfType<CloneInsertAttributeAttribute>();
        internal string PropertyAccessibility { get; set; }
        private string PropertyAccessibilityString => PropertyAccessibility == null ? "public " : PropertyAccessibility + " ";
        private CloneSetterAccessibilityAttribute SetterAccessibility { get; set; }
        private string SetterAccessibilityString => SetterAccessibility == null ? "" : SetterAccessibility.Choice + " ";
        public string PlaceholderMemoryWriteMethod { get; set; }
        private int? IntroducedWithVersion { get; set; }
        private int? EliminatedWithVersion { get; set; }
        internal bool DoNotEnumerate { get; set; }
        private bool IncludeRefProperty { get; set; }
        internal bool BrotliCompress { get; set; }
        public string SkipCondition { get; set; }
        public string InitializeWhenSkipped { get; set; }
        internal bool TrackDirtinessNonSerialized { get; set; }
        private ConditionsCodeGenerator ReadInclusionConditional { get; set; }
        private ConditionsCodeGenerator WriteInclusionConditional { get; set; }
        private string CodeBeforeSet { get; set; }
        private string CodeAfterSet { get; set; }
        private string CodeOnDeserialized { get; set; }
        private string CodeOnAccessed { get; set; }
        private bool IsGuaranteedFixedLength { get; set; }
        private int FixedLength { get; set; }
        private bool IsGuaranteedSmall { get; set; }
        private string LengthPrefixTypeString => IsGuaranteedFixedLength ? "out" : (IsGuaranteedSmall ? "Byte" : "Int");
        private string WriteDefaultLengthString =>
            !IsGuaranteedFixedLength || FixedLength == 1 ?
                $"writer.Write(({(IsGuaranteedSmall || IsGuaranteedFixedLength ? "byte" : "uint")})0);"
            :
                $@"for (int indexInFixedLength = 0; indexInFixedLength < {FixedLength}; indexInFixedLength++)
                    {{
                        writer.Write((byte)0);
                    }}";

        private bool IncludableWhenExcludingMostChildren { get; set; }
        private bool ExcludableWhenIncludingMostChildren { get; set; }
        private bool AllowLazinatorInNonLazinator { get; set; }

        /* Output customization */
        public LazinatorConfig Config => ContainingObjectDescription.Config;
        private string StepThroughPropertiesString => IIF(ContainingObjectDescription.StepThroughProperties, $@"
                        [DebuggerStepThrough]");
        private string ConfirmDirtinessConsistencyCheck => $@"
                            LazinatorUtilities.ConfirmDescendantDirtinessConsistency(this);";
        private string RepeatedCodeExecution => ""; // change to expose some action that should be repeated before and after each variable is set.
        private string IIF(bool x, string y) => x ? y : ""; // Include if function
        private string IIF(bool x, Func<string> y) => x ? y() : ""; // Same but with a function to produce the string

        #endregion

        #region Constructors

        public PropertyDescription()
        {

        }

        public PropertyDescription(IPropertySymbol propertySymbol, ObjectDescription container, NullableContext nullableContextSetting, string derivationKeyword, string propertyAccessibility, bool isLast)
        {
            PropertySymbol = propertySymbol;
            IsAbstract = PropertySymbol.Type.IsAbstract;
            ContainingObjectDescription = container;
            NullableContextSetting = nullableContextSetting;
            PropertyName = propertySymbol.Name;
            DerivationKeyword = derivationKeyword;
            PropertyAccessibility = propertyAccessibility;
            IsLast = isLast;

            ParseAccessibilityAttribute();
            if (propertySymbol.GetMethod == null)
                throw new LazinatorCodeGenException($"ILazinator interface property {PropertyName} in {ContainingObjectDescription?.NameIncludingGenerics} must include a get method.");
            if (propertySymbol.SetMethod == null && SetterAccessibility == null)
                throw new LazinatorCodeGenException($"ILazinator interface property {PropertyName} in {ContainingObjectDescription?.NameIncludingGenerics} must include a set method or a SetterAccessibilityAttribute.");
            if (propertySymbol.SetMethod != null && SetterAccessibility != null && SetterAccessibility.Choice != "public")
                throw new LazinatorCodeGenException($"ILazinator interface property {PropertyName} in {ContainingObjectDescription?.NameIncludingGenerics} should omit the set because because the SetterAccessibilityAttribute specifies non-public accessibility.");

            ParseVersionAttributes();

            ParseOtherPropertyAttributes();

            SetPropertyType(propertySymbol.Type as ITypeSymbol);

            SetReadAndWriteMethodNames();

            SetInclusionConditionals();
        }

        public PropertyDescription(ITypeSymbol typeSymbol, ObjectDescription containingObjectDescription, NullableContext nullableContextSetting, PropertyDescription containingPropertyDescription, string propertyName = null)
        {
            // This is only used for defining the type on the inside of the generics, plus underlying type for arrays.
            TypeSymbolIfNoProperty = typeSymbol;
            ContainingObjectDescription = containingObjectDescription;
            NullableContextSetting = nullableContextSetting;
            ContainingPropertyDescription = containingPropertyDescription;
            PropertyName = propertyName;
            IsAbstract = typeSymbol.IsAbstract;
            SetPropertyType(typeSymbol);
            SetReadAndWriteMethodNames();
        }

        public override string ToString()
        {
            return $"{AppropriatelyQualifiedTypeNameEncodable} {PropertyName}";
        }

        #endregion

        #region Attribute parsing

        private void ParseAccessibilityAttribute()
        {
            CloneSetterAccessibilityAttribute attribute = UserAttributes.OfType<CloneSetterAccessibilityAttribute>().FirstOrDefault();
            SetterAccessibility = attribute;
        }

        private void ParseVersionAttributes()
        {
            CloneEliminatedWithVersionAttribute eliminated = UserAttributes.OfType<CloneEliminatedWithVersionAttribute>().FirstOrDefault();
            EliminatedWithVersion = eliminated?.Version;
            CloneIntroducedWithVersionAttribute introduced = UserAttributes.OfType<CloneIntroducedWithVersionAttribute>().FirstOrDefault();
            IntroducedWithVersion = introduced?.Version;
        }

        private void ParseOtherPropertyAttributes()
        {
            ClonePlaceholderMemoryAttribute placeholderMemory = UserAttributes.OfType<ClonePlaceholderMemoryAttribute>().FirstOrDefault();
            if (placeholderMemory != null)
                PlaceholderMemoryWriteMethod = placeholderMemory.WriteMethod;
            CloneIncludableChildAttribute includable = UserAttributes.OfType<CloneIncludableChildAttribute>().FirstOrDefault();
            IncludableWhenExcludingMostChildren = includable != null;
            CloneExcludableChildAttribute excludable = UserAttributes.OfType<CloneExcludableChildAttribute>().FirstOrDefault();
            ExcludableWhenIncludingMostChildren = excludable != null;
            CloneDoNotEnumerateAttribute doNotEnumerate = UserAttributes.OfType<CloneDoNotEnumerateAttribute>().FirstOrDefault();
            DoNotEnumerate = doNotEnumerate != null;
            CloneIncludeRefPropertyAttribute includeRefProperty = UserAttributes.OfType<CloneIncludeRefPropertyAttribute>().FirstOrDefault();
            IncludeRefProperty = includeRefProperty != null;
            CloneBrotliCompressAttribute brotliCompress = UserAttributes.OfType<CloneBrotliCompressAttribute>().FirstOrDefault();
            BrotliCompress = brotliCompress != null;
            if (IncludeRefProperty && !IsPrimitive)
                throw new LazinatorCodeGenException($"The IncludeRefPropertyAttribute was applied to {PropertySymbol}, but can be used only with a non-primitive property.");
            CloneAllowLazinatorInNonLazinatorAttribute allowLazinatorInNonLazinator = UserAttributes.OfType<CloneAllowLazinatorInNonLazinatorAttribute>().FirstOrDefault();
            AllowLazinatorInNonLazinator = allowLazinatorInNonLazinator != null;

            CloneOnSetAttribute onSetAttribute = UserAttributes.OfType<CloneOnSetAttribute>().FirstOrDefault();
            CodeBeforeSet = onSetAttribute?.CodeBeforeSet ?? "";
            CodeAfterSet = onSetAttribute?.CodeAfterSet ?? "";

            CloneOnDeserializedAttribute onDeserializedAttribute = UserAttributes.OfType<CloneOnDeserializedAttribute>().FirstOrDefault();
            CodeOnDeserialized = onDeserializedAttribute?.CodeToInsert ?? "";

            CloneOnPropertyAccessedAttribute onAccessedAttribute = UserAttributes.OfType<CloneOnPropertyAccessedAttribute>().FirstOrDefault();
            CodeOnAccessed = onAccessedAttribute?.CodeToInsert ?? "";

            CloneRelativeOrderAttribute relativeOrder = UserAttributes.OfType<CloneRelativeOrderAttribute>().FirstOrDefault();
            RelativeOrder = relativeOrder?.RelativeOrder ?? 0;
            CloneSkipIfAttribute skipIf = UserAttributes.OfType<CloneSkipIfAttribute>().FirstOrDefault();
            if (skipIf != null)
            {
                SkipCondition = skipIf.SkipCondition;
                InitializeWhenSkipped = skipIf.InitializeWhenSkipped;
            }
        }

        private string GetAttributesToInsert()
        {
            if (InsertAttributes.Any())
            {
                string textToInsert = "";
                foreach (var a in InsertAttributes)
                    textToInsert += $@"[{a.AttributeText}]
                                        ";
                return textToInsert;
            }
            else return "";
        }

        private void SetInclusionConditionals()
        {
            ReadInclusionConditional = InclusionConditionalHelper(true);
            WriteInclusionConditional = InclusionConditionalHelper(false);
        }

        private ConditionsCodeGenerator InclusionConditionalHelper(bool readVersion)
        {
            string versionNumberVariable = readVersion ? "serializedVersionNumber" : "LazinatorObjectVersion";
            List<string> conditions = new List<string>();
            if (BackingAccessFieldIncluded)
            {
                if (PropertyType == LazinatorPropertyType.LazinatorStruct || PropertyType == LazinatorPropertyType.LazinatorStructNullable || PropertyType == LazinatorPropertyType.LazinatorClassOrInterface || PropertyType == LazinatorPropertyType.LazinatorNonnullableClassOrInterface || PropertyType == LazinatorPropertyType.OpenGenericParameter)
                {
                    conditions.Add("includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren");
                    if (!IncludableWhenExcludingMostChildren)
                        conditions.Add("includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren");
                    if (ExcludableWhenIncludingMostChildren)
                        conditions.Add("includeChildrenMode != IncludeChildrenMode.ExcludeOnlyExcludableChildren");
                }
                if (IntroducedWithVersion != null)
                    conditions.Add($"{versionNumberVariable} >= {IntroducedWithVersion}");
                if (EliminatedWithVersion != null)
                    conditions.Add($"{versionNumberVariable} < {EliminatedWithVersion}");
            }
            if (!conditions.Any())
                return new ConditionsCodeGenerator(new List<ConditionCodeGenerator>(), true);
            return new ConditionsCodeGenerator(conditions.Select(x => new ConditionCodeGenerator(x)).ToList(), true);
        }

        #endregion

        #region Property type

        private static readonly string[] SupportedAsPrimitives = new string[]
            {"bool", "byte", "sbyte", "char", "short", "ushort", "int", "uint", "long", "ulong", "string", "DateTime", "TimeSpan", "Guid", "float", "double", "decimal"};

        private void SetPropertyType(ITypeSymbol typeSymbol)
        {
            INamedTypeSymbol namedTypeSymbol = typeSymbol as INamedTypeSymbol;

            if (namedTypeSymbol == null && typeSymbol.TypeKind == TypeKind.TypeParameter)
            {
                Nullable = true;
                PropertyType = LazinatorPropertyType.OpenGenericParameter;
                DerivationKeyword = "virtual ";
                return;
            }

            Nullable = IsNullableGeneric(namedTypeSymbol);
            if (Nullable)
            {
                // handle a nullable type (which might be a nullable primitive type or a nullable struct / valuetuple
                SetEnumOrPrimitivePropertyType(namedTypeSymbol);
                if (PropertyType == LazinatorPropertyType.LazinatorStruct)
                    PropertyType = LazinatorPropertyType.LazinatorStructNullable;
                return;
            }
            else
            {
                // look for primitive type
                if (typeSymbol.Name == "string?")
                    Nullable = true;
                else if (typeSymbol.Name == "string")
                    Nullable = !NullableModeEnabled;
                if (namedTypeSymbol?.EnumUnderlyingType != null)
                    SetEnumEquivalentType(namedTypeSymbol);
                if (SupportedAsPrimitives.Contains(EnumEquivalentType ?? ShortTypeNameWithoutNullableIndicator))
                {
                    PropertyType = LazinatorPropertyType.PrimitiveType;
                    return;
                }
            }

            bool isILazinator = typeSymbol.Interfaces.Any(x => x.Name == "ILazinator" || x.Name == "ILazinator?");
            bool isRecursiveDefinition = false;
            if (namedTypeSymbol != null)
            {
                if (SymbolEqualityComparer.Default.Equals(namedTypeSymbol, ContainingObjectDescription?.InterfaceTypeSymbol))
                {
                    if (!isILazinator)
                        throw new LazinatorCodeGenException(
                        "If an interface defines itself recursively, then it must explicitly declare that it implements ILazinator.");
                    isRecursiveDefinition = true;
                }

                if (SymbolEqualityComparer.Default.Equals(namedTypeSymbol, ContainingObjectDescription?.ILazinatorTypeSymbol))
                    isRecursiveDefinition = true;
            }
            if (!isILazinator && !isRecursiveDefinition && namedTypeSymbol != null)
            {
                if (
                    (namedTypeSymbol.AllInterfaces.Any(x => x.HasAttributeOfType<CloneLazinatorAttribute>()))
                    ||
                    (namedTypeSymbol.TypeKind == TypeKind.Interface && namedTypeSymbol.HasAttributeOfType<CloneLazinatorAttribute>())
                    )
                {
                    if (namedTypeSymbol.TypeKind == TypeKind.Interface && !namedTypeSymbol.AllInterfaces.Any(x => x.Name == "ILazinator" || x.Name == "ILazinator?"))
                        throw new LazinatorCodeGenException($"To use {namedTypeSymbol} as a type in {ContainingObjectDescription.FullyQualifiedObjectName}, you must make {namedTypeSymbol} inherit directly from ILazinator.");
                    isILazinator = true; // code behind isn't implemented yet but it will be
                }
            }

            bool isSelfSerializable = isRecursiveDefinition || isILazinator;
            if (isSelfSerializable)
                SetLazinatorPropertyType(namedTypeSymbol);
            else
            {
                // look for supported collections and tuples
                if (typeSymbol.TypeKind == TypeKind.Array)
                { // array is not a generic type, so we handle it specially
                    SetArrayPropertyType(typeSymbol as IArrayTypeSymbol);
                    return;
                }

                if (SetSupportedTupleAndCollectionsPropertyType(namedTypeSymbol))
                    return;

                SetNonserializedTypeNameAndPropertyType(namedTypeSymbol);
            }
        }

        private void SetNonserializedTypeNameAndPropertyType(INamedTypeSymbol t)
        {
            Nullable = t.TypeKind == TypeKind.Class || IsNullableGeneric(t);
            PropertyType = LazinatorPropertyType.NonLazinator;
            NonSerializedIsStruct = t.IsValueType;
            InterchangeTypeName = Config?.GetInterchangeConverterTypeName(t);
            DirectConverterTypeName = Config?.GetDirectConverterTypeName(t);
            string fullyQualifiedTypeName = t.GetFullyQualifiedNameWithoutGlobal();
            if (InterchangeTypeName != null && DirectConverterTypeName != null)
                throw new LazinatorCodeGenException($"{fullyQualifiedTypeName} has both an interchange converter and a direct converter type listed. Only one should be used.");
            if (InterchangeTypeName == null && DirectConverterTypeName == null)
            {
                if (fullyQualifiedTypeName == "Lazinator.Core.ILazinator" || fullyQualifiedTypeName == "Lazinator.Core.ILazinator?")
                    throw new LazinatorCodeGenException($"You cannot include ILazinator as a type to be serialized or as a type argument in a Lazinator interface. (The reason for this is that some Lazinator types do not serialize their ID.) To define a property that can deserialize a large number of Lazinator types, create a nonexclusive interface (possibly implementing no properties) and then define your Lazinator types as implementing that interface. This nonexclusive interface can then be used as the type for a Lazinator property.");
                else
                    throw new LazinatorCodeGenException($"{fullyQualifiedTypeName} is a non-Lazinator type or interface. To use it as a type for a Lazinator property, you must either make it a Lazinator type or use a Lazinator.config file to specify either an interchange converter (i.e., a Lazinator object accept the non-Lazinator type as a parameter in its constructor) or a direct converter for it. Alternatively, if there is a constructor whose parameters match public properties (not fields) of the type, it can be handled automatically. If this is an interface, then define the NonexclusiveLazinator attribute and make sure that the interface inherits from ILazinator.");
            }
        }

        public string GetNullCheckPlusPrecedingConditionIfThen(ConditionCodeGenerator precedingNullCheckCondition, string propertyName, string consequent, string elseConsequent)
        {
            return new ConditionalCodeGenerator(new ConditionsCodeGenerator(new List<ConditionCodeGenerator>() { precedingNullCheckCondition, new ConditionCodeGenerator(GetNullCheck(propertyName)) }, true), consequent, elseConsequent).ToString();
        }

        public string GetNullCheckIfThen(string propertyName, string consequent, string elseConsequent)
        {
            return new ConditionalCodeGenerator(GetNullCheck(propertyName), consequent, elseConsequent).ToString();
        }

        public string GetNullCheckIfThenButOnlyIfNullable(bool nullable, string propertyName, string consequent, string elseConsequent) => nullable ? GetNullCheckIfThen(propertyName, consequent, elseConsequent) + "\r\n" : "";

        public string GetNullCheck(string propertyName)
        {
            string nullCheck;
            if (IsMemoryOrSpan)
            {
                if (Nullable)
                    nullCheck = $"{propertyName} == null";
                else
                    nullCheck = $"{propertyName}.Length == 0"; // use as equivalent of null
            }
            else if (IsDefinitelyStruct && PropertyType != LazinatorPropertyType.LazinatorStructNullable)
                nullCheck = $"false";
            else // could be open generic or class -- either way, what we're interested in is whether this is dereferenceable
                nullCheck = $"{propertyName} == null";
            return nullCheck;
        }

        public ConditionCodeGenerator GetNonNullCheck(bool includeAccessedCheck, string propertyName = null)
        {
            if (propertyName == null)
                propertyName = PropertyName;
            ConditionCodeGenerator nonNullCheck;
            if (includeAccessedCheck)
            {
                if (IsMemoryOrSpan)
                    nonNullCheck = $"{propertyName}.Length != 0"; // use as equivalent of null
                else if (IsDefinitelyStruct && PropertyType != LazinatorPropertyType.LazinatorStructNullable)
                    nonNullCheck = "true";
                else
                    nonNullCheck = ConditionsCodeGenerator.AndCombine($"{BackingFieldAccessedString}", IsNonNullableWithNonNullableBackingField ? "true" : $"_{propertyName} != null");
            }
            else
            {
                if (IsMemoryOrSpan)
                    nonNullCheck = $"{propertyName}.Length != 0"; // use as equivalent of nullelse
                else if (IsDefinitelyStruct && PropertyType != LazinatorPropertyType.LazinatorStructNullable)
                    nonNullCheck = "true";
                else
                    nonNullCheck = $"{propertyName} != null";
            }
            return nonNullCheck;
        }

        private bool SetSupportedTupleAndCollectionsPropertyType(INamedTypeSymbol t)
        {
            if (t.TupleUnderlyingType != null)
                t = t.TupleUnderlyingType;
            if (t.IsGenericType)
            {
                string name = t.Name;
                CheckSupportedTuples(name);
                if (PropertyType == LazinatorPropertyType.SupportedTuple)
                {
                    SetSupportedTupleNullabilityAndInnerProperties(t, name);
                    return true;
                }

                // now look for supported collections
                CheckSupportedCollections(name);
                if (PropertyType == LazinatorPropertyType.SupportedCollection)
                {
                    SetSupportedCollectionNullability(t, name);
                    SetSupportedCollectionInnerProperties(t);
                    return true;
                }
            }

            return (HandleRecordLikeType(t));
        }

        private void SetEnumOrPrimitivePropertyType(INamedTypeSymbol namedTypeSymbol)
        {
            SetPropertyType(namedTypeSymbol.TypeArguments[0] as INamedTypeSymbol);
            Nullable = true;
            if (EnumEquivalentType != null)
                EnumEquivalentType += "?";
            if (PropertyType == LazinatorPropertyType.PrimitiveType)
                PropertyType = LazinatorPropertyType.PrimitiveTypeNullable;
        }

        private void SetLazinatorPropertyType(INamedTypeSymbol t)
        {
            try
            {
                if (t.TypeKind == TypeKind.Class || t.TypeKind == TypeKind.Interface || t.TypeKind == TypeKind.Array)
                {
                    if (NullableModeEnabled && !SymbolEndsWithQuestionMark)
                    {
                        Nullable = false;
                        PropertyType = LazinatorPropertyType.LazinatorNonnullableClassOrInterface;
                    }
                    else
                    {
                        Nullable = true;
                        PropertyType = LazinatorPropertyType.LazinatorClassOrInterface;
                    }
                }
                else
                {
                    Nullable = IsNullableGeneric(t);
                    PropertyType = Nullable ? LazinatorPropertyType.LazinatorStructNullable : LazinatorPropertyType.LazinatorStruct;
                }

                IsInterface = t.TypeKind == TypeKind.Interface;
                if (!IsInterface)
                {
                    var exclusiveInterface = LazinatorCompilation.NameTypedSymbolFromString[ContainingObjectDescription.Compilation.TypeToExclusiveInterface[LazinatorCompilation.TypeSymbolToString(t.OriginalDefinition)]];
                    CloneLazinatorAttribute attribute = ContainingObjectDescription.Compilation.GetFirstAttributeOfType<CloneLazinatorAttribute>(exclusiveInterface); // we already know that the interface exists, and there should be only one
                    if (attribute == null)
                        throw new LazinatorCodeGenException(
                            "Lazinator attribute is required for each interface implementing ILazinator, including inherited attributes.");
                    UniqueIDForLazinatorType = attribute.UniqueID;
                    CloneSmallLazinatorAttribute smallAttribute =
                        ContainingObjectDescription.Compilation.GetFirstAttributeOfType<CloneSmallLazinatorAttribute>(exclusiveInterface);
                    if (smallAttribute != null)
                        IsGuaranteedSmall = true;

                    CloneFixedLengthLazinatorAttribute fixedLengthAttribute =
                        ContainingObjectDescription.Compilation.GetFirstAttributeOfType<CloneFixedLengthLazinatorAttribute>(exclusiveInterface);
                    if (fixedLengthAttribute != null)
                    {
                        IsGuaranteedFixedLength = true;
                        FixedLength = fixedLengthAttribute.FixedLength;
                    }
                }

                if (t.IsGenericType)
                {
                    // This is a generic Lazinator type, e.g., MySelfSerializingDictionary<int, long> or MyType<T,U> where T,U : ILazinator
                    SetInnerProperties(t);
                    return;
                }
            }
            catch (Exception ex)
            {
                throw new LazinatorCodeGenException($"SetSelfSerializablePropertyType failed; message {ex.Message}");
            }
        }

        private void SetArrayPropertyType(IArrayTypeSymbol t)
        {
            ArrayRank = t.Rank;
            SupportedCollectionType = LazinatorSupportedCollectionType.Array;
            Nullable = NullableModeEnabled ? SymbolEndsWithQuestionMark : true;
            PropertyType = LazinatorPropertyType.SupportedCollection;
            InnerProperties = new List<PropertyDescription>()
            {
                new PropertyDescription(t.ElementType, ContainingObjectDescription, NullableContextSetting, this)
            };
        }

        private string ReverseBracketOrder(string arrayCode)
        {
            // with jagged arrays, the outside [] is listed FIRST, instead of last. While List<int>[] means that the [] is the outside, with int[][,], the [,] is the inside. So we need to reverse the order. But since we will have already reversed the inner type, we actually just need to move the last bracket first.
            if (arrayCode.Contains("]["))
            {
                List<int> locationOfOpenBrackets = new List<int>();
                List<int> locationOfCloseBrackets = new List<int>();
                int startLoc = -1;
                int index = 0;
                bool keepGoing = true;
                do
                {
                    index = arrayCode.IndexOf("][", startLoc + 1);
                    if (index > 0)
                    {
                        locationOfCloseBrackets.Add(index);
                        startLoc = index;
                        keepGoing = true;
                    }
                    else
                    {
                        index = arrayCode.IndexOf("]", startLoc + 1);
                        locationOfCloseBrackets.Add(index);
                        keepGoing = false;
                    }

                } while (keepGoing);

                foreach (var closeBracketLocation in locationOfCloseBrackets)
                {
                    int indexOpen = arrayCode.Substring(0, closeBracketLocation).LastIndexOf('[');
                    locationOfOpenBrackets.Add(indexOpen);
                }

                StringBuilder bracketsReversed = new StringBuilder();
                locationOfOpenBrackets.Insert(0, locationOfOpenBrackets.Last());
                locationOfCloseBrackets.Insert(0, locationOfCloseBrackets.Last());
                for (int i = 0; i < locationOfOpenBrackets.Count() - 1; i++)
                {
                    string substring = arrayCode.Substring(locationOfOpenBrackets[i], locationOfCloseBrackets[i] - locationOfOpenBrackets[i] + 1);
                    bracketsReversed.Append(substring);
                }

                string overall = arrayCode.Substring(0, locationOfOpenBrackets[1]) + bracketsReversed.ToString();
                if (arrayCode.Length > locationOfCloseBrackets.Last() + 1)
                    overall += arrayCode.Substring(locationOfCloseBrackets.Last() + 1);
                return overall;
            }
            else
                return arrayCode;
        }

        private bool HandleRecordLikeType(INamedTypeSymbol t)
        {
            // We look for a record-like type only after we have determined that the type does not implement ILazinator and we don't have the other supported tuple types (e.g., ValueTuples, KeyValuePair). We need to make sure that for each parameter in the constructor with the most parameters, there is a unique property with the same name (case insensitive as to first letter). If so, we assume that this property corresponds to the parameter, though there is no inherent guarantee that this is true. 
            var recordLikeTypes = ContainingObjectDescription.Compilation.RecordLikeTypes;
            if (!recordLikeTypes.ContainsKey(LazinatorCompilation.TypeSymbolToString(t)) || (Config?.IgnoreRecordLikeTypes.Any(x => x.ToUpper() == (UseFullyQualifiedNames ? t.GetFullyQualifiedNameWithoutGlobal().ToUpper() : t.GetMinimallyQualifiedName())) ?? false))
            {
                return false;
            }

            TypeSymbolIfNoProperty = t;
            PropertyType = LazinatorPropertyType.SupportedTuple;
            SupportedTupleType = LazinatorSupportedTupleType.RecordLikeType;
            Nullable = !t.IsValueType;

            InnerProperties = recordLikeTypes[LazinatorCompilation.TypeSymbolToString(t)]
                .Select(x => GetNewPropertyDescriptionAvoidingRecursion(x.property.Type, ContainingObjectDescription, this, x.property.Name)).ToList();
            return true;
        }

        public string ContainingTypesPrefix(ITypeSymbol symbol)
        {
            // a containing type is the supertype of a subtype, i.e. with nested classes
            if (symbol.ContainingType == null)
                return "";
            return String.Join(".", symbol.GetContainingTypes().Select(x => x.Name).ToArray()) + ".";
        }

        public IEnumerable<PropertyDescription> ContainingPropertyHierarchy()
        {
            if (ContainingPropertyDescription != null)
            {
                yield return ContainingPropertyDescription;
                foreach (var cpd in ContainingPropertyDescription.ContainingPropertyHierarchy())
                    yield return cpd;
            }
        }

        public PropertyDescription GetNewPropertyDescriptionAvoidingRecursion(ITypeSymbol typeSymbol, ObjectDescription containingObjectDescription, PropertyDescription containingPropertyDescription, string propertyName)
        {
            // see if the property has already been defined (in case this is a recursive hierarchy)
            foreach (PropertyDescription pd in ContainingPropertyHierarchy())
                if (SymbolEqualityComparer.Default.Equals(pd.TypeSymbolIfNoProperty, typeSymbol))
                    throw new LazinatorCodeGenException($"The type {typeSymbol} is recursively defined. Recursive record-like types are not supported.");
            return new PropertyDescription(typeSymbol, containingObjectDescription, NullableContextSetting, containingPropertyDescription, propertyName);
        }

        private void CheckSupportedTuples(string nameWithoutArity)
        {
            if (nameWithoutArity == "ValueTuple")
            {
                PropertyType = LazinatorPropertyType.SupportedTuple;
                SupportedTupleType = LazinatorSupportedTupleType.ValueTuple;
                Nullable = false;
            }
            else if (nameWithoutArity == "Tuple")
            {
                PropertyType = LazinatorPropertyType.SupportedTuple;
                SupportedTupleType = LazinatorSupportedTupleType.Tuple;
                Nullable = true;
            }
            else if (nameWithoutArity == "KeyValuePair")
            {
                PropertyType = LazinatorPropertyType.SupportedTuple;
                SupportedTupleType = LazinatorSupportedTupleType.KeyValuePair;
                Nullable = false;
            }
        }

        private void SetSupportedTupleNullabilityAndInnerProperties(INamedTypeSymbol t, string nameWithoutArity)
        {
            SetInnerProperties(t, nameWithoutArity);
            Nullable = SupportedTupleType switch
            {
                LazinatorSupportedTupleType.KeyValuePair => SymbolEndsWithQuestionMark,
                LazinatorSupportedTupleType.ValueTuple => SymbolEndsWithQuestionMark,
                LazinatorSupportedTupleType.RecordLikeType => SymbolEndsWithQuestionMark, // DEBUG -- must differentiate class and struct? try using record like class and struct within non-nullable context
                LazinatorSupportedTupleType.Tuple => NullableModeEnabled ? SymbolEndsWithQuestionMark : true,
                _ => throw new NotImplementedException(),
            };
        }

        private void SetInnerProperties(INamedTypeSymbol t, string name = null)
        {
            var typeArguments = t.TypeArguments;
            bool isGenericType = t.IsGenericType;
            SetInnerProperties(typeArguments);
        }

        private void SetInnerProperties(ImmutableArray<ITypeSymbol> typeArguments)
        {
            InnerProperties = typeArguments
                            .Select(x => new PropertyDescription(x, ContainingObjectDescription, NullableContextSetting, this)).ToList();
        }

        public IEnumerable<PropertyDescription> PropertyAndInnerProperties()
        {
            yield return this;
            if (InnerProperties != null)
                foreach (var inner in InnerProperties)
                    foreach (var innerEnumerated in inner.PropertyAndInnerProperties())
                        yield return innerEnumerated;
        }

        private void CheckSupportedCollections(string nameWithoutArity)
        {
            switch (nameWithoutArity)
            {
                case "List":
                    SupportedCollectionType = LazinatorSupportedCollectionType.List;
                    PropertyType = LazinatorPropertyType.SupportedCollection;
                    break;
                case "LinkedList":
                    SupportedCollectionType = LazinatorSupportedCollectionType.LinkedList;
                    PropertyType = LazinatorPropertyType.SupportedCollection;
                    break;
                case "SortedSet":
                    SupportedCollectionType = LazinatorSupportedCollectionType.SortedSet;
                    PropertyType = LazinatorPropertyType.SupportedCollection;
                    break;
                case "Queue":
                    SupportedCollectionType = LazinatorSupportedCollectionType.Queue;
                    PropertyType = LazinatorPropertyType.SupportedCollection;
                    break;
                case "Stack":
                    SupportedCollectionType = LazinatorSupportedCollectionType.Stack;
                    PropertyType = LazinatorPropertyType.SupportedCollection;
                    break;
                case "Memory":
                    SupportedCollectionType = LazinatorSupportedCollectionType.Memory;
                    PropertyType = LazinatorPropertyType.SupportedCollection;
                    break;
                case "ReadOnlySpan":
                    SupportedCollectionType = LazinatorSupportedCollectionType.ReadOnlySpan;
                    PropertyType = LazinatorPropertyType.SupportedCollection;
                    break;
                case "ReadOnlyMemory":
                    SupportedCollectionType = LazinatorSupportedCollectionType.ReadOnlyMemory;
                    PropertyType = LazinatorPropertyType.SupportedCollection;
                    break;
                case "HashSet":
                    SupportedCollectionType = LazinatorSupportedCollectionType.HashSet;
                    PropertyType = LazinatorPropertyType.SupportedCollection;
                    break;
                case "Dictionary":
                    SupportedCollectionType = LazinatorSupportedCollectionType.Dictionary;
                    PropertyType = LazinatorPropertyType.SupportedCollection;
                    break;
                case "SortedDictionary":
                    SupportedCollectionType = LazinatorSupportedCollectionType.SortedDictionary;
                    PropertyType = LazinatorPropertyType.SupportedCollection;
                    break;
                case "SortedList":
                    SupportedCollectionType = LazinatorSupportedCollectionType.SortedList;
                    PropertyType = LazinatorPropertyType.SupportedCollection;
                    break;
                default:
                    break;
            }
        }

        static INamedTypeSymbol keyValuePairType = null;
        private void SetSupportedCollectionNullability(INamedTypeSymbol t, string nameWithoutArity)
        {
            Nullable = SupportedCollectionType switch
            {
                LazinatorSupportedCollectionType.Array => ReferenceTypeIsNullable,
                LazinatorSupportedCollectionType.List => ReferenceTypeIsNullable,
                LazinatorSupportedCollectionType.HashSet => ReferenceTypeIsNullable,
                LazinatorSupportedCollectionType.Dictionary => ReferenceTypeIsNullable,
                LazinatorSupportedCollectionType.Queue => ReferenceTypeIsNullable,
                LazinatorSupportedCollectionType.Stack => ReferenceTypeIsNullable,
                LazinatorSupportedCollectionType.SortedDictionary => ReferenceTypeIsNullable,
                LazinatorSupportedCollectionType.SortedList => ReferenceTypeIsNullable,
                LazinatorSupportedCollectionType.LinkedList => ReferenceTypeIsNullable,
                LazinatorSupportedCollectionType.SortedSet => ReferenceTypeIsNullable,
                LazinatorSupportedCollectionType.Memory => SymbolEndsWithQuestionMark,
                LazinatorSupportedCollectionType.ReadOnlySpan => SymbolEndsWithQuestionMark,
                LazinatorSupportedCollectionType.ReadOnlyMemory => SymbolEndsWithQuestionMark,
                _ => throw new NotImplementedException(),
            };
        }

        private void SetSupportedCollectionInnerProperties(INamedTypeSymbol t)
        {
            InnerProperties = t.TypeArguments
                            .Select(x => new PropertyDescription(x, ContainingObjectDescription, NullableContextSetting, this)).ToList();

            if (SupportedCollectionType == LazinatorSupportedCollectionType.Memory || SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlyMemory || SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlySpan)
            {
                if (InnerProperties[0].Nullable)
                    throw new LazinatorCodeGenException("Cannot use Lazinator to serialize Memory/Span with nullable generic arguments."); // this is because we can't cast easily in this context
            }

            if (SupportedCollectionType == LazinatorSupportedCollectionType.Dictionary || SupportedCollectionType == LazinatorSupportedCollectionType.SortedDictionary || SupportedCollectionType == LazinatorSupportedCollectionType.SortedList)
            {
                // We process a Dictionary by treating it as a collection with KeyValuePairs. Thus, we must change to a single inner property of type KeyValuePair, which in turn has two inner properties equal to the properties of the type in our actual dictionary.
                if (keyValuePairType == null)
                    keyValuePairType = ContainingObjectDescription.Compilation.Compilation.GetTypeByMetadataName(typeof(KeyValuePair<,>).FullName);
                INamedTypeSymbol constructed = keyValuePairType.Construct(t.TypeArguments.ToArray());
                var replacementInnerProperty = new PropertyDescription(constructed, ContainingObjectDescription, NullableContextSetting, this); // new PropertyDescription("System.Collections.Generic", "KeyValuePair", t.TypeArguments, ContainingObjectDescription);
                InnerProperties = new List<PropertyDescription>() { replacementInnerProperty };
            }
        }

        private static bool IsNullableGeneric(INamedTypeSymbol t)
        {
            if (t == null)
                return false;
            return (t.IsGenericType && t.Name == "Nullable");
        }

        #endregion

        #region Defining


        public void AppendPropertyDefinitionString(CodeStringBuilder sb)
        {
            if (ContainingObjectDescription.IsAbstract)
                AppendAbstractPropertyDefinitionString(sb);
            else if (PropertyType == LazinatorPropertyType.PrimitiveType || PropertyType == LazinatorPropertyType.PrimitiveTypeNullable)
                AppendPrimitivePropertyDefinitionString(sb);
            else
                AppendNonPrimitivePropertyDefinitionString(sb);
        }

        private void AppendAbstractPropertyDefinitionString(CodeStringBuilder sb)
        {
            string abstractDerivationKeyword = GetModifiedDerivationKeyword();
            string propertyString = $@"
                    {IIF(BackingAccessFieldIncluded, $@"{ContainingObjectDescription.HideBackingField}{ContainingObjectDescription.ProtectedIfApplicable}bool {BackingFieldAccessedString}{IIF(ContainingObjectDescription.ObjectType != LazinatorObjectType.Struct && !ContainingObjectDescription.GeneratingRefStruct, " = false")};")}
                {IIF(IncludeRefProperty, $@"{ContainingObjectDescription.HideMainProperty}{PropertyAccessibilityString}{abstractDerivationKeyword}ref {AppropriatelyQualifiedTypeName} {PropertyName}_Ref
                {{
                    get;
                }}
                ")}{GetAttributesToInsert()}{ContainingObjectDescription.HideMainProperty}{PropertyAccessibilityString}{abstractDerivationKeyword}{AppropriatelyQualifiedTypeName} {PropertyName}
                {{
                    get;
                    set;
                }}
";
            sb.Append(propertyString);
        }

        private string GetModifiedDerivationKeyword()
        {
            if (ContainingObjectDescription.ObjectType == LazinatorObjectType.Struct || ContainingObjectDescription.IsSealed || ContainingObjectDescription.GeneratingRefStruct)
                return "";
            string modifiedDerivationKeyword = DerivationKeyword;
            if (modifiedDerivationKeyword == null)
            {
                if (ContainsOpenGenericInnerProperty)
                    modifiedDerivationKeyword = "virtual "; // whether the container is or isn't abstract, this could be closed later on, so "virtual" is appropriate.
                else if (ContainingObjectDescription.IsAbstract)
                    modifiedDerivationKeyword = "abstract ";
            }

            return modifiedDerivationKeyword;
        }

        private void AppendPrimitivePropertyDefinitionString(CodeStringBuilder sb)
        {
            string propertyString = $@"
                {ContainingObjectDescription.HideBackingField}{ContainingObjectDescription.ProtectedIfApplicable}{AppropriatelyQualifiedTypeName} {BackingFieldString}{IIF(AppropriatelyQualifiedTypeName == "string" && NullableModeEnabled, " = \"\"")};
        {IIF(IncludeRefProperty, $@"{ContainingObjectDescription.HideMainProperty}{PropertyAccessibilityString}{GetModifiedDerivationKeyword()}ref {AppropriatelyQualifiedTypeName} {PropertyName}_Ref
                {{
                    get
                    {{
                        IsDirty = true;
                        return ref {BackingFieldString};
                    }}
                }}
                ")}{GetAttributesToInsert()}{ContainingObjectDescription.HideMainProperty}{PropertyAccessibilityString}{GetModifiedDerivationKeyword()}{AppropriatelyQualifiedTypeName} {PropertyName}
        {{{StepThroughPropertiesString}
            get
            {{
                return {BackingFieldString};
            }}{StepThroughPropertiesString}
            {SetterAccessibilityString}set
            {{
                IsDirty = true;
                {CodeBeforeSet}{BackingFieldString} = value;{CodeAfterSet}{RepeatedCodeExecution}
            }}
        }}
";
            sb.Append(propertyString);
        }

        private void AppendNonPrimitivePropertyDefinitionString(CodeStringBuilder sb)
        {
            if (PlaceholderMemoryWriteMethod != null)
            {
                if (SupportedCollectionType != LazinatorSupportedCollectionType.ReadOnlyMemory)
                    throw new LazinatorCodeGenException("PlaceholderMemory attribute should be used only with type ReadOnlyMemory<byte>.");
                AppendPlaceholderMemoryProperty(sb);
                return;
            }

            if (SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlySpan)
            {
                AppendReadOnlySpanProperty(sb);
                return;
            }

            string assignment = GetAssignmentString();


            // Prepare code needed for _Lazinate private method. This is used to instantiate a Lazinator when the get accessor is called and the 
            // Lazinator has not previously been accessed. We need both code to create a default value (if there is no underlying memory) and 
            // to recreate the value by deserializing, if there is underlying memory.
            // Here's the create default code:
            string createDefault = GetCreateDefaultString();
            // And here's where we set the recreation code for when underlying memory exists.
            string recreation = GetRecreationString(assignment);

            // Now we need to worry about the set accessor. 
            string propertyTypeDependentSet = GetPropertyTypeDependentSetString();

            string lazinateContents = GetLazinateContents(createDefault, recreation); 

            sb.Append($@"
                {ContainingObjectDescription.HideBackingField}{ContainingObjectDescription.ProtectedIfApplicable}{AppropriatelyQualifiedTypeName}{IIF(AddQuestionMarkInBackingFieldForNonNullable && !AppropriatelyQualifiedTypeName.EndsWith("?"), "?")} {BackingFieldString};
        {GetAttributesToInsert()}{ContainingObjectDescription.HideMainProperty}{PropertyAccessibilityString}{GetModifiedDerivationKeyword()}{AppropriatelyQualifiedTypeName} {PropertyName}
        {{{StepThroughPropertiesString}
            get
            {{
                {IIF(BackingAccessFieldIncluded, $@"if ({BackingFieldNotAccessedString})
                {{
                    Lazinate_{PropertyName}();
                }}")}{IIF(IsNonLazinatorType && !TrackDirtinessNonSerialized && (!RoslynHelpers.IsReadOnlyStruct(Symbol) || ContainsLazinatorInnerProperty || ContainsOpenGenericInnerProperty), $@"
                    IsDirty = true;")} {IIF(CodeOnAccessed != "", $@"
                {CodeOnAccessed}")}
                return {BackingFieldAccessWithPossibleException};
            }}{StepThroughPropertiesString}
            set
            {{{propertyTypeDependentSet}{RepeatedCodeExecution}
                IsDirty = true;
                DescendantIsDirty = true;
                {CodeBeforeSet}{BackingFieldString} = value;{CodeAfterSet}{IIF(IsNonLazinatorType && TrackDirtinessNonSerialized, $@"
                {BackingDirtyFieldString} = true;")}{IIF(BackingAccessFieldIncluded, $@"
                {BackingFieldAccessedString} = true;")}{RepeatedCodeExecution}
            }}
        }}{(GetModifiedDerivationKeyword() == "override " || !BackingAccessFieldIncluded ? "" : $@"
        {ContainingObjectDescription.HideBackingField}{ContainingObjectDescription.ProtectedIfApplicable}bool {BackingFieldAccessedString};")}{IIF(BackingAccessFieldIncluded, $@"
        private void Lazinate_{PropertyName}()
        {{{lazinateContents}
        }}")}

");

            // Copy property
            if ((PropertyType == LazinatorPropertyType.LazinatorStruct || PropertyType == LazinatorPropertyType.LazinatorStructNullable) && !ContainsOpenGenericInnerProperty)
            { // append copy property so that we can create item on stack if it doesn't need to be edited and hasn't been allocated yet

                sb.Append($@"{GetAttributesToInsert()}{ContainingObjectDescription.HideMainProperty}{PropertyAccessibilityString}{AppropriatelyQualifiedTypeName} {PropertyName}_Copy
                            {{{StepThroughPropertiesString}
                                get
                                {{
                                    {ConditionalCodeGenerator.ConsequentPossibleOnlyIf(BackingAccessFieldIncluded, BackingFieldNotAccessedString, $@"if (LazinatorObjectBytes.Length == 0)
                                        {{
                                            return {DefaultExpression};
                                        }}
                                        else
                                        {{
                                            LazinatorMemory childData = {ChildSliceString};
                                            var toReturn = new {AppropriatelyQualifiedTypeNameWithoutNullableIndicator}();
                                            toReturn.DeserializeLazinator(childData);
                                            toReturn.IsDirty = false;
                                            return toReturn;
                                        }}")}
                                    {IIF(PropertyType == LazinatorPropertyType.LazinatorStructNullable, $@"if ({BackingFieldString} == null)
                                    {{
                                        return null;
                                    }}
                                    ")}var cleanCopy = {BackingFieldWithPossibleValueDereference};
                                    cleanCopy.IsDirty = false;
                                    cleanCopy.DescendantIsDirty = false;
                                    return cleanCopy;
                                }}
                            }}
");
            }

            if (TrackDirtinessNonSerialized)
                AppendDirtinessTracking(sb);
        }

        private string GetPropertyTypeDependentSetString()
        {
            string propertyTypeDependentSet = "";
            if (IsClassOrInterface)
            {
                if (ContainerIsClass) // change the parents collection so that it refers to the new value and not the old value. But other values in the parents collection will not be affected.
                {
                    if (Nullable)
                        propertyTypeDependentSet = $@"
                            if ({BackingFieldString} != null)
                            {{
                                {BackingFieldString}.LazinatorParents = {BackingFieldString}.LazinatorParents.WithRemoved(this);
                            }}
                            if (value != null)
                            {{
                                value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                            }}
                            ";
                    else
                        propertyTypeDependentSet = $@"
                            _ = value ?? throw new ArgumentNullException(nameof(value));
                            if ({BackingFieldString} != null)
                            {{
                                {BackingFieldString}.LazinatorParents = {BackingFieldString}.LazinatorParents.WithRemoved(this);
                            }}
                            value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                            ";
                }
                else
                    propertyTypeDependentSet = $@"";

            }
            else if (IsLazinatorStruct)
            { // Because a struct is always copied, it can only have one parent. 
                if (Nullable)
                    propertyTypeDependentSet = $@"{IIF(ContainerIsClass, $@"
                        if (value.HasValue)
                        {{
                            var copy = value.Value;
                            copy.LazinatorParents = new LazinatorParentsCollection(this);
                            value = copy;
                        }}
                    ")}
                        ";
                else
                    propertyTypeDependentSet = $@"{IIF(ContainerIsClass, $@"
                        value.LazinatorParents = new LazinatorParentsCollection(this);")}
                        ";
            }
            else if (PropertyType == LazinatorPropertyType.OpenGenericParameter)
            {
                if (ContainingObjectDescription.ObjectType == LazinatorObjectType.Class && !ContainingObjectDescription.GeneratingRefStruct)
                    propertyTypeDependentSet = $@"
                        if (value != null && value.IsStruct)
                        {{{IIF(ContainerIsClass, $@"
                            value.LazinatorParents = new LazinatorParentsCollection(this);")}
                        }}
                        else
                        {{
                            if ({BackingFieldString} != null)
                            {{
                                {BackingFieldString}.LazinatorParents = {BackingFieldString}.LazinatorParents.WithRemoved(this);
                            }}
                            if (value != null)
                            {{
                                value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                            }}
                        }}
                            ";
                else
                    propertyTypeDependentSet = $@"";

            }

            return propertyTypeDependentSet;
        }

        private string GetCreateDefaultString()
        {
            string createDefault = $@"{BackingFieldString} = {DefaultExpression};{IIF(IsNonLazinatorType && TrackDirtinessNonSerialized, $@"
                                        {BackingDirtyFieldString} = true; ")}";
            if (IsLazinatorStruct)
                createDefault = $@"{BackingFieldString}{DefaultInitializationIfPossible(AppropriatelyQualifiedTypeName)};{IIF(ContainerIsClass && PropertyType != LazinatorPropertyType.LazinatorStructNullable, $@"
                                {BackingFieldString}.LazinatorParents = new LazinatorParentsCollection(this);")}";
            else if (PropertyType == LazinatorPropertyType.OpenGenericParameter)
                createDefault = $@"{BackingFieldString}{DefaultInitializationIfPossible(AppropriatelyQualifiedTypeName)};{IIF(ContainerIsClass, $@"
                                if ({BackingFieldString} != null)
                                {{ // {PropertyName} is a struct
                                    {BackingFieldString}.LazinatorParents = new LazinatorParentsCollection(this);
                                }}{IfInitializationRequiredAddElseThrow}")}";
            return createDefault;
        }

        private string GetAssignmentString()
        {
            string assignment;
            string selfReference = IIF(ContainingObjectDescription.ObjectType == LazinatorObjectType.Class && !ContainingObjectDescription.GeneratingRefStruct, ", this");
            if (PropertyType == LazinatorPropertyType.LazinatorClassOrInterface || PropertyType == LazinatorPropertyType.LazinatorNonnullableClassOrInterface || PropertyType == LazinatorPropertyType.LazinatorStruct || PropertyType == LazinatorPropertyType.LazinatorStructNullable)
            {
                if (IsInterface)
                    assignment =
                    $@"
                        {BackingFieldString} = DeserializationFactory.Instance.CreateBasedOnType<{AppropriatelyQualifiedTypeName}>(childData{selfReference}); ";
                else if (IsAbstract)
                    assignment =
                    $@"
                        {BackingFieldString} = DeserializationFactory.Instance.CreateAbstractType<{AppropriatelyQualifiedTypeName}>(childData{selfReference}); ";
                else
                    assignment =
                        $@"
                        {BackingFieldString} = DeserializationFactory.Instance.CreateBaseOrDerivedType({UniqueIDForLazinatorType}, (c, p) => new {AppropriatelyQualifiedTypeNameWithoutNullableIndicator}(c, p), childData{selfReference}); ";
            }
            else if (PropertyType == LazinatorPropertyType.OpenGenericParameter)
            {
                assignment =
                    $@"
                        {BackingFieldString} = DeserializationFactory.Instance.CreateBasedOnType<{AppropriatelyQualifiedTypeName}>(childData{selfReference}); ";
            }
            else
            {
                bool automaticallyMarkDirtyWhenContainedObjectIsCreated = TrackDirtinessNonSerialized && ContainingObjectDescription.ObjectType == LazinatorObjectType.Class && !ContainingObjectDescription.GeneratingRefStruct; // (1) unless we're tracking dirtiness, there is no field to set when the descendant informs us that it is dirty; (2) with a struct, we can't use an anonymous lambda (and more fundamentally can't pass a delegate to the struct method. Thus, if a struct has a supported collection, we can't automatically set DescendantIsDirty for the struct based on a change in some contained entity.
                assignment = $"{BackingFieldString} = {DirectConverterTypeNamePrefix}ConvertFromBytes_{AppropriatelyQualifiedTypeNameEncodable}(childData);";
            }
            if (CodeOnDeserialized != "")
                assignment += $@"
                        {CodeOnDeserialized}";
            return assignment;
        }

        private string GetRecreationString(string assignment)
        {
            string recreation;
            if (PropertyType == LazinatorPropertyType.LazinatorStruct || PropertyType == LazinatorPropertyType.LazinatorStructNullable
           || ((PropertyType == LazinatorPropertyType.LazinatorClassOrInterface || PropertyType == LazinatorPropertyType.LazinatorNonnullableClassOrInterface) && ContainingObjectDescription.IsSealed))
            {
                // we manually create the type and set the fields. Note that we don't want to call DeserializationFactory, because we would need to pass the field by ref (and we don't need to check for inherited types), and we would need to box a struct in conversion. We follow a similar pattern for sealed classes, because we don't have to worry about inheritance. 
                recreation = GetManualObjectCreation();
            }
            else if (PropertyType == LazinatorPropertyType.OpenGenericParameter)
            {
                // NOTE: If we new for sure that the open generic type would not be a derived type, we could use GetManualObjectCreation.
                // But if we have code that says _MyT = new T() { ... }, then that won't work if this property is instantiated with an
                // open generic type of U : T.
                //if (Symbol is ITypeParameterSymbol typeParameterSymbol && typeParameterSymbol.HasConstructorConstraint)
                //    creation = GetManualObjectCreation();
                //else
                recreation = $@"{assignment}";
            }
            else
                recreation = $@"{assignment}";
            return recreation;
        }

        public string GetLazinateContentsForConstructor() => GetLazinateContents(GetCreateDefaultString(), GetRecreationString(GetAssignmentString()), false);
        private string GetLazinateContents(string createDefault, string recreation, bool defineChildData = true)
        {
            return $@"
            {ConditionalCodeGenerator.ConsequentPossibleOnlyIf(Nullable || NonNullableThatCanBeUninitialized, "LazinatorObjectBytes.Length == 0", createDefault, $@"{IIF(defineChildData, "LazinatorMemory ")}childData = {ChildSliceString};
                {recreation}")}{IIF(BackingAccessFieldIncluded, $@"
            {BackingFieldAccessedString} = true;")}";
        }

        private string GetManualObjectCreation()
        {
            // if the container object containing this property is a struct, then we can't set LazinatorParents. Meanwhile, if this object is a struct, then we don't need to worry about the case of a null item. 
            string nullItemCheck = PropertyType == LazinatorPropertyType.LazinatorStruct || NonNullableThatRequiresInitialization
                ? ""
                : $@"if (childData.Length == 0)
                        {{
                            {BackingFieldString} = default;
                        }}
                        else ";
            string lazinatorParentClassSet = ContainingObjectDescription.ObjectType == LazinatorObjectType.Struct || ContainingObjectDescription.GeneratingRefStruct ? "" : $@"
                            {{
                                LazinatorParents = new LazinatorParentsCollection(this)
                            }}";

            string doCreation = $@"{BackingFieldString} = new {AppropriatelyQualifiedTypeNameWithoutNullableIndicator}({ConstructorInitialization}){lazinatorParentClassSet};
                        ";
            if (PropertyType == LazinatorPropertyType.LazinatorStructNullable)
                doCreation += $@" var copy = {BackingFieldString}.Value;
                                copy.DeserializeLazinator(childData);
                                {BackingFieldString} = copy;";
            else
                doCreation += $@" {BackingFieldString}.DeserializeLazinator(childData);";
            string creation = nullItemCheck == "" ? doCreation : $@"{nullItemCheck}
                    {{
                        {doCreation}
                    }}";
            return creation;
        }

        private void AppendPlaceholderMemoryProperty(CodeStringBuilder sb)
        {
            sb.AppendLine($@"public ReadOnlyMemory<byte> {PropertyName}
                {{
                    get => throw new NotImplementedException(); // placeholder only
                    set => throw new NotImplementedException(); // placeholder only
                }}");
        }

        private void AppendReadOnlySpanProperty(CodeStringBuilder sb) => AppendReadOnlyMemoryOrReadOnlySpanProperty(sb);

        private void AppendReadOnlyMemoryOrReadOnlySpanProperty(CodeStringBuilder sb)
        {
            var innerFullType = InnerProperties[0].AppropriatelyQualifiedTypeName;
            string castToSpanOfCorrectType;
            castToSpanOfCorrectType = GetReadOnlySpanBackingFieldCast();
            string coreOfGet = SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlySpan ? $@"return {GetReadOnlySpanBackingFieldCast("childData")};" : $@"{BackingFieldString} = childData.ReadOnlyMemory;
                    {BackingFieldAccessedString} = true;"; // for a read-only span, we directly return the lazinator, and don't update accessed, where the property hasn't been accessed
            sb.Append($@"{ContainingObjectDescription.HideBackingField}private ReadOnlyMemory<byte> {BackingFieldString};
        {GetAttributesToInsert()}{ContainingObjectDescription.HideMainProperty}{PropertyAccessibilityString}{GetModifiedDerivationKeyword()}{AppropriatelyQualifiedTypeName} {PropertyName}
        {{{StepThroughPropertiesString}
            get
            {{
                {ConditionalCodeGenerator.ElseConsequentPossibleOnlyIf(BackingAccessFieldIncluded, new ConditionCodeGenerator(BackingFieldNotAccessedString), $@"LazinatorMemory childData = {ChildSliceString};
                    {coreOfGet}")}
                return {castToSpanOfCorrectType};
            }}{StepThroughPropertiesString}
            set
            {{
                {RepeatedCodeExecution}IsDirty = true;
                {BackingFieldString} = new ReadOnlyMemory<byte>({GetSpanCast(innerFullType, true)}(value).ToArray());
                {IIF(BackingAccessFieldIncluded, $@"{BackingFieldAccessedString} = true;
                ")}{RepeatedCodeExecution}
            }}
        }}
        {IIF(BackingAccessFieldIncluded, $@"{ContainingObjectDescription.ProtectedIfApplicable}bool {BackingFieldAccessedString};
")}");
        }

        private string GetSpanCast(string type, bool toByte)
        {
            if (type == "byte")
                return "";
            if (new string[]
                {"Int16", "Int32", "Int64", "UInt16", "UInt32", "UInt64", "DateTime", "TimeSpan"}.Contains(type))
                return toByte ? "Spans.CastSpanFrom" + type : "Spans.CastSpanTo" + type;
            return toByte ? $"MemoryMarshal.Cast<{type}, byte>" : $"MemoryMarshal.Cast<byte, {type}>";
        }

        private string GetReadOnlySpanBackingFieldCast(string propertyName = null)
        {
            if (propertyName == null)
                propertyName = "_" + PropertyName;
            var innerFullType = InnerProperties[0].AppropriatelyQualifiedTypeName;
            string spanAccessor = IIF(SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlySpan, ".Span");
            string castToSpanOfCorrectType;
            if (innerFullType == "byte")
                castToSpanOfCorrectType = $"{propertyName}{spanAccessor}";
            else castToSpanOfCorrectType = $"{GetSpanCast(innerFullType, false)}({propertyName}{spanAccessor})";
            return castToSpanOfCorrectType;
        }

        private void AppendDirtinessTracking(CodeStringBuilder sb)
        {
            sb.Append($@"
        {ContainingObjectDescription.HideBackingField}private bool {BackingDirtyFieldString};
        {ContainingObjectDescription.HideMainProperty}public bool {PropertyName}_Dirty
        {{{StepThroughPropertiesString}
            get => {BackingDirtyFieldString};{StepThroughPropertiesString}
            set
            {{
                {RepeatedCodeExecution}if ({BackingDirtyFieldString} != value)
                {{
                    {BackingDirtyFieldString} = value;
                }}
                if (value && !IsDirty)
                {{
                    IsDirty = true;
                }}{RepeatedCodeExecution}
            }}
        }}
");
        }

        private void SetEnumEquivalentType(INamedTypeSymbol t)
        {
            EnumEquivalentType = RoslynHelpers.RegularizeTypeName(t.EnumUnderlyingType.Name, NullableModeEnabled && Nullable);
        }

        #endregion

        #region Reading and writing

        private void SetReadAndWriteMethodNames()
        {
            if (PropertyType == LazinatorPropertyType.PrimitiveType ||
                PropertyType == LazinatorPropertyType.PrimitiveTypeNullable)
            {
                string name = EnumEquivalentType ?? ShortTypeName;
                if (name == "string" && BrotliCompress)
                    name += "_brotli";
                ReadMethodName = PrimitiveReadWriteMethodNames.ReadNames[name];
                WriteMethodName = PrimitiveReadWriteMethodNames.WriteNames[name];
            }
        }

        AccessorModifierTypes GetModifierType(MethodInfo methodInfo)
        {
            if (methodInfo.IsPublic)
                return AccessorModifierTypes.PublicModifier;
            if (methodInfo.IsPrivate)
                return AccessorModifierTypes.PrivateModifier;
            if (methodInfo.IsFamilyOrAssembly)
                return AccessorModifierTypes.ProtectedInternalModifier;
            if (methodInfo.IsAssembly && methodInfo.IsFamily)
                return AccessorModifierTypes.PrivateProtectedModifier;
            if (methodInfo.IsAssembly)
                return AccessorModifierTypes.InternalModifier;
            if (methodInfo.IsFamily)
                return AccessorModifierTypes.ProtectedModifier;
            throw new NotSupportedException();
        }



        public void AppendPropertyReadString(CodeStringBuilder sb)
        {
            string skipCheckString = SkipCondition == null ? "" : $@"
                            if ({SkipCondition})
                            {{
                                {InitializeWhenSkipped}
                            }}
                            else
                            {{";
            if (IsPrimitive)
            {
                if (SkipCondition != null)
                    sb.AppendLine(skipCheckString);
                sb.AppendLine(
                        new ConditionalCodeGenerator(ReadInclusionConditional, $@"{BackingFieldString} = {EnumEquivalentCastToEnum}span.{ReadMethodName}(ref bytesSoFar);").ToString());
            }
            else
            {
                if (OmitLengthBecauseDefinitelyLast)
                {
                    sb.AppendLine($@"{BackingFieldByteIndex} = bytesSoFar;{skipCheckString}
                                        bytesSoFar = span.Length;");
                }
                else if (IsGuaranteedFixedLength)
                {
                    if (FixedLength == 1)
                        sb.AppendLine($@"{BackingFieldByteIndex} = bytesSoFar;{skipCheckString}
                                            bytesSoFar++;");
                    else
                        sb.AppendLine($@"{BackingFieldByteIndex} = bytesSoFar;{skipCheckString}
                                        bytesSoFar += {FixedLength};");
                }
                else if (IsGuaranteedSmall)
                    sb.AppendLine(
                        $@"{BackingFieldByteIndex} = bytesSoFar;{skipCheckString}
                            " + new ConditionalCodeGenerator(ReadInclusionConditional,
                            "bytesSoFar = span.ToByte(ref bytesSoFar) + bytesSoFar;"));
                else
                    sb.AppendLine(
                        $@"{BackingFieldByteIndex} = bytesSoFar;{skipCheckString}
                            " + new ConditionalCodeGenerator(ReadInclusionConditional,
                            "bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;"));
            }
            if (SkipCondition != null)
            {
                sb.AppendLine($@"}}");
            }
        }

        public void AppendPropertyWriteString(CodeStringBuilder sb)
        {
            // We remember the startOfObjectPosition, and then update the stored buffer at the end,
            // because we can't change the _ByteIndex until after the write, since we may need
            // to read from storage during the write.
            if (!IsPrimitive)
                sb.AppendLine("startOfObjectPosition = writer.Position;");
            // Now, we have to consider the SkipCondition, from a SkipIf attribute. We don't write if the skip condition is
            // met (but still must update the byte index).
            if (SkipCondition != null)
                sb.AppendLine($@"if (!({SkipCondition}))
                            {{");
            // Now, we consider versioning information.
            if (IsPrimitive)
                sb.AppendLine(
                        new ConditionalCodeGenerator(WriteInclusionConditional, $"{WriteMethodName}(ref writer, {EnumEquivalentCastToEquivalentType}{BackingFieldString});").ToString());
            else
            {
                // Finally, the main code for writing a serialized or non serialized object.
                if (PropertyType == LazinatorPropertyType.LazinatorClassOrInterface || PropertyType == LazinatorPropertyType.LazinatorNonnullableClassOrInterface || PropertyType == LazinatorPropertyType.LazinatorStruct || PropertyType == LazinatorPropertyType.LazinatorStructNullable || PropertyType == LazinatorPropertyType.OpenGenericParameter)
                    AppendPropertyWriteString_Lazinator(sb);
                else
                    AppendPropertyWriteString_NonLazinator(sb);
            }
            if (SkipCondition != null)
                sb.AppendLine($@"}}");
            // Now, we update the byte index (and remove buffers for certain supported collections).
            if (!IsPrimitive)
            {
                string removeBuffers = "";
                if (IsSupportedCollectionOrTuple && !IsSimpleListOrArray &&
                    InnerProperties.Any(x => x.IsPossiblyStruct))
                    removeBuffers = new ConditionalCodeGenerator(GetNonNullCheck(true), $" {BackingFieldString} = ({AppropriatelyQualifiedTypeName}) CloneOrChange_{AppropriatelyQualifiedTypeNameEncodable}({BackingFieldAccessWithPossibleException}, l => l.RemoveBufferInHierarchy(), true);").ToString();
                sb.AppendLine($@"if (updateStoredBuffer)
                                {{
                                    {BackingFieldByteIndex} = startOfObjectPosition - startPosition;{removeBuffers}
                                }}");
            }
        }

        private void AppendPropertyWriteString_NonLazinator(CodeStringBuilder sb)
        {
            string omitLengthSuffix = IIF(OmitLengthBecauseDefinitelyLast, "_WithoutLengthPrefix");
            string writeMethodName = PlaceholderMemoryWriteMethod == null ? $"ConvertToBytes_{AppropriatelyQualifiedTypeNameEncodable}" : PlaceholderMemoryWriteMethod;
            if (PlaceholderMemoryWriteMethod == null)
            {
                sb.Append($"{EnsureDeserialized()}");
                if (ContainingObjectDescription.ObjectType == LazinatorObjectType.Class && !ContainingObjectDescription.GeneratingRefStruct)
                {
                    string isBelievedDirtyString = ConditionsCodeGenerator.OrCombine(
                        TrackDirtinessNonSerialized ? $"{PropertyName}_Dirty" : $"{BackingFieldAccessedString}",
                        $"(includeChildrenMode != OriginalIncludeChildrenMode)");
                    sb.AppendLine(
                        $@"WriteNonLazinatorObject{omitLengthSuffix}(
                    nonLazinatorObject: {BackingFieldString}, isBelievedDirty: {isBelievedDirtyString},
                    isAccessed: {BackingFieldAccessedString}, writer: ref writer,
                    getChildSliceForFieldFn: () => {ChildSliceString},
                    verifyCleanness: {(TrackDirtinessNonSerialized ? "verifyCleanness" : "false")},
                    binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
                        {DirectConverterTypeNamePrefix}{writeMethodName}(ref w, {BackingFieldStringOrContainedSpanWithPossibleException(null)},
                            includeChildrenMode, v, updateStoredBuffer));");

                }
                else
                {
                    // as above, must copy local struct variables for anon lambda.
                    string binaryWriterAction;
                    if (PlaceholderMemoryWriteMethod == null &&
                        (SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlySpan))
                        binaryWriterAction = $"copy_{PropertyName}.Write(ref w)";
                    else
                        binaryWriterAction =
                            $"{DirectConverterTypeNamePrefix}{writeMethodName}(ref w, copy_{PropertyName}, includeChildrenMode, v, updateStoredBuffer)";
                    string isBelievedDirtyString = ConditionsCodeGenerator.OrCombine(
                        TrackDirtinessNonSerialized ? $"{PropertyName}_Dirty" : $"{BackingFieldAccessedString}",
                        $"(includeChildrenMode != OriginalIncludeChildrenMode)");
                    sb.AppendLine(
                        $@"var serializedBytesCopy_{PropertyName} = LazinatorMemoryStorage;
                        var byteIndexCopy_{PropertyName} = {BackingFieldByteIndex};
                        var byteLengthCopy_{PropertyName} = {BackingFieldByteLength};
                        var copy_{PropertyName} = {BackingFieldString};
                        WriteNonLazinatorObject{omitLengthSuffix}(
                        nonLazinatorObject: {BackingFieldString}, isBelievedDirty: {isBelievedDirtyString},
                        isAccessed: {BackingFieldAccessedString}, writer: ref writer,
                        getChildSliceForFieldFn: () => GetChildSlice(serializedBytesCopy_{PropertyName}, byteIndexCopy_{PropertyName}, byteLengthCopy_{PropertyName}{ChildSliceEndString}),
                        verifyCleanness: {(TrackDirtinessNonSerialized ? "verifyCleanness" : "false")},
                        binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
                            {binaryWriterAction});");

                }
            }
            else
                sb.AppendLine($@"WriteNonLazinatorObject{omitLengthSuffix}(
                        nonLazinatorObject: default, isBelievedDirty: true,
                        isAccessed: true, writer: ref writer,
                        getChildSliceForFieldFn: () => {ChildSliceString},
                        verifyCleanness: {(TrackDirtinessNonSerialized ? "verifyCleanness" : "false")},
                        binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
                            {DirectConverterTypeNamePrefix}{writeMethodName}(ref w, default,
                                includeChildrenMode, v, updateStoredBuffer));");
        }

        private void AppendPropertyWriteString_Lazinator(CodeStringBuilder sb)
        {
            string withInclusionConditional = null;
            string propertyNameOrCopy = PropertyType == LazinatorPropertyType.LazinatorStructNullable ? "copy" : $"{BackingFieldString}";
            Func<string, string> lazinatorNullableStructNullCheck = originalString => PropertyType == LazinatorPropertyType.LazinatorStructNullable ? GetNullCheckIfThen($"{BackingFieldString}", $"WriteNullChild(ref writer, {(IsGuaranteedSmall ? "true" : "false")}, {(IsGuaranteedFixedLength || OmitLengthBecauseDefinitelyLast ? "true" : "false")});", originalString) : originalString;
            if (ContainingObjectDescription.ObjectType == LazinatorObjectType.Class && !ContainingObjectDescription.GeneratingRefStruct)
            {
                string mainWriteString = $@"{IIF(PropertyType == LazinatorPropertyType.LazinatorStructNullable, $@"var copy = {BackingFieldString}.Value;
                            ")}WriteChild(ref writer, ref {propertyNameOrCopy}, includeChildrenMode, {BackingFieldAccessedString}, () => {ChildSliceString}, verifyCleanness, updateStoredBuffer, {(IsGuaranteedSmall ? "true" : "false")}, {(IsGuaranteedFixedLength || OmitLengthBecauseDefinitelyLast ? "true" : "false")}, this);{IIF(PropertyType == LazinatorPropertyType.LazinatorStructNullable, $@"
                                {BackingFieldString} = copy;")}";
                withInclusionConditional =
                    new ConditionalCodeGenerator(WriteInclusionConditional, $@"{EnsureDeserialized()}{lazinatorNullableStructNullCheck(mainWriteString)}").ToString();
            }
            else
            {
                // for structs, we can't pass local struct variables in the lambda, so we have to copy them over. We'll assume we have to do this with open generics too.
                string mainWriteString = $@"var serializedBytesCopy = LazinatorMemoryStorage;
                            var byteIndexCopy = {BackingFieldByteIndex};
                            var byteLengthCopy = {BackingFieldByteLength};
                            {IIF(PropertyType == LazinatorPropertyType.LazinatorStructNullable, $@"var copy = {BackingFieldString}.Value;
                            ")}WriteChild(ref writer, ref {propertyNameOrCopy}, includeChildrenMode, {BackingFieldAccessedString}, () => GetChildSlice(serializedBytesCopy, byteIndexCopy, byteLengthCopy{ChildSliceEndString}), verifyCleanness, updateStoredBuffer, {(IsGuaranteedSmall ? "true" : "false")}, {(IsGuaranteedFixedLength || OmitLengthBecauseDefinitelyLast ? "true" : "false")}, null);{IIF(PropertyType == LazinatorPropertyType.LazinatorStructNullable, $@"
                                {BackingFieldString} = copy;")}";
                withInclusionConditional =
                    $@"{new ConditionalCodeGenerator(WriteInclusionConditional, $"{EnsureDeserialized()}{lazinatorNullableStructNullCheck(mainWriteString)}")}";
            }
            sb.AppendLine(withInclusionConditional);
        }

        private string EnsureDeserialized()
        {
            return $@"{new ConditionalCodeGenerator(
                ConditionsCodeGenerator.AndCombine(
                    $"(includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode)",
                    $"{BackingFieldNotAccessedString}"), 
                $"var deserialized = {PropertyName};")}";
        }

        public void AppendCopyPropertyToClone(CodeStringBuilder sb, string nameOfCloneVariable)
        {
            string copyInstruction = "";
            if (IsLazinator)
            {
                string nonNullStatement = $@"{nameOfCloneVariable}.{PropertyName} = ({AppropriatelyQualifiedTypeName}) {PropertyName}{IIF(PropertyType == LazinatorPropertyType.LazinatorStructNullable, ".Value")}.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);";
                if (!Nullable)
                    copyInstruction = nonNullStatement;
                else
                    copyInstruction = GetNullCheckIfThen(PropertyName,
                        $"{nameOfCloneVariable}.{PropertyName} = {DefaultExpression};",
                        nonNullStatement);
            }
            else if (IsPrimitive)
                copyInstruction = $"{nameOfCloneVariable}.{PropertyName} = {PropertyName};";
            else if ((PropertyType == LazinatorPropertyType.NonLazinator && HasInterchangeType) || PropertyType == LazinatorPropertyType.SupportedCollection || PropertyType == LazinatorPropertyType.SupportedTuple)
            {
                copyInstruction = $"{nameOfCloneVariable}.{PropertyName} = CloneOrChange_{AppropriatelyQualifiedTypeNameEncodable}({PropertyName}, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);";
            }
            else if (PropertyType == LazinatorPropertyType.NonLazinator)
                copyInstruction = $"{nameOfCloneVariable}.{PropertyName} = {DirectConverterTypeNamePrefix}CloneOrChange_{AppropriatelyQualifiedTypeNameEncodable}({PropertyName}, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);";
            sb.AppendLine(new ConditionalCodeGenerator(WriteInclusionConditional, copyInstruction).ToString());
        }

        #endregion

        #region Supported collections

        public void AppendSupportedCollectionConversionMethods(CodeStringBuilder sb, HashSet<string> alreadyGenerated)
        {
            if (PropertyType != LazinatorPropertyType.SupportedCollection)
                return;

            if (alreadyGenerated.Contains(AppropriatelyQualifiedTypeNameEncodable))
                return;
            alreadyGenerated.Add(AppropriatelyQualifiedTypeNameEncodable);

            if (SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlySpan)
            {
                AppendReadOnlySpan_ConvertToBytes(sb);
                AppendReadOnlySpanOrMemory_Clone(sb);
                return;
            }

            AppendSupportedCollection_ConvertFromBytes(sb);
            AppendSupportedCollection_ConvertToBytes(sb);
            AppendSupportedCollection_CloneOrChange(sb);

            RecursivelyAppendConversionMethods(sb, alreadyGenerated);
        }

        private void AppendReadOnlySpanOrMemory_Clone(CodeStringBuilder sb)
        {
            bool isMemory = AppropriatelyQualifiedTypeName.StartsWith("Memory") || AppropriatelyQualifiedTypeName.StartsWith("ReadOnlyMemory");
            string memoryOrSpanWord = isMemory ? "Memory" : "Span";
            string innerFullType = InnerProperties[0].AppropriatelyQualifiedTypeName;
            string innerFullTypeSizeEquivalent = (innerFullType == "DateTime" || innerFullType == "TimeSpan") ? "long" : innerFullType;
            string source = (innerFullType == "byte") ? "itemToClone" : $"{GetSpanCast(innerFullType, true)}(itemToClone)";
            string toReturn = (innerFullType == "byte") ? "clone" : $"{GetSpanCast(innerFullType, false)}(clone)";

            sb.AppendLine($@"private static {AppropriatelyQualifiedTypeName} CloneOrChange_{AppropriatelyQualifiedTypeNameEncodable}({AppropriatelyQualifiedTypeName} itemToClone, Func<{ILazinatorString}, {ILazinatorString}> cloneOrChangeFunc, bool avoidCloningIfPossible)
            {{
                var clone = new {memoryOrSpanWord}<byte>(new byte[itemToClone.Length * sizeof({innerFullTypeSizeEquivalent})]);
                {source}.CopyTo(clone);
                return {toReturn};
            }}");
        }

        private void AppendReadOnlySpan_ConvertToBytes(CodeStringBuilder sb) => AppendReadOnlySpanOrMemory_ConvertToBytes(sb, true);

        private void AppendReadOnlyMemory_ConvertToBytes(CodeStringBuilder sb) => AppendReadOnlySpanOrMemory_ConvertToBytes(sb, false);

        private void AppendReadOnlySpanOrMemory_ConvertToBytes(CodeStringBuilder sb, bool isSpan)
        {
            // this method is used within classes, but not within structs
            if (ContainingObjectDescription.ObjectType != LazinatorObjectType.Class || ContainingObjectDescription.GeneratingRefStruct)
                return;

            string innerFullType = InnerProperties[0].AppropriatelyQualifiedTypeName;
            string innerTypeEncodable = InnerProperties[0].AppropriatelyQualifiedTypeNameEncodable;

            sb.Append($@"
                         private static void ConvertToBytes_{AppropriatelyQualifiedTypeNameEncodable}(ref BinaryBufferWriter writer, {AppropriatelyQualifiedTypeName} itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
                        {{
                            ReadOnlySpan<byte> toConvert = {GetSpanCast(innerFullType, true)}(itemToConvert{(isSpan ? "" : ".Span")});
                            for (int i = 0; i < toConvert.Length; i++)
                            {{
                                writer.Write(toConvert[i]);
                            }}
                        }}
                    ");
        }

        private void AppendSupportedCollection_ConvertToBytes(CodeStringBuilder sb)
        {
            string lengthWord, itemString, itemStringSetup, forStatement, cloneString;
            GetItemAccessStrings(out lengthWord, out itemString, out itemStringSetup, out forStatement, out cloneString);

            if (
                (
                    (SupportedCollectionType == LazinatorSupportedCollectionType.Memory && InnerProperties[0].AppropriatelyQualifiedTypeName != "byte")
                    ||
                    SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlyMemory
                )
                && Nullable)
            {
                // we will use a method for the Nullable, then an inner method for the non-nullable case,
                // even though the WriteNonLazinator takes care of the nullable, so there is nothing to do 
                // but call the inner method.
                sb.Append($@"

                    private static void ConvertToBytes_{AppropriatelyQualifiedTypeNameEncodable}(ref BinaryBufferWriter writer, {AppropriatelyQualifiedTypeName} itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
                    {{
                        if (itemToConvert == null)
                        {{
                            writer.Write((bool)true);
                            return;
                        }}
                        writer.Write((bool)false);
                        {DirectConverterTypeNamePrefix}ConvertToBytes_{AppropriatelyQualifiedTypeNameEncodableWithoutNullable}(ref writer, itemToConvert.Value, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                    }}
");
            }
            else
            {
                string writeCollectionLengthCommand;
                if (ArrayRank > 1)
                {
                    StringBuilder arrayStringBuilder = new StringBuilder();
                    for (int i = 0; i < ArrayRank; i++)
                    {
                        string writeLengthForRankString = $"CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.GetLength({i}));";
                        if (i < ArrayRank - 1)
                            arrayStringBuilder.AppendLine(writeLengthForRankString);
                        else
                            arrayStringBuilder.Append(writeLengthForRankString);
                    }
                    writeCollectionLengthCommand = arrayStringBuilder.ToString();
                }
                else
                    writeCollectionLengthCommand = $"CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.{lengthWord});";
                string writeCommand = InnerProperties[0].GetSupportedCollectionWriteCommands(itemString, IsSimpleListOrArray);
                if ((SupportedCollectionType == LazinatorSupportedCollectionType.Memory || SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlyMemory) && InnerProperties[0].AppropriatelyQualifiedTypeName == "byte")
                {
                    sb.AppendLine($@"

                    private static void ConvertToBytes_{AppropriatelyQualifiedTypeNameEncodable}(ref BinaryBufferWriter writer, {AppropriatelyQualifiedTypeName} itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
                    {{");
                    if (Nullable)
                        sb.Append($@"if (itemToConvert == null)
                            {{
                                writer.Write((bool)true);
                                return;
                            }}
                            writer.Write((bool)false);
                            writer.Write(itemToConvert.Value.Span);
                        }}
                        ");
                    else
                        sb.Append($@"writer.Write(itemToConvert.Span);
                            }}
                            ");
                }
                else
                    sb.Append($@"

                    private static void ConvertToBytes_{AppropriatelyQualifiedTypeNameEncodable}(ref BinaryBufferWriter writer, {AppropriatelyQualifiedTypeName} itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
                    {{
                        {(SupportedCollectionType == LazinatorSupportedCollectionType.Memory || SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlyMemory ? "" : $@"if (itemToConvert == {DefaultExpression})
                        {{
                            return;
                        }}
                        ")}{writeCollectionLengthCommand}
                        {forStatement}
                        {{{itemStringSetup}{writeCommand}
                        }}
                    }}
");
            }
        }

        private void AppendSupportedCollection_CloneOrChange(CodeStringBuilder sb)
        {
            PropertyDescription innerProperty = InnerProperties[0];
            string collectionAddItem, collectionAddNull;
            innerProperty.GetSupportedCollectionAddCommands(this, out collectionAddItem, out collectionAddNull);
            collectionAddItem = collectionAddItem.Replace("item ", "itemCopied ").Replace("item;", "itemCopied;").Replace("item)", "itemCopied)").Replace("item.", "itemCopied.");
            bool avoidCloningIfPossibleOption = IsSimpleListOrArray && innerProperty.IsLazinator;
            string creationText = GetCreationText(avoidCloningIfPossibleOption);

            string lengthWord, itemString, itemStringSetup, forStatement, cloneString;
            GetItemAccessStrings(out lengthWord, out itemString, out itemStringSetup, out forStatement, out cloneString, "itemToClone");

            if (Nullable && (SupportedCollectionType == LazinatorSupportedCollectionType.Memory || SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlyMemory))
                lengthWord = $"Value.{lengthWord}";
            if (ArrayRank > 1)
                forStatement = DeleteLines(forStatement, (int)ArrayRank); // we will define collectionLengths for each dimension in creation statement and don't need to redefine

            sb.Append($@"
                    private static {AppropriatelyQualifiedTypeName} CloneOrChange_{AppropriatelyQualifiedTypeNameEncodable}({AppropriatelyQualifiedTypeName} itemToClone, Func<{innerProperty.ILazinatorString}, {innerProperty.ILazinatorString}> cloneOrChangeFunc, bool avoidCloningIfPossible)
                    {{
                        {(GetNullCheckIfThenButOnlyIfNullable(Nullable, "itemToClone", "return default;", ""))}int collectionLength = itemToClone.{lengthWord};{IIF(ArrayRank > 1, () => "\n" + String.Join("\n", Enumerable.Range(0, ArrayRank.Value).Select(x => $"int collectionLength{x} = itemToClone.GetLength({x});")))}
                        {creationText}
                        {forStatement}
                        {{
                            {IIF(avoidCloningIfPossibleOption, $@"if (avoidCloningIfPossible)
                            {{
                                if ({innerProperty.GetNonNullCheck(false, "itemToClone[itemIndex]")})
                                {{
                                    itemToClone[itemIndex] = ({innerProperty.AppropriatelyQualifiedTypeName}) (cloneOrChangeFunc(itemToClone[itemIndex]){innerProperty.PossibleUnsetException});
                                }}
                                continue;
                            }}
                            ")}{IIF(innerProperty.Nullable, innerProperty.GetNullCheckIfThen(itemString, $@"{collectionAddNull}", $@"var itemCopied = {cloneString};
                                {collectionAddItem}"))}{IIF(!innerProperty.Nullable, $@"var itemCopied = {cloneString};
                                {collectionAddItem}")}
                        }}
                        return collection;
                    }}
");
        }

        private static string DeleteLines(string input, int linesToSkip)
        {
            int startIndex = 0;
            for (int i = 0; i < linesToSkip; ++i)
                startIndex = input.IndexOf('\n', startIndex) + 1;
            return input.Substring(startIndex);
        }

        private void GetItemAccessStrings(out string lengthWord, out string itemString, out string itemStringSetup, out string forStatement, out string cloneString, string itemAccessName = "itemToConvert")
        {
            bool isArray = SupportedCollectionType == LazinatorSupportedCollectionType.Array;
            if (SupportedCollectionType == LazinatorSupportedCollectionType.List || SupportedCollectionType == LazinatorSupportedCollectionType.SortedSet || SupportedCollectionType == LazinatorSupportedCollectionType.LinkedList ||
                SupportedCollectionType == LazinatorSupportedCollectionType.HashSet || SupportedCollectionType == LazinatorSupportedCollectionType.Dictionary || SupportedCollectionType == LazinatorSupportedCollectionType.SortedDictionary || SupportedCollectionType == LazinatorSupportedCollectionType.SortedList || SupportedCollectionType == LazinatorSupportedCollectionType.Queue || SupportedCollectionType == LazinatorSupportedCollectionType.Stack)
            {
                lengthWord = "Count";
            }
            else if (SupportedCollectionType == LazinatorSupportedCollectionType.Memory || SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlyMemory)
            {
                lengthWord = "Length";
            }
            else if (isArray)
            {
                lengthWord = "Length";
            }
            else
                lengthWord = null;
            itemStringSetup = "";
            if (SupportedCollectionType == LazinatorSupportedCollectionType.HashSet || SupportedCollectionType == LazinatorSupportedCollectionType.Dictionary || SupportedCollectionType == LazinatorSupportedCollectionType.SortedDictionary || SupportedCollectionType == LazinatorSupportedCollectionType.SortedList)
            {
                forStatement = $@"foreach (var item in {itemAccessName})";
                itemString = "item"; // can't index into hash set
            }
            else if (SupportedCollectionType == LazinatorSupportedCollectionType.Memory || SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlyMemory)
            {
                forStatement =
                        $@"var {itemAccessName}Span = {itemAccessName}{IIF(Nullable, ".Value")}.Span;
                        int {itemAccessName}Count = {itemAccessName}Span.{lengthWord};
                        for (int itemIndex = 0; itemIndex < {itemAccessName}Count; itemIndex++)";
                itemString =
                    $"{itemAccessName}Span[itemIndex]"; // this is needed for Memory<T>, since we don't have a foreach method defined, and is likely slightly more performant anyway
            }
            else if (ArrayRank > 1)
            { // normal rank arrays can be handled separately
                StringBuilder arrayStringBuilder = new StringBuilder();
                int i = 0;
                for (i = 0; i < ArrayRank; i++)
                    arrayStringBuilder.AppendLine($"int collectionLength{i} = {itemAccessName}.GetLength({i});");
                for (i = 0; i < ArrayRank; i++)
                {
                    string stringForRank = $"for (int itemIndex{i} = 0; itemIndex{i} < collectionLength{i}; itemIndex{i}++)";
                    if (i == ArrayRank - 1)
                        arrayStringBuilder.Append(stringForRank);
                    else
                        arrayStringBuilder.AppendLine(stringForRank);
                }

                forStatement = arrayStringBuilder.ToString();

                string innerArrayText = (String.Join(", ", Enumerable.Range(0, (int)ArrayRank).Select(j => $"itemIndex{j}")));
                itemString = $"{itemAccessName}[{innerArrayText}]";
            }
            else if (SupportedCollectionType == LazinatorSupportedCollectionType.Queue)
            {
                forStatement =
                    $@"int {itemAccessName}Count = {itemAccessName}.{lengthWord};
                        var q = System.Linq.Enumerable.ToList({itemAccessName});
                        for (int itemIndex = 0; itemIndex < {itemAccessName}Count; itemIndex++)";
                itemString =
                    "q[itemIndex]";
            }
            else if (SupportedCollectionType == LazinatorSupportedCollectionType.LinkedList)
            {
                forStatement =
                    $@"int {itemAccessName}Count = {itemAccessName}.{lengthWord};
                        for (int itemIndex = 0; itemIndex < {itemAccessName}Count; itemIndex++)";
                itemString =
                    $"System.Linq.Enumerable.ElementAt({itemAccessName}, itemIndex)";
            }
            else if (SupportedCollectionType == LazinatorSupportedCollectionType.Stack)
            {
                forStatement =
                    $@"int {itemAccessName}Count = {itemAccessName}.{lengthWord};
                        var stackReversed = System.Linq.Enumerable.ToList({itemAccessName});
                        stackReversed.Reverse();
                        for (int itemIndex = 0; itemIndex < {itemAccessName}Count; itemIndex++)";
                itemString =
                    "stackReversed[itemIndex]";
            }
            else if (SupportedCollectionType == LazinatorSupportedCollectionType.SortedSet)
            {

                forStatement =
                    $@"int {itemAccessName}Count = {itemAccessName}.{lengthWord};
                        var sortedSet = System.Linq.Enumerable.ToList({itemAccessName});
                        for (int itemIndex = 0; itemIndex < {itemAccessName}Count; itemIndex++)";
                itemString =
                    "sortedSet[itemIndex]";
            }
            else
            {
                forStatement =
                        $@"int {itemAccessName}Count = {itemAccessName}.{lengthWord};
                        for (int itemIndex = 0; itemIndex < {itemAccessName}Count; itemIndex++)";
                itemString =
                    $"{itemAccessName}[itemIndex]"; // this is needed for Memory<T>, since we don't have a foreach method defined, and is likely slightly more performant anyway
            }

            cloneString = InnerProperties[0].GetCloneStringWithinCloneMethod(itemString);
        }

        private string GetCloneStringWithinCloneMethod(string itemString)
        {
            string cloneString;
            if (IsLazinator)
            {
                cloneString = $"(cloneOrChangeFunc({itemString}){PossibleUnsetException})";
                //if (IsLazinatorStruct)
                //    cloneString = $"cloneOrChangeFunc({itemString}).CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer)";
                //else
                //    cloneString = $"{itemString}?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer)";
            }
            else if (SupportedCollectionType != null || SupportedTupleType != null)
                cloneString = $"CloneOrChange_{AppropriatelyQualifiedTypeNameEncodable}({itemString}, cloneOrChangeFunc, avoidCloningIfPossible)";
            else if (IsPrimitive || IsNonLazinatorType)
                cloneString = itemString;
            else
                throw new NotImplementedException();
            return $"({AppropriatelyQualifiedTypeName}) " + cloneString;
        }

        private void AppendSupportedCollection_ConvertFromBytes(CodeStringBuilder sb)
        {
            if ((SupportedCollectionType == LazinatorSupportedCollectionType.Memory || SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlyMemory) && InnerProperties[0].AppropriatelyQualifiedTypeName == "byte")
            {
                sb.AppendLine($@"
                    private static {AppropriatelyQualifiedTypeName} ConvertFromBytes_{AppropriatelyQualifiedTypeNameEncodable}(LazinatorMemory storage)
                    {{");
                if (Nullable)
                    sb.Append($@"int index = 0;
                            bool isNull = storage.ReadOnlySpan.ToBoolean(ref index);
                            if (isNull)
                            {{
                                return null;
                            }}
                            ReadOnlySpan<byte> span = storage.Span.Slice(1);
                            return span.ToArray();
                        }}");
                else
                    sb.Append($@"return storage.Memory.ToArray();
                    }}"
                    );
                return;
            }

            string creationText = GetCreationText(false);

            string readCollectionLengthCommand = null, forStatementCommand = null;
            if (ArrayRank > 1)
            {
                StringBuilder arrayStringBuilder = new StringBuilder();
                for (int i = 0; i < ArrayRank; i++)
                {
                    string rankLengthCommand = $"int collectionLength{i} = span.ToDecompressedInt(ref bytesSoFar);";
                    if (i == ArrayRank - 1)
                        arrayStringBuilder.Append(rankLengthCommand);
                    else
                        arrayStringBuilder.AppendLine(rankLengthCommand);
                }
                readCollectionLengthCommand = arrayStringBuilder.ToString();
                arrayStringBuilder = new StringBuilder();
                for (int i = 0; i < ArrayRank; i++)
                {
                    string forStatementCommandPart = $"for (int itemIndex{i} = 0; itemIndex{i} < collectionLength{i}; itemIndex{i}++)";
                    if (i == ArrayRank - 1)
                        arrayStringBuilder.Append(forStatementCommandPart);
                    else
                        arrayStringBuilder.AppendLine(forStatementCommandPart);
                }
                forStatementCommand = arrayStringBuilder.ToString();
            }
            else
            {
                readCollectionLengthCommand = $"int collectionLength = span.ToDecompressedInt(ref bytesSoFar);";
                forStatementCommand = $"for (int itemIndex = 0; itemIndex < collectionLength; itemIndex++)";
            }

            PropertyDescription innerProperty = InnerProperties[0];
            CheckForLazinatorInNonLazinator(innerProperty);
            string readCommand = innerProperty.GetSupportedCollectionReadCommands(this);
            string preliminaryNullCheck;
            if (Nullable && (SupportedCollectionType == LazinatorSupportedCollectionType.Memory || SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlyMemory))
            {
                preliminaryNullCheck = $@"int index = 0;
                        bool isNull = storage.ReadOnlySpan.ToBoolean(ref index);
                        if (isNull)
                        {{
                            return null;
                        }}
                        ReadOnlySpan<byte> span = storage.Span.Slice(1);";
            }
            else
            {
                preliminaryNullCheck = $@"{IIF(Nullable || IsSupportedTupleType, $@"if (storage.Length == 0)
                        {{
                            return {DefaultExpression};
                        }}
                        ")}ReadOnlySpan<byte> span = storage.Span;";
            }
            sb.Append($@"
                    private static {AppropriatelyQualifiedTypeName} ConvertFromBytes_{AppropriatelyQualifiedTypeNameEncodable}(LazinatorMemory storage)
                    {{
                        {preliminaryNullCheck}
                        int bytesSoFar = 0;
                        {readCollectionLengthCommand}

                        {creationText}
                        {forStatementCommand}
                        {{{readCommand}
                        }}

                        return collection;
                    }}");
        }

        private string GetCreationText(bool avoidCloningIfPossibleOption)
        {
            string creationText;
            bool isArray = SupportedCollectionType == LazinatorSupportedCollectionType.Array;
            if (SupportedCollectionType == LazinatorSupportedCollectionType.List ||
                SupportedCollectionType == LazinatorSupportedCollectionType.HashSet ||
                SupportedCollectionType == LazinatorSupportedCollectionType.Dictionary ||
                SupportedCollectionType == LazinatorSupportedCollectionType.SortedList ||
                SupportedCollectionType == LazinatorSupportedCollectionType.Queue ||
                SupportedCollectionType == LazinatorSupportedCollectionType.Stack)
            {
                creationText = $@"{AppropriatelyQualifiedTypeName} collection = {IIF(avoidCloningIfPossibleOption, $"avoidCloningIfPossible ? itemToClone : ")}new {AppropriatelyQualifiedTypeNameWithoutNullableIndicator}(collectionLength);";
            }
            else if (SupportedCollectionType == LazinatorSupportedCollectionType.SortedDictionary || SupportedCollectionType == LazinatorSupportedCollectionType.SortedSet || SupportedCollectionType == LazinatorSupportedCollectionType.LinkedList)
            {
                creationText = $"{AppropriatelyQualifiedTypeName} collection = {IIF(avoidCloningIfPossibleOption, $"avoidCloningIfPossible ? itemToClone : ")}new {AppropriatelyQualifiedTypeNameWithoutNullableIndicator}();"; // can't initialize collection length
            }
            else if (IsMemoryOrSpan)
            {
                creationText =
                    $@"{AppropriatelyQualifiedTypeNameWithoutNullableIndicator} collection = new {AppropriatelyQualifiedTypeNameWithoutNullableIndicator}(new {InnerProperties[0].AppropriatelyQualifiedTypeNameWithoutNullableIndicator}[collectionLength]);
                            var collectionAsSpan = collection.Span;"; // for now, create array on the heap
                if (SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlyMemory)
                    creationText = creationText.Replace("ReadOnlyMemory<", "Memory<"); // we must use Memory here so that it can be assigned to
            }
            else if (isArray)
            {
                if (ArrayRank == 1)
                {
                    string newExpression = ReverseBracketOrder($"{InnerProperties[0].AppropriatelyQualifiedTypeName}[collectionLength]");
                    creationText = $"{AppropriatelyQualifiedTypeName} collection = {IIF(avoidCloningIfPossibleOption, $"avoidCloningIfPossible ? itemToClone : ")}new {newExpression};";
                }
                else
                {
                    string innerArrayText = (String.Join(", ", Enumerable.Range(0, (int)ArrayRank).Select(j => $"collectionLength{j}")));
                    string newExpression = ReverseBracketOrder($"{InnerProperties[0].AppropriatelyQualifiedTypeName}[{innerArrayText}]");
                    creationText = $"{AppropriatelyQualifiedTypeName} collection = new {newExpression};";
                }
            }
            else
                throw new NotImplementedException();
            return creationText;
        }

        private void CheckForLazinatorInNonLazinator(PropertyDescription innerProperty)
        {
            if (innerProperty.IsLazinator && (Config?.ProhibitLazinatorInNonLazinator ?? false) && !OutmostPropertyDescription.AllowLazinatorInNonLazinator)
                throw new LazinatorCodeGenException($"Property {PropertyName} includes a Lazinator generic type in a non-Lazinator collection, but this is prohibited in a configuration setting. Consider using the AllowLazinatorInNonLazinator attribute.");
        }

        private void RecursivelyAppendConversionMethods(CodeStringBuilder sb, HashSet<string> alreadyGenerated)
        {
            foreach (var inner in InnerProperties)
            {
                inner.AppendSupportedCollectionConversionMethods(sb, alreadyGenerated);
                inner.AppendSupportedTupleConversionMethods(sb, alreadyGenerated);
                inner.AppendInterchangeTypes(sb, alreadyGenerated);
            }
        }

        private string GetSupportedCollectionReadCommands(PropertyDescription outerProperty)
        {
            string collectionAddItem, collectionAddNull;
            GetSupportedCollectionAddCommands(outerProperty, out collectionAddItem, out collectionAddNull);

            if (IsPrimitive)
                return ($@"
                        {AppropriatelyQualifiedTypeName} item = {EnumEquivalentCastToEnum}span.{ReadMethodName}(ref bytesSoFar);
                        {collectionAddItem}");
            else
            {
                bool innerTypeIsNullable = outerProperty.InnerProperties[0].Nullable;
                bool addNullPossible = innerTypeIsNullable;
                if (IsNonLazinatorType)
                {
                    if (Nullable)
                        return ($@"
                            int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                            {ConditionalCodeGenerator.ConsequentPossibleOnlyIf(addNullPossible, "lengthCollectionMember == 0", collectionAddNull, $@"LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                                var item = {DirectConverterTypeNamePrefix}ConvertFromBytes_{AppropriatelyQualifiedTypeNameEncodable}(childData);
                                {collectionAddItem}")}bytesSoFar += lengthCollectionMember;");
                    else
                    {
                        return ($@"
                            int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                            LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                            var item = {DirectConverterTypeNamePrefix}ConvertFromBytes_{AppropriatelyQualifiedTypeNameEncodable}(childData);
                                {collectionAddItem}
                            bytesSoFar += lengthCollectionMember;");
                    }
                }
                else // Lazinator type
                {
                    string lengthCollectionMemberString = $"int lengthCollectionMember = {GetSpanReadLength()};";
                    if (Nullable)
                        return ($@"
                            {lengthCollectionMemberString}
                            {ConditionalCodeGenerator.ConsequentPossibleOnlyIf(addNullPossible, "lengthCollectionMember == 0", collectionAddNull, $@"LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                                var item = DeserializationFactory.Instance.CreateBasedOnType<{AppropriatelyQualifiedTypeName}>(childData);
                                {collectionAddItem}")}bytesSoFar += lengthCollectionMember;");
                    else
                        return (
                            $@"
                            {lengthCollectionMemberString}
                            LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                            var item = new {AppropriatelyQualifiedTypeNameWithoutNullableIndicator}();
                            item.DeserializeLazinator(childData);
                            {collectionAddItem}
                            bytesSoFar += lengthCollectionMember;");
                }
            }
        }

        private string GetSpanReadLength()
        {
            if (IsGuaranteedFixedLength)
                return $"{FixedLength}";
            else if (IsGuaranteedSmall)
                return "span.ToByte(ref bytesSoFar)";
            else
                return "span.ToInt32(ref bytesSoFar)";
        }

        private void GetSupportedCollectionAddCommands(PropertyDescription outerProperty, out string collectionAddItem, out string collectionAddNull)
        {
            if (outerProperty.SupportedCollectionType == LazinatorSupportedCollectionType.Array)
            {
                if (outerProperty.ArrayRank == 1)
                {
                    collectionAddItem = "collection[itemIndex] = item;";
                    collectionAddNull = $"collection[itemIndex] = {DefaultExpression};";
                }
                else
                {

                    string innerArrayText = (String.Join(", ", Enumerable.Range(0, (int)outerProperty.ArrayRank).Select(j => $"itemIndex{j}")));
                    collectionAddItem = $"collection[{innerArrayText}] = item;";
                    collectionAddNull = $"collection[{innerArrayText}] = {DefaultExpression};";
                }
            }
            else if (outerProperty.SupportedCollectionType == LazinatorSupportedCollectionType.Memory || outerProperty.SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlyMemory)
            {
                collectionAddItem = "collectionAsSpan[itemIndex] = item;";
                collectionAddNull = $"collectionAsSpan[itemIndex] = {DefaultExpression};";
            }
            else if (outerProperty.SupportedCollectionType == LazinatorSupportedCollectionType.Dictionary || outerProperty.SupportedCollectionType == LazinatorSupportedCollectionType.SortedDictionary || outerProperty.SupportedCollectionType == LazinatorSupportedCollectionType.SortedList)
            {
                // the outer type is a dictionary
                collectionAddItem = "collection.Add(item.Key, item.Value);";
                collectionAddNull = ""; // no null entries in dictionary
            }
            else if (outerProperty.SupportedCollectionType == LazinatorSupportedCollectionType.Queue)
            {
                collectionAddItem = "collection.Enqueue(item);";
                collectionAddNull = $"collection.Enqueue({DefaultExpression});";
            }
            else if (outerProperty.SupportedCollectionType == LazinatorSupportedCollectionType.Stack)
            {
                collectionAddItem = "collection.Push(item);";
                collectionAddNull = $"collection.Push({DefaultExpression});";
            }
            else if (outerProperty.SupportedCollectionType == LazinatorSupportedCollectionType.LinkedList)
            {
                collectionAddItem = "collection.AddLast(item);";
                collectionAddNull = $"collection.AddLast({DefaultExpression});";
            }
            else
            {
                collectionAddItem = "collection.Add(item);";
                collectionAddNull = $"collection.Add({DefaultExpression});";
            }
        }

        private string GetSupportedCollectionWriteCommands(string itemString, bool outerPropertyIsSimpleListOrArray)
        {
            string GetSupportedCollectionWriteCommandsHelper()
            {
                if (IsPrimitive)
                    return ($@"
                    {WriteMethodName}(ref writer, {EnumEquivalentCastToEquivalentType}{itemString});");
                else if (IsNonLazinatorType)
                    return ($@"
                    void action(ref BinaryBufferWriter w) => {DirectConverterTypeNamePrefix}ConvertToBytes_{AppropriatelyQualifiedTypeNameEncodable}(ref w, {BackingFieldStringOrContainedSpan(itemString)}, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                    WriteToBinaryWith{LengthPrefixTypeString}LengthPrefix(ref writer, action);");
                else if (IsPossiblyStruct && outerPropertyIsSimpleListOrArray)
                {
                    return ($@"
                    void action(ref BinaryBufferWriter w) 
                    {{
                        var copy = {itemString};{IIF(PropertyType == LazinatorPropertyType.LazinatorStructNullable, $@"
                        if (copy != null)
                        {{")}
                        copy.{IIF(PropertyType == LazinatorPropertyType.LazinatorStructNullable, "Value.")}SerializeExistingBuffer(ref w, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                        {itemString} = copy;{IIF(PropertyType == LazinatorPropertyType.LazinatorStructNullable, $@"
                        }}")}
                    }}
                    WriteToBinaryWith{LengthPrefixTypeString}LengthPrefix(ref writer, action);");
                }
                else
                {
                    return ($@"
                    void action(ref BinaryBufferWriter w) => {itemString}{IIF(PropertyType == LazinatorPropertyType.LazinatorStructNullable, "?")}{NullForgiveness}.SerializeExistingBuffer(ref w, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                    WriteToBinaryWith{LengthPrefixTypeString}LengthPrefix(ref writer, action);");
                }
            }

            string writeCommand = GetSupportedCollectionWriteCommandsHelper();
            string fullWriteCommands;
            if (Nullable)
            {
                if (IsPrimitive)
                    fullWriteCommands = $@"{writeCommand}";
                else if (PropertyType == LazinatorPropertyType.OpenGenericParameter)
                    fullWriteCommands =
                        $@"
                    if (System.Collections.Generic.EqualityComparer<{AppropriatelyQualifiedTypeName}>.Default.Equals({itemString}, {DefaultExpression}))
                    {{
                        writer.Write((uint)0);
                    }}
                    else 
                    {{
                        {writeCommand}
                    }}
                    ";
                else
                    fullWriteCommands =
                        $@"
                    if ({itemString} == {DefaultExpression})
                    {{
                        {WriteDefaultLengthString}
                    }}
                    else 
                    {{
                        {writeCommand}
                    }}
                    ";

            }
            else
                fullWriteCommands = writeCommand;
            return fullWriteCommands;
        }

        #endregion

        #region Supported tuples

        public void AppendSupportedTupleConversionMethods(CodeStringBuilder sb, HashSet<string> alreadyGenerated)
        {
            if (PropertyType != LazinatorPropertyType.SupportedTuple)
                return;

            if (alreadyGenerated.Contains(AppropriatelyQualifiedTypeNameEncodable))
                return;
            alreadyGenerated.Add(AppropriatelyQualifiedTypeNameEncodable);

            sb.Append($@"
                    private static {AppropriatelyQualifiedTypeName} {DirectConverterTypeNamePrefix}ConvertFromBytes_{AppropriatelyQualifiedTypeNameEncodable}(LazinatorMemory storage)
                    {{
                        {IIF(Nullable || IsSupportedTupleType, $@"if (storage.Length == 0)
                        {{
                            return default;
                        }}
                        ")}ReadOnlySpan<byte> span = storage.ReadOnlySpan;

                        int bytesSoFar = 0;
                        ");

            for (int i = 0; i < InnerProperties.Count; i++)
            {
                sb.Append(InnerProperties[i].GetSupportedTupleReadCommand("item" + (i + 1)));
                sb.AppendLine();
            }

            string itemnamesLowercase = String.Join(", ", Enumerable.Range(1, InnerProperties.Count).Select(x => "item" + x));
            sb.Append(
                    $@"
                        var tupleType = {(SupportedTupleType == LazinatorSupportedTupleType.ValueTuple ? "" : $"new {AppropriatelyQualifiedTypeNameWithoutNullableIndicator}")}({itemnamesLowercase});

                        return tupleType;
                    }}

                    private static void ConvertToBytes_{AppropriatelyQualifiedTypeNameEncodable}(ref BinaryBufferWriter writer, {AppropriatelyQualifiedTypeName} itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
                    {{
                    ");

            if (Nullable)
                sb.Append($@"if (itemToConvert == null)
                            {{
                                return;
                            }}
                            ");

            if (SupportedTupleType == LazinatorSupportedTupleType.KeyValuePair)
            {
                sb.Append(InnerProperties[0].GetSupportedTupleWriteCommand("Key", SupportedTupleType.Value, Nullable));
                sb.AppendLine();
                sb.Append(InnerProperties[1].GetSupportedTupleWriteCommand("Value", SupportedTupleType.Value, Nullable));
                sb.AppendLine();
            }
            else if (SupportedTupleType == LazinatorSupportedTupleType.RecordLikeType)
            {
                for (int i = 0; i < InnerProperties.Count; i++)
                {
                    sb.Append(InnerProperties[i].GetSupportedTupleWriteCommand(InnerProperties[i].PropertyName, SupportedTupleType.Value, Nullable));
                    sb.AppendLine();
                }
            }
            else for (int i = 0; i < InnerProperties.Count; i++)
                {
                    sb.Append(InnerProperties[i].GetSupportedTupleWriteCommand("Item" + (i + 1), SupportedTupleType.Value, Nullable));
                    sb.AppendLine();
                }

            sb.AppendLine($"}}");

            AppendSupportedTupleCloneMethod(sb);

            RecursivelyAppendConversionMethods(sb, alreadyGenerated);
        }

        private string GetSupportedTupleReadCommand(string itemName)
        {
            if (IsPrimitive)
                return ($@"
                        {AppropriatelyQualifiedTypeName} {itemName} = {EnumEquivalentCastToEnum}span.{ReadMethodName}(ref bytesSoFar);");
            else if (IsNonLazinatorType)
                return ($@"
                        {AppropriatelyQualifiedTypeName} {itemName}{DefaultInitializationIfPossible(AppropriatelyQualifiedTypeName)};
                        int lengthCollectionMember_{itemName} = span.ToInt32(ref bytesSoFar);
                        if (lengthCollectionMember_{itemName} != 0)
                        {{
                            LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember_{itemName});
                            {itemName} = {DirectConverterTypeNamePrefix}ConvertFromBytes_{AppropriatelyQualifiedTypeNameEncodable}(childData);
                        }}{IfInitializationRequiredAddElseThrow}
                        bytesSoFar += lengthCollectionMember_{itemName};");
            else
            {
                CheckForLazinatorInNonLazinator(this);
                if (PropertyType == LazinatorPropertyType.LazinatorStruct)
                    return ($@"
                        {AppropriatelyQualifiedTypeName} {itemName}{DefaultInitializationIfPossible(AppropriatelyQualifiedTypeName)};
                        int lengthCollectionMember_{itemName} = {GetSpanReadLength()};
                        if (lengthCollectionMember_{itemName} != 0)
                        {{
                            LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember_{itemName});
                            {itemName} = new {AppropriatelyQualifiedTypeNameWithoutNullableIndicator}();
                            {itemName}.DeserializeLazinator(childData);;
                        }}{IfInitializationRequiredAddElseThrow}
                        bytesSoFar += lengthCollectionMember_{itemName};");
                else
                {
                    if (NonNullableThatRequiresInitialization && !UseNullableBackingFieldsForNonNullableReferenceTypes)
                        return ($@"
                            int lengthCollectionMember_{itemName} = {GetSpanReadLength()};
                            LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember_{itemName});
                            {AppropriatelyQualifiedTypeName} {itemName} = DeserializationFactory.Instance.CreateBasedOnType<{AppropriatelyQualifiedTypeName}>(childData);
                            bytesSoFar += lengthCollectionMember_{itemName};");

                    return ($@"
                        {AppropriatelyQualifiedTypeName} {itemName}{DefaultInitializationIfPossible(AppropriatelyQualifiedTypeName)};
                        int lengthCollectionMember_{itemName} = {GetSpanReadLength()};
                        if (lengthCollectionMember_{itemName} != 0)
                        {{
                            LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember_{itemName});
                            {itemName} = DeserializationFactory.Instance.CreateBasedOnType<{AppropriatelyQualifiedTypeName}>(childData);
                        }}{IfInitializationRequiredAddElseThrow}
                        bytesSoFar += lengthCollectionMember_{itemName};");
                }
            }
        }

        private string GetSupportedTupleWriteCommand(string itemName, LazinatorSupportedTupleType outerTupleType, bool outerTypeIsNullable)
        {
            string itemToConvertItemName =
                $"itemToConvert{IIF((outerTupleType == LazinatorSupportedTupleType.ValueTuple || outerTupleType == LazinatorSupportedTupleType.KeyValuePair) && outerTypeIsNullable, ".Value")}.{itemName}";
            if (IsPrimitive)
                return ($@"
                        {WriteMethodName}(ref writer, {EnumEquivalentCastToEquivalentType}{itemToConvertItemName});");
            else if (IsNonLazinatorType)
            {
                if (Nullable)
                    return ($@"
                            if ({itemToConvertItemName} == null)
                            {{
                                {WriteDefaultLengthString}
                            }}
                            else
                            {{
                                void action{itemName}(ref BinaryBufferWriter w) => {DirectConverterTypeNamePrefix}ConvertToBytes_{AppropriatelyQualifiedTypeNameEncodable}(ref w, {itemToConvertItemName}, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                                WriteToBinaryWithIntLengthPrefix(ref writer, action{itemName});
                            }}");
                else return $@"
                            void action{itemName}(ref BinaryBufferWriter w) => {DirectConverterTypeNamePrefix}ConvertToBytes_{AppropriatelyQualifiedTypeNameEncodable}(ref w, {itemToConvertItemName}, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                            WriteToBinaryWithIntLengthPrefix(ref writer, action{itemName});";
            }
            else
            {
                if (PropertyType == LazinatorPropertyType.LazinatorStruct || PropertyType == LazinatorPropertyType.LazinatorStructNullable)
                    return ($@"
                        void action{itemName}(ref BinaryBufferWriter w) => {itemToConvertItemName}{IIF(PropertyType == LazinatorPropertyType.LazinatorStructNullable, "?")}.SerializeExistingBuffer(ref w, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                        WriteToBinaryWith{LengthPrefixTypeString}LengthPrefix(ref writer, action{itemName});");
                else
                    return ($@"
                        if ({itemToConvertItemName} == null)
                        {{
                            {WriteDefaultLengthString}
                        }}
                        else
                        {{
                            void action{itemName}(ref BinaryBufferWriter w) => {itemToConvertItemName}.SerializeExistingBuffer(ref w, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                            WriteToBinaryWith{LengthPrefixTypeString}LengthPrefix(ref writer, action{itemName});
                        }};");
            }
        }

        private void AppendSupportedTupleCloneMethod(CodeStringBuilder sb)
        {
            string[] itemStrings = null;
            switch (SupportedTupleType)
            {
                case LazinatorSupportedTupleType.KeyValuePair:
                    itemStrings = new string[] { "Key", "Value" };
                    break;
                case LazinatorSupportedTupleType.RecordLikeType:
                    itemStrings = InnerProperties.Select(x => x.PropertyName).ToArray();
                    break;
                case LazinatorSupportedTupleType.Tuple:
                case LazinatorSupportedTupleType.ValueTuple:
                    itemStrings = Enumerable.Range(1, InnerProperties.Count).Select(x => $"Item{x}").ToArray();
                    break;

            }
            // zip the inner properties and item strings, then get the clone string using each corresponding item, and then join into a string.
            string propertyAccess = Nullable ? "itemToConvert?." : "itemToConvert.";
            string innerClones = String.Join(",",
                InnerProperties
                    .Zip(
                        itemStrings,
                        (x, y) => new { InnerProperty = x, ItemString = "(" + propertyAccess + y + (Nullable && !x.Nullable ? " ?? default" : "") + ")" })
                    .Select(z => z.InnerProperty.GetCloneStringWithinCloneMethod(z.ItemString))
                );
            string creationText = SupportedTupleType == LazinatorSupportedTupleType.ValueTuple ? $"({innerClones})" : $"new {AppropriatelyQualifiedTypeNameWithoutNullableIndicator}({innerClones})";

            // because we have a single cloneOrChangeFunc for the ILazinator, we don't make the nullability item specific
            sb.Append($@"
                    private static {AppropriatelyQualifiedTypeName} CloneOrChange_{AppropriatelyQualifiedTypeNameEncodable}({AppropriatelyQualifiedTypeName} itemToConvert, Func<{ILazinatorString}, {ILazinatorString}> cloneOrChangeFunc, bool avoidCloningIfPossible)
                    {{
                        {IIF(Nullable, GetNullCheckIfThen("itemToConvert", $@"return {DefaultExpression};", ""))}{IIF(Nullable, $@"
                            ")}return {creationText};
                    }}
            ");
        }

        #endregion

        #region Interchange types

        public void AppendInterchangeTypes(CodeStringBuilder sb, HashSet<string> alreadyGenerated)
        {
            if (PropertyType != LazinatorPropertyType.NonLazinator || !HasInterchangeType)
                return;

            if (alreadyGenerated.Contains(AppropriatelyQualifiedTypeNameEncodable))
                return;
            alreadyGenerated.Add(AppropriatelyQualifiedTypeNameEncodable);

            sb.Append($@"
                   private static {AppropriatelyQualifiedTypeName} ConvertFromBytes_{AppropriatelyQualifiedTypeNameEncodable}(LazinatorMemory storage)
                        {{
                            {ConditionalCodeGenerator.ConsequentPossibleOnlyIf(Nullable || IsMemoryOrSpan || IsSupportedTupleType, "storage.Length == 0", $"return {DefaultExpression};")}{InterchangeTypeName} interchange = new {InterchangeTypeNameWithoutNullabilityIndicator}(storage);
                            return interchange.Interchange_{AppropriatelyQualifiedTypeNameEncodable}(false);
                        }}

                        private static void ConvertToBytes_{AppropriatelyQualifiedTypeNameEncodable}(ref BinaryBufferWriter writer,
                            {AppropriatelyQualifiedTypeName} itemToConvert, IncludeChildrenMode includeChildrenMode,
                            bool verifyCleanness, bool updateStoredBuffer)
                        {{
                            {GetNullCheckIfThen("itemToConvert", $@"return;", "")}
                            {InterchangeTypeName} interchange = new {InterchangeTypeNameWithoutNullabilityIndicator}(itemToConvert);
                            interchange.SerializeExistingBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                        }}


                        private static {AppropriatelyQualifiedTypeName} CloneOrChange_{AppropriatelyQualifiedTypeNameEncodable}({AppropriatelyQualifiedTypeName} itemToClone, Func<{ILazinatorString}, {ILazinatorString}> cloneOrChangeFunc, bool avoidCloningIfPossible)
                        {{
                            {GetNullCheckIfThenButOnlyIfNullable(Nullable, "itemToClone", $"return {DefaultExpression};", "")}{InterchangeTypeName} interchange = new {InterchangeTypeNameWithoutNullabilityIndicator}(itemToClone);
                            return interchange.Interchange_{AppropriatelyQualifiedTypeNameEncodable}(avoidCloningIfPossible ? false : true);
                        }}
                        ");
        }

        #endregion
    }
}

