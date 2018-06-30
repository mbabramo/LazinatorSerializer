using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using LazinatorAnalyzer.AttributeClones;
using LazinatorAnalyzer.Settings;
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
        private bool ContainerIsClass => ContainingObjectDescription.ObjectType == LazinatorObjectType.Class;
        private bool ContainerIsStruct => ContainingObjectDescription.ObjectType == LazinatorObjectType.Struct;
        private PropertyDescription ContainingPropertyDescription { get; set; }
        public PropertyDescription OutmostPropertyDescription => ContainingPropertyDescription?.OutmostPropertyDescription ?? this;
        private int UniqueIDForLazinatorType { get; set; }
        internal IPropertySymbol PropertySymbol { get; set; }
        private ITypeSymbol TypeSymbolIfNoProperty { get; set; }
        private ITypeSymbol Symbol => PropertySymbol != null ? (ITypeSymbol) PropertySymbol.Type : (ITypeSymbol) TypeSymbolIfNoProperty;
        internal bool GenericConstrainedToClass => Symbol is ITypeParameterSymbol typeParameterSymbol && typeParameterSymbol.HasReferenceTypeConstraint;
        internal bool GenericConstrainedToStruct => Symbol is ITypeParameterSymbol typeParameterSymbol && typeParameterSymbol.HasValueTypeConstraint;
        private bool IsClassOrInterface => PropertyType == LazinatorPropertyType.LazinatorClassOrInterface || (PropertyType == LazinatorPropertyType.OpenGenericParameter && GenericConstrainedToClass);
        private bool IsLazinatorStruct => PropertyType == LazinatorPropertyType.LazinatorStruct || (PropertyType == LazinatorPropertyType.OpenGenericParameter && GenericConstrainedToStruct);
        internal string DerivationKeyword { get; set; }
        private bool IsAbstract { get; set; }
        internal bool Nullable { get; set; }
        private bool HasParameterlessConstructor => PropertySymbol.Type is INamedTypeSymbol namedTypeSymbol && namedTypeSymbol.InstanceConstructors.Any(y => !y.IsImplicitlyDeclared && !y.Parameters.Any());
        private bool IsInterface { get; set; }
        private int? ArrayRank { get; set; }
        internal bool IsDefinedInLowerLevelInterface { get; set; }
        internal bool IsLast { get; set; }
        private bool OmitLengthBecauseDefinitelyLast => (IsLast && ContainingObjectDescription.IsSealedOrStruct && ContainingObjectDescription.Version == -1);
        private string ChildSliceString => $"GetChildSlice(LazinatorObjectBytes, _{PropertyName}_ByteIndex, _{PropertyName}_ByteLength{ChildSliceEndString})";
        private string ChildSliceEndString => $", {(OmitLengthBecauseDefinitelyLast ? "true" : "false")}, {(IsGuaranteedSmall ? "true" : "false")}, {(IsGuaranteedFixedLength ? $"{FixedLength}" : "null")}";

        /* Property type */
        internal LazinatorPropertyType PropertyType { get; set; }
        internal LazinatorSupportedCollectionType? SupportedCollectionType { get; set; }
        private LazinatorSupportedTupleType? SupportedTupleType { get; set; }
        internal bool IsPrimitive => PropertyType == LazinatorPropertyType.PrimitiveType || PropertyType == LazinatorPropertyType.PrimitiveTypeNullable;
        internal bool IsLazinator => PropertyType == LazinatorPropertyType.LazinatorClassOrInterface || PropertyType == LazinatorPropertyType.LazinatorStruct || PropertyType == LazinatorPropertyType.OpenGenericParameter;
        internal bool IsNotPrimitiveOrOpenGeneric => PropertyType != LazinatorPropertyType.OpenGenericParameter && PropertyType != LazinatorPropertyType.PrimitiveType && PropertyType != LazinatorPropertyType.PrimitiveTypeNullable;
        internal bool IsNonSerializedType => PropertyType == LazinatorPropertyType.NonSelfSerializingType || PropertyType == LazinatorPropertyType.SupportedCollection || PropertyType == LazinatorPropertyType.SupportedTuple;

        /* Names */
        private bool UseFullyQualifiedNames => (ContainingObjectDescription.Compilation.Config?.UseFullyQualifiedNames ?? false) || HasFullyQualifyAttribute || Symbol.ContainingType != null;
        private string ShortTypeName => RegularizeTypeName(Symbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat));
        private string ShortTypeNameWithoutNullableIndicator => WithoutNullableIndicator(ShortTypeName);
        internal string FullyQualifiedTypeName => Symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        private string FullyQualifiedNameWithoutNullableIndicator => WithoutNullableIndicator(FullyQualifiedTypeName);
        internal string AppropriatelyQualifiedTypeName => UseFullyQualifiedNames ? FullyQualifiedTypeName : ShortTypeName;
        private string AppropriatelyQualifiedNameWithoutNullableIndicator => UseFullyQualifiedNames ? FullyQualifiedNameWithoutNullableIndicator : ShortTypeNameWithoutNullableIndicator;

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
        internal string BackingFieldString => (SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlySpan || SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlyMemory) ?
                    GetReadOnlySpanBackingFieldCast() : $"_{PropertyName}";

        /* Enums */
        private string EnumEquivalentType { get; set; }
        private string EnumEquivalentCastToEquivalentType => EnumEquivalentType != null ? $"({EnumEquivalentType}) " : $"";
        private string EnumEquivalentCastToEnum => EnumEquivalentType != null ? $"({AppropriatelyQualifiedTypeName})" : $"";

        /* Inner properties */
        private List<PropertyDescription> InnerProperties { get; set; }
        private bool ContainsOpenGenericInnerProperty => InnerProperties != null && InnerProperties.Any(x => x.PropertyType == LazinatorPropertyType.OpenGenericParameter || x.ContainsOpenGenericInnerProperty);
        internal string NullableStructValueAccessor => IIF(PropertyType == LazinatorPropertyType.LazinatorStruct && Nullable , ".Value");

        /* Conversion */
        private string InterchangeTypeName { get; set; }
        private string DirectConverterTypeName { get; set; }
        private string DirectConverterTypeNamePrefix => DirectConverterTypeName == "" || DirectConverterTypeName == null ? "" : DirectConverterTypeName + ".";
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
        private string CustomNonlazinatorWrite { get; set; }
        private int? IntroducedWithVersion { get; set; }
        private int? EliminatedWithVersion { get; set; }
        public string SkipCondition { get; set; }
        public string InitializeWhenSkipped { get; set; }
        internal bool TrackDirtinessNonSerialized { get; set; }
        private string ReadInclusionConditional { get; set; }
        private string WriteInclusionConditional { get; set; }
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
        public LazinatorConfig Config => ContainingObjectDescription.Compilation?.Config;
        private bool Tracing => Config?.IncludeTracingCode ?? false;
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

        public PropertyDescription(IPropertySymbol propertySymbol, ObjectDescription container, string derivationKeyword, string propertyAccessibility, bool isLast)
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
                throw new LazinatorCodeGenException($"ILazinator interface property {PropertyName} in {ContainingObjectDescription?.NameIncludingGenerics} should omit the set because because it uses an inconsistent SetterAccessibilityAttribute.");

            ParseVersionAttributes();

            ParseOtherPropertyAttributes();

            SetPropertyType(propertySymbol.Type as ITypeSymbol);

            SetReadAndWriteMethodNames();

            SetInclusionConditionals();
        }

        public PropertyDescription(ITypeSymbol typeSymbol, ObjectDescription containingObjectDescription, PropertyDescription containingPropertyDescription, string propertyName = null)
        {
            // This is only used for defining the type on the inside of the generics, plus underlying type for arrays.
            TypeSymbolIfNoProperty = typeSymbol;
            ContainingObjectDescription = containingObjectDescription;
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
            CloneCustomNonlazinatorWriteAttribute nonlazinatorWrite = UserAttributes.OfType<CloneCustomNonlazinatorWriteAttribute>().FirstOrDefault();
            if (nonlazinatorWrite != null)
                CustomNonlazinatorWrite = nonlazinatorWrite.WriteMethod;
            CloneIncludableChildAttribute includable = UserAttributes.OfType<CloneIncludableChildAttribute>().FirstOrDefault();
            IncludableWhenExcludingMostChildren = includable != null;
            CloneExcludableChildAttribute excludable = UserAttributes.OfType<CloneExcludableChildAttribute>().FirstOrDefault();
            ExcludableWhenIncludingMostChildren = excludable != null;
            CloneAllowLazinatorInNonLazinatorAttribute allowLazinatorInNonLazinator = UserAttributes.OfType<CloneAllowLazinatorInNonLazinatorAttribute>().FirstOrDefault();
            AllowLazinatorInNonLazinator = allowLazinatorInNonLazinator != null;
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

        private string InclusionConditionalHelper(bool readVersion)
        {
            string versionNumberVariable = readVersion ? "serializedVersionNumber" : "LazinatorObjectVersion";
            List<string> conditions = new List<string>();
            if (PropertyType == LazinatorPropertyType.LazinatorStruct || PropertyType == LazinatorPropertyType.LazinatorClassOrInterface || PropertyType == LazinatorPropertyType.OpenGenericParameter)
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
            if (!conditions.Any())
                return "";
            return $@"if ({String.Join(" && ", conditions)}) ";
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
            
            Nullable = IsNullableType(namedTypeSymbol);
            if (Nullable)
            {
                // handle a nullable type (which might be a nullable primitive type or a nullable struct / valuetuple
                SetNullablePropertyType(namedTypeSymbol);
                if (PropertyType == LazinatorPropertyType.LazinatorStruct)
                    throw new LazinatorCodeGenException($"Type {typeSymbol} is a nullable Lazinator struct. This is not yet supported. Use WNullableStruct instead.");
                return;
            }
            else
            {
                // look for primitive type
                if (typeSymbol.Name == "string")
                    Nullable = true;
                if (namedTypeSymbol?.EnumUnderlyingType != null)
                    SetEnumEquivalentType(namedTypeSymbol);
                if (SupportedAsPrimitives.Contains(EnumEquivalentType ?? ShortTypeNameWithoutNullableIndicator))
                {
                    PropertyType = LazinatorPropertyType.PrimitiveType;
                    return;
                }
            }

            bool isILazinator = typeSymbol.Interfaces.Any(x => x.Name == "ILazinator");
            bool isRecursiveDefinition = false;
            if (namedTypeSymbol != null)
            {
                if (namedTypeSymbol.Equals(ContainingObjectDescription?.InterfaceTypeSymbol))
                {
                    if (!isILazinator)
                        throw new LazinatorCodeGenException(
                        "If an interface defines itself recursively, then it must explicitly declare that it implements ILazinator.");
                    isRecursiveDefinition = true;
                }

                if (namedTypeSymbol.Equals(ContainingObjectDescription?.ILazinatorTypeSymbol))
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
                    if (namedTypeSymbol.TypeKind == TypeKind.Interface && !namedTypeSymbol.AllInterfaces.Any(x => x.Name == "ILazinator"))
                        throw new LazinatorCodeGenException($"To use {namedTypeSymbol} as a type in {ContainingObjectDescription.FullyQualifiedObjectName}, you must make {namedTypeSymbol} inherit directly from ILazinator.");
                    isILazinator = true; // code behind isn't implemented yet but it will be
                }
            }

            bool isSelfSerializable = isRecursiveDefinition || isILazinator;
            if (isSelfSerializable)
                SetSelfSerializablePropertyType(namedTypeSymbol);
            else
            {
                // look for supported collections and tuples
                if (typeSymbol.TypeKind == TypeKind.Array)
                { // array is not a generic type, so we handle it specially
                    SetArrayPropertyType(typeSymbol as IArrayTypeSymbol);
                    return;
                }

                if (HandleSupportedTuplesAndCollections(namedTypeSymbol))
                    return;

                SetNonserializedTypeNameAndPropertyType(namedTypeSymbol);
            }
        }

        private void SetNonserializedTypeNameAndPropertyType(INamedTypeSymbol t)
        {
            Nullable = t.TypeKind == TypeKind.Class;
            PropertyType = LazinatorPropertyType.NonSelfSerializingType;
            InterchangeTypeName = ContainingObjectDescription.Compilation.Config?.GetInterchangeConverterTypeName(t);
            DirectConverterTypeName = ContainingObjectDescription.Compilation.Config?.GetDirectConverterTypeName(t);
            string fullyQualifiedTypeName = t.GetFullyQualifiedNameWithoutGlobal();
            if (InterchangeTypeName != null && DirectConverterTypeName != null)
                throw new LazinatorCodeGenException($"{fullyQualifiedTypeName} has both an interchange converter and a direct converter type listed. Only one should be used.");
            if (InterchangeTypeName == null && DirectConverterTypeName == null)
            {
                if (fullyQualifiedTypeName == "Lazinator.Core.ILazinator")
                    throw new LazinatorCodeGenException($"You cannot include ILazinator as a type to be serialized or as a type argument in a Lazinator interface. (The reason for this is that some Lazinator types do not serialize their ID.) To define a property that can deserialize a large number of Lazinator types, create a nonexclusive interface (possibly implementing no properties) and then define your Lazinator types as implementing that interface. This nonexclusive interface can then be used as the type for a Lazinator property.");
                else
                    throw new LazinatorCodeGenException($"{fullyQualifiedTypeName} is a non-Lazinator type. To use it as a type for a Lazinator property, you must either make it a Lazinator type or use a Lazinator.config file to specify either an interchange converter (i.e., a Lazinator object accept the non-Lazinator type as a parameter in its constructor) or a direct converter for it. Alternatively, if there is a constructor whose parameters match public properties (not fields) of the type, it can be handled automatically.");
            }
        }

        public string GetNullCheck()
        {
            string nullCheck;
            if (PropertyType == LazinatorPropertyType.LazinatorStruct || PropertyType == LazinatorPropertyType.OpenGenericParameter)
                nullCheck = $"System.Collections.Generic.EqualityComparer<{AppropriatelyQualifiedTypeName}>.Default.Equals({PropertyName}, default({AppropriatelyQualifiedTypeName}))";
            else
                nullCheck = $"{PropertyName} == null";
            return nullCheck;
        }

        public string GetNonNullCheck(bool includeAccessedCheck)
        {
            string nonNullCheck;
            if (includeAccessedCheck)
            {
                if (PropertyType == LazinatorPropertyType.LazinatorStruct || PropertyType == LazinatorPropertyType.OpenGenericParameter)
                    nonNullCheck = $"_{PropertyName}_Accessed && !System.Collections.Generic.EqualityComparer<{AppropriatelyQualifiedTypeName}>.Default.Equals(_{PropertyName}, default({AppropriatelyQualifiedTypeName}))";
                else
                    nonNullCheck = $"_{PropertyName}_Accessed && _{PropertyName} != null";
            }
            else
            {
                if (PropertyType == LazinatorPropertyType.LazinatorStruct || PropertyType == LazinatorPropertyType.OpenGenericParameter)
                    nonNullCheck = $"!System.Collections.Generic.EqualityComparer<{AppropriatelyQualifiedTypeName}>.Default.Equals({PropertyName}, default({AppropriatelyQualifiedTypeName}))";
                else
                    nonNullCheck = $"{PropertyName} != null";
            }
            return nonNullCheck;
        }

        private bool HandleSupportedTuplesAndCollections(INamedTypeSymbol t)
        {
            if (t.TupleUnderlyingType != null)
                t = t.TupleUnderlyingType;
            if (t.IsGenericType)
            {
                string name = t.Name;
                CheckSupportedTuples(name);
                if (PropertyType == LazinatorPropertyType.SupportedTuple)
                {
                    SetSupportedTuplePropertyType(t, name);
                    return true;
                }

                // now look for supported collections
                CheckSupportedCollections(name);
                if (PropertyType == LazinatorPropertyType.SupportedCollection)
                {
                    SetSupportedCollectionTypeNameAndPropertyType(t, name);
                    return true;
                }
            }

            return (HandleRecordLikeType(t));
        }

        private void SetNullablePropertyType(INamedTypeSymbol namedTypeSymbol)
        {
            SetPropertyType(namedTypeSymbol.TypeArguments[0] as INamedTypeSymbol);
            Nullable = true;
            if (EnumEquivalentType != null)
                EnumEquivalentType += "?";
            if (PropertyType == LazinatorPropertyType.PrimitiveType)
                PropertyType = LazinatorPropertyType.PrimitiveTypeNullable;
        }

        private void SetSelfSerializablePropertyType(INamedTypeSymbol t)
        {
            if (t.TypeKind == TypeKind.Class || t.TypeKind == TypeKind.Interface || t.TypeKind == TypeKind.Array)
            {
                Nullable = true;
                PropertyType = LazinatorPropertyType.LazinatorClassOrInterface;
            }
            else
                PropertyType = LazinatorPropertyType.LazinatorStruct;

            IsInterface = t.TypeKind == TypeKind.Interface;
            if (!IsInterface)
            {
                var exclusiveInterface = ContainingObjectDescription.Compilation.TypeToExclusiveInterface[t.OriginalDefinition];
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

        private void SetArrayPropertyType(IArrayTypeSymbol t)
        {
            ArrayRank = t.Rank;
            SupportedCollectionType = LazinatorSupportedCollectionType.Array;
            Nullable = true;
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
            // We look for a record-like type only after we have determined that the type does not implement ILazinator and we don't have the other supported tuple types (e.g., ValueTuples, KeyValuePair). We need to make sure that for each parameter in the constructor with the most parameters, there is a unique property with the same name (case insensitive as to first letter). If so, we assume that this property corresponds to the parameter, though there is no inherent guarantee that this is true. 
            var recordLikeTypes = ContainingObjectDescription.Compilation.RecordLikeTypes;
            if (!recordLikeTypes.ContainsKey(t) || (ContainingObjectDescription.Compilation.Config?.IgnoreRecordLikeTypes.Any(x => x.ToUpper() == (UseFullyQualifiedNames ? t.GetFullyQualifiedNameWithoutGlobal().ToUpper() : t.GetMinimallyQualifiedName())) ?? false))
            {
                return false;
            }

            TypeSymbolIfNoProperty = t;
            PropertyType = LazinatorPropertyType.SupportedTuple;
            SupportedTupleType = LazinatorSupportedTupleType.RecordLikeType;
            Nullable = false;

            InnerProperties = recordLikeTypes[t]
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
                if (pd.TypeSymbolIfNoProperty == typeSymbol)
                    throw new LazinatorCodeGenException($"The type {typeSymbol} is recursively defined. Recursive record-like types are not supported.");
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

        private void SetSupportedTuplePropertyType(INamedTypeSymbol t, string nameWithoutArity)
        {
            SetInnerProperties(t, nameWithoutArity);
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
        private void SetSupportedCollectionTypeNameAndPropertyType(INamedTypeSymbol t, string nameWithoutArity)
        {
            if (SupportedCollectionType != LazinatorSupportedCollectionType.Memory && SupportedCollectionType != LazinatorSupportedCollectionType.ReadOnlySpan && SupportedCollectionType != LazinatorSupportedCollectionType.ReadOnlyMemory)
                Nullable = true;

            InnerProperties = t.TypeArguments
                .Select(x => new PropertyDescription(x, ContainingObjectDescription, this)).ToList();

            if (SupportedCollectionType == LazinatorSupportedCollectionType.Memory || SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlySpan || SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlyMemory)
                if (InnerProperties[0].Nullable)
                    throw new LazinatorCodeGenException("Cannot use Lazinator to serialize Memory/Span with nullable generic arguments."); // this is because we can't cast easily in this context

            if (SupportedCollectionType == LazinatorSupportedCollectionType.Dictionary || SupportedCollectionType == LazinatorSupportedCollectionType.SortedDictionary || SupportedCollectionType == LazinatorSupportedCollectionType.SortedList)
            {
                // We process a Dictionary by treating it as a collection with KeyValuePairs. Thus, we must change to a single inner property of type KeyValuePair, which in turn has two inner properties equal to the properties of the type in our actual dictionary.
                if (keyValuePairType == null)
                    keyValuePairType = ContainingObjectDescription.Compilation.Compilation.GetTypeByMetadataName(typeof(KeyValuePair<,>).FullName);
                INamedTypeSymbol constructed = keyValuePairType.Construct(t.TypeArguments.ToArray());
                var replacementInnerProperty = new PropertyDescription(constructed, ContainingObjectDescription, this); // new PropertyDescription("System.Collections.Generic", "KeyValuePair", t.TypeArguments, ContainingObjectDescription);
                InnerProperties = new List<PropertyDescription>() { replacementInnerProperty };
            }
        }

        private static bool IsNullableType(INamedTypeSymbol t)
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
            string propertyString = $@"{ContainingObjectDescription.ProtectedIfApplicable}bool _{PropertyName}_Accessed{IIF(ContainingObjectDescription.ObjectType != LazinatorObjectType.Struct, " = false")};
        {GetAttributesToInsert()}{PropertyAccessibilityString}{abstractDerivationKeyword}{AppropriatelyQualifiedTypeName} {PropertyName}
        {{
            get;
            set;
        }}
";
            sb.Append(propertyString);
        }

        private string GetModifiedDerivationKeyword()
        {
            if (ContainingObjectDescription.ObjectType == LazinatorObjectType.Struct || ContainingObjectDescription.IsSealed)
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
            string propertyString = $@"        {ContainingObjectDescription.ProtectedIfApplicable}{AppropriatelyQualifiedTypeName} _{PropertyName};
        {GetAttributesToInsert()}{PropertyAccessibilityString}{GetModifiedDerivationKeyword()}{AppropriatelyQualifiedTypeName} {PropertyName}
        {{{StepThroughPropertiesString}
            get
            {{
                return _{PropertyName};
            }}{StepThroughPropertiesString}
            {SetterAccessibilityString}set
            {{
                IsDirty = true;
                _{PropertyName} = value;{RepeatedCodeExecution}
            }}
        }}
";
            sb.Append(propertyString);
        }

        private void AppendNonPrimitivePropertyDefinitionString(CodeStringBuilder sb)
        {
            if (SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlySpan)
            {
                AppendReadOnlySpanProperty(sb);
                return;
            }

            if (SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlyMemory)
            {
                AppendReadOnlyMemoryProperty(sb);
                return;
            }

            string assignment;
            string selfReference = IIF(ContainingObjectDescription.ObjectType == LazinatorObjectType.Class, ", this");
            if (PropertyType == LazinatorPropertyType.LazinatorClassOrInterface || PropertyType == LazinatorPropertyType.LazinatorStruct)
            {
                if (IsInterface)
                    assignment =
                    $@"
                        _{PropertyName} = DeserializationFactory.Instance.CreateBasedOnType<{AppropriatelyQualifiedTypeName}>(childData{selfReference}); ";
                else if (IsAbstract)
                    assignment =
                    $@"
                        _{PropertyName} = DeserializationFactory.Instance.CreateAbstractType<{AppropriatelyQualifiedTypeName}>(childData{selfReference}); ";
                else
                    assignment =
                        $@"
                        _{PropertyName} = DeserializationFactory.Instance.CreateBaseOrDerivedType({UniqueIDForLazinatorType}, () => new {AppropriatelyQualifiedTypeName}(), childData{selfReference}); ";
            }
            else if (PropertyType == LazinatorPropertyType.OpenGenericParameter)
            {
                assignment =
                    $@"
                        _{PropertyName} = DeserializationFactory.Instance.CreateBasedOnType<{AppropriatelyQualifiedTypeName}>(childData{selfReference}); ";
            }
            else
            {
                bool automaticallyMarkDirtyWhenContainedObjectIsCreated = TrackDirtinessNonSerialized && ContainingObjectDescription.ObjectType == LazinatorObjectType.Class; // (1) unless we're tracking dirtiness, there is no field to set when the descendant informs us that it is dirty; (2) with a struct, we can't use an anonymous lambda (and more fundamentally can't pass a delegate to the struct method. Thus, if a struct has a supported collection, we can't automatically set DescendantIsDirty for the struct based on a change in some contained entity.
                assignment = $"_{PropertyName} = {DirectConverterTypeNamePrefix}ConvertFromBytes_{AppropriatelyQualifiedTypeNameEncodable}(childData);";
            }

            string createDefault = $@"_{PropertyName} = default({AppropriatelyQualifiedTypeName});{IIF(IsNonSerializedType && TrackDirtinessNonSerialized, $@"
                                        _{PropertyName}_Dirty = true; ")}";
            if (IsLazinatorStruct)
                createDefault = $@"_{PropertyName} = default({ AppropriatelyQualifiedTypeName});{IIF(ContainerIsClass, $@"
                                _{PropertyName}.LazinatorParents = new LazinatorParentsCollection(this);")}";
            else if (PropertyType == LazinatorPropertyType.OpenGenericParameter)
                createDefault = $@"_{PropertyName} = default({ AppropriatelyQualifiedTypeName});{IIF(ContainerIsClass, $@"
                                if (_{PropertyName} != null)
                                {{ // {PropertyName} is a struct
                                    _{PropertyName}.LazinatorParents = new LazinatorParentsCollection(this);
                                }}")}";

            string recreation;
            if (PropertyType == LazinatorPropertyType.LazinatorStruct 
                || (PropertyType == LazinatorPropertyType.LazinatorClassOrInterface && ContainingObjectDescription.IsSealed))
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


            string propertyTypeDependentSet = "";
            if (IsClassOrInterface)
            {
                if (ContainerIsClass)
                    propertyTypeDependentSet = $@"
                        if (_{PropertyName} != null)
                        {{
                            _{PropertyName}.LazinatorParents = _{PropertyName}.LazinatorParents.WithRemoved(this);
                        }}
                        if (value != null)
                        {{
                            value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                        }}
                        ";
                else
                    propertyTypeDependentSet = $@"";

            }
            else if (IsLazinatorStruct)
            {
                propertyTypeDependentSet = $@"{IIF(ContainerIsClass, $@"
                    value.LazinatorParents = new LazinatorParentsCollection(this);")}
                    ";
            }
            else if (PropertyType == LazinatorPropertyType.OpenGenericParameter)
            {
                if (ContainingObjectDescription.ObjectType == LazinatorObjectType.Class)
                    propertyTypeDependentSet = $@"
                        if (value != null && value.IsStruct)
                        {{{IIF(ContainerIsClass, $@"
                            value.LazinatorParents = new LazinatorParentsCollection(this);")}
                        }}
                        else
                        {{
                            if (_{PropertyName} != null)
                            {{
                                _{PropertyName}.LazinatorParents = _{PropertyName}.LazinatorParents.WithRemoved(this);
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

            sb.Append($@"{ContainingObjectDescription.ProtectedIfApplicable}{AppropriatelyQualifiedTypeName} _{PropertyName};
        {GetAttributesToInsert()}{PropertyAccessibilityString}{GetModifiedDerivationKeyword()}{AppropriatelyQualifiedTypeName} {PropertyName}
        {{{StepThroughPropertiesString}
            get
            {{
                if (!_{PropertyName}_Accessed)
                {{
                    if (_LazinatorObjectBytes.Length == 0)
                    {{
                        {createDefault}
                    }}
                    else
                    {{
                        ReadOnlyMemory<byte> childData = {ChildSliceString};
                        {recreation}
                    }}
                    _{PropertyName}_Accessed = true;
                }}{IIF(IsNonSerializedType && !TrackDirtinessNonSerialized && !RoslynHelpers.IsReadOnlyStruct(Symbol), $@"
                    IsDirty = true;")} 
                return _{PropertyName};
            }}{StepThroughPropertiesString}
            set
            {{{propertyTypeDependentSet}{RepeatedCodeExecution}IsDirty = true;
                DescendantIsDirty = true;
                _{PropertyName} = value;{IIF(IsNonSerializedType && TrackDirtinessNonSerialized, $@"
                _{PropertyName}_Dirty = true;")}
                _{PropertyName}_Accessed = true;{RepeatedCodeExecution}
            }}
        }}{(GetModifiedDerivationKeyword() == "override " ? "" : $@"
        {ContainingObjectDescription.ProtectedIfApplicable}bool _{PropertyName}_Accessed;")}
");

            // Copy property
            if (PropertyType == LazinatorPropertyType.LazinatorStruct && !ContainsOpenGenericInnerProperty)
            { // append copy property so that we can create item on stack if it doesn't need to be edited and hasn't been allocated yet

                sb.Append($@"{GetAttributesToInsert()}{PropertyAccessibilityString}{AppropriatelyQualifiedTypeName} {PropertyName}_Copy
                            {{{StepThroughPropertiesString}
                                get
                                {{
                                    if (!_{PropertyName}_Accessed)
                                    {{
                                        if (_LazinatorObjectBytes.Length == 0)
                                        {{
                                            return default({AppropriatelyQualifiedTypeName});
                                        }}
                                        else
                                        {{
                                            ReadOnlyMemory<byte> childData = {ChildSliceString};
                                            return new {AppropriatelyQualifiedTypeName}()
                                            {{
                                                LazinatorObjectBytes = childData,
                                                IsDirty = false
                                            }};
                                        }}
                                    }}
                                    var cleanCopy = _{PropertyName};
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

        private string GetManualObjectCreation()
        {
            // if the container object containing this property is a struct, then we can't set LazinatorParents. Meanwhile, if this object is a struct, then we don't need to worry about the case of a null item. 
            string nullItemCheck = PropertyType == LazinatorPropertyType.LazinatorStruct
                ? ""
                : $@"if (childData.Length == 0)
                        {{
                            _{PropertyName} = default;
                        }}
                        else ";
            string lazinatorParentClassSet = ContainingObjectDescription.ObjectType == LazinatorObjectType.Struct ? "" : $@"
                            LazinatorParents = new LazinatorParentsCollection(this),";
            string creation = $@"{nullItemCheck}_{PropertyName} = new {AppropriatelyQualifiedTypeName}()
                    {{{lazinatorParentClassSet}
                        LazinatorObjectBytes = childData,
                    }};";
            return creation;
        }

        private void AppendReadOnlyMemoryProperty(CodeStringBuilder sb) => AppendReadOnlySpanOrMemoryProperty(sb, false);

        private void AppendReadOnlySpanProperty(CodeStringBuilder sb) => AppendReadOnlySpanOrMemoryProperty(sb, true);

        private void AppendReadOnlySpanOrMemoryProperty(CodeStringBuilder sb, bool isSpan)
        {
            var innerFullType = InnerProperties[0].AppropriatelyQualifiedTypeName;
            string castToSpanOfCorrectType;
            castToSpanOfCorrectType = GetReadOnlySpanBackingFieldCast();
            sb.Append($@"private ReadOnlyMemory<byte> _{PropertyName};
        {GetAttributesToInsert()}{PropertyAccessibilityString}{GetModifiedDerivationKeyword()}{AppropriatelyQualifiedTypeName} {PropertyName}
        {{{StepThroughPropertiesString}
            get
            {{
                if (!_{PropertyName}_Accessed)
                {{
                    ReadOnlyMemory<byte> childData = {ChildSliceString};
                    _{PropertyName} = childData;
                    _{PropertyName}_Accessed = true;
                }}
                return {castToSpanOfCorrectType};
            }}{StepThroughPropertiesString}
            set
            {{
                {RepeatedCodeExecution}IsDirty = true;
                _{PropertyName} = {(isSpan ? $"new ReadOnlyMemory<byte>(MemoryMarshal.Cast<{innerFullType}, byte>(value).ToArray());" : "value;")}
                _{PropertyName}_Accessed = true;{RepeatedCodeExecution}
            }}
        }}
        {ContainingObjectDescription.ProtectedIfApplicable}bool _{PropertyName}_Accessed;
");
        }

        private string GetReadOnlySpanBackingFieldCast()
        {
            var innerFullType = InnerProperties[0].AppropriatelyQualifiedTypeName;
            string spanAccessor = IIF(SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlySpan, ".Span");
            string castToSpanOfCorrectType;
            if (innerFullType == "byte")
                castToSpanOfCorrectType = $"_{PropertyName}{spanAccessor}";
            else castToSpanOfCorrectType = $"MemoryMarshal.Cast<byte, {innerFullType}>(_{PropertyName}{spanAccessor})";
            return castToSpanOfCorrectType;
        }

        private void AppendDirtinessTracking(CodeStringBuilder sb)
        {
            sb.Append($@"
        private bool _{PropertyName}_Dirty;
        public bool {PropertyName}_Dirty
        {{{StepThroughPropertiesString}
            get => _{PropertyName}_Dirty;{StepThroughPropertiesString}
            set
            {{
                {RepeatedCodeExecution}if (_{PropertyName}_Dirty != value)
                {{
                    _{PropertyName}_Dirty = value;
                    if (value && !IsDirty)
                    {{
                        IsDirty = true;
                    }}
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
                ReadMethodName = PrimitiveReadWriteMethodNames.ReadNames[EnumEquivalentType ?? ShortTypeName];
                WriteMethodName = PrimitiveReadWriteMethodNames.WriteNames[EnumEquivalentType ?? ShortTypeName];
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
                        CreateConditionalForSingleLine(ReadInclusionConditional, $@"_{PropertyName} = {EnumEquivalentCastToEnum}span.{ReadMethodName}(ref bytesSoFar);"));
            }
            else
            {
                if (OmitLengthBecauseDefinitelyLast)
                {
                    sb.AppendLine($@"_{PropertyName}_ByteIndex = bytesSoFar;{skipCheckString}
                                        bytesSoFar = span.Length;");
                }
                else if (IsGuaranteedFixedLength)
                {
                    if (FixedLength == 1)
                        sb.AppendLine($@"_{PropertyName}_ByteIndex = bytesSoFar;{skipCheckString}
                                            bytesSoFar++;");
                    else
                        sb.AppendLine($@"_{PropertyName}_ByteIndex = bytesSoFar;{skipCheckString}
                                        bytesSoFar += {FixedLength};");
                }
                else if (IsGuaranteedSmall)
                    sb.AppendLine(
                        $@"_{PropertyName}_ByteIndex = bytesSoFar;{skipCheckString}
                            " + CreateConditionalForSingleLine(ReadInclusionConditional,
                            "bytesSoFar = span.ToByte(ref bytesSoFar) + bytesSoFar;"));
                else
                    sb.AppendLine(
                        $@"_{PropertyName}_ByteIndex = bytesSoFar;{skipCheckString}
                            " + CreateConditionalForSingleLine(ReadInclusionConditional,
                            "bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;"));
            }
            if (SkipCondition != null)
            {
                sb.AppendLine($@"}}");
            }
        }

        private string CreateConditionalForSingleLine(string conditional, string consequent, string elseConsequent = null)
        {
            if (conditional.Trim() == "")
                return consequent;
            var conditionalString = $@"{conditional}
                        {{
                            {consequent}
                        }}";
            if (elseConsequent != null && elseConsequent != "")
                conditionalString += $@"
                        else
                        {{
                            {elseConsequent}
                        }}";
            return conditionalString;
        }

        public void AppendPropertyWriteString(CodeStringBuilder sb)
        {
            if (SkipCondition != null)
                sb.AppendLine($@"if (!({SkipCondition}))
                            {{");
            if (IsPrimitive)
                sb.AppendLine(
                        CreateConditionalForSingleLine(WriteInclusionConditional, $"{WriteMethodName}(writer, {EnumEquivalentCastToEquivalentType}_{PropertyName});"));
            else
            {
                sb.AppendLine("startOfObjectPosition = writer.Position;");
                if (PropertyType == LazinatorPropertyType.LazinatorClassOrInterface || PropertyType == LazinatorPropertyType.LazinatorStruct || PropertyType == LazinatorPropertyType.OpenGenericParameter)
                    AppendPropertyWriteString_SelfSerialized(sb);
                else
                    AppendPropertyWriteString_NonSelfSerialized(sb);
            }
            if (SkipCondition != null)
                sb.AppendLine($@"}}");
            if (!IsPrimitive)
                sb.AppendLine($@"if (updateStoredBuffer)
                                {{
                                    _{PropertyName}_ByteIndex = startOfObjectPosition - startPosition;
                                }}");
        }

        private void AppendPropertyWriteString_NonSelfSerialized(CodeStringBuilder sb)
        {
            string omitLengthSuffix = IIF(OmitLengthBecauseDefinitelyLast, "_WithoutLengthPrefix");
            string writeMethodName = CustomNonlazinatorWrite == null ? $"ConvertToBytes_{AppropriatelyQualifiedTypeNameEncodable}" : CustomNonlazinatorWrite;
            if (ContainingObjectDescription.ObjectType == LazinatorObjectType.Class)
            {
                sb.AppendLine(
                    $@"WriteNonLazinatorObject{omitLengthSuffix}(
                        nonLazinatorObject: _{PropertyName}, isBelievedDirty: {(TrackDirtinessNonSerialized ? $"{PropertyName}_Dirty" : $"_{PropertyName}_Accessed")},
                        isAccessed: _{PropertyName}_Accessed, writer: writer,
                        getChildSliceForFieldFn: () => {ChildSliceString},
                        verifyCleanness: {(TrackDirtinessNonSerialized ? "verifyCleanness" : "false")},
                        binaryWriterAction: (w, v) =>
                            {DirectConverterTypeNamePrefix}{writeMethodName}(w, {BackingFieldString},
                                includeChildrenMode, v, updateStoredBuffer));");
            }
            else
            { // as above, must copy local struct variables for anon lambda.
                string binaryWriterAction;
                if (CustomNonlazinatorWrite == null && (SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlySpan || SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlyMemory))
                    binaryWriterAction = $"copy_{PropertyName}.Write(w)";
                else
                    binaryWriterAction = $"{DirectConverterTypeNamePrefix}{writeMethodName}(w, copy_{PropertyName}, includeChildrenMode, v, updateStoredBuffer)";
                sb.AppendLine(
                    $@"var serializedBytesCopy_{PropertyName} = LazinatorObjectBytes;
                        var byteIndexCopy_{PropertyName} = _{PropertyName}_ByteIndex;
                        var byteLengthCopy_{PropertyName} = _{PropertyName}_ByteLength;
                        var copy_{PropertyName} = _{PropertyName};
                        WriteNonLazinatorObject{omitLengthSuffix}(
                        nonLazinatorObject: _{PropertyName}, isBelievedDirty: {(TrackDirtinessNonSerialized ? $"{PropertyName}_Dirty" : $"_{PropertyName}_Accessed")},
                        isAccessed: _{PropertyName}_Accessed, writer: writer,
                        getChildSliceForFieldFn: () => GetChildSlice(serializedBytesCopy_{PropertyName}, byteIndexCopy_{PropertyName}, byteLengthCopy_{PropertyName}{ChildSliceEndString}),
                        verifyCleanness: {(TrackDirtinessNonSerialized ? "verifyCleanness" : "false")},
                        binaryWriterAction: (w, v) =>
                            {binaryWriterAction});");

            }
        }

        private void AppendPropertyWriteString_SelfSerialized(CodeStringBuilder sb)
        {
            if (ContainingObjectDescription.ObjectType == LazinatorObjectType.Class)
            {
                sb.AppendLine(
                    CreateConditionalForSingleLine(WriteInclusionConditional, $"WriteChild(writer, _{PropertyName}, includeChildrenMode, _{PropertyName}_Accessed, () => {ChildSliceString}, verifyCleanness, updateStoredBuffer, {(IsGuaranteedSmall ? "true" : "false")}, {(IsGuaranteedFixedLength || OmitLengthBecauseDefinitelyLast ? "true" : "false")}, this);"));
            }
            else
            {
                // for structs, we can't pass local struct variables in the lambda, so we have to copy them over. We'll assume we have to do this with open generics too.
                sb.AppendLine(
                    $@"{WriteInclusionConditional} 
                        {{
                            var serializedBytesCopy = LazinatorObjectBytes;
                            var byteIndexCopy = _{PropertyName}_ByteIndex;
                            var byteLengthCopy = _{PropertyName}_ByteLength;
                            WriteChild(writer, _{PropertyName}, includeChildrenMode, _{PropertyName}_Accessed, () => GetChildSlice(serializedBytesCopy, byteIndexCopy, byteLengthCopy{ChildSliceEndString}), verifyCleanness, updateStoredBuffer, {(IsGuaranteedSmall ? "true" : "false")}, {(IsGuaranteedFixedLength || OmitLengthBecauseDefinitelyLast ? "true" : "false")}, null);
                        }}");
            }
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
                AppendReadOnlySpan_SerializeExistingBuffer(sb);
                return;
            }
            if (SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlyMemory)
            {
                AppendReadOnlyMemory_SerializeExistingBuffer(sb);
                return;
            }

            AppendSupportedCollection_ConvertFromBytes(sb);
            AppendSupportedCollection_SerializeExistingBuffer(sb);

            RecursivelyAppendConversionMethods(sb, alreadyGenerated);
        }

        private void AppendReadOnlySpan_SerializeExistingBuffer(CodeStringBuilder sb) => AppendReadOnlySpanOrMemory_SerializeExistingBuffer(sb, true);

        private void AppendReadOnlyMemory_SerializeExistingBuffer(CodeStringBuilder sb) => AppendReadOnlySpanOrMemory_SerializeExistingBuffer(sb, false);

        private void AppendReadOnlySpanOrMemory_SerializeExistingBuffer(CodeStringBuilder sb, bool isSpan)
        {
            // this method is used within classes, but not within structs
            if (ContainingObjectDescription.ObjectType != LazinatorObjectType.Class)
                return;

            string innerFullType = InnerProperties[0].AppropriatelyQualifiedTypeName;
            string innerTypeEncodable = InnerProperties[0].AppropriatelyQualifiedTypeNameEncodable;

            sb.Append($@"
                         private static void ConvertToBytes_{AppropriatelyQualifiedTypeNameEncodable}(BinaryBufferWriter writer, {AppropriatelyQualifiedTypeName} itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
                        {{
                            ReadOnlySpan<byte> toConvert = MemoryMarshal.Cast<{innerFullType}, byte>(itemToConvert{(isSpan ? "" : ".Span")});
                            for (int i = 0; i < toConvert.Length; i++)
                            {{
                                writer.Write(toConvert[i]);
                            }}
                        }}
                    ");
        }

        private void AppendSupportedCollection_SerializeExistingBuffer(CodeStringBuilder sb)
        {
            bool isArray = SupportedCollectionType == LazinatorSupportedCollectionType.Array;

            string lengthWord;
            if (SupportedCollectionType == LazinatorSupportedCollectionType.List || SupportedCollectionType == LazinatorSupportedCollectionType.SortedSet || SupportedCollectionType == LazinatorSupportedCollectionType.LinkedList ||
                SupportedCollectionType == LazinatorSupportedCollectionType.HashSet || SupportedCollectionType == LazinatorSupportedCollectionType.Dictionary || SupportedCollectionType == LazinatorSupportedCollectionType.SortedDictionary || SupportedCollectionType == LazinatorSupportedCollectionType.SortedList || SupportedCollectionType == LazinatorSupportedCollectionType.Queue || SupportedCollectionType == LazinatorSupportedCollectionType.Stack)
            {
                lengthWord = "Count";
            }
            else if (SupportedCollectionType == LazinatorSupportedCollectionType.Memory)
            {
                lengthWord = "Length";
            }
            else if (isArray)
            {
                lengthWord = "Length";
            }
            else
                throw new NotImplementedException();


            string itemString, itemStringSetup = "", forStatement;
            if (SupportedCollectionType == LazinatorSupportedCollectionType.HashSet || SupportedCollectionType == LazinatorSupportedCollectionType.Dictionary || SupportedCollectionType == LazinatorSupportedCollectionType.SortedDictionary || SupportedCollectionType == LazinatorSupportedCollectionType.SortedList)
            {
                forStatement = $@"foreach (var item in itemToConvert)";
                itemString = "item"; // can't index into hash set
            }
            else if (SupportedCollectionType == LazinatorSupportedCollectionType.Memory)
            {
                forStatement = $@"itemToConvertSpan = itemToConvert.Span;foreach (var item in itemToConvert)";
                forStatement =
                        $@"var itemToConvertSpan = itemToConvert.Span;
                        int itemToConvertCount = itemToConvertSpan.{lengthWord};
                        for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)";
                itemString =
                    "itemToConvertSpan[itemIndex]"; // this is needed for Memory<T>, since we don't have a foreach method defined, and is likely slightly more performant anyway
            }
            else if (ArrayRank > 1)
            { // normal rank arrays can be handled separately
                StringBuilder arrayStringBuilder = new StringBuilder();
                int i = 0;
                for (i = 0; i < ArrayRank; i++)
                    arrayStringBuilder.AppendLine($"int length{i} = itemToConvert.GetLength({i});");
                for (i = 0; i < ArrayRank; i++)
                {
                    string stringForRank = $"for (int itemIndex{i} = 0; itemIndex{i} < length{i}; itemIndex{i}++)";
                    if (i == ArrayRank - 1)
                        arrayStringBuilder.Append(stringForRank);
                    else
                        arrayStringBuilder.AppendLine(stringForRank);
                }

                forStatement = arrayStringBuilder.ToString();

                string innerArrayText = (String.Join(", ", Enumerable.Range(0, (int)ArrayRank).Select(j => $"itemIndex{j}")));
                itemString = $"itemToConvert[{innerArrayText}]";
            }
            else if (SupportedCollectionType == LazinatorSupportedCollectionType.Queue)
            {
                forStatement =
                    $@"int itemToConvertCount = itemToConvert.{lengthWord};
                        for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)";
                itemStringSetup = $@"
                                        var dequeuedItem = itemToConvert.Dequeue();";
                itemString =
                    "dequeuedItem";
            }
            else if (SupportedCollectionType == LazinatorSupportedCollectionType.LinkedList)
            {
                forStatement =
                    $@"int itemToConvertCount = itemToConvert.{lengthWord};
                        for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)";
                itemString =
                    "System.Linq.Enumerable.ElementAt(itemToConvert, itemIndex)";
            }
            else if (SupportedCollectionType == LazinatorSupportedCollectionType.Stack)
            {
                forStatement =
                    $@"int itemToConvertCount = itemToConvert.{lengthWord};
                        var stackReversed = System.Linq.Enumerable.ToList(itemToConvert);
                        stackReversed.Reverse();
                        for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)";
                itemString =
                    "stackReversed[itemIndex]";
            }
            else if (SupportedCollectionType == LazinatorSupportedCollectionType.SortedSet)
            {

                forStatement =
                    $@"int itemToConvertCount = itemToConvert.{lengthWord};
                        var sortedSet = System.Linq.Enumerable.ToList(itemToConvert);
                        for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)";
                itemString =
                    "sortedSet[itemIndex]";
            }
            else
            {
                forStatement =
                        $@"int itemToConvertCount = itemToConvert.{lengthWord};
                        for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)";
                itemString =
                    "itemToConvert[itemIndex]"; // this is needed for Memory<T>, since we don't have a foreach method defined, and is likely slightly more performant anyway
            }

            if (SupportedCollectionType == LazinatorSupportedCollectionType.Memory && Nullable)
            {
                // we need a method for the Nullable, then an inner method for the non-nullable case
                sb.Append($@"

                    private static void ConvertToBytes_{AppropriatelyQualifiedTypeNameEncodable}(BinaryBufferWriter writer, {AppropriatelyQualifiedTypeName} itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
                    {{
                        if (itemToConvert == null)
                        {{
                            writer.Write((uint)0);
                        }}
                        else
                        {{
                            {DirectConverterTypeNamePrefix}ConvertToBytes_{AppropriatelyQualifiedTypeNameEncodableWithoutNullable}(writer, itemToConvert.Value, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                        }}
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
                        string writeLengthForRankString = $"CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.GetLength({i}));";
                        if (i < ArrayRank - 1)
                            arrayStringBuilder.AppendLine(writeLengthForRankString);
                        else
                            arrayStringBuilder.Append(writeLengthForRankString);
                    }
                    writeCollectionLengthCommand = arrayStringBuilder.ToString();
                }
                else
                    writeCollectionLengthCommand = $"CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.{lengthWord});";
                string writeCommand = InnerProperties[0].GetSupportedCollectionWriteCommands(itemString);
                sb.Append($@"

                    private static void ConvertToBytes_{AppropriatelyQualifiedTypeNameEncodable}(BinaryBufferWriter writer, {AppropriatelyQualifiedTypeName} itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
                    {{
                        {(SupportedCollectionType == LazinatorSupportedCollectionType.Memory ? "" : $@"if (itemToConvert == default({AppropriatelyQualifiedTypeName}))
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

        private void AppendSupportedCollection_ConvertFromBytes(CodeStringBuilder sb)
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
                creationText = $"{AppropriatelyQualifiedTypeName} collection = new {AppropriatelyQualifiedTypeName}(collectionLength);";
            }
            else if (SupportedCollectionType == LazinatorSupportedCollectionType.SortedDictionary || SupportedCollectionType == LazinatorSupportedCollectionType.SortedSet || SupportedCollectionType == LazinatorSupportedCollectionType.LinkedList)
            {
                creationText = $"{AppropriatelyQualifiedTypeName} collection = new {AppropriatelyQualifiedTypeName}();"; // can't initialize collection length
            }
            else if (SupportedCollectionType == LazinatorSupportedCollectionType.Memory)
            {
                creationText =
                    $@"{AppropriatelyQualifiedNameWithoutNullableIndicator} collection = new {AppropriatelyQualifiedNameWithoutNullableIndicator}(new {InnerProperties[0].AppropriatelyQualifiedTypeName}[collectionLength]);
                            var collectionAsSpan = collection.Span;"; // for now, create array on the heap
            }
            else if (isArray)
            {
                if (ArrayRank == 1)
                {
                    string newExpression = ReverseBracketOrder($"{InnerProperties[0].AppropriatelyQualifiedTypeName}[collectionLength]");
                    creationText = $"{AppropriatelyQualifiedTypeName} collection = new {newExpression};";
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
                    string forStatementCommandPart = $"for (int i{i} = 0; i{i} < collectionLength{i}; i{i}++)";
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
                forStatementCommand = $"for (int i = 0; i < collectionLength; i++)";
            }

            PropertyDescription innerProperty = InnerProperties[0];
            CheckForLazinatorInNonLazinator(innerProperty);
            string readCommand = innerProperty.GetSupportedCollectionReadCommands(this);
            sb.Append($@"
                    private static {AppropriatelyQualifiedTypeName} ConvertFromBytes_{AppropriatelyQualifiedTypeNameEncodable}(ReadOnlyMemory<byte> storage)
                    {{
                        if (storage.Length == 0)
                        {{
                            return default({AppropriatelyQualifiedTypeName});
                        }}
                        ReadOnlySpan<byte> span = storage.Span;

                        int bytesSoFar = 0;
                        {readCollectionLengthCommand}

                        {creationText}
                        {forStatementCommand}
                        {{{readCommand}
                        }}

                        return collection;
                    }}");
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
            if (outerProperty.SupportedCollectionType == LazinatorSupportedCollectionType.Array)
            {
                if (outerProperty.ArrayRank == 1)
                {
                    collectionAddItem = "collection[i] = item;";
                    collectionAddNull = $"collection[i] = default({AppropriatelyQualifiedTypeName});";
                }
                else
                {

                    string innerArrayText = (String.Join(", ", Enumerable.Range(0, (int)outerProperty.ArrayRank).Select(j => $"i{j}")));
                    collectionAddItem = $"collection[{innerArrayText}] = item;";
                    collectionAddNull = $"collection[{innerArrayText}] = default({AppropriatelyQualifiedTypeName});";
                }
            }
            else if (outerProperty.SupportedCollectionType == LazinatorSupportedCollectionType.Memory)
            {
                collectionAddItem = "collectionAsSpan[i] = item;";
                collectionAddNull = $"collectionAsSpan[i] = default({AppropriatelyQualifiedTypeName});";
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
                collectionAddNull = $"collection.Enqueue(default({AppropriatelyQualifiedTypeName}));";
            }
            else if (outerProperty.SupportedCollectionType == LazinatorSupportedCollectionType.Stack)
            {
                collectionAddItem = "collection.Push(item);";
                collectionAddNull = $"collection.Push(default({AppropriatelyQualifiedTypeName}));";
            }
            else if (outerProperty.SupportedCollectionType == LazinatorSupportedCollectionType.LinkedList)
            {
                collectionAddItem = "collection.AddLast(item);";
                collectionAddNull = $"collection.AddLast(default({AppropriatelyQualifiedTypeName}));";
            }
            else
            {
                collectionAddItem = "collection.Add(item);";
                collectionAddNull = $"collection.Add(default({AppropriatelyQualifiedTypeName}));";
            }
            if (IsPrimitive)
                return ($@"
                        {AppropriatelyQualifiedTypeName} item = {EnumEquivalentCastToEnum}span.{ReadMethodName}(ref bytesSoFar);
                        {collectionAddItem}");
            else if (IsNonSerializedType)
            {
                if (Nullable)
                    return ($@"
                        int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                        if (lengthCollectionMember == 0)
                        {{
                            {collectionAddNull}
                        }}
                        else
                        {{
                            ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                            var item = {DirectConverterTypeNamePrefix}ConvertFromBytes_{AppropriatelyQualifiedTypeNameEncodable}(childData);
                            {collectionAddItem}
                        }}
                        bytesSoFar += lengthCollectionMember;");
                else
                    return ($@"
                        int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                        ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                        var item = {DirectConverterTypeNamePrefix}ConvertFromBytes_{AppropriatelyQualifiedTypeNameEncodable}(childData);
                            {collectionAddItem}
                        bytesSoFar += lengthCollectionMember;");
            }
            else // Lazinator type
            {
                string lengthCollectionMemberString = null;
                if (IsGuaranteedFixedLength)
                    lengthCollectionMemberString = $"int lengthCollectionMember = {FixedLength};";
                else if (IsGuaranteedSmall)
                    lengthCollectionMemberString = "int lengthCollectionMember = span.ToByte(ref bytesSoFar);";
                else
                    lengthCollectionMemberString = "int lengthCollectionMember = span.ToInt32(ref bytesSoFar);";
                if (Nullable)
                    return ($@"
                        {lengthCollectionMemberString}
                        if (lengthCollectionMember == 0)
                        {{
                            {collectionAddNull}
                        }}
                        else
                        {{
                            ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                            var item = DeserializationFactory.Instance.CreateBasedOnType<{AppropriatelyQualifiedTypeName}>(childData);
                            {collectionAddItem}
                        }}
                        bytesSoFar += lengthCollectionMember;");
                else
                    return (
                        $@"
                        {lengthCollectionMemberString}
                        ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                        var item = new {AppropriatelyQualifiedTypeName}()
                        {{
                            LazinatorObjectBytes = childData,
                        }};
                        {collectionAddItem}
                        bytesSoFar += lengthCollectionMember;");
            }
        }

        private string GetSupportedCollectionWriteCommands(string itemString)
        {
            string GetSupportedCollectionWriteCommandsHelper()
            {
                if (IsPrimitive)
                    return ($@"
                    {WriteMethodName}(writer, {EnumEquivalentCastToEquivalentType}{itemString});");
                else if (IsNonSerializedType)
                    return ($@"
                    void action(BinaryBufferWriter w) => {DirectConverterTypeNamePrefix}ConvertToBytes_{AppropriatelyQualifiedTypeNameEncodable}(writer, {itemString}, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                    WriteToBinaryWith{LengthPrefixTypeString}LengthPrefix(writer, action);");
                else
                    return ($@"
                    void action(BinaryBufferWriter w) => {itemString}.SerializeExistingBuffer(writer, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                    WriteToBinaryWith{LengthPrefixTypeString}LengthPrefix(writer, action);");
            }

            string writeCommand = GetSupportedCollectionWriteCommandsHelper();
            string fullWriteCommands;
            if (Nullable)
            {
                if (PropertyType == LazinatorPropertyType.OpenGenericParameter)
                    fullWriteCommands =
                        $@"
                    if (System.Collections.Generic.EqualityComparer<{AppropriatelyQualifiedTypeName}>.Default.Equals({itemString}, default({AppropriatelyQualifiedTypeName})))
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
                    if ({itemString} == default({AppropriatelyQualifiedTypeName}))
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
                    private static {AppropriatelyQualifiedTypeName} {DirectConverterTypeNamePrefix}ConvertFromBytes_{AppropriatelyQualifiedTypeNameEncodable}(ReadOnlyMemory<byte> storage)
                    {{
                        if (storage.Length == 0)
                        {{
                            return default;
                        }}
                        ReadOnlySpan<byte> span = storage.Span;

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
                        var tupleType = {(SupportedTupleType == LazinatorSupportedTupleType.ValueTuple ? "" : $"new {AppropriatelyQualifiedNameWithoutNullableIndicator}")}({itemnamesLowercase});

                        return tupleType;
                    }}

                    private static void ConvertToBytes_{AppropriatelyQualifiedTypeNameEncodable}(BinaryBufferWriter writer, {AppropriatelyQualifiedTypeName} itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
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

            RecursivelyAppendConversionMethods(sb, alreadyGenerated);
        }

        private string GetSupportedTupleReadCommand(string itemName)
        {
            if (IsPrimitive)
                return ($@"
                        {AppropriatelyQualifiedTypeName} {itemName} = {EnumEquivalentCastToEnum}span.{ReadMethodName}(ref bytesSoFar);");
            else if (IsNonSerializedType)
                return ($@"
                        {AppropriatelyQualifiedTypeName} {itemName} = default;
                        int lengthCollectionMember_{itemName} = span.ToInt32(ref bytesSoFar);
                        if (lengthCollectionMember_{itemName} != 0)
                        {{
                            ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember_{itemName});
                            {itemName} = {DirectConverterTypeNamePrefix}ConvertFromBytes_{AppropriatelyQualifiedTypeNameEncodable}(childData);
                        }}
                        bytesSoFar += lengthCollectionMember_{itemName};");
            else
            {
                CheckForLazinatorInNonLazinator(this);
                if (PropertyType == LazinatorPropertyType.LazinatorStruct && !Nullable)
                    return ($@"
                        {AppropriatelyQualifiedTypeName} {itemName} = default;
                        int lengthCollectionMember_{itemName} = span.ToInt32(ref bytesSoFar);
                        if (lengthCollectionMember_{itemName} != 0)
                        {{
                            ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember_{itemName});
                            {itemName} = new {AppropriatelyQualifiedTypeName}()
                            {{
                                LazinatorObjectBytes = childData,
                            }};
                        }}
                        bytesSoFar += lengthCollectionMember_{itemName};");
                else return ($@"
                        {AppropriatelyQualifiedTypeName} {itemName} = default;
                        int lengthCollectionMember_{itemName} = span.ToInt32(ref bytesSoFar);
                        if (lengthCollectionMember_{itemName} != 0)
                        {{
                            ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember_{itemName});
                            {itemName} = DeserializationFactory.Instance.CreateBasedOnType<{AppropriatelyQualifiedTypeName}>(childData);
                        }}
                        bytesSoFar += lengthCollectionMember_{itemName};");
            }
        }

        private string GetSupportedTupleWriteCommand(string itemName, LazinatorSupportedTupleType outerTupleType, bool outerTypeIsNullable)
        {
            string itemToConvertItemName =
                $"itemToConvert{IIF((outerTupleType == LazinatorSupportedTupleType.ValueTuple || outerTupleType == LazinatorSupportedTupleType.KeyValuePair) && outerTypeIsNullable, ".Value")}.{itemName}";
            if (IsPrimitive)
                return ($@"
                        {WriteMethodName}(writer, {EnumEquivalentCastToEquivalentType}{itemToConvertItemName});");
            else if (IsNonSerializedType)
            {
                if (Nullable)
                    return ($@"
                            if ({itemToConvertItemName} == null)
                            {{
                                {WriteDefaultLengthString}
                            }}
                            else
                            {{
                                void action{itemName}(BinaryBufferWriter w) => {DirectConverterTypeNamePrefix}ConvertToBytes_{AppropriatelyQualifiedTypeNameEncodable}(writer, {itemToConvertItemName}, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                                WriteToBinaryWithIntLengthPrefix(writer, action{itemName});
                            }}");
                else return $@"
                            void action{itemName}(BinaryBufferWriter w) => {DirectConverterTypeNamePrefix}ConvertToBytes_{AppropriatelyQualifiedTypeNameEncodable}(writer, {itemToConvertItemName}, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                            WriteToBinaryWithIntLengthPrefix(writer, action{itemName});";
            }
            else
            {
                if (PropertyType == LazinatorPropertyType.LazinatorStruct && !Nullable)
                    return ($@"
                        void action{itemName}(BinaryBufferWriter w) => {itemToConvertItemName}.SerializeExistingBuffer(writer, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                        WriteToBinaryWith{LengthPrefixTypeString}LengthPrefix(writer, action{itemName});");
                else
                    return ($@"
                        if ({itemToConvertItemName} == null)
                        {{
                            {WriteDefaultLengthString}
                        }}
                        else
                        {{
                            void action{itemName}(BinaryBufferWriter w) => {itemToConvertItemName}.SerializeExistingBuffer(writer, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                            WriteToBinaryWith{LengthPrefixTypeString}LengthPrefix(writer, action{itemName});
                        }};");
            }
        }

        #endregion
        
        #region Interchange types

        public void AppendInterchangeTypes(CodeStringBuilder sb, HashSet<string> alreadyGenerated)
        {
            if (PropertyType != LazinatorPropertyType.NonSelfSerializingType || !HasInterchangeType)
                return;

            if (alreadyGenerated.Contains(AppropriatelyQualifiedTypeNameEncodable))
                return;
            alreadyGenerated.Add(AppropriatelyQualifiedTypeNameEncodable);

            sb.Append($@"
                   private static {AppropriatelyQualifiedTypeName} ConvertFromBytes_{AppropriatelyQualifiedTypeNameEncodable}(ReadOnlyMemory<byte> storage)
                        {{
                            {InterchangeTypeName} interchange = new {InterchangeTypeName}()
                            {{
                                LazinatorObjectBytes = storage
                            }};
                            return interchange.Interchange_{AppropriatelyQualifiedTypeNameEncodable}();
                        }}

                        private static void ConvertToBytes_{AppropriatelyQualifiedTypeNameEncodable}(BinaryBufferWriter writer,
                            {AppropriatelyQualifiedTypeName} itemToConvert, IncludeChildrenMode includeChildrenMode,
                            bool verifyCleanness, bool updateStoredBuffer)
                        {{
                            {InterchangeTypeName} interchange = new {InterchangeTypeName}(itemToConvert);
                            interchange.SerializeExistingBuffer(writer, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                        }}
                        ");
        }

        #endregion
    }
}
