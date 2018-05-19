using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading.Tasks;
using Lazinator.CodeDescription;
using LazinatorCodeGen.AttributeClones;
using LazinatorAnalyzer.Roslyn;
using Microsoft.CodeAnalysis.CSharp;

namespace LazinatorCodeGen.Roslyn
{
    public static class RoslynHelpers
    {

        public static string GetEncodableVersionOfIdentifier(this ISymbol symbol)
        {
            return GetEncodableVersionOfIdentifier(symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
        }

        public static string GetEncodableVersionOfIdentifier(string identifier)
        {
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
                            b.Append("@B");
                            break;
                        case ']':
                            b.Append("@B");
                            break;
                        case '(':
                            b.Append("@B");
                            break;
                        case ')':
                            b.Append("@B");
                            break;
                        case '@':
                            b.Append("@@");
                            break;
                        case '.':
                            b.Append("_");
                            break;
                        case '_':
                            b.Append("@_");
                            break;
                        case ',':
                            b.Append("@A");
                            break;
                        default:
                            b.Append("@u" + ((short)c).ToString());
                            break;
                    }
                }
            }
            return b.ToString();
        }

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

        public static ImmutableList<IPropertySymbol> GetPropertySymbolsBaseLevels(this INamedTypeSymbol namedTypeSymbol)
        {
            return namedTypeSymbol.AllInterfaces.SelectMany(x => x.GetPropertySymbols()).ToImmutableList();
        }

        public static void GetPropertiesForType(this INamedTypeSymbol namedSymbolType, out ImmutableList<IPropertySymbol> propertiesThisLevel, out ImmutableList<IPropertySymbol> propertiesLowerLevels)
        {
            propertiesThisLevel = namedSymbolType.GetPropertySymbols();
            propertiesLowerLevels = namedSymbolType.GetPropertySymbolsBaseLevels();
        }

        public static IEnumerable<Attribute> GetKnownAttributes(ISymbol symbol)
        {
            return symbol.GetAttributes().Select(x => AttributeConverter.ConvertAttribute(x)).Where(x => x != null);
        }

        public static IEnumerable<T> GetKnownAttributes<T>(ISymbol symbol) where T : Attribute
        {
            return symbol.GetAttributes().Select(x => AttributeConverter.ConvertAttribute(x)).OfType<T>();
        }

        public static T GetKnownAttribute<T>(ISymbol symbol) where T : Attribute
        {
            return GetKnownAttributes<T>(symbol).SingleOrDefault();
        }


        public static bool HasAttributeOfType<T>(this ISymbol symbol) where T : Attribute
        {
            return GetKnownAttributes<T>(symbol).Any();
        }

        public static bool HasAttribute(this ISymbol symbol, INamedTypeSymbol attributeSymbol)
        {
            return symbol.GetAttributes().Any(x => x.AttributeClass.GetFullyQualifiedName().Equals(attributeSymbol.GetFullyQualifiedName()));
        }

        public static INamedTypeSymbol GetTopLevelInterfaceImplementingAttribute(this INamedTypeSymbol lazinatorObject, INamedTypeSymbol attributeType)
        {
            return lazinatorObject.Interfaces
                        .Where(x => x.HasAttribute(attributeType))
                        .OrderByDescending(x => x.GetMembers().Count())
                        .FirstOrDefault();
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


        public static string PrettyTypeName(INamedTypeSymbol t)
        {
            if (t.IsGenericType)
            {
                IEnumerable<string> innerTypeNames = t.TypeArguments.Select(x => x is INamedTypeSymbol namedx ? PrettyTypeName(namedx) : x.Name);
                return string.Format(
                    "{0}<{1}>",
                    t.Name,
                    string.Join(", ", innerTypeNames));
            }

            return RegularizeTypeName(t.Name);
        }


        public static string GetSubclassHierarchy(this ITypeSymbol typeSymbol)
        {
            string hierarchy = "";
            ITypeSymbol current = typeSymbol;
            while (current.ContainingType != null)
            {
                hierarchy += current.ContainingType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat) + ".";
                current = current.ContainingType;
            }
            return hierarchy;
        }

        public static string GetSubclassHierarchyEncodable(this ITypeSymbol typeSymbol)
        {
            string hierarchy = "";
            ITypeSymbol current = typeSymbol;
            while (current.ContainingType != null)
            {
                hierarchy += EncodableTypeName(current.ContainingType) + "_";
                current = current.ContainingType;
            }
            return hierarchy;
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
            return typeName;
        }

        public static string GetFullyQualifiedName(this ISymbol symbol)
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

        public static bool TypeDeclarationIncludesMethod(TypeDeclarationSyntax typeDeclaration, string methodName)
        {
            HashSet<MethodDeclarationSyntax> methodDeclarations = GetMethodDeclarations(typeDeclaration);
            return methodDeclarations.Any(x => x.Identifier.Text == methodName);
        }

        private static HashSet<MethodDeclarationSyntax> GetMethodDeclarations(TypeDeclarationSyntax typeDeclaration)
        {
            return new HashSet<MethodDeclarationSyntax>(typeDeclaration.DescendantNodes().Where(x => x is MethodDeclarationSyntax).Cast<MethodDeclarationSyntax>());
        }

        public static bool IsReadOnlyStruct(this ITypeSymbol type)
        {
            if (type == null || type.TypeKind != TypeKind.Struct)
                return false;
            // It does not appear to be possible to determine this only through the semantic model, as DeclarationModifiers is not in the public API of Roslyn. Thus, we look at the Syntax tree.
            bool isReadOnly = (type.DeclaringSyntaxReferences
                    .Any(x => ((StructDeclarationSyntax)x.SyntaxTree.GetRoot().FindNode(x.Span))
                        .Modifiers.Any(y => y.Text == "readonly")));
            return isReadOnly;
        }
    }
}
