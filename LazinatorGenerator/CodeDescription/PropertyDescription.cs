using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Lazinator.Attributes;
using LazinatorGenerator.AttributeClones;
using LazinatorGenerator.Settings;
using LazinatorGenerator.Support;
using LazinatorCodeGen.Roslyn;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static LazinatorCodeGen.Roslyn.RoslynHelpers;
using LazinatorGenerator.CodeDescription;

namespace Lazinator.CodeDescription
{
    public class PropertyDescription
    {
        #region Properties

        /* Type and object information */
        private LazinatorObjectDescription ContainingObjectDescription { get; set; }
        public LazinatorImplementingTypeInfo LazinatorCompilation => ContainingObjectDescription.ImplementingTypeInfo;
        private Compilation Compilation => LazinatorCompilation.Compilation;
        private bool ContainerIsClass => ContainingObjectDescription.ObjectType == LazinatorObjectType.Class && !ContainingObjectDescription.GeneratingRefStruct;
        private bool ContainerIsStruct => ContainingObjectDescription.ObjectType == LazinatorObjectType.Struct || ContainingObjectDescription.GeneratingRefStruct;
        private PropertyDescription ContainingPropertyDescription { get; set; }
        public PropertyDescription OutmostPropertyDescription => ContainingPropertyDescription?.OutmostPropertyDescription ?? this;
        private int UniqueIDForLazinatorType { get; set; }
        internal IPropertySymbol PropertySymbol { get; set; }
        private ITypeSymbol TypeSymbolIfNoProperty { get; set; }
        private ITypeSymbol Symbol => PropertySymbol != null ? (ITypeSymbol)PropertySymbol.Type : (ITypeSymbol)TypeSymbolIfNoProperty;
        internal bool GenericConstrainedToClass => Symbol is ITypeParameterSymbol typeParameterSymbol && typeParameterSymbol.HasReferenceTypeConstraint;
        internal bool GenericConstrainedToStruct => (Symbol is ITypeParameterSymbol typeParameterSymbol && typeParameterSymbol.HasValueTypeConstraint) || (PropertyType == LazinatorPropertyType.OpenGenericParameter && Symbol.IsValueType);
        internal string DerivationKeyword { get; set; }
        private bool IsAbstract { get; set; }
        public bool NullableModeEnabled => ContainingObjectDescription.NullableModeEnabled;
        public string QuestionMarkIfOutputNullableModeEnabled => OutputNullableModeEnabled ? "?" : "";
        public string QuestionMarkIfNullableAndNullableModeEnabled => Nullable && NullableModeEnabled ? "?" : "";
        public bool OutputNullableModeEnabled => NullableModeEnabled;
        public bool OutputNullableModeInherited => NullableModeEnabled;
        public string OutputQuestionMarkIfNullableModeEnabled => OutputNullableModeEnabled ? "?" : "";
        public string OutputQuestionMarkIfNullableAndNullableModeEnabled => Nullable && OutputNullableModeEnabled ? "?" : "";

        public string ILazinatorString => "ILazinator" + QuestionMarkIfOutputNullableModeEnabled;
        public string ILazinatorStringWithItemSpecificNullability => "ILazinator" + QuestionMarkIfNullableAndNullableModeEnabled;
        internal bool Nullable { get; set; }
        internal bool SymbolEndsWithQuestionMark => Symbol.ToString().EndsWith("?");
        internal bool ReferenceTypeReportedAsNullable => NullableModeEnabled ? SymbolEndsWithQuestionMark : true;
        internal bool ValueTypeReportedAsNullable => SymbolEndsWithQuestionMark;
        internal bool TypeReportedAsNullable => Symbol.IsValueType ? ValueTypeReportedAsNullable : ReferenceTypeReportedAsNullable;
        internal string NullForgiveness => NullableModeEnabled ? "!" : "";
        internal string NullForgivenessIfNonNullable => NullableModeEnabled && !Nullable ? "!" : "";
        private bool HasParameterlessConstructor => PropertySymbol.Type is INamedTypeSymbol namedTypeSymbol && namedTypeSymbol.InstanceConstructors.Any(y => !y.IsImplicitlyDeclared && !y.Parameters.Any());
        private bool IsInterface { get; set; }
        private int? ArrayRank { get; set; }
        internal bool IsDefinedInLowerLevelInterface { get; set; }
        internal bool IsLast { get; set; }
        private bool OmitLengthBecauseDefinitelyLast => (IsLast && ContainingObjectDescription.IsSealedOrStruct && ContainingObjectDescription.Version == -1);
        private string ChildSliceString => $"GetChildSlice(LazinatorMemoryStorage, {BackingFieldByteIndex}, {BackingFieldByteLength}{ChildSliceLastParametersString})"; 
        private string ChildSliceStringMaybeAsync(bool couldBeAsync = true) => couldBeAsync && WithinAsync ? $"{ContainingObjectDescription.MaybeAwaitWord}GetChildSlice{ContainingObjectDescription.MaybeAsyncWord}(LazinatorMemoryStorage, {BackingFieldByteIndex}, {BackingFieldByteLength}{ChildSliceLastParametersString})" : ChildSliceString;
        private string ChildSliceStringDefinitelyAsync => $"await GetChildSliceAsync(LazinatorMemoryStorage, {BackingFieldByteIndex}, {BackingFieldByteLength}{ChildSliceLastParametersString})";

        private bool AllLengthsPrecedeChildren => true;
        public bool SkipLengthForThisProperty => IsGuaranteedFixedLength || OmitLengthBecauseDefinitelyLast;
        public bool UsesLengthValue => AllLengthsPrecedeChildren && !SkipLengthForThisProperty && IsLazinator;
        private string ChildSliceLastParametersString => $", {(IsGuaranteedFixedLength ? $"{FixedLength}" : "null")}";
        internal string IncrementChildStartBySizeOfLength => OmitLengthBecauseDefinitelyLast || IsGuaranteedFixedLength ? "" : (SingleByteLength ? " + sizeof(byte)" : " + sizeof(int)");
        internal string DecrementTotalLengthBySizeOfLength => OmitLengthBecauseDefinitelyLast || IsGuaranteedFixedLength ? "" : (SingleByteLength ? " - sizeof(byte)" : " - sizeof(int)");

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
        internal bool IsMemory => PropertyType == LazinatorPropertyType.SupportedCollection &&
                                        (SupportedCollectionType == LazinatorSupportedCollectionType.Memory);
        internal bool IsMemoryOrSpan => PropertyType == LazinatorPropertyType.SupportedCollection &&
                                        (SupportedCollectionType == LazinatorSupportedCollectionType.Memory ||
                                        SupportedCollectionType ==
                                        LazinatorSupportedCollectionType.ReadOnlyMemory ||
                                        SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlySpan);
        internal bool IsReadOnlySpan => PropertyType == LazinatorPropertyType.SupportedCollection &&
                                        SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlySpan;
        internal bool IsNonGenericLazinator => PropertyType == LazinatorPropertyType.LazinatorClassOrInterface || PropertyType == LazinatorPropertyType.LazinatorNonnullableClassOrInterface || PropertyType == LazinatorPropertyType.LazinatorStruct || PropertyType == LazinatorPropertyType.LazinatorStructNullable;
        internal bool IsLazinator => IsNonGenericLazinator || PropertyType == LazinatorPropertyType.OpenGenericParameter;

        internal INamedTypeSymbol ExclusiveInterfaceSymbol => IsNonGenericLazinator ? LazinatorCompilation.GetExclusiveInterface(Symbol as INamedTypeSymbol) : null;

        internal bool TypeHasLazinatorAsyncAttribute => ExclusiveInterfaceSymbol?.HasAttributeOfType<CloneAsyncLazinatorMemoryAttribute>() ?? false;
        internal bool TypeImplementsILazinatorAsync => Symbol.Name == "Lazinator.Core.ILazinatorAsync" || ((Symbol as INamedTypeSymbol)?.AllInterfaces.Any(x => x.Name == "Lazinator.Core.ILazinatorAsync") ?? false);
        internal bool AsyncWithinAsync => WithinAsync && TypeImplementsILazinatorAsync;
        internal bool IsSupportedCollectionOrTuple => PropertyType == LazinatorPropertyType.SupportedCollection || PropertyType == LazinatorPropertyType.SupportedTuple;

        internal bool IsSupportedCollectionReferenceType => PropertyType == LazinatorPropertyType.SupportedCollection && SupportedCollectionType != LazinatorSupportedCollectionType.Memory && SupportedCollectionType != LazinatorSupportedCollectionType.ReadOnlyMemory && SupportedCollectionType != LazinatorSupportedCollectionType.ReadOnlySpan;
        internal bool IsSupportedTupleReferenceType => PropertyType == LazinatorPropertyType.SupportedTuple && SupportedTupleType == LazinatorSupportedTupleType.Tuple;

