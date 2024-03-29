﻿using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Lazinator.CodeDescription;
using LazinatorGenerator.AttributeClones;
using Microsoft.CodeAnalysis.CSharp;

namespace LazinatorCodeGen.Roslyn
{
    public static class RoslynHelpers
    {
        #region Names

        public static string GetEncodableVersionOfIdentifier(this ISymbol symbol, bool fullyQualified)
        {
            return GetEncodableVersionOfIdentifier(symbol.ToDisplayString(fullyQualified ? SymbolDisplayFormat.FullyQualifiedFormat : SymbolDisplayFormat.MinimallyQualifiedFormat));
        }

        public static string GetEncodableVersionOfIdentifier(string identifier)
        {
            if (identifier.StartsWith("global::"))
                identifier = identifier.Substring(8);
            StringBuilder b = new StringBuilder();
            foreach (var c in identifier)
            {
                if (Char.IsLetterOrDigit(c))
                    b.Append(c);
                else
                {
                    switch (c)
                    {
                        case '[':
                            b.Append("_B");
                            break;
                        case ']':
                            b.Append("_b");
                            break;
                        case '(':
                            b.Append("_P");
                            break;
                        case ')':
                            b.Append("_p");
                            break;
                        case '<':
                            b.Append("_G");
                            break;
                        case '>':
                            b.Append("_g");
                            break;
                        case '.':
                            b.Append("__");
                            break;
                        case '_':
                            b.Append("_u");
                            break;
                        case ',':
                            b.Append("_c");
                            break;
                        case '`':
                            b.Append("_q");
                            break;
                        case '?':
                            b.Append("_n");
                            break;
                        default:
                            b.Append("_C" + ((short)c).ToString());
                            break;
                    }
                }
            }
            return b.ToString();
        }

        public static string PrettyTypeName(INamedTypeSymbol t, bool nullable, bool nullableContextEnabled)
        {
            if (t.IsGenericType)
            {
                IEnumerable<string> innerTypeNames = t.TypeArguments.Select(x => x is INamedTypeSymbol namedx ? PrettyTypeName(namedx, nullable, nullableContextEnabled) : x.Name);
                return string.Format(
                    "{0}<{1}>",
                    t.Name,
                    string.Join(", ", innerTypeNames));
            }

            return RegularizeTypeName(t.Name);
        }

        public static string EncodableTypeName(ITypeSymbol typeSymbol)
        {
            string name = typeSymbol.Name;
            if (typeSymbol is INamedTypeSymbol namedTypeSymbol)
            {
                if (namedTypeSymbol.TupleUnderlyingType != null)
                    name = namedTypeSymbol.TupleUnderlyingType.Name;
                var typeArguments = namedTypeSymbol.TupleUnderlyingType?.TypeArguments ?? namedTypeSymbol.TypeArguments;
                bool isGeneric = namedTypeSymbol.IsGenericType;
                return EncodableTypeName(name, typeArguments);
            }
            return name;
        }

        public static string EncodableTypeName(string name, ImmutableArray<ITypeSymbol> typeArguments)
        {
            if (typeArguments != null && typeArguments.Any())
            { // Note: This will cover Nullable<T> as well as other generics.
                return string.Format(
                    "{0}_{1}",
                    name,
                    string.Join("_", typeArguments.Select(x => EncodableTypeName(x))));
            }

            string regularized = RegularizeTypeName(name);

            return regularized;
        }

        private static readonly Dictionary<string, string> TypeRegularization = new Dictionary<string, string>()
        {
            { "Boolean", "bool" },
            { "Byte", "byte" },
            { "SByte", "sbyte" },
            { "Char", "char" },
            { "Decimal", "decimal" },
            { "Single", "float" },
            { "Double", "double" },
            { "Int16", "short" },
            { "UInt16", "ushort" },
            { "Int32", "int" },
            { "UInt32", "uint" },
            { "Int64", "long" },
            { "UInt64", "ulong" },
            { "String", "string" },
        };

        public static string RegularizeTypeName(string typeName)
        {
            if (TypeRegularization.ContainsKey(typeName))
                return TypeRegularization[typeName];
            var withoutNullableIndicator = WithoutNullableIndicator(typeName);
            if (TypeRegularization.ContainsKey(withoutNullableIndicator))
                return TypeRegularization[withoutNullableIndicator] + "?"; // restore ?
            return typeName;
        }

        public static string WithoutNullableIndicator(string typeName)
        {
            if (typeName.EndsWith("?"))
                return typeName.Substring(0, typeName.Length - 1);
            else
                return typeName;
        }


