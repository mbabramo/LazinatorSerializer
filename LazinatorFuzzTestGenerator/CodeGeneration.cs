using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LazinatorGenerator.Generator;
using LazinatorGenerator.Settings;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Reflection;
using CodeGenHelper;

namespace LazinatorFuzzTestGenerator
{
    internal class CodeGeneration
    {
        public static string GetCodeBasePath(string project = "")
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            string codeBaseLocation = assembly.Location;
            string projectInOverallFolder = "LazinatorSerializer\\";
            string throughProject =
                codeBaseLocation.Substring(0, codeBaseLocation.IndexOf(projectInOverallFolder) + projectInOverallFolder.Length) + project;
            //throughProject = throughProject.Substring("file:///".Length);
            return throughProject;
        }

        public static Compilation CreateCompilation(List<(string folder, string filename, string code)> files) => CreateCompilation(GetCodeBasePath(), files);

        public static Compilation CreateCompilation(string solutionFolder, List<(string folder, string filename, string code)> files)
        {
            var syntaxTrees = new List<SyntaxTree>();

            foreach (var file in files)
            {
                var syntaxTree = CSharpSyntaxTree.ParseText(file.code);
                syntaxTrees.Add(syntaxTree);
            }

            // Assuming .NET 8.0 libraries are available in the standard .NET installation path or nuget cache,
            // and assuming the projects build their outputs to a standard location relative to the solution folder.
            var netStandardPath = typeof(object).Assembly.Location;
            string netCorePath = Path.GetDirectoryName(typeof(object).GetTypeInfo().Assembly.Location)!;
            var lazinatorDllPath = Path.Combine(solutionFolder, "Lazinator", "bin", "Debug", "net8.0", "Lazinator.dll");
            var lazinatorCollectionsDllPath = Path.Combine(solutionFolder, "Lazinator.Collections", "bin", "Debug", "net8.0", "Lazinator.Collections.dll");

            bool useProjectReferences = false;
            List<MetadataReference> references = new List<MetadataReference>();
            if ( useProjectReferences)
            {
                // DEBUG references = AdhocWorkspaceManager.GetProjectReferences();
            }
            else
                references = new List<MetadataReference>
                {
                    MetadataReference.CreateFromFile(lazinatorDllPath),
                    MetadataReference.CreateFromFile(lazinatorCollectionsDllPath),
                    MetadataReference.CreateFromFile(netStandardPath),
                    MetadataReference.CreateFromFile(Path.Combine(netCorePath!, "System.Runtime.dll")), // System.Runtime.dll
                    MetadataReference.CreateFromFile(Path.Combine(netCorePath!, "System.Collections.dll")), // System.Collections.dll
                    MetadataReference.CreateFromFile(typeof(Queryable).Assembly.Location), // System.Linq.Queryable.dll
                    MetadataReference.CreateFromFile(typeof(int).Assembly.Location), // System.Private.CoreLib.dll
                    // Add other necessary references here
                };



            var compilation = CSharpCompilation.Create(
                "DynamicCompilation",
                syntaxTrees: syntaxTrees,
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            return compilation;
        }

        public static List<AttributeSyntax> FindLazinatorAttributes(Compilation compilation)
        {
            var lazinatorAttributeType = "Lazinator";
            var lazinatorAttributes = new List<AttributeSyntax>();

            foreach (var tree in compilation.SyntaxTrees)
            {
                var semanticModel = compilation.GetSemanticModel(tree);
                var attributeSyntaxes = tree.GetRoot().DescendantNodes().OfType<AttributeSyntax>();

                foreach (var attributeSyntax in attributeSyntaxes)
                {
                    var attributeType = semanticModel.GetTypeInfo(attributeSyntax).Type;
                    if (attributeType != null && attributeType.ToDisplayString() == lazinatorAttributeType)
                    {
                        lazinatorAttributes.Add(attributeSyntax);
                    }
                }
            }

            return lazinatorAttributes;
        }

        public static List<INamedTypeSymbol> FindTypesWithLazinatorAttribute(Compilation compilation)
        {
            var lazinatorAttributeTypeString = "Lazinator";
            var typesWithLazinatorAttribute = new List<INamedTypeSymbol>();

            var lazinatorAttributes = FindLazinatorAttributes(compilation);

            foreach (var attributeSyntax in lazinatorAttributes)
            {
                var semanticModel = compilation.GetSemanticModel(attributeSyntax.SyntaxTree);
                var attributeSymbolInfo = semanticModel.GetSymbolInfo(attributeSyntax);

                // Move up the syntax tree to find the attributed type's declaration
                var typeDeclaration = attributeSyntax.Ancestors().OfType<TypeDeclarationSyntax>().FirstOrDefault();
                if (typeDeclaration != null)
                {
                    var typeSymbol = semanticModel.GetDeclaredSymbol(typeDeclaration);
                    if (typeSymbol is INamedTypeSymbol namedTypeSymbol &&
                        namedTypeSymbol.GetAttributes().Any(attr => SymbolEqualityComparer.Default.Equals(attr.AttributeClass, attributeSymbolInfo.Symbol?.ContainingType) &&
                        attr.AttributeClass?.ToDisplayString() == lazinatorAttributeTypeString))
                    {
                        if (!typesWithLazinatorAttribute.Any(ts => SymbolEqualityComparer.Default.Equals(ts, namedTypeSymbol)))
                        {
                            typesWithLazinatorAttribute.Add(namedTypeSymbol);
                        }
                    }
                }
            }

            return typesWithLazinatorAttribute;
        }



        private LazinatorPairInformation GetLazinatorPairInformation(Compilation compilation, LazinatorConfig config, INamedTypeSymbol interfaceSymbol)
        {
            LazinatorPairFinder analyzer = new LazinatorPairFinder(compilation, config); // we're not running the analyzer, but the analyzer code can help us get the LazinatorPairInfo.
            LazinatorPairInformation pairInfo = analyzer.GetLazinatorPairInfo(compilation, interfaceSymbol);
            return pairInfo;
        }



    }
}