        internal bool IsSupportedReferenceType => IsSupportedCollectionReferenceType || IsSupportedTupleReferenceType;
        internal bool IsSupportedValueType => IsSupportedCollectionOrTuple && !IsSupportedReferenceType;
        internal bool IsNonNullableReferenceType => !Nullable && (
            PropertyType == LazinatorPropertyType.LazinatorNonnullableClassOrInterface
            || IsSupportedReferenceType);
        internal bool IsNonNullableValueTypeWithNonNullableReferenceType => !Nullable && !IsNonNullableReferenceType && InnerProperties != null && InnerProperties.Any(x => x.IsNonNullableReferenceType); // note: this may actually not matter, since a value type can always be initialized with "default" without generating a compiler error, even if it contains a nonnullable type. In principle, we could still try to catch these in some circumstances and throw UnsetLazinator errors. But if the compiler doesn't give a warning, perhaps we ought not as well. Thus, we omit using this as a basis for NonNullableThatRequiresInitialization.
        internal bool IsNonNullableRecordLikeTypeInNullableEnabledContext => !Nullable && PropertyType == LazinatorPropertyType.SupportedTuple && SupportedTupleType == LazinatorSupportedTupleType.RecordLikeType && OutputNullableModeEnabled;
        internal bool NonNullableThatRequiresInitialization => IsNonNullableReferenceType || IsNonNullableRecordLikeTypeInNullableEnabledContext; // || IsNonNullableValueTypeWithNonNullableReferenceType; (see above for explanation for why this is commented out.)
        internal bool NonNullableThatCanBeUninitialized => !Nullable && !NonNullableThatRequiresInitialization;
        public static bool UseNullableBackingFieldsForNonNullableReferenceTypes => false; // if TRUE, then we use a null backing field and add checks for PossibleUnsetException. If FALSE, then we don't do that, and instead we set the backing field in every constructor.
        internal bool AddQuestionMarkInBackingFieldForNonNullable => NullableModeEnabled && UseNullableBackingFieldsForNonNullableReferenceTypes && NonNullableThatRequiresInitialization;
        internal bool IsNonNullableWithNonNullableBackingField => NullableModeEnabled && !UseNullableBackingFieldsForNonNullableReferenceTypes && NonNullableThatRequiresInitialization;
        internal bool NullForgivenessInsteadOfPossibleUnsetException => NullableModeEnabled && !UseNullableBackingFieldsForNonNullableReferenceTypes && NonNullableThatRequiresInitialization;
        internal string BackingFieldStringOrContainedSpan(string propertyName) => (SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlySpan) ?
                    GetReadOnlySpanBackingFieldCast(propertyName) : (propertyName ?? BackingFieldString);
        internal string PossibleUnsetException => $"{IIF(AddQuestionMarkInBackingFieldForNonNullable, $" ?? throw new UnsetNonnullableLazinatorException()")}{NullableForgivenessIfNeeded}";
        internal string NullableForgivenessIfNeeded => $"{IIF(NullForgivenessInsteadOfPossibleUnsetException, "!")}";
        internal string NullableForgivenessIfPossiblyNeeded => $"{IIF(NullableModeEnabled, "!")}";
        internal string PossibleUnsetExceptionWithForgivenessWithinNotNull => $"{IIF(AddQuestionMarkInBackingFieldForNonNullable, $" ?? throw new UnsetNonnullableLazinatorException()")}{IIF(NullableModeEnabled && !AddQuestionMarkInBackingFieldForNonNullable, "!")}";
        internal string DefaultInitializationIfPossible(string defaultType) => $"{IIF(!AddQuestionMarkInBackingFieldForNonNullable, $" = default{IIF(defaultType != null, $"({defaultType})")}")}";
        string elseThrowString = $@"
                else 
                {{ 
                    throw new UnsetNonnullableLazinatorException(); 
                }}";
        internal string IfInitializationRequiredAddElseThrow => $"{IIF(AddQuestionMarkInBackingFieldForNonNullable, elseThrowString)}";
        internal string BackingFieldAccessWithPossibleException => $"{BackingFieldString}{PossibleUnsetException}";
        internal string BackingFieldStringOrContainedSpanWithPossibleException(string propertyName) => $"{BackingFieldStringOrContainedSpan(propertyName)}{PossibleUnsetException}";
        internal string BackingFieldWithPossibleValueDereference => $"{BackingFieldString}{IIF(PropertyType == LazinatorPropertyType.LazinatorStructNullable || (PropertyType == LazinatorPropertyType.LazinatorStructNullable || (IsDefinitelyStruct && Nullable)), $@".Value")}";
        internal string BackingFieldWithPossibleValueDereferenceWithPossibleException => $"{BackingFieldString}{IIF(PropertyType == LazinatorPropertyType.LazinatorStructNullable, $@".Value")}{PossibleUnsetException}";
        internal bool IsSupportedCollectionOrTupleOrNonLazinatorWithInterchangeType => IsSupportedCollectionOrTuple || (PropertyType == LazinatorPropertyType.NonLazinator && HasInterchangeType);
        internal bool IsNotPrimitiveOrOpenGeneric => PropertyType != LazinatorPropertyType.OpenGenericParameter && PropertyType != LazinatorPropertyType.PrimitiveType && PropertyType != LazinatorPropertyType.PrimitiveTypeNullable;
        internal bool IsNonLazinatorType => PropertyType == LazinatorPropertyType.NonLazinator || PropertyType == LazinatorPropertyType.SupportedCollection || PropertyType == LazinatorPropertyType.SupportedTuple;
        internal bool IsNonLazinatorTypeWithoutInterchange => PropertyType == LazinatorPropertyType.NonLazinator && !HasInterchangeType;
        internal string ConstructorInitialization => IIF(PropertyType != LazinatorPropertyType.LazinatorStruct && PropertyType != LazinatorPropertyType.LazinatorStructNullable && !NonSerializedIsStruct, "IncludeChildrenMode.IncludeAllChildren");
        internal string ConstructorInitializationWithChildData
        {
            get
            {
                string basicInitialization = ConstructorInitialization;
                if (basicInitialization == "" || basicInitialization == "IncludeChildrenMode.IncludeAllChildren")
                    return "childData";
                return $"{basicInitialization}, childData";
            }
        }

        /* Names */
        private bool UseFullyQualifiedNames => (Config?.UseFullyQualifiedNames ?? false) || HasFullyQualifyAttribute || Symbol.ContainingType != null;
        private string ShortTypeName() => RegularizeTypeName(Symbol.GetMinimallyQualifiedName(OutputNullableModeEnabled));
        private string ShortTypeNameWithoutNullableIndicator => WithoutNullableIndicator(ShortTypeName());
        internal string FullyQualifiedTypeName => RegularizeTypeName(Symbol.GetFullyQualifiedName(OutputNullableModeEnabled));
        private string FullyQualifiedNameWithoutNullableIndicator => WithoutNullableIndicator(FullyQualifiedTypeName);
        internal string AppropriatelyQualifiedTypeNameHelper()
        {
            string typeName = UseFullyQualifiedNames ? FullyQualifiedTypeName : ShortTypeName();
            if (typeName.EndsWith("?"))
                return typeName;
            if (OutputNullableModeEnabled && Nullable)
                return typeName + "?";
            return typeName;
        }
        internal string AppropriatelyQualifiedTypeName => AppropriatelyQualifiedTypeNameHelper();

        public string DefaultExpression => PropertyType switch { LazinatorPropertyType.LazinatorStructNullable => "null", LazinatorPropertyType.LazinatorClassOrInterface => "null", LazinatorPropertyType.LazinatorNonnullableClassOrInterface => "irrelevant /* won't end up in code */", _ => $"default({AppropriatelyQualifiedTypeName})" };
        private string AppropriatelyQualifiedTypeNameWithoutNullableIndicator => UseFullyQualifiedNames ? FullyQualifiedNameWithoutNullableIndicator : ShortTypeNameWithoutNullableIndicator;

        private string AppropriatelyQualifiedTypeNameWithoutNullableIndicatorIfNonnullableReferenceType => PropertyType == LazinatorPropertyType.LazinatorNonnullableClassOrInterface ? AppropriatelyQualifiedTypeNameWithoutNullableIndicator : AppropriatelyQualifiedTypeName;

        internal string ShortTypeNameEncodable => Symbol.GetEncodableVersionOfIdentifier(false);
        private string ShortTypeNameEncodableWithoutNullable => (Symbol as INamedTypeSymbol).TypeArguments[0].GetEncodableVersionOfIdentifier(false);
        internal string FullyQualifiedTypeNameEncodable => Symbol.GetEncodableVersionOfIdentifier(true);
        private string FullyQualifiedTypeNameEncodableWithoutNullable => (Symbol as INamedTypeSymbol).TypeArguments[0].GetEncodableVersionOfIdentifier(true);
        internal string AppropriatelyQualifiedTypeNameEncodable => Symbol.GetEncodableVersionOfIdentifier(UseFullyQualifiedNames);
        private string AppropriatelyQualifiedTypeNameEncodableWithoutNullable => (Symbol as INamedTypeSymbol).TypeArguments[0].GetEncodableVersionOfIdentifier(UseFullyQualifiedNames);

        public string Namespace => (IsDefinitelyStruct && Nullable) ? ((INamedTypeSymbol)Symbol).TypeArguments[0].GetFullNamespace() : Symbol.GetFullNamespace();
        private string WriteMethodName { get; set; }
        private string ReadMethodName { get; set; }
        internal string PropertyName { get; set; }
        internal string PropertyNameForConstructorParameter => char.ToLower(PropertyName[0]) + PropertyName.Substring(1);
        internal string PropertyNameWithTypeNameForConstructorParameter => $"{AppropriatelyQualifiedTypeName} {PropertyNameForConstructorParameter}";
        internal string AssignParameterToBackingField => $@"{BackingFieldString} = {PropertyNameForConstructorParameter};
                            ";

        internal string BackingFieldString => $"_{PropertyName}";

        internal bool BackingAccessFieldIncluded => PlaceholderMemoryWriteMethod == null && !IsNonNullableWithNonNullableBackingField && !IsNonNullableRecordLikeTypeInNullableEnabledContext;
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
        /* Async */
        private bool WithinAsync => ContainingObjectDescription.AsyncLazinatorMemory;
        private string AsyncIfNeeded(bool async) => async && WithinAsync ? "Async" : "";
        private string AwaitIfNeeded(bool async) => async && WithinAsync ? "await " : "";

        /* Inner properties */
        public List<PropertyDescription> InnerProperties { get; set; }
        private bool ContainsOpenGenericInnerProperty => InnerProperties != null && InnerProperties.Any(x => x.PropertyType == LazinatorPropertyType.OpenGenericParameter || x.ContainsOpenGenericInnerProperty);
        private bool ContainsLazinatorInnerProperty => InnerProperties != null && InnerProperties.Any(x => x.PropertyType == LazinatorPropertyType.LazinatorClassOrInterface || x.PropertyType == LazinatorPropertyType.LazinatorNonnullableClassOrInterface || x.ContainsLazinatorInnerProperty);
        internal string NullableStructValueAccessor => IIF(PropertyType == LazinatorPropertyType.LazinatorStructNullable, ".Value");

        private bool InitializeRecordLikeTypePropertiesDirectly { get; set; }

        /* Conversion */
        private string InterchangeTypeName { get; set; }
        private string InterchangeTypeNameWithoutNullabilityIndicator => WithoutNullableIndicator(InterchangeTypeName);
        private bool NonSerializedIsStruct { get; set; }
        private string DirectConverterTypeName { get; set; }
        internal string DirectConverterTypeNamePrefix => DirectConverterTypeName == "" || DirectConverterTypeName == null ? "" : DirectConverterTypeName + ".";
        private bool HasInterchangeType => InterchangeTypeName != null;

