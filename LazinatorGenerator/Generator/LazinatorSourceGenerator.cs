using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Text;

namespace LazinatorGenerator.Generator
{
    [Generator]
    public class LazinatorSourceGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext 
context)
        {
            // Each Config file will be in an immutable array. We will need to assign each source file to a config file (or null, if none is applicable).
            IncrementalValueProvider<ImmutableArray<AdditionalText>> 
            additionalTextsProvider = context.AdditionalTextsProvider.Where(x => x.Path.EndsWith("LazinatorConfig.Json")).Collect();

            // DEBUG -- change to ForAttributeWithMetadataName -- https://www.thinktecture.com/en/net-core/roslyn-source-generators-high-level-api-forattributewithmetadataname/ and https://github.com/dotnet/roslyn/blob/7d64f6edcc0ee5fe4399a7966e1a6b2bd3ef9005/src/Compilers/Core/Portable/SourceGeneration/Nodes/SyntaxValueProvider_ForAttributeWithMetadataName.cs#L78
            IncrementalValuesProvider<InterfaceDeclarationSyntax> ilazinatorInterfaceDeclarations = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: static (s, _) => IsSyntaxTargetForGeneration(s), // select interfaces with attributes
                    transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx)) // select interfaces with Lazinator attributes
                .Where(static m => m is not null)!; // filter out attributed enums that we don't care about

            // Combine the selected enums with the `Compilation`
            IncrementalValueProvider<(Compilation, ImmutableArray<InterfaceDeclarationSyntax>)> compilationAndInterfaceSyntaxes
                = context.CompilationProvider.Combine(ilazinatorInterfaceDeclarations.Collect());

            // Generate the source using the compilation and enums
            context.RegisterSourceOutput(compilationAndInterfaceSyntaxes,
                static (spc, source) => AddSourceFromInterfaceDeclarations(source.Item1, source.Item2, spc));
        }

        static bool IsSyntaxTargetForGeneration(SyntaxNode node)
    => node is InterfaceDeclarationSyntax m && m.AttributeLists.Count > 0;

        static InterfaceDeclarationSyntax GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
        {
            // we know the node is an InterfaceDeclarationSyntax thanks to IsSyntaxTargetForGeneration
            var interfaceDeclarationContext = (InterfaceDeclarationSyntax)context.Node;

            // loop through all the attributes on the method
            foreach (AttributeListSyntax attributeListSyntax in interfaceDeclarationContext.AttributeLists)
            {
                foreach (AttributeSyntax attributeSyntax in attributeListSyntax.Attributes)
                {
                    if (context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol attributeSymbol)
                    {
                        // weird, we couldn't get the symbol, ignore it.
                        // This could be a result of a failure to add a reference.
                        continue;
                    }

                    INamedTypeSymbol attributeContainingTypeSymbol = attributeSymbol.ContainingType;
                    string fullName = attributeContainingTypeSymbol.ToDisplayString();

                    // Is the attribute the [EnumExtensions] attribute?
                    if (fullName == "Lazinator.Attributes.LazinatorAttribute")
                    {
                        // return the context
                        return interfaceDeclarationContext;
                    }
                }
            }

            // we didn't find the attribute we were looking for
            return null;
        }

        static void AddSourceFromInterfaceDeclarations(Compilation compilation, ImmutableArray<InterfaceDeclarationSyntax> interfaceDeclarations, SourceProductionContext context)
        {

            //LazinatorCompilation lazinatorCompilation = new LazinatorCompilation(compilation, existingType, config);

            //var d = new ObjectDescription(lazinatorCompilation.ImplementingTypeSymbol, lazinatorCompilation, codeBehindPath, true);
            foreach (var interfaceDeclaration in interfaceDeclarations)
                context.AddSource($"{interfaceDeclaration.Identifier}.g.cs", SourceText.From($"// {interfaceDeclaration.Identifier}", Encoding.UTF8));
        }

    }

    
}