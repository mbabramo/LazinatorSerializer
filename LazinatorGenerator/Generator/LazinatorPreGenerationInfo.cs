﻿using Lazinator.CodeDescription;
using LazinatorCodeGen.Roslyn;
using LazinatorGenerator.CodeDescription;
using LazinatorGenerator.Settings;
using LazinatorGenerator.Support;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;

namespace LazinatorGenerator.Generator
{
    internal struct LazinatorPreGenerationInfo
    {
        internal GeneratorAttributeSyntaxContext SyntaxContext;
        internal string LazinatorObjectDeclarations;
        internal LazinatorConfig Config;
        internal SemanticModel SemanticModel => SyntaxContext.SemanticModel;
        internal CSharpCompilation Compilation => (CSharpCompilation) SemanticModel.Compilation;
        internal INamedTypeSymbol InterfaceSymbol => (INamedTypeSymbol) SyntaxContext.TargetSymbol;
        internal LazinatorPreGenerationInfo(GeneratorAttributeSyntaxContext syntaxContext, LazinatorConfig config)
        {
            SyntaxContext = syntaxContext;
            Config = config;

            var interfaceSymbol = syntaxContext.SemanticModel.GetDeclaredSymbol(syntaxContext.TargetNode);
            if (interfaceSymbol is not INamedTypeSymbol namedTypeSymbol || namedTypeSymbol.TypeKind != TypeKind.Interface)
            {
                return; // Not an interface or symbol could not be determined
            }

            var interfaceFullName = interfaceSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

            ImmutableArray<INamedTypeSymbol> implementations = Compilation.SyntaxTrees
                .SelectMany(syntaxTree =>
                {
                    var semanticModel = syntaxContext.SemanticModel.Compilation.GetSemanticModel(syntaxTree); // Correctly obtain the SemanticModel here
                    return syntaxTree.GetRoot().DescendantNodes()
                        .Select(node => semanticModel.GetDeclaredSymbol(node) as INamedTypeSymbol) // Use the obtained SemanticModel
                        .Where(typeSymbol => typeSymbol != null && typeSymbol.AllInterfaces.Any(intf => intf.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) == interfaceFullName));
                })
                .ToImmutableArray();

            // Adjusting this to only include the declaration syntax of each implementing type
            LazinatorObjectDeclarations = String.Join(";", implementations
                .SelectMany(impl => impl.DeclaringSyntaxReferences
                    .Select(syntaxReference => syntaxReference.GetSyntax().SyntaxTree.GetRoot().DescendantNodes()
                        .OfType<BaseTypeDeclarationSyntax>() // Filter for ClassDeclarationSyntax and StructDeclarationSyntax nodes
                        .FirstOrDefault(node => node.Identifier.ValueText == impl.Name))) // Ensure we're matching the correct class/struct declaration
                .Where(node => node != null) // Ensure that we have valid syntax nodes
                .Select(x => GetSignature(x)));
        }

        static string GetSignature(BaseTypeDeclarationSyntax node)
        {
            StringBuilder signature = new StringBuilder();

            // Add modifiers (public, sealed, abstract, etc.)
            foreach (var modifier in node.Modifiers)
            {
                signature.Append(modifier.Text + " ");
            }

            // Add type keyword (class or struct) and name
            if (node is ClassDeclarationSyntax)
            {
                signature.Append("class ");
            }
            else if (node is StructDeclarationSyntax)
            {
                signature.Append("struct ");
            }
            signature.Append(node.Identifier.ValueText);

            // Add base types and implemented interfaces
            if (node.BaseList != null)
            {
                signature.Append(" : ");
                foreach (var baseType in node.BaseList.Types)
                {
                    signature.Append(baseType.ToString() + ", ");
                }
                // Remove the last comma and space if they exist
                if (node.BaseList.Types.Count > 0)
                {
                    signature.Length -= 2;
                }
            }

            // Optionally, include attribute lists applied to the type
            foreach (var attributeList in node.AttributeLists)
            {
                foreach (var attribute in attributeList.Attributes)
                {
                    signature.Insert(0, $"[{attribute}] ");
                }
            }

            return signature.ToString();
        }


        public override int GetHashCode()
        {
            return (SyntaxContext, Config, LazinatorObjectDeclarations).GetHashCode();
        }
        
        public override bool Equals(object obj)
        {
            if (!(obj is LazinatorPreGenerationInfo other))
                return false;
            bool returnVal = Config.Equals(other.Config) && SyntaxContext.TargetNode.IsEquivalentTo(other.SyntaxContext.TargetNode, false) && LazinatorObjectDeclarations == other.LazinatorObjectDeclarations;
            return returnVal;
        }

        private static bool AreDeclarationsEquivalent(ImmutableArray<BaseTypeDeclarationSyntax> array1, ImmutableArray<BaseTypeDeclarationSyntax> array2)
        {
            // Check if the arrays have the same number of elements
            if (array1.Length != array2.Length) return false;

            // Since there's no guarantee the declarations are in the same order,
            // we must ensure each element in one array has an equivalent in the other.
            // This is a simplistic approach and might need optimization for large arrays.
            foreach (var item1 in array1)
            {
                bool foundEquivalent = false;
                foreach (var item2 in array2)
                {
                    if (item1.IsEquivalentTo(item2))
                    {
                        foundEquivalent = true;
                        break;
                    }
                }
                if (!foundEquivalent) return false;
            }

            // If we reach here, every element in array1 has an equivalent in array2
            return true;
        }


        internal LazinatorCodeGenerationResult ExecuteSourceGeneration(IDateTimeNow dateTimeNowProvider)
        {
            LazinatorPairInformation pairInfo = GetLazinatorPairInformation(); 
            if (pairInfo == null)
                return new LazinatorCodeGenerationResult(null, null, null, default, default);
            LazinatorImplementingTypeInfo implementingTypeInfo = new LazinatorImplementingTypeInfo(Compilation, pairInfo.LazinatorObject, Config);

            try
            {
                var objectDescription = new LazinatorObjectDescription(implementingTypeInfo.ImplementingTypeSymbol, implementingTypeInfo, Config, dateTimeNowProvider, false);
                var generatedCode = objectDescription.GetCodeBehind();
                string path = objectDescription.ObjectNameEncodable + Config.GeneratedCodeFileExtension;
                return new LazinatorCodeGenerationResult(objectDescription.FullyQualifiedObjectName_InNullableMode, path, generatedCode, implementingTypeInfo.GetDependencyInfo(), null);
            }
            catch (LazinatorCodeGenException e)
            {
                var descriptor = new DiagnosticDescriptor(
                    id: "LAZIN",
                    title: "Lazinator code generation error",
                    messageFormat: e.Message,
                    category: "tests",
                    defaultSeverity: DiagnosticSeverity.Error,
                    isEnabledByDefault: true);
                Diagnostic diagnostic = Diagnostic.Create(descriptor, pairInfo.PrimaryLocation);
                return new LazinatorCodeGenerationResult(implementingTypeInfo.ImplementingTypeSymbol.GetFullyQualifiedNameWithoutGlobal(true), null, null, implementingTypeInfo.GetDependencyInfo(), diagnostic);
            }
        }

        private LazinatorPairInformation GetLazinatorPairInformation()
        {
            LazinatorPairFinder analyzer = new LazinatorPairFinder(Compilation, Config); // we're not running the analyzer, but the analyzer code can help us get the LazinatorPairInfo.
            LazinatorPairInformation pairInfo = analyzer.GetLazinatorPairInfo(Compilation, InterfaceSymbol);
            return pairInfo;
        }

    }
}