        /* Attributes */
        private IEnumerable<Attribute> UserAttributes => ContainingObjectDescription.ImplementingTypeInfo.GetAttributes(PropertySymbol);
        internal int RelativeOrder { get; set; }
        internal bool HasFullyQualifyAttribute => UserAttributes.OfType<CloneFullyQualifyAttribute>().Any();
        internal bool HasEagerAttribute => UserAttributes.OfType<CloneEagerAttribute>().Any();
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
        internal bool Uncompressed { get; set; }
        public string SkipCondition { get; set; }
        public string InitializeWhenSkipped { get; set; }
        internal bool TrackDirtinessNonSerialized { get; set; }
        private ConditionsCodeGenerator ReadInclusionConditional { get; set; }
        private ConditionsCodeGenerator WriteInclusionConditional { get; set; }
        private ConditionsCodeGenerator ReadInclusionConditionalOptions { get; set; }
        private ConditionsCodeGenerator WriteInclusionConditionalOptions { get; set; }
        private string CodeBeforeSet { get; set; }
        private string CodeAfterSet { get; set; }
        private string CodeOnDeserialized { get; set; }
        private string CodeOnAccessed { get; set; }
        private int SizeOfLength { get; set; } = sizeof(int);
        private string SizeOfLengthIfIncludedString => AllLengthsPrecedeChildren || SkipLengthForThisProperty ? "SizeOfLength.SkipLength" : SizeOfLengthString;
        private string SizeOfLengthString => BytesUsedForLength() switch
        {
            0 => "SizeOfLength.SkipLength",
            1 => "SizeOfLength.Byte",
            2 => "SizeOfLength.Int16",
            4 => "SizeOfLength.Int32",
            8 => "SizeOfLength.Int64",
            _ => "SizeOfLength.Int32"
        };
        private string SizeOfLengthTypeString => BytesUsedForLength() switch
        {
            0 => "",
            1 => "byte",
            2 => "Int16",
            4 => "int",
            8 => "int64",
            _ => "int"
        };
        private string SizeOfLengthTypeStringCaps => BytesUsedForLength() switch
        {
            0 => "",
            1 => "Byte",
            2 => "Int16",
            4 => "Int32",
            8 => "Int64",
            _ => "Int32"
        };
        private bool IsGuaranteedFixedLength { get; set; }
        private int FixedLength { get; set; }
        private bool SingleByteLength { get; set; }
        private string LengthPrefixTypeString => IsGuaranteedFixedLength ? "out" : SizeOfLengthTypeStringCaps;
        private string WriteDefaultLengthString =>
            !IsGuaranteedFixedLength || FixedLength == 1 ?
                $"writer.Write(({(SingleByteLength || IsGuaranteedFixedLength ? "byte" : "int")})0);"
            :
                $@"for (int indexInFixedLength = 0; indexInFixedLength < {FixedLength}; indexInFixedLength++)
                    {{
                        writer.Write((byte)0);
                    }}";

        private bool IncludableWhenExcludingMostChildren { get; set; }
        private bool ExcludableWhenIncludingMostChildren { get; set; }
        private bool AllowLazinatorInNonLazinator { get; set; }

