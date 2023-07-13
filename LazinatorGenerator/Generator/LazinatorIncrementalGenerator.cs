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
    /// Define the incremental generator, creating a pipeline to take maximum advantage of caching to avoid unnecessary repetitive code generation.
    public class LazinatorIncrementalGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext 
context)
        {
            // Find the syntax contexts (i.e., interface declarations decorated with LazinatorAttribute)
            IncrementalValuesProvider<GeneratorAttributeSyntaxContext> syntaxContexts = context.SyntaxProvider
                          .ForAttributeWithMetadataName("Lazinator.Attributes.LazinatorAttribute", IsSyntaxTargetForGeneration, (ctx, cancellationToken) => ctx); // Note: We stick with the GeneratorAttributeSyntaxContext for now, so that we can combine with the LazinatorConfig.

            // For each syntax context, we need the appropriate LazinatorConfig. We will use the config file in the same directory as the source file, if it exists, or in the closest parent/ancestor directory, or the default config. 
            // Find our LazinatorConfig.json and Lazinatorconfig.json files.
            IncrementalValuesProvider<AdditionalText> additionalTextsProvider = context.AdditionalTextsProvider
                .Where(x => x.Path.EndsWith(LazinatorConfigLoader.ConfigFileName) || x.Path.EndsWith(LazinatorConfigLoader.AltConfigFileName));
            /// Extract the path and text from each of these. 
            IncrementalValuesProvider<(string path, string text)> additionalTextsPathAndContents = additionalTextsProvider.Select<AdditionalText, (string path, string text)>((additionalText, cancellationToken) => 
            {
                string filePath = additionalText.Path.TrimEnd(Path.DirectorySeparatorChar);
                string fileText = additionalText.GetText(cancellationToken).ToString();
                return (filePath, fileText);
            });
            // Parse the JSON to generate LazinatorConfig objects, placed into an ImmutableArray. Note that we have designed LazinatorConfig to have fast hashing, so that the generator can use appropriate caching efficiently.
            IncrementalValueProvider<ImmutableArray<LazinatorConfig>> configLocationsAndContents = additionalTextsPathAndContents.Where(x => x.path != null).Select((x, cancellationToken) => new LazinatorConfig(x.path, x.text)).Collect();
            // Now, we choose the appropriate LazinatorConfig from this ImmutableArray for each of our syntax contexts (or the default LazinatorConfig if none is found). (Note: If this step turns out to be costly, we could just store the entire immutable array within our LazinatorSingleSourceInfo object or some other object and then defer choosing the appropriate source until we know that we need to generate the source.
            IncrementalValuesProvider<(GeneratorAttributeSyntaxContext SyntaxContext, LazinatorConfig Config)> sourcesAndConfigs = syntaxContexts.Combine(configLocationsAndContents).Select((x, cancellationToken) => (x.Left, ChooseAppropriateConfig(Path.GetDirectoryName(x.Left.TargetNode.SyntaxTree.FilePath), x.Right)));

            IncrementalValuesProvider<LazinatorSingleSourceInfo> singleSourceInfos = sourcesAndConfigs.Select((x, cancellationToken) => new LazinatorSingleSourceInfo(x.SyntaxContext, x.Config));

            
            // Generate the source using the compilation and enums
            context.RegisterSourceOutput(singleSourceInfos,
                static (spc, singleSourceInfo) => singleSourceInfo.GenerateSource(spc));
        }

        static bool IsSyntaxTargetForGeneration(SyntaxNode node, CancellationToken cancellationToken)
        {
            if (node.ToString().Contains("FastRead"))
            {
                var DEBUG = 0;
            }
            return node is InterfaceDeclarationSyntax m && m.AttributeLists.Count > 0;
        }

        static LazinatorConfig ChooseAppropriateConfig(string path, ImmutableArray<LazinatorConfig> candidateConfigs)
        {
            LazinatorConfig? bestConfigSoFar = null;
            for (int i = 0; i < candidateConfigs.Length; i++)
            {
                string candidateConfigPath = candidateConfigs[i].ConfigFilePath;
                if (candidateConfigPath.Length > (bestConfigSoFar?.ConfigFilePath.Length ?? 0) && path.StartsWith(candidateConfigPath))
                    bestConfigSoFar = candidateConfigs[i];
            }
            return bestConfigSoFar ?? new LazinatorConfig();
        }

    }

    
}