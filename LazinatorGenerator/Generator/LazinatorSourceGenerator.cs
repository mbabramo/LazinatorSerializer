using LazinatorCodeGen.Roslyn;
using LazinatorGenerator.Settings;
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
            IncrementalValuesProvider<AdditionalText> additionalTextsProvider = context.AdditionalTextsProvider
                .Where(x => x.Path.EndsWith(LazinatorConfigLoader.ConfigFileName) || x.Path.EndsWith(LazinatorConfigLoader.AltConfigFileName));
            IncrementalValuesProvider<(string path, string text)> additionalTextsPathAndContents = additionalTextsProvider.Select<AdditionalText, (string path, string text)>((additionalText, cancellationToken) => LazinatorConfigLoader.GetTextAndPathFromAdditionalText(additionalText, cancellationToken));


                .SelectMany(x => (x.Path, x.GetText()?.ToString() ?? ""));

            IncrementalValuesProvider<LazinatorNodeSemanticInfo> semanticInfos = context.SyntaxProvider
                          .ForAttributeWithMetadataName("Lazinator.Attributes.LazinatorAttribute", IsSyntaxTargetForGeneration, GetSemanticTargetForGeneration);

            // Combine the selected enums with the `Compilation`
            IncrementalValueProvider<(Compilation Left, ImmutableArray<LazinatorNodeSemanticInfo> Right)> compilationAndSemanticInfos
                = context.CompilationProvider.Combine(semanticInfos.Collect());

            // Generate the source using the compilation and enums
            context.RegisterSourceOutput(compilationAndSemanticInfos,
                static (spc, source) => AddSourceFromSemanticInformation(source.Item1, source.Item2, spc));
        }
        
        static bool IsSyntaxTargetForGeneration(SyntaxNode node, CancellationToken cancellationToken)
    => node is InterfaceDeclarationSyntax m && m.AttributeLists.Count > 0;
        
        static LazinatorNodeSemanticInfo GetSemanticTargetForGeneration(GeneratorAttributeSyntaxContext context, CancellationToken cancellationToken)
        {
            var targetSymbol = context.TargetSymbol;
            var type = targetSymbol as INamedTypeSymbol;

            //SemanticModel semanticModel = context.SemanticModel;
            //var compilation = semanticModel.Compilation;
            //var nullableContext = semanticModel.GetNullableContext(context.TargetNode.Span.Start);
            //var fullyQualifiedName = type.GetFullyQualifiedNameWithoutGlobal(nullableContext.AnnotationsEnabled());

            var filename = context.TargetSymbol.Locations.First().SourceSpan.Start.
            return new LazinatorNodeSemanticInfo(targetSymbol.GetFullNamespace(), targetSymbol.GetFullMetadataName());
        }
        
        static void AddSourceFromSemanticInformation(Compilation compilation, ImmutableArray<LazinatorNodeSemanticInfo> semanticInfos, SourceProductionContext context)
        {

            LazinatorCompilation lazinatorCompilation = new LazinatorCompilation(compilation, existingType, config);

            //var d = new ObjectDescription(lazinatorCompilation.ImplementingTypeSymbol, lazinatorCompilation, codeBehindPath, true);
            foreach (var interfaceDeclaration in semanticInfos)
                context.AddSource($"{interfaceDeclaration}.g.cs", SourceText.From($"// {interfaceDeclaration}", Encoding.UTF8));
        }

    }

    
}