        /* Output customization */
        public LazinatorConfig? Config => ContainingObjectDescription.Config;
        private string StepThroughPropertiesString => IIF(ContainingObjectDescription.StepThroughProperties, $@"
                        [DebuggerStepThrough]");
        private string ConfirmDirtinessConsistencyCheck => $@"
                            LazinatorUtilities.ConfirmDescendantDirtinessConsistency(this);";
        private string RepeatedCodeExecution => ""; // change to expose some action that should be repeated before and after each variable is set.
        private string IIF(bool x, string y) => x ? y : ""; // Include if function
        private string IIFELSE(bool x, string y, string z) => x ? y : z; 
        private string IIF(bool x, Func<string> y) => x ? y() : ""; // Same but with a function to produce the string

        #endregion

        #region Constructors

        public PropertyDescription()
        {

        }

        public PropertyDescription(IPropertySymbol propertySymbol, LazinatorObjectDescription container, string derivationKeyword, string propertyAccessibility, bool isLast)
        {
            PropertySymbol = propertySymbol;
            IsAbstract = PropertySymbol.Type.IsAbstract;
            ContainingObjectDescription = container;
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
                throw new LazinatorCodeGenException($"ILazinator interface property {PropertyName} in {ContainingObjectDescription?.NameIncludingGenerics} should omit the set because the SetterAccessibilityAttribute specifies non-public accessibility.");

            ParseVersionAttributes();

            ParseOtherPropertyAttributes();

            SetPropertyType(propertySymbol.Type as ITypeSymbol);

            SetReadAndWriteMethodNames();

            SetInclusionConditionals();
        }


        public PropertyDescription(ITypeSymbol typeSymbol, LazinatorObjectDescription containingObjectDescription, PropertyDescription containingPropertyDescription, string propertyName = null)
        {
            // This is only used for defining the type on the inside of the generics, plus underlying type for arrays.
            TypeSymbolIfNoProperty = typeSymbol;
            ContainingObjectDescription = containingObjectDescription;
            ContainingPropertyDescription = containingPropertyDescription;
            PropertyName = propertyName;
            IsAbstract = typeSymbol.IsAbstract;
            SetPropertyType(typeSymbol);
            SetReadAndWriteMethodNames();
            //Debug.WriteLine($"{AppropriatelyQualifiedTypeName} {PropertyName} Containing object {containingObjectDescription.NameIncludingGenerics} Containing property {containingPropertyDescription?.PropertyName} Nullable contexts: {nullableContextSetting}, {outputNullableContextSetting}");
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
            CloneUncompressedAttribute uncompressed = UserAttributes.OfType<CloneUncompressedAttribute>().FirstOrDefault();
            Uncompressed = uncompressed != null;
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
            ReadInclusionConditional = InclusionConditionalHelper(true, false);
            WriteInclusionConditional = InclusionConditionalHelper(false, false);
            ReadInclusionConditionalOptions = InclusionConditionalHelper(true, true);
            WriteInclusionConditionalOptions = InclusionConditionalHelper(false, true);
        }

        public ConditionsCodeGenerator InclusionConditionalHelper(bool readVersion, bool optionsVariable)
        {
            string versionNumberVariable = readVersion ? "serializedVersionNumber" : "LazinatorObjectVersion";
            string includeChildrenModeVariable = optionsVariable ? "options.IncludeChildrenMode" : "includeChildrenMode";
            List<string> conditions = new List<string>();
            if (BackingAccessFieldIncluded)
            {
                if (PropertyType == LazinatorPropertyType.LazinatorStruct || PropertyType == LazinatorPropertyType.LazinatorStructNullable || PropertyType == LazinatorPropertyType.LazinatorClassOrInterface || PropertyType == LazinatorPropertyType.LazinatorNonnullableClassOrInterface || PropertyType == LazinatorPropertyType.OpenGenericParameter)
                {
                    conditions.Add($"{includeChildrenModeVariable} != IncludeChildrenMode.ExcludeAllChildren");
                    if (!IncludableWhenExcludingMostChildren)
                        conditions.Add($"{includeChildrenModeVariable} != IncludeChildrenMode.IncludeOnlyIncludableChildren");
                    if (ExcludableWhenIncludingMostChildren)
                        conditions.Add($"{includeChildrenModeVariable} != IncludeChildrenMode.ExcludeOnlyExcludableChildren");
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
                if (GenericConstrainedToStruct || (NullableModeEnabled && typeSymbol.NullableAnnotation == NullableAnnotation.NotAnnotated))
                    Nullable = false;
                else
                    Nullable = true;
                PropertyType = LazinatorPropertyType.OpenGenericParameter;
                if (!ContainingObjectDescription.IsSealedOrStruct)
                    DerivationKeyword = "virtual ";
                SizeOfLength = 4;
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
                if (typeSymbol.Name == "string?" || typeSymbol.Name == "String?")
                    Nullable = true;
                else if (typeSymbol.Name == "string" || typeSymbol.Name == "String")
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
            Nullable = NullableModeEnabled ? SymbolEndsWithQuestionMark : t.TypeKind == TypeKind.Class || IsNullableGeneric(t);
            PropertyType = LazinatorPropertyType.NonLazinator;
            NonSerializedIsStruct = t.IsValueType;
            InterchangeTypeName = Config?.GetInterchangeConverterTypeName(t);
            DirectConverterTypeName = Config?.GetDirectConverterTypeName(t);
            string fullyQualifiedTypeName = t.GetFullyQualifiedNameWithoutGlobal(NullableModeEnabled);
            if (InterchangeTypeName != null && DirectConverterTypeName != null)
                throw new LazinatorCodeGenException($"{fullyQualifiedTypeName} has both an interchange converter and a direct converter type listed. Only one should be used.");
            if (InterchangeTypeName == null && DirectConverterTypeName == null)
            {
                if (fullyQualifiedTypeName == "Lazinator.Core.ILazinator" || fullyQualifiedTypeName == "Lazinator.Core.ILazinator?")
                    throw new LazinatorCodeGenException($"You cannot include ILazinator as a type to be serialized or as a type argument in a Lazinator interface. (The reason for this is that some Lazinator types do not serialize their ID.) To define a property that can deserialize a large number of Lazinator types, create a nonexclusive interface (possibly implementing no properties) and then define your Lazinator types as implementing that interface. This nonexclusive interface can then be referenced in a property as a type to serialized, and you can set that property to any type implementing the interface.");
                else
                {
                    if (t.TypeKind == TypeKind.Interface)
                        throw new LazinatorCodeGenException($"Error generating {PropertyName} in {ContainingObjectDescription.SimpleName}: {fullyQualifiedTypeName} is a non-Lazinator type or interface. Define the NonexclusiveLazinator attribute on the interface and make sure that the interface inherits from ILazinator. Alternatively, to use it as a type for a Lazinator property, you must either make it a Lazinator type or use a LazinatorConfig.json file to specify either an interchange converter (i.e., a Lazinator object accept the non-Lazinator type as a parameter in its constructor) or a direct converter for it.") ;
                    else
                        throw new LazinatorCodeGenException($"Error generating {PropertyName} in {ContainingObjectDescription.SimpleName}: {fullyQualifiedTypeName} is a non-Lazinator type or interface. To use it as a type for a Lazinator property, you must either make it a Lazinator type or use a LazinatorConfig.json file to specify either an interchange converter (i.e., a Lazinator object accept the non-Lazinator type as a parameter in its constructor) or a direct converter for it. Alternatively, if there is a constructor whose parameters match public properties (not fields) of the type, it can be handled automatically, depending on the LazinatorConfig.json settings. ");
                }
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
            SetPropertyType(namedTypeSymbol.TypeArguments[0]);
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
                    var exclusiveInterface = LazinatorImplementingTypeInfo.NameTypedSymbolFromString[ContainingObjectDescription.ImplementingTypeInfo.TypeToExclusiveInterface[LazinatorImplementingTypeInfo.TypeSymbolToString(t.OriginalDefinition)]];
                    CloneLazinatorAttribute attribute = ContainingObjectDescription.ImplementingTypeInfo.GetFirstAttributeOfType<CloneLazinatorAttribute>(exclusiveInterface); // we already know that the interface exists, and there should be only one
                    if (attribute == null)
                        throw new LazinatorCodeGenException(
                            "Lazinator attribute is required for each interface implementing ILazinator, including inherited interfaces.");
                    UniqueIDForLazinatorType = attribute.UniqueID;

                    CloneFixedLengthLazinatorAttribute fixedLengthAttribute =
                        ContainingObjectDescription.ImplementingTypeInfo.GetFirstAttributeOfType<CloneFixedLengthLazinatorAttribute>(exclusiveInterface);
                    if (fixedLengthAttribute != null)
                    {
                        IsGuaranteedFixedLength = true;
                        SizeOfLength = 0;
                        FixedLength = fixedLengthAttribute.FixedLength;
                    }

                    // Note: This is for Lazinators only. Nonlazinators will always have length 4 (for int).
                    CloneSizeOfLengthAttribute sizeOfLengthAttribute = ContainingObjectDescription.ImplementingTypeInfo.GetFirstAttributeOfType<CloneSizeOfLengthAttribute>(exclusiveInterface);
                    if (sizeOfLengthAttribute == null)
                        SizeOfLength = 4;
                    else
                    {
                        SizeOfLength = sizeOfLengthAttribute.SizeOfLength;
                        if (SizeOfLength == 1)
                            SingleByteLength = true;
                        if (SizeOfLength == 0 && !IsGuaranteedFixedLength)
                            throw new Exception("SizeOfLengthAttribute should not have SizeOfLength set to 0 unless a FixedLengthAttribute is also included.");
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
                throw new LazinatorCodeGenException($"Unexpected exception. SetLazinatorPropertyType failed on property {t.ToString()} of {ContainingObjectDescription.NameIncludingGenerics}; message {ex.Message}");
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
                new PropertyDescription(t.ElementType, ContainingObjectDescription, this)
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
            var originalDefinition = t.OriginalDefinition; // if defined as MyClass?, then we want just MyClass
            // We look for a record-like type only after we have determined that the type does not implement ILazinator and we don't have the other supported tuple types (e.g., ValueTuples, KeyValuePair). We need to make sure that for each parameter in the constructor with the most parameters, there is a unique property with the same name (case insensitive as to first letter). If so, we assume that this property corresponds to the parameter, though there is no inherent guarantee that this is true. 
            var recordLikeTypes = ContainingObjectDescription.ImplementingTypeInfo.RecordLikeTypes;
            if (recordLikeTypes == null || !recordLikeTypes.ContainsKey(LazinatorImplementingTypeInfo.TypeSymbolToString(originalDefinition)) || ((Config?.IgnoreRecordLikeTypes != null) && (Config?.IgnoreRecordLikeTypes.Any(x => x.ToUpper() == (UseFullyQualifiedNames ? t.GetFullyQualifiedNameWithoutGlobal(NullableModeEnabled).ToUpper() : originalDefinition.GetMinimallyQualifiedName(NullableModeEnabled))) ?? false)))
            {
                return false;
            }

            TypeSymbolIfNoProperty = t;
            PropertyType = LazinatorPropertyType.SupportedTuple;
            SupportedTupleType = LazinatorSupportedTupleType.RecordLikeType;
            Nullable = TypeReportedAsNullable;

            List<(IParameterSymbol parameterSymbol, IPropertySymbol property)> recordLikeType = recordLikeTypes[LazinatorImplementingTypeInfo.TypeSymbolToString(originalDefinition)];
            if (recordLikeType.Any(x => x.parameterSymbol == null))
                InitializeRecordLikeTypePropertiesDirectly = true;
            InnerProperties = recordLikeType
                .Select(x => GetPropertyDescriptionForPropertyDefinedElsewhere(x.property.Type, ContainingObjectDescription, this, x.property.Name)).ToList();
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

        public PropertyDescription GetPropertyDescriptionForPropertyDefinedElsewhere(ITypeSymbol typeSymbol, LazinatorObjectDescription containingObjectDescription, PropertyDescription containingPropertyDescription, string propertyName)
        {
            // see if the property has already been defined (in case this is a recursive hierarchy)
            foreach (PropertyDescription pd in ContainingPropertyHierarchy())
                if (SymbolEqualityComparer.Default.Equals(pd.TypeSymbolIfNoProperty, typeSymbol))
                    throw new LazinatorCodeGenException($"The type {typeSymbol} is recursively defined. Recursive record-like types are not supported by Lazinator.");
            // If the typeSymbol is "string" and the nullable context is disabled, then it should actually be nullable. 
            return new PropertyDescription(typeSymbol, containingObjectDescription, containingPropertyDescription, propertyName);
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
                LazinatorSupportedTupleType.RecordLikeType => SymbolEndsWithQuestionMark,
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
                            .Select(x => new PropertyDescription(x, ContainingObjectDescription, this)).ToList();
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
                LazinatorSupportedCollectionType.Array => ReferenceTypeReportedAsNullable,
                LazinatorSupportedCollectionType.List => ReferenceTypeReportedAsNullable,
                LazinatorSupportedCollectionType.HashSet => ReferenceTypeReportedAsNullable,
                LazinatorSupportedCollectionType.Dictionary => ReferenceTypeReportedAsNullable,
                LazinatorSupportedCollectionType.Queue => ReferenceTypeReportedAsNullable,
                LazinatorSupportedCollectionType.Stack => ReferenceTypeReportedAsNullable,
                LazinatorSupportedCollectionType.SortedDictionary => ReferenceTypeReportedAsNullable,
                LazinatorSupportedCollectionType.SortedList => ReferenceTypeReportedAsNullable,
                LazinatorSupportedCollectionType.LinkedList => ReferenceTypeReportedAsNullable,
                LazinatorSupportedCollectionType.SortedSet => ReferenceTypeReportedAsNullable,
                LazinatorSupportedCollectionType.Memory => SymbolEndsWithQuestionMark,
                LazinatorSupportedCollectionType.ReadOnlySpan => SymbolEndsWithQuestionMark,
                LazinatorSupportedCollectionType.ReadOnlyMemory => SymbolEndsWithQuestionMark,
                _ => throw new NotImplementedException(),
            };
        }

        private void SetSupportedCollectionInnerProperties(INamedTypeSymbol t)
        {
            InnerProperties = t.TypeArguments
                            .Select(x => new PropertyDescription(x, ContainingObjectDescription, this)).ToList();

            if (SupportedCollectionType == LazinatorSupportedCollectionType.Memory || SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlyMemory || SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlySpan)
            {
                if (InnerProperties[0].Nullable)
                    throw new LazinatorCodeGenException("Cannot use Lazinator to serialize Memory/Span with nullable generic arguments."); // this is because we can't cast easily in this context
            }

            if (SupportedCollectionType == LazinatorSupportedCollectionType.Dictionary || SupportedCollectionType == LazinatorSupportedCollectionType.SortedDictionary || SupportedCollectionType == LazinatorSupportedCollectionType.SortedList)
            {
                // We process a Dictionary by treating it as a collection with KeyValuePairs. Thus, we must change to a single inner property of type KeyValuePair, which in turn has two inner properties equal to the properties of the type in our actual dictionary.
                if (keyValuePairType == null)
                    keyValuePairType = Compilation.GetTypeByMetadataName(typeof(KeyValuePair<,>).FullName);
                INamedTypeSymbol constructed = keyValuePairType.Construct(t.TypeArguments.ToArray());
                var replacementInnerProperty = new PropertyDescription(constructed, ContainingObjectDescription, this); // new PropertyDescription("System.Collections.Generic", "KeyValuePair", t.TypeArguments, ContainingObjectDescription);
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


        public void AppendPropertyDefinitionString(CodeStringBuilder sb, bool includeTracingCode)
        {
            if (ContainingObjectDescription.IsAbstract)
                AppendAbstractPropertyDefinitionString(sb);
            else if (PropertyType == LazinatorPropertyType.PrimitiveType || PropertyType == LazinatorPropertyType.PrimitiveTypeNullable)
                AppendPrimitivePropertyDefinitionString(sb);
            else
                AppendNonPrimitivePropertyDefinitionString(sb, includeTracingCode);
        }

        private void AppendAbstractPropertyDefinitionString(CodeStringBuilder sb)
        {
            if (SetterAccessibilityString == "private ")
                throw new LazinatorCodeGenException($"Property {PropertyName} is in abstract class {ContainingObjectDescription.FullyQualifiedObjectName} and so cannot have a private setter.");
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
                    {SetterAccessibilityString}set;
                }}
";
            sb.Append(propertyString);
        }

        private string GetModifiedDerivationKeyword()
        {
            if (ContainingObjectDescription.ObjectType == LazinatorObjectType.Struct ||  ContainingObjectDescription.GeneratingRefStruct)
                return "";
            if (ContainingObjectDescription.IsSealed)
            {
                if (DerivationKeyword == "override ")
                    return DerivationKeyword;
                return "";
            }
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

        private void AppendNonPrimitivePropertyDefinitionString(CodeStringBuilder sb, bool includeTracingCode)
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
            string createDefault = GetCreateDefaultString(sb);
            // And here's where we set the recreation code for when underlying memory exists.
            string recreation = GetRecreationString(sb, assignment);

            // Now we need to worry about the set accessor. 
            string propertyTypeDependentSet = GetPropertyTypeDependentSetString(sb);

            string propertyGetContents(bool async) => $@"
                {IIF(BackingAccessFieldIncluded, $@"if ({BackingFieldNotAccessedString})
                {{{IIF(includeTracingCode, $@"
TabbedText.WriteLine($""Accessing {PropertyName}"");")}
                    {AwaitIfNeeded(async)}Lazinate{PropertyName}{AsyncIfNeeded(async)}();
                }}")}{IIF(IsMemory || (IsNonLazinatorType && !TrackDirtinessNonSerialized && (!RoslynHelpers.IsReadOnlyStruct(Symbol) || ContainsLazinatorInnerProperty || ContainsOpenGenericInnerProperty)), $@"
                    IsDirty = true;")} {IIF(CodeOnAccessed != "", $@"
                {CodeOnAccessed}")}
                return {BackingFieldAccessWithPossibleException};";
            sb.Append($@"
                {ContainingObjectDescription.HideBackingField}{ContainingObjectDescription.ProtectedIfApplicable}{AppropriatelyQualifiedTypeName}{IIF(AddQuestionMarkInBackingFieldForNonNullable && !AppropriatelyQualifiedTypeName.EndsWith("?"), "?")} {BackingFieldString};
        {GetAttributesToInsert()}{ContainingObjectDescription.HideMainProperty}{PropertyAccessibilityString}{GetModifiedDerivationKeyword()}{AppropriatelyQualifiedTypeName} {PropertyName}
        {{{StepThroughPropertiesString}
            get
            {{{propertyGetContents(false)}
            }}{StepThroughPropertiesString}
            {SetterAccessibilityString}set
            {{{propertyTypeDependentSet}{RepeatedCodeExecution}
                IsDirty = true;
                DescendantIsDirty = true;
                {CodeBeforeSet}{BackingFieldString} = value;{CodeAfterSet}{IIF(IsNonLazinatorType && TrackDirtinessNonSerialized, $@"
                {BackingDirtyFieldString} = true;")}{IIF(BackingAccessFieldIncluded, $@"
                {BackingFieldAccessedString} = true;")}{RepeatedCodeExecution}
            }}
        }}{(GetModifiedDerivationKeyword() == "override " || !BackingAccessFieldIncluded ? "" : $@"
        {ContainingObjectDescription.HideBackingField}{ContainingObjectDescription.ProtectedIfApplicable}bool {BackingFieldAccessedString};")}{IIF(BackingAccessFieldIncluded, $@"
        private void Lazinate{PropertyName}()
        {{{GetLazinateContents(createDefault, recreation, true, false, includeTracingCode)}
        }}")}{IIF(BackingAccessFieldIncluded && WithinAsync, $@"
        private async Task Lazinate{PropertyName}Async()
        {{{GetLazinateContents(createDefault, recreation, true, true, includeTracingCode)}
        }}
        public async ValueTask<{AppropriatelyQualifiedTypeName}> Get{PropertyName}Async()
        {{{propertyGetContents(true)}
        }}")}

");

            // Copy property
            if ((PropertyType == LazinatorPropertyType.LazinatorStruct || PropertyType == LazinatorPropertyType.LazinatorStructNullable) && !ContainsOpenGenericInnerProperty)
            { // append copy property so that we can create item on stack if it doesn't need to be edited and hasn't been allocated yet

                sb.Append($@"{GetAttributesToInsert()}{ContainingObjectDescription.HideMainProperty}{PropertyAccessibilityString}{AppropriatelyQualifiedTypeName} {PropertyName}_Copy
                            {{{StepThroughPropertiesString}
                                get
                                {{
                                    {ConditionalCodeGenerator.ConsequentPossibleOnlyIf(BackingAccessFieldIncluded, BackingFieldNotAccessedString, $@"if (LazinatorMemoryStorage.Length == 0)
                                        {{
                                            return {DefaultExpression};
                                        }}
                                        else
                                        {{
                                            LazinatorMemory childData = {ChildSliceString};{IIF(WithinAsync, $@"
                                            childData.LoadInitialReadOnlyMemory();")}
                                            var toReturn = new {AppropriatelyQualifiedTypeNameWithoutNullableIndicator}(childData);
                                            toReturn.IsDirty = false;{IIF(WithinAsync, $@"
                                            childData.ConsiderUnloadInitialReadOnlyMemory();")}
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

        private string GetPropertyTypeDependentSetString(CodeStringBuilder sb)
        {
            string propertyTypeDependentSet = "";
            if (IsClassOrInterface)
            {
                if (ContainerIsClass) // change the parents collection so that it refers to the new value and not the old value. But other values in the parents collection will not be affected.
                {
                    if (ContainingObjectDescription.IsSingleParent)
                    {
                        if (Nullable)
                            propertyTypeDependentSet = $@"
                                if ({BackingFieldString} != null)
                                {{
                                    {BackingFieldString}.LazinatorParents = new LazinatorParentsCollection(null);
                                }}
                                if (value != null)
                                {{
                                    value.LazinatorParents = new LazinatorParentsCollection(this);
                                }}
                                ";
                        else
                            propertyTypeDependentSet = $@"
                                _ = value ?? throw new ArgumentNullException(nameof(value));
                                if ({BackingFieldString} != null)
                                {{
                                    {BackingFieldString}.LazinatorParents = new LazinatorParentsCollection(null);
                                }}
                                value.LazinatorParents = new LazinatorParentsCollection(this);
                                ";
                    }
                    else
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
                            copy.LazinatorParents = new LazinatorParentsCollection(this);{sb.GetNextLocationString()}
                            value = copy;
                        }}
                    ")}
                        ";
                else
                    propertyTypeDependentSet = $@"{IIF(ContainerIsClass, $@"
                        value.LazinatorParents = new LazinatorParentsCollection(this);")}{sb.GetNextLocationString()}
                        ";
            }
            else if (PropertyType == LazinatorPropertyType.OpenGenericParameter)
            {
                if (ContainingObjectDescription.ObjectType == LazinatorObjectType.Class && !ContainingObjectDescription.GeneratingRefStruct)
                    propertyTypeDependentSet = $@"
                        if (value != null && value.IsStruct){sb.GetNextLocationString()}
                        {{{IIF(ContainerIsClass, $@"
                            value.LazinatorParents = new LazinatorParentsCollection(this, null);")}{sb.GetNextLocationString()}
                        }}
                        else
                        {{
                            if ({BackingFieldString} != null)
                            {{
                                {BackingFieldString}.LazinatorParents = {BackingFieldString}.LazinatorParents.WithRemoved(this);{sb.GetNextLocationString()}
                            }}
                            if (value != null)
                            {{
                                value.LazinatorParents = value.LazinatorParents.WithAdded(this);{sb.GetNextLocationString()}
                            }}
                        }}
                            ";
                else
                    propertyTypeDependentSet = $@"";

            }

            return propertyTypeDependentSet;
        }

        private string GetCreateDefaultString(CodeStringBuilder sb)
        {
            string createDefault = $@"{BackingFieldString} = {DefaultExpression};{IIF(IsNonLazinatorType && TrackDirtinessNonSerialized, $@"
                                        {BackingDirtyFieldString} = true; ")}";
            if (IsLazinatorStruct)
                createDefault = $@"{BackingFieldString}{DefaultInitializationIfPossible(AppropriatelyQualifiedTypeName)};{IIF(ContainerIsClass && PropertyType != LazinatorPropertyType.LazinatorStructNullable && (!IsDefinitelyStruct || !Nullable), $@"
                                {BackingFieldString}.LazinatorParents = new LazinatorParentsCollection(this, null);")}{sb.GetNextLocationString()}";
            else if (PropertyType == LazinatorPropertyType.OpenGenericParameter)
                createDefault = $@"{BackingFieldString}{DefaultInitializationIfPossible(AppropriatelyQualifiedTypeName)};{IIF(ContainerIsClass, $@"
                                if ({BackingFieldString} != null)
                                {{ // {PropertyName} is a struct
                                    {BackingFieldString}.LazinatorParents = new LazinatorParentsCollection(this, null);{sb.GetNextLocationString()}
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

        private string GetRecreationString(CodeStringBuilder sb, string assignment)
        {
            string recreation;
            if (PropertyType == LazinatorPropertyType.LazinatorStruct || PropertyType == LazinatorPropertyType.LazinatorStructNullable
           || ((PropertyType == LazinatorPropertyType.LazinatorClassOrInterface || PropertyType == LazinatorPropertyType.LazinatorNonnullableClassOrInterface) && ContainingObjectDescription.IsSealed))
            {
                // we manually create the type and set the fields. Note that we don't want to call DeserializationFactory, because we would need to pass the field by ref (and we don't need to check for inherited types), and we would need to box a struct in conversion. We follow a similar pattern for sealed classes, because we don't have to worry about inheritance. 
                recreation = GetManualObjectCreation(sb);
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

        public string GetLazinateContentsForConstructor(CodeStringBuilder sb, bool includeTracingCode) => GetLazinateContents(GetCreateDefaultString(sb), GetRecreationString(sb, GetAssignmentString()), false, false, includeTracingCode);
        private string GetLazinateContents(string createDefault, string recreation, bool defineChildData, bool async, bool includeTracingCode)
        {
            return $@"
            {ConditionalCodeGenerator.ConsequentPossibleOnlyIf(Nullable || NonNullableThatCanBeUninitialized, "LazinatorMemoryStorage.Length == 0", createDefault, $@"{IIF(defineChildData, "LazinatorMemory ")}childData = {(async ? ChildSliceStringDefinitelyAsync : ChildSliceString)};{IIF(!async && WithinAsync, $@"
                childData.LoadInitialReadOnlyMemory();")}{IIF(includeTracingCode, $@"
TabbedText.WriteLine($""{ILazinatorString} location: {{childData.ToLocationString()}}"");")}{recreation}{IIF(async, $@"
                await childData.ConsiderUnloadReadOnlyMemoryAsync();")}{IIF(!async && WithinAsync, $@"
                childData.ConsiderUnloadInitialReadOnlyMemory();")}")}{IIF(BackingAccessFieldIncluded, $@"
            {BackingFieldAccessedString} = true;")}";
        }

        private string GetManualObjectCreation(CodeStringBuilder sb)
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
                                LazinatorParents = new LazinatorParentsCollection(this, null){sb.GetNextLocationString()}
                            }}";

            string doCreation = $@"{BackingFieldString} = new {AppropriatelyQualifiedTypeNameWithoutNullableIndicator}({ConstructorInitializationWithChildData}){lazinatorParentClassSet};
                        ";
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
            string coreOfGet = SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlySpan ? $@"return {GetReadOnlySpanBackingFieldCast("childData", true)};" : $@"{BackingFieldString} = childData.InitialReadOnlyMemory;
                    {BackingFieldAccessedString} = true;"; // for a read-only span, we directly return the lazinator, and don't update accessed, where the property hasn't been accessed
            sb.Append($@"{ContainingObjectDescription.HideBackingField}private ReadOnlyMemory<byte> {BackingFieldString};
        {GetAttributesToInsert()}{ContainingObjectDescription.HideMainProperty}{PropertyAccessibilityString}{GetModifiedDerivationKeyword()}{AppropriatelyQualifiedTypeName} {PropertyName}
        {{{StepThroughPropertiesString}
            get
            {{
                {ConditionalCodeGenerator.ElseConsequentPossibleOnlyIf(BackingAccessFieldIncluded, new ConditionCodeGenerator(BackingFieldNotAccessedString), $@"LazinatorMemory childData = {ChildSliceString};{IIF(WithinAsync, $@"
                    childData.LoadInitialReadOnlyMemory();")}
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
")}{IIF(WithinAsync, $@"public async ValueTask Ensure{PropertyName}LoadedAsync()
        {{
            LazinatorMemory childData = {ChildSliceStringDefinitelyAsync};
        }}
        ")}"); /* A ReadOnlySpan cannot be asynchronously returned, but we can call a method to make sure that it is loaded, and then we can access the property synchronously. */
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

        private string GetReadOnlySpanBackingFieldCast(string propertyName = null, bool initialSpan = false)
        {
            if (propertyName == null)
                propertyName = "_" + PropertyName;
            var innerFullType = InnerProperties[0].AppropriatelyQualifiedTypeName;
            string spanAccessor = IIF(SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlySpan, initialSpan ? ".InitialReadOnlyMemory.Span" : ".Span");
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
            EnumEquivalentType = RoslynHelpers.RegularizeTypeName(t.EnumUnderlyingType.Name);
        }

        #endregion

        #region Reading and writing

        private void SetReadAndWriteMethodNames()
        {
            if (PropertyType == LazinatorPropertyType.PrimitiveType ||
                PropertyType == LazinatorPropertyType.PrimitiveTypeNullable)
            {
                string name = EnumEquivalentType ?? ShortTypeName();
                if (Uncompressed)
                {

                    ReadMethodName = PrimitiveReadWriteMethodNames.ReadNamesUncompressed[name];
                    WriteMethodName = PrimitiveReadWriteMethodNames.WriteNamesUncompressed[name];
                }
                else
                {
                    ReadMethodName = PrimitiveReadWriteMethodNames.ReadNames[name];
                    WriteMethodName = PrimitiveReadWriteMethodNames.WriteNames[name];
                }
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



        public void AppendPropertyReadString(CodeStringBuilder sb, bool includeTracingCode)
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
                if (includeTracingCode)
                    sb.AppendLine($"TabbedText.WriteLine($\"Reading {PropertyName} at byte location {{bytesSoFar}}\"); ");
                sb.AppendLine(
                        new ConditionalCodeGenerator(ReadInclusionConditional, $@"{BackingFieldString} = {EnumEquivalentCastToEnum}span.{ReadMethodName}(ref bytesSoFar);").ToString());
            }
            else
            {
                if (includeTracingCode)
                {
                    sb.AppendLine($"TabbedText.WriteLine($\"{PropertyName}: Length is {{bytesSoFar}} past above position; start location is {{indexOfFirstChild + totalChildrenBytes}} past above position\"); ");
                }
                sb.Append($@"{BackingFieldByteIndex} = indexOfFirstChild + totalChildrenBytes;{skipCheckString}
                        ");
                if (OmitLengthBecauseDefinitelyLast)
                {
                    sb.AppendLine($@"
                                        totalChildrenBytes = span.Length - bytesSoFar;");
                }
                else if (IsGuaranteedFixedLength)
                {
                    if (FixedLength == 1)
                        sb.AppendLine($@"totalChildrenBytes++;");
                    else
                        sb.AppendLine($@"totalChildrenBytes += {FixedLength};");
                }
                else
                {
                    string sizeOfLengthString = SizeOfLengthTypeStringCaps;
                    sb.AppendLine(new ConditionalCodeGenerator(ReadInclusionConditional,
                            $"totalChildrenBytes += span.To{sizeOfLengthString}(ref bytesSoFar);").ToString());
                }

            }
            if (SkipCondition != null)
            {
                sb.AppendLine($@"}}");
            }
        }

        public int BytesUsedForLength()
        {
            if (IsPrimitive || OmitLengthBecauseDefinitelyLast || IsGuaranteedFixedLength)
                return 0;
            return SizeOfLength;
        }

        public void AppendPropertyWriteString(CodeStringBuilder sb)
        {
            string writerParameter = IsPrimitive ? "writer" : ContainingObjectDescription.MaybeAsyncConditional("writer.Writer", "writer");

            // We remember the startOfChildPosition, and then update the stored buffer at the end,
            // because we can't change the _ByteIndex until after the write, since we may need
            // to read from storage during the write.
            if (!IsPrimitive)
            {
                sb.AppendLine($"startOfChildPosition = {writerParameter}.{ContainingObjectDescription.ActiveOrOverallMemoryPosition};");
            }
            // Now, we have to consider the SkipCondition, from a SkipIf attribute. We don't write if the skip condition is
            // met (but still must update the byte index).
            if (SkipCondition != null)
                sb.AppendLine($@"if (!({SkipCondition}))
                            {{");
            // Now, we consider versioning information.
            if (IsPrimitive)
                sb.AppendLine(
                        new ConditionalCodeGenerator(WriteInclusionConditionalOptions, $"{WriteMethodName}(ref {writerParameter}, {EnumEquivalentCastToEquivalentType}{BackingFieldString});").ToString());
            else
            {
                // Finally, the main code for writing a serialized or non serialized object.
                if (IsLazinator)
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
                bool mustCast = ContainingObjectDescription.Splittable && !ContainingObjectDescription.RequiresLongLengths; // we're using longs for startOfChildPosition but not for BackingFieldByteIndex
                sb.AppendLine($@"if (options.UpdateStoredBuffer)
                                    {{
                                        {BackingFieldByteIndex} = {IIF(mustCast, "(int) (")}startOfChildPosition - startOfObjectPosition{IIF(mustCast, ")")};
                                        {removeBuffers}
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
                        $"(options.IncludeChildrenMode != OriginalIncludeChildrenMode)");
                    sb.AppendLine(
                        $@"{ContainingObjectDescription.MaybeAwaitWord}WriteNonLazinatorObject{omitLengthSuffix}{ContainingObjectDescription.MaybeAsyncWord}(
                    nonLazinatorObject: {BackingFieldString}, isBelievedDirty: {isBelievedDirtyString},
                    isAccessed: {BackingFieldAccessedString}, writer: {ContainingObjectDescription.MaybeAsyncConditional("writer", "ref writer")},
                    getChildSliceForFieldFn: {ContainingObjectDescription.Maybe_asyncWord}() => {ChildSliceStringMaybeAsync()},
                    verifyCleanness: {(TrackDirtinessNonSerialized ? "options.VerifyCleanness" : "false")},
                    binaryWriterAction: (ref BufferWriter w, bool v) =>
                        {DirectConverterTypeNamePrefix}{writeMethodName}(ref w, {BackingFieldStringOrContainedSpanWithPossibleException(null)},
                            options));");

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
                            $"{DirectConverterTypeNamePrefix}{writeMethodName}(ref w, copy_{PropertyName}, options)";
                    string isBelievedDirtyString = ConditionsCodeGenerator.OrCombine(
                        TrackDirtinessNonSerialized ? $"{PropertyName}_Dirty" : $"{BackingFieldAccessedString}",
                        $"(options.IncludeChildrenMode != OriginalIncludeChildrenMode)");
                    sb.AppendLine(
                        $@"var serializedBytesCopy_{PropertyName} = LazinatorMemoryStorage;
                        var byteIndexCopy_{PropertyName} = {BackingFieldByteIndex};
                        var byteLengthCopy_{PropertyName} = {BackingFieldByteLength};
                        var copy_{PropertyName} = {BackingFieldString};
                        WriteNonLazinatorObject{omitLengthSuffix}(
                        nonLazinatorObject: {BackingFieldString}, isBelievedDirty: {isBelievedDirtyString},
                        isAccessed: {BackingFieldAccessedString}, writer: ref writer{ContainingObjectDescription.MaybeAsyncConditional(".Writer", "")},
                        getChildSliceForFieldFn: () => GetChildSlice(serializedBytesCopy_{PropertyName}, byteIndexCopy_{PropertyName}, byteLengthCopy_{PropertyName}{ChildSliceLastParametersString}),
                        verifyCleanness: {(TrackDirtinessNonSerialized ? "options.VerifyCleanness" : "false")},
                        binaryWriterAction: (ref BufferWriter w, bool v) =>
                            {binaryWriterAction});");

                }
            }
            else
                sb.AppendLine($@"WriteNonLazinatorObject{omitLengthSuffix}(
                        nonLazinatorObject: default, isBelievedDirty: true,
                        isAccessed: true, writer: ref writer,
                        getChildSliceForFieldFn: () => {ChildSliceStringMaybeAsync()},
                        verifyCleanness: {(TrackDirtinessNonSerialized ? "options.VerifyCleanness" : "false")},
                        binaryWriterAction: (ref BufferWriter w, bool v) =>
                            {DirectConverterTypeNamePrefix}{writeMethodName}(ref w, default,
                                options));");
        }

        private void AppendPropertyWriteString_Lazinator(CodeStringBuilder sb)
        {
            string withInclusionConditional = null;
            bool nullableStruct = PropertyType == LazinatorPropertyType.LazinatorStructNullable || (IsDefinitelyStruct && Nullable);
            string propertyNameOrCopy = nullableStruct ? "copy" : $"{BackingFieldString}";
            Func<string, string> lazinatorNullableStructNullCheck; 
            string lengthString = "";
            if (AllLengthsPrecedeChildren && !SkipLengthForThisProperty)
            {
                if (SizeOfLength != 8)
                {
                    lengthString = $@"lengthValue = {ContainingObjectDescription.LengthValueExpression(true)};
                        if (lengthValue > {SizeOfLengthTypeString}.MaxValue)
                        {{
                            ThrowHelper.ThrowTooLargeException({SizeOfLengthTypeString}.MaxValue);
                        }}
                        writer.RecordLength(({SizeOfLengthTypeString}) lengthValue);";
                }
                else
                {
                    lengthString = $@"lengthValue = {ContainingObjectDescription.LengthValueExpression(true)};
                        writer.RecordLength(lengthValue);";
                }
                lazinatorNullableStructNullCheck = originalString => PropertyType == LazinatorPropertyType.LazinatorStructNullable ? GetNullCheckIfThen($"{BackingFieldString}", $@"WriteNullChild_LengthsSeparate(ref writer, {(SingleByteLength ? "true" : "false")});", originalString) : originalString;
            }
            else
                lazinatorNullableStructNullCheck = originalString => PropertyType == LazinatorPropertyType.LazinatorStructNullable ? GetNullCheckIfThen($"{BackingFieldString}", $@"WriteNullChild(ref writer);", originalString) : originalString;
            string callWriteChild = AsyncWithinAsync ? ContainingObjectDescription.MaybeAsyncConditional($"await{ContainingObjectDescription.NoteAsyncUsed} WriteChildAsync(writer,", $"WriteChild(ref writer, ref") : ContainingObjectDescription.MaybeAsyncConditional($"await{ContainingObjectDescription.NoteAsyncUsed} WriteNonAsyncChildAsync(writer,", $"WriteChild(ref writer, ref");
            if (ContainingObjectDescription.ObjectType == LazinatorObjectType.Class && !ContainingObjectDescription.GeneratingRefStruct)
            {
                string mainWriteString = $@"{IIF(nullableStruct, $@"var copy = {BackingFieldString}.Value;
                            ")}{callWriteChild} {propertyNameOrCopy}{NullForgivenessIfNonNullable}, options, {BackingFieldAccessedString}, {ContainingObjectDescription.Maybe_asyncWord}() => {ChildSliceStringMaybeAsync()}, this);{IIF(PropertyType == LazinatorPropertyType.LazinatorStructNullable, $@"
                                {BackingFieldString} = copy;")}
                                {lengthString}";
                withInclusionConditional =
                    new ConditionalCodeGenerator(WriteInclusionConditionalOptions, $@"{EnsureDeserialized()}{lazinatorNullableStructNullCheck(mainWriteString)}").ToString();
            }
            else
            {
                // for structs, we can't pass local struct variables in the lambda, so we have to copy them over. We'll assume we have to do this with open generics too.
                string mainWriteString = $@"var serializedBytesCopy = LazinatorMemoryStorage;
                            var byteIndexCopy = {BackingFieldByteIndex};
                            var byteLengthCopy = {BackingFieldByteLength};
                            {IIF(PropertyType == LazinatorPropertyType.LazinatorStructNullable, $@"var copy = {BackingFieldString}.Value;
                            ")}{callWriteChild} {propertyNameOrCopy}{NullForgivenessIfNonNullable}, options, {BackingFieldAccessedString}, () => GetChildSlice(serializedBytesCopy, byteIndexCopy, byteLengthCopy{ChildSliceLastParametersString}), null);{IIF(PropertyType == LazinatorPropertyType.LazinatorStructNullable, $@"
                                {BackingFieldString} = copy;")}
                                {lengthString}";
                withInclusionConditional =
                    $@"{new ConditionalCodeGenerator(WriteInclusionConditionalOptions, $"{EnsureDeserialized()}{lazinatorNullableStructNullCheck(mainWriteString)}")}";
            }
            if (withInclusionConditional != "")
                sb.AppendLine(withInclusionConditional);
            
        }

        private string EnsureDeserialized()
        {
            if (PropertyType == LazinatorPropertyType.SupportedCollection && SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlySpan)
                return ""; // ReadOnlySpan gives direct memory access, so we don't need to deserialize anything
            return $@"{new ConditionalCodeGenerator(
                ConditionsCodeGenerator.AndCombine(
                    $"(options.IncludeChildrenMode != IncludeChildrenMode.IncludeAllChildren || options.IncludeChildrenMode != OriginalIncludeChildrenMode)",
                    $"{BackingFieldNotAccessedString}"),
                $"var deserialized = {ContainingObjectDescription.MaybeAsyncPropertyName(this)};")}
                ";
        }

        public void AppendCopyPropertyToClone(CodeStringBuilder sb, string nameOfCloneVariable)
        {
            string copyInstruction = "";
            string propertyAccess = ContainingObjectDescription.MaybeAsyncPropertyName(this);
            string cloneAsyncWord = AsyncWithinAsync ? ContainingObjectDescription.MaybeAsyncWord : "";
            string cloneAwaitWord = AsyncWithinAsync ? ContainingObjectDescription.MaybeAwaitWord : "";
            if (IsLazinator)
            {
                string nonNullStatement = $@"{nameOfCloneVariable}.{PropertyName} = ({AppropriatelyQualifiedTypeName}) {cloneAwaitWord}{propertyAccess}{IIF(IsDefinitelyStruct && Nullable, ".Value")}.CloneLazinator{cloneAsyncWord}(includeChildrenMode, CloneBufferOptions.NoBuffer){NullableForgivenessIfPossiblyNeeded};{sb.GetNextLocationString()}";
                if (!Nullable)
                    copyInstruction = nonNullStatement;
                else
                    copyInstruction = GetNullCheckIfThen(propertyAccess,
                        $"{nameOfCloneVariable}.{PropertyName} = {DefaultExpression};{sb.GetNextLocationString()}{sb.GetNextLocationString()}",
                        nonNullStatement);
            }
            else if (IsPrimitive)
                copyInstruction = $"{nameOfCloneVariable}.{PropertyName} = {PropertyName};{sb.GetNextLocationString()}";
            else if ((PropertyType == LazinatorPropertyType.NonLazinator && HasInterchangeType) || PropertyType == LazinatorPropertyType.SupportedCollection || PropertyType == LazinatorPropertyType.SupportedTuple)
            {
                copyInstruction = $"{nameOfCloneVariable}.{PropertyName} = CloneOrChange_{AppropriatelyQualifiedTypeNameEncodable}({propertyAccess}, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false){NullableForgivenessIfPossiblyNeeded};{sb.GetNextLocationString()}";
            }
            else if (PropertyType == LazinatorPropertyType.NonLazinator)
                copyInstruction = $"{nameOfCloneVariable}.{PropertyName} = {DirectConverterTypeNamePrefix}CloneOrChange_{AppropriatelyQualifiedTypeNameEncodable}({propertyAccess}, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false){NullableForgivenessIfPossiblyNeeded};{sb.GetNextLocationString()}";
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
                         private static void ConvertToBytes_{AppropriatelyQualifiedTypeNameEncodable}(ref BufferWriter writer, {AppropriatelyQualifiedTypeName} itemToConvert, LazinatorSerializationOptions options)
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

                    private static void ConvertToBytes_{AppropriatelyQualifiedTypeNameEncodable}(ref BufferWriter writer, {AppropriatelyQualifiedTypeName} itemToConvert, LazinatorSerializationOptions options)
                    {{
                        if (itemToConvert == null)
                        {{
                            writer.Write((bool)true);
                            return;
                        }}
                        writer.Write((bool)false);
                        {DirectConverterTypeNamePrefix}ConvertToBytes_{AppropriatelyQualifiedTypeNameEncodableWithoutNullable}(ref writer, itemToConvert.Value, options);
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

                    private static void ConvertToBytes_{AppropriatelyQualifiedTypeNameEncodable}(ref BufferWriter writer, {AppropriatelyQualifiedTypeName} itemToConvert, LazinatorSerializationOptions options)
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

                    private static void ConvertToBytes_{AppropriatelyQualifiedTypeNameEncodable}(ref BufferWriter writer, {AppropriatelyQualifiedTypeName} itemToConvert, LazinatorSerializationOptions options)
                    {{
                        {(SupportedCollectionType == LazinatorSupportedCollectionType.Memory || SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlyMemory ? "" : $@"if (itemToConvert == {DefaultExpression})
                        {{
                            {((NullableModeEnabled && !Nullable) ? $@"ThrowHelper.ThrowSerializingNullNonNullable(""{PropertyName}"");
                            " : "")}return;
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
            innerProperty.GetSupportedCollectionAddCommands(this, "itemCopied", out collectionAddItem, out collectionAddNull);
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
                        {(GetNullCheckIfThenButOnlyIfNullable(Nullable, "itemToClone", "return default;", ""))}int collectionLength = itemToClone.{lengthWord};{IIF(ArrayRank > 1, () => "\r\n" + String.Join("\r\n", Enumerable.Range(0, ArrayRank.Value).Select(x => $"int collectionLength{x} = itemToClone.GetLength({x});")))}
                        {creationText}
                        {forStatement}
                        {{
                            {IIF(avoidCloningIfPossibleOption, $@"if (avoidCloningIfPossible)
                            {{
                                if ({innerProperty.GetNonNullCheck(false, "itemToClone[itemIndex]")})
                                {{
                                    itemToClone[itemIndex] = ({innerProperty.AppropriatelyQualifiedTypeName}) ((cloneOrChangeFunc(itemToClone[itemIndex]){NullableForgivenessIfPossiblyNeeded}));
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

            cloneString = InnerProperties[0].GetCloneStringWithinCloneMethod(itemString, GetTypeNameOfInnerProperty(InnerProperties[0]));
        }

        private string GetTypeNameOfInnerProperty(PropertyDescription innerProperty)
        {
            // the inner property might be from a different nullability context and therefore might omit a "?" when one is necessary
            // (e.g., if the type is "string")
            string typeName = innerProperty.AppropriatelyQualifiedTypeName;
            if (NullableModeEnabled && !typeName.EndsWith("?") && innerProperty.Nullable)
                typeName += "?";
            return typeName;
        }


        private string GetCloneStringWithinCloneMethod(string itemString, string typeName)
        {
            string cloneString;
            if (IsLazinator)
            {
                cloneString = $"(cloneOrChangeFunc({itemString}){NullableForgivenessIfPossiblyNeeded})";
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
            return $"({typeName}) " + cloneString;
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
                            bool isNull = storage.InitialReadOnlyMemory.Span.ToBoolean(ref index);
                            if (isNull)
                            {{
                                return null;
                            }}
                            ReadOnlySpan<byte> span = storage.InitialReadOnlyMemory.Span.Slice(1);
                            return span.ToArray();
                        }}");
                else
                    sb.Append($@"return storage.InitialReadOnlyMemory.ToArray();
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
                    string rankLengthCommand = $"int collectionLength{i} = span.ToDecompressedInt32(ref bytesSoFar);";
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
                readCollectionLengthCommand = $"int collectionLength = span.ToDecompressedInt32(ref bytesSoFar);";
                forStatementCommand = $"for (int itemIndex = 0; itemIndex < collectionLength; itemIndex++)";
            }

            PropertyDescription innerProperty = InnerProperties[0];
            CheckForLazinatorInNonLazinator(innerProperty);
            string readCommand = innerProperty.GetSupportedCollectionReadCommands(this);
            string preliminaryNullCheck;
            if (Nullable && (SupportedCollectionType == LazinatorSupportedCollectionType.Memory || SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlyMemory))
            {
                preliminaryNullCheck = $@"int index = 0;
                        bool isNull = storage.InitialReadOnlyMemory.Span.ToBoolean(ref index);
                        if (isNull)
                        {{
                            return null;
                        }}
                        ReadOnlySpan<byte> span = storage.InitialReadOnlyMemory.Span.Slice(1);";
            }
            else
            {
                preliminaryNullCheck = $@"{IIF(Nullable || IsSupportedTupleType, $@"if (storage.Length == 0)
                        {{
                            return {DefaultExpression};
                        }}
                        ")}ReadOnlySpan<byte> span = storage.InitialReadOnlyMemory.Span;";
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
            GetSupportedCollectionAddCommands(outerProperty, "item", out collectionAddItem, out collectionAddNull);

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
                    else if (PropertyType == LazinatorPropertyType.OpenGenericParameter)
                        return (
                            $@"
                            {lengthCollectionMemberString}
                            LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                            var item = DeserializationFactory.Instance.CreateBasedOnType<{AppropriatelyQualifiedTypeName}>(childData);
                            {collectionAddItem}
                            bytesSoFar += lengthCollectionMember;");
                    else
                        return (
                            $@"
                            {lengthCollectionMemberString}
                            LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                            var item = new {AppropriatelyQualifiedTypeNameWithoutNullableIndicator}(childData);
                            {collectionAddItem}
                            bytesSoFar += lengthCollectionMember;");
                }
            }
        }

        private string GetSpanReadLength()
        {
            if (IsGuaranteedFixedLength)
                return $"{FixedLength}";
            else if (SingleByteLength)
                return "span.ToByte(ref bytesSoFar)";
            else
                return "span.ToInt32(ref bytesSoFar)";
        }

        private void GetSupportedCollectionAddCommands(PropertyDescription outerProperty, string itemWord, out string collectionAddItem, out string collectionAddNull)
        {
            if (NullableModeEnabled && outerProperty.InnerProperties[0].Nullable == false)
                itemWord += "!";
            if (outerProperty.SupportedCollectionType == LazinatorSupportedCollectionType.Array)
            {
                if (outerProperty.ArrayRank == 1)
                {
                    collectionAddItem = $"collection[itemIndex] = {itemWord};";
                    collectionAddNull = $"collection[itemIndex] = {DefaultExpression};";
                }
                else
                {

                    string innerArrayText = (String.Join(", ", Enumerable.Range(0, (int)outerProperty.ArrayRank).Select(j => $"itemIndex{j}")));
                    collectionAddItem = $"collection[{innerArrayText}] = {itemWord};";
                    collectionAddNull = $"collection[{innerArrayText}] = {DefaultExpression};";
                }
            }
            else if (outerProperty.SupportedCollectionType == LazinatorSupportedCollectionType.Memory || outerProperty.SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlyMemory)
            {
                collectionAddItem = $"collectionAsSpan[itemIndex] = {itemWord};";
                collectionAddNull = $"collectionAsSpan[itemIndex] = {DefaultExpression};";
            }
            else if (outerProperty.SupportedCollectionType == LazinatorSupportedCollectionType.Dictionary || outerProperty.SupportedCollectionType == LazinatorSupportedCollectionType.SortedDictionary || outerProperty.SupportedCollectionType == LazinatorSupportedCollectionType.SortedList)
            {
                // the outer type is a dictionary
                string keyWord = "item.Key", valueWord = "item.Value";
                if (NullableModeEnabled && outerProperty.InnerProperties[0].Nullable == false)
                    keyWord += "!";
                if (NullableModeEnabled && outerProperty.InnerProperties[0].Nullable == false)
                    valueWord += "!";
                collectionAddItem = $"collection.Add({keyWord}, {valueWord});";
                collectionAddNull = ""; // no null entries in dictionary
            }
            else if (outerProperty.SupportedCollectionType == LazinatorSupportedCollectionType.Queue)
            {
                collectionAddItem = $"collection.Enqueue({itemWord});";
                collectionAddNull = $"collection.Enqueue({DefaultExpression});";
            }
            else if (outerProperty.SupportedCollectionType == LazinatorSupportedCollectionType.Stack)
            {
                collectionAddItem = $"collection.Push({itemWord});";
                collectionAddNull = $"collection.Push({DefaultExpression});";
            }
            else if (outerProperty.SupportedCollectionType == LazinatorSupportedCollectionType.LinkedList)
            {
                collectionAddItem = $"collection.AddLast({itemWord});";
                collectionAddNull = $"collection.AddLast({DefaultExpression});";
            }
            else
            {
                collectionAddItem = $"collection.Add({itemWord});";
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
                    void action(ref BufferWriter w) => {DirectConverterTypeNamePrefix}ConvertToBytes_{AppropriatelyQualifiedTypeNameEncodable}(ref w, {BackingFieldStringOrContainedSpan(itemString)}, options);
                    WriteToBinaryWith{LengthPrefixTypeString}LengthPrefix(ref writer, action);");
                else if (IsPossiblyStruct && outerPropertyIsSimpleListOrArray)
                {
                    return ($@"
                    void action(ref BufferWriter w) 
                    {{
                        var copy = {itemString};{IIF(PropertyType == LazinatorPropertyType.LazinatorStructNullable, $@"
                        if (copy != null)
                        {{")}
                        copy.{IIF(PropertyType == LazinatorPropertyType.LazinatorStructNullable, "Value.")}SerializeToExistingBuffer(ref w, options);
                        {itemString} = copy;{IIF(PropertyType == LazinatorPropertyType.LazinatorStructNullable, $@"
                        }}")}
                    }}
                    WriteToBinaryWith{LengthPrefixTypeString}LengthPrefix(ref writer, action);");
                }
                else
                {
                    return ($@"
                    void action(ref BufferWriter w) => {itemString}{IIF(PropertyType == LazinatorPropertyType.LazinatorStructNullable, "?")}{NullForgiveness}.SerializeToExistingBuffer(ref w, options);
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
                        writer.Write((int)0);
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
                        ")}ReadOnlySpan<byte> span = storage.InitialReadOnlyMemory.Span;

                        int bytesSoFar = 0;
                        ");

            for (int i = 0; i < InnerProperties.Count; i++)
            {
                sb.Append(InnerProperties[i].GetSupportedTupleReadCommand("item" + (i + 1)));
                sb.AppendLine();
            }

            IEnumerable<string> assignments;
            if (InitializeRecordLikeTypePropertiesDirectly)
                assignments = Enumerable.Range(1, InnerProperties.Count).Select(x => InnerProperties[x - 1].PropertyName + " = " + "item" + x);
            else
                assignments = Enumerable.Range(1, InnerProperties.Count).Select(x => "item" + x);

            sb.Append(
                    $@"
                        var itemToCreate = {(SupportedTupleType == LazinatorSupportedTupleType.ValueTuple ? "" : $"new {AppropriatelyQualifiedTypeNameWithoutNullableIndicator}")}{GetInnerPropertyAssignments(InitializeRecordLikeTypePropertiesDirectly, assignments)};

                        return itemToCreate;
                    }}

                    ");
            sb.Append($@"private static void ConvertToBytes_{AppropriatelyQualifiedTypeNameEncodable}(ref BufferWriter writer, {AppropriatelyQualifiedTypeName} itemToConvert, LazinatorSerializationOptions options)
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
                sb.Append(InnerProperties[0].GetSupportedTupleWriteCommand("Key", SupportedTupleType.Value, Nullable, Symbol.IsValueType));
                sb.AppendLine();
                sb.Append(InnerProperties[1].GetSupportedTupleWriteCommand("Value", SupportedTupleType.Value, Nullable, Symbol.IsValueType));
                sb.AppendLine();
            }
            else if (SupportedTupleType == LazinatorSupportedTupleType.RecordLikeType)
            {
                for (int i = 0; i < InnerProperties.Count; i++)
                {
                    sb.Append(InnerProperties[i].GetSupportedTupleWriteCommand(InnerProperties[i].PropertyName, SupportedTupleType.Value, Nullable, Symbol.IsValueType));
                    sb.AppendLine();
                }
            }
            else for (int i = 0; i < InnerProperties.Count; i++)
                {
                    sb.Append(InnerProperties[i].GetSupportedTupleWriteCommand("Item" + (i + 1), SupportedTupleType.Value, Nullable, Symbol.IsValueType));
                    sb.AppendLine();
                }

            sb.AppendLine($"}}");

            AppendSupportedTupleCloneMethod(sb);

            RecursivelyAppendConversionMethods(sb, alreadyGenerated);
        }

        private string GetInnerPropertyAssignments(bool assignPropertiesDirectly, IEnumerable<string> assignments)
        {
            if (assignPropertiesDirectly)
            {
                string propertyAssignmentStrings = string.Join(",\r\n", assignments);
                return $@"()
                            {{
                                {propertyAssignmentStrings}
                            }}";
            }
            else
            {
                string commaSeparatedAssignments = String.Join(", ", assignments);
                return $"({commaSeparatedAssignments})";
            }
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
                            {itemName} = new {AppropriatelyQualifiedTypeNameWithoutNullableIndicator}(childData);
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

        private string GetSupportedTupleWriteCommand(string itemName, LazinatorSupportedTupleType outerTupleType, bool outerTypeIsNullable, bool outerTypeIsValueType)
        {
            string itemToConvertItemName =
                $"itemToConvert{IIF((outerTupleType == LazinatorSupportedTupleType.ValueTuple || outerTupleType == LazinatorSupportedTupleType.KeyValuePair || (outerTupleType == LazinatorSupportedTupleType.RecordLikeType && outerTypeIsNullable && outerTypeIsValueType)) && outerTypeIsNullable, ".Value")}.{itemName}";
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
                                void action{itemName}(ref BufferWriter w) => {DirectConverterTypeNamePrefix}ConvertToBytes_{AppropriatelyQualifiedTypeNameEncodable}(ref w, {itemToConvertItemName}, options);
                                WriteToBinaryWithInt32LengthPrefix(ref writer, action{itemName});
                            }}");
                else return $@"
                            void action{itemName}(ref BufferWriter w) => {DirectConverterTypeNamePrefix}ConvertToBytes_{AppropriatelyQualifiedTypeNameEncodable}(ref w, {itemToConvertItemName}, options);
                            WriteToBinaryWithInt32LengthPrefix(ref writer, action{itemName});";
            }
            else
            {
                if (PropertyType == LazinatorPropertyType.LazinatorStruct || PropertyType == LazinatorPropertyType.LazinatorStructNullable)
                    return ($@"
                        void action{itemName}(ref BufferWriter w) => {itemToConvertItemName}{IIF(PropertyType == LazinatorPropertyType.LazinatorStructNullable, "?")}.SerializeToExistingBuffer(ref w, options);
                        WriteToBinaryWith{LengthPrefixTypeString}LengthPrefix(ref writer, action{itemName});");
                else
                    return ($@"
                        if ({itemToConvertItemName} == null)
                        {{
                            {WriteDefaultLengthString}
                        }}
                        else
                        {{
                            void action{itemName}(ref BufferWriter w) => {itemToConvertItemName}.SerializeToExistingBuffer(ref w, options);
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
            List<string> innerCloneStrings = InnerProperties
                                .Zip(
                                    itemStrings,
                                    (x, y) => new { InnerProperty = x, ItemString = "(" + propertyAccess + y + ((NullableModeEnabled && !x.Nullable) ? "!" : "") + (Nullable && !x.Nullable && !NullableModeEnabled ? " ?? default" : "") + ")" })
                                .Select(z => (InitializeRecordLikeTypePropertiesDirectly ? z.InnerProperty.PropertyName + " = " : "") + z.InnerProperty.GetCloneStringWithinCloneMethod(z.ItemString, GetTypeNameOfInnerProperty(z.InnerProperty)))
                                .ToList();
            string innerPropertyAssignments = GetInnerPropertyAssignments(InitializeRecordLikeTypePropertiesDirectly, innerCloneStrings);
            string creationText = SupportedTupleType == LazinatorSupportedTupleType.ValueTuple ? innerPropertyAssignments : $"new {AppropriatelyQualifiedTypeNameWithoutNullableIndicator}{innerPropertyAssignments}";

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

                        private static void ConvertToBytes_{AppropriatelyQualifiedTypeNameEncodable}(ref BufferWriter writer,
                            {AppropriatelyQualifiedTypeName} itemToConvert, LazinatorSerializationOptions options)
                        {{
                            {GetNullCheckIfThen("itemToConvert", $@"return;", "")}
                            {InterchangeTypeName} interchange = new {InterchangeTypeNameWithoutNullabilityIndicator}(itemToConvert);
                            interchange.SerializeToExistingBuffer(ref writer, options);
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