        private static SymbolDisplayFormat MinimallyQualifiedFormat(bool outputNullableModeEnabled) => outputNullableModeEnabled ? SymbolDisplayFormat.MinimallyQualifiedFormat.WithMiscellaneousOptions(SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier) : SymbolDisplayFormat.MinimallyQualifiedFormat.WithMiscellaneousOptions(SymbolDisplayMiscellaneousOptions.None);
        private static SymbolDisplayFormat FullyQualifiedFormat(bool outputNullableModeEnabled) => outputNullableModeEnabled ? SymbolDisplayFormat.FullyQualifiedFormat.WithMemberOptions(SymbolDisplayMemberOptions.IncludeContainingType).WithMiscellaneousOptions(SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier) : SymbolDisplayFormat.FullyQualifiedFormat.WithMemberOptions(SymbolDisplayMemberOptions.IncludeContainingType).WithMiscellaneousOptions(SymbolDisplayMiscellaneousOptions.None);

        public static string GetMinimallyQualifiedName(this ISymbol symbol, bool outputNullableModeEnabled)
        {
            return symbol.ToDisplayString(MinimallyQualifiedFormat(outputNullableModeEnabled));
        }

        public static string GetFullyQualifiedName(this ISymbol symbol, bool outputNullableModeEnabled)
        {
            return symbol.ToDisplayString(FullyQualifiedFormat(outputNullableModeEnabled));
        }

        public static string GetFullyQualifiedNameWithoutGlobal(this ISymbol symbol, bool outputNullableModeEnabled)
        {
            return symbol.ToDisplayString(FullyQualifiedFormat(outputNullableModeEnabled).WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted));
        }

        public static string GetFullyQualifiedMetadataName(this ISymbol symbol)
        {
            if (symbol == null)
                return "";
            return GetFullNamespace(symbol) + "." + symbol.MetadataName;
        }

        public static string GetFullNamespacePlusSimpleName(this ISymbol symbol)
        {
            if (symbol == null)
                return "";
            return GetFullNamespace(symbol) + "." + symbol.Name;
        }

        public static string GetFullNamespace(this ISymbol symbol)
        {
            if (symbol?.ContainingNamespace == null)
                return "";
            string higherNamespace = GetFullNamespace(symbol.ContainingNamespace);
            if (higherNamespace != "")
                return higherNamespace + "." + symbol.ContainingNamespace.Name;
            else
                return symbol.ContainingNamespace.Name;
        }

        public static string GetNameWithoutGenericArity(Type t)
        {
            string name = t.Name;
            return GetNameWithoutGenericArity(name);
        }

        public static string GetNameWithoutGenericArity(string name)
        {
            int index = name.IndexOf('`');
            return index == -1 ? name : name.Substring(0, index);
        }

        public static string GetFullMetadataName(this ISymbol s)
        {
            if (s == null || IsRootNamespace(s))
            {
                return string.Empty;
            }

            var sb = new StringBuilder(s.MetadataName);
            var last = s;

            s = s.ContainingSymbol;

            while (!IsRootNamespace(s))
            {
                if (s is ITypeSymbol && last is ITypeSymbol)
                {
                    sb.Insert(0, '+');
                }
                else
                {
                    sb.Insert(0, '.');
                }

                sb.Insert(0, s.OriginalDefinition.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat));
                //sb.Insert(0, s.MetadataName);
                s = s.ContainingSymbol;
            }

            return sb.ToString();
        }

        private static bool IsRootNamespace(ISymbol symbol)
        {
            INamespaceSymbol s = null;
            return ((s = symbol as INamespaceSymbol) != null) && s.IsGlobalNamespace;
        }

        public static IEnumerable<ITypeSymbol> GetTypeAndContainedTypes(this ITypeSymbol typeSymbol)
        {
            if (typeSymbol is INamedTypeSymbol namedTypeSymbol)
            {
                yield return namedTypeSymbol;
                foreach (var innerType in namedTypeSymbol.TypeArguments)
                    if (innerType is INamedTypeSymbol namedInnerType)
                        foreach (ITypeSymbol innerTypeOrWithin in GetTypeAndContainedTypes(namedInnerType))
                            yield return innerTypeOrWithin;
                if (namedTypeSymbol.BaseType != null)
                    foreach (var baseType in GetTypeAndContainedTypes(namedTypeSymbol.BaseType))
                        yield return baseType;
            }
        }

        public static IEnumerable<ITypeSymbol> GetContainingTypes(this ITypeSymbol typeSymbol)
        {
            ITypeSymbol current = typeSymbol.ContainingType;
            Stack<ITypeSymbol> stack = new Stack<ITypeSymbol>();
            while (current != null)
            {
                stack.Push(current);
                current = current.ContainingType;
            }
            while (stack.Any())
                yield return stack.Pop();
        }

        public static IEnumerable<string> GetNamespacesOfTypesAndContainedTypes(this ITypeSymbol typeSymbol)
        {
            return GetTypeAndContainedTypes(typeSymbol).Select(x => x.GetFullNamespace());
        }

        public static IEnumerable<string> GetNamespacesOfContainingTypes(this ITypeSymbol typeSymbol)
        {
            return GetContainingTypes(typeSymbol).Select(x => x.GetFullNamespace());
        }

        #endregion

        #region Properties

        public static ImmutableList<RoslynProperty> GetPropertiesWithAccessors(this INamedTypeSymbol namedTypeSymbol)
        {
            var properties = namedTypeSymbol.GetMembers().Where(x => x.Kind == SymbolKind.Property).ToList();
            var methods = namedTypeSymbol.GetMembers().Where(x => x.Kind == SymbolKind.Method).ToList();
            return properties.Select(x => new RoslynProperty() { Property = x, GetMethod = methods.SingleOrDefault(y => y.Name == "get_" + x.Name), SetMethod = methods.SingleOrDefault(y => y.Name == "set_" + x.Name) }).ToImmutableList();
        }

        public static ImmutableList<IPropertySymbol> GetPropertySymbols(this INamedTypeSymbol namedTypeSymbol)
        {
            var properties = namedTypeSymbol.GetMembers().Where(x => x.Kind == SymbolKind.Property).Cast<IPropertySymbol>().ToImmutableList();
            return properties;
        }

        public static ImmutableList<IPropertySymbol> GetPropertySymbolsBaseLevels(this INamedTypeSymbol namedTypeSymbol, bool includeOnlyLowerLevelPropertiesFromInterfaces)
        {
            if (includeOnlyLowerLevelPropertiesFromInterfaces)
                return namedTypeSymbol.AllInterfaces.SelectMany(x => x.GetPropertySymbols()).ToImmutableList();
            var baseType = namedTypeSymbol.BaseType;
            ImmutableList<IPropertySymbol> result = ImmutableList.Create<IPropertySymbol>();
            while (baseType != null && baseType.Name != "Object")
            {
                result = result.AddRange(baseType.GetPropertySymbols());
                baseType = baseType.BaseType;
            }
            return result;
        }

        public static void GetPropertiesForType(this INamedTypeSymbol namedSymbolType, bool includeOnlyLowerLevelPropertiesFromInterfaces, out ImmutableList<IPropertySymbol> propertiesThisLevel, out ImmutableList<IPropertySymbol> propertiesLowerLevels)
        {
            propertiesThisLevel = namedSymbolType.GetPropertySymbols();
            propertiesLowerLevels = namedSymbolType.GetPropertySymbolsBaseLevels(includeOnlyLowerLevelPropertiesFromInterfaces);
        }

        #endregion

        #region Attributes

        public static IEnumerable<Attribute> GetKnownAttributes(ISymbol symbol)
        {
            return symbol.GetAttributes().Select(x => AttributeConverter.ConvertAttribute(x)).Where(x => x != null);
        }

        public static IEnumerable<T> GetKnownAttributes<T>(ISymbol symbol) where T : Attribute
        {
            return symbol.GetAttributes().Select(x => AttributeConverter.ConvertAttribute(x)).OfType<T>();
        }

        public static T GetKnownAttribute<T>(this ISymbol symbol) where T : Attribute
        {
            return GetKnownAttributes<T>(symbol).SingleOrDefault();
        }

        public static IEnumerable<T> GetAttributesIncludingBase<T>(this INamedTypeSymbol symbol) where T : Attribute
        {
            foreach (T t in GetKnownAttributes<T>(symbol))
                yield return t;
            if (symbol.BaseType != null)
                foreach (T t in symbol.BaseType.GetAttributesIncludingBase<T>())
                    yield return t;
        }

        public static bool HasAttributeOfType<T>(this ISymbol symbol) where T : Attribute
        {
            return GetKnownAttributes<T>(symbol).Any();
        }

        public static bool HasAttribute(this ISymbol symbol, INamedTypeSymbol attributeSymbol)
        {
            return symbol.GetAttributes().Any(x => x.AttributeClass.GetFullNamespacePlusSimpleName().Equals(attributeSymbol.GetFullNamespacePlusSimpleName()));
        }

        public static INamedTypeSymbol GetTopLevelInterfaceImplementingAttribute(this INamedTypeSymbol lazinatorObject, INamedTypeSymbol attributeType)
        {
            var allInterfaces = lazinatorObject.Interfaces
                        .Where(x => x.HasAttribute(attributeType))
                        .OrderByDescending(x => x.GetMembers().Count());
            var result = allInterfaces
                        .FirstOrDefault();
            var remaining = allInterfaces.Skip(1);
            if (result == null)
                return null;
            return result;

        }

        public static List<INamedTypeSymbol> GetInterfacesWithAttributeOfType<T>(this INamedTypeSymbol namedTypeSymbol)
        {
            ImmutableArray<ISymbol> members = namedTypeSymbol.GetMembers();
            var interfaces =
                namedTypeSymbol.AllInterfaces
                    .Where(x => x.HasAttributeOfType<CloneLazinatorAttribute>())
                    .OrderByDescending(x => x.GetMembers().Length)
                    .ToList();
            return interfaces;
        }

        public static object GetAttributeConstructorValueByParameterName
        (
            this AttributeData attributeData,
            string argName
        )
        {

            if (attributeData.AttributeConstructor == null)
                return null; // attribute is not fully formed yet

            // Get the parameter
            IParameterSymbol parameterSymbol = attributeData.AttributeConstructor
                .Parameters
                .Where((constructorParam) => constructorParam.Name == argName).FirstOrDefault();

            // get the index of the parameter
            int parameterIdx = attributeData.AttributeConstructor.Parameters.IndexOf(parameterSymbol);
            if (parameterIdx == -1)
                return null;

            // get the construct argument corresponding to this parameter
            TypedConstant constructorArg = attributeData.ConstructorArguments[parameterIdx];

            // return the value passed to the attribute
            return constructorArg.Value;
        }

        public static object[] GetAttributeConstructorValuesByParameterName
        (
            this AttributeData attributeData,
            string argName
        )
        {

            if (attributeData.AttributeConstructor == null)
                return null; // attribute is not fully formed yet

            // Get the parameter
            IParameterSymbol parameterSymbol = attributeData.AttributeConstructor
                .Parameters
                .Where((constructorParam) => constructorParam.Name == argName).FirstOrDefault();

            // get the index of the parameter
            int parameterIdx = attributeData.AttributeConstructor.Parameters.IndexOf(parameterSymbol);
            if (parameterIdx == -1)
                return null;

            // get the construct argument corresponding to this parameter
            TypedConstant constructorArg = attributeData.ConstructorArguments[parameterIdx];

            // return the value passed to the attribute
            return constructorArg.Values.Select(x => x.Value).ToArray();
        }

        #endregion

        #region Types and interfaces

        public static INamedTypeSymbol GetCorrespondingExclusiveInterface(INamedTypeSymbol type)
        {
            var allInterfaces = type.Interfaces;
            //var minimalInterfaces = allInterfaces.Except
            //        (allInterfaces.SelectMany(t => t.GetInterfaces()))
            //    .Where(x => x.GetCustomAttributes(typeof(LazinatorAttribute), false).Any())
            //    .ToList();
            if (allInterfaces.Count() != 1)
                throw new LazinatorCodeGenException(
                    $"There must be a one-to-one relationship between each ILazinator type and an interface, directly implemented by that type, with a Lazinator attribute. The type {type} directly implements {allInterfaces.Count()} interfaces with a Lazinator attribute.");
            var correspondingInterface = allInterfaces.Single();
            return correspondingInterface;
        }

        public static bool TypeDeclarationIncludesMethod(this TypeDeclarationSyntax typeDeclaration, string methodName)
        {
            HashSet<MethodDeclarationSyntax> methodDeclarations = GetMethodDeclarations(typeDeclaration);
            return methodDeclarations.Any(x => x.Identifier.Text == methodName);
        }

        public static bool TypeDeclarationIncludesParameterlessConstructor(this TypeDeclarationSyntax typeDeclaration)
        {
            return typeDeclaration.ChildNodes().OfType<ConstructorDeclarationSyntax>().Cast<ConstructorDeclarationSyntax>().Any(x => !x.ParameterList.Parameters.Any());
        }

        public static bool TypeDeclarationIncludesAttribute(this TypeDeclarationSyntax typeDeclaration, string attributeName)
        {
            return typeDeclaration.AttributeLists.Any(y => y.Attributes.Any(z => ((string)(z.Name as IdentifierNameSyntax)?.Identifier.Value) == attributeName));
        }

        public static HashSet<MethodDeclarationSyntax> GetMethodDeclarations(this TypeDeclarationSyntax typeDeclaration)
        {
            return new HashSet<MethodDeclarationSyntax>(typeDeclaration.DescendantNodes()
                .Where(x => x is MethodDeclarationSyntax)
                .Cast<MethodDeclarationSyntax>());
        }

        public static HashSet<TypeDeclarationSyntax> GetSubtypeDeclarations(this TypeDeclarationSyntax typeDeclaration)
        {
            return new HashSet<TypeDeclarationSyntax>(typeDeclaration.DescendantNodes()
                .Where(x => x is TypeDeclarationSyntax)
                .Cast<TypeDeclarationSyntax>());
        }

        public static bool IsRecord(this ITypeSymbol type)
        {
            if (type == null || type.TypeKind != TypeKind.Class)
                return false;
            // Syntax analysis (we are trying to avoid this because we want to be able to inspect external types): bool isRecord = (type.DeclaringSyntaxReferences.Any(x => (x.SyntaxTree.GetRoot().FindNode(x.Span) is RecordDeclarationSyntax)));
            bool isRecord = type.GetMembers().Any(x => x.Kind == SymbolKind.Property && x.Name == "EqualityContract" && x.IsImplicitlyDeclared);
            return isRecord;
        }

        public static bool IsReadOnlyStruct(this ITypeSymbol type)
        {
            if (type == null || type.TypeKind != TypeKind.Struct)
                return false;
            bool isReadOnly = type.IsReadOnly;
            return isReadOnly;
        }

        public static bool IsNonAbstractTypeWithConstructor(this INamedTypeSymbol type)
        {
            return type.Constructors.Any() && type.TypeKind == TypeKind.Class && !type.IsAbstract;
        }

        #endregion

        #region Compilation

        public static IEnumerable<ISymbol> GetAllSymbols(CSharpCompilation compilation, SyntaxNode root)
        {
            var noDuplicates = new HashSet<ISymbol>(SymbolEqualityComparer.Default);

            var model = compilation.GetSemanticModel(root.SyntaxTree);

            foreach (var node in root.DescendantNodesAndSelf())
            {
                switch (node.Kind())
                {
                    case SyntaxKind.ExpressionStatement:
                    case SyntaxKind.InvocationExpression:
                        break;
                    default:
                        ISymbol symbol = model.GetSymbolInfo(node).Symbol;

                        if (symbol != null)
                        {
                            if (noDuplicates.Add(symbol))
                                yield return symbol;
                        }
                        break;
                }
            }
        }


        public static NullableContext GetNullableContextForSymbol(this ISymbol symbol, Compilation compilation, bool verifyAllMatch = true)
        {
            bool definitelyNullableEnabled = symbol.ToString().EndsWith("?");
            SyntaxReference syntaxReference = symbol.DeclaringSyntaxReferences.FirstOrDefault();
            if (syntaxReference == null)
                return definitelyNullableEnabled ? NullableContext.Enabled : NullableContext.ContextInherited;
            var syntaxTree = syntaxReference.SyntaxTree;
            if (!compilation.ContainsSyntaxTree(syntaxTree))
            {
                // This symbol is not in the compilation. (This may happen if we create Lazinator code for an object from some other assembly, for
                // example for testing purposes.)
                // In this case, we'll just assume for now that the nullable context on the original is disabled, unless it's not nullable.
                return NullableContext.Disabled;
            }
            var nullableContext = compilation.GetSemanticModel(syntaxTree).GetNullableContext(syntaxReference.Span.Start);
            for (int i = 0; i < symbol.DeclaringSyntaxReferences.Length; i++)
            {
                SyntaxReference declaringSyntaxReference = symbol.DeclaringSyntaxReferences[i];
                int spanStart = declaringSyntaxReference.Span.Start;
                var nullableContext2 = compilation.GetSemanticModel(declaringSyntaxReference.SyntaxTree).GetNullableContext(spanStart);
                if (nullableContext.AnnotationsEnabled() != nullableContext2.AnnotationsEnabled())
                {
                    throw new LazinatorCodeGenException($"Lazinator requires that nullability context must be same for all instances of type symbol {symbol}.");
                }
            }

            return nullableContext;
        }

        public static bool ParentAndChildShareNullabilityContext(NullableContext parentContext, NullableContext childContext) => parentContext.AnnotationsEnabled() == childContext.AnnotationsEnabled();

        #endregion
    }
}
