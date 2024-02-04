using LazinatorCodeGen.Roslyn;
using LazinatorGenerator.Settings;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection.Metadata;
using System.Reflection;
using System.Text;
using System.Threading.Channels;
using LazinatorGenerator.Support;

namespace LazinatorGenerator.Generator
{
    [Generator]
    /// Define the incremental generator, creating a pipeline to take maximum advantage of caching to avoid unnecessary repetitive code generation. .Net runs the pipeline and when it sees an object, it looks to see whether it is cached, in which case it uses the cached version in producing the next step of the pipeline. So, we have to be careful to make sure that when something has been cached but is out of date (because it, a config file, or a dependency has changed), we recognize that and create a cache key that will force the creation of a new object.
    public class LazinatorIncrementalGenerator : IIncrementalGenerator
    {

        public IDateTimeNow DateTimeNowProvider { get; set; }
        
        public void Initialize(IncrementalGeneratorInitializationContext 
context)
        {

            // Set the DateTimeNowProvider if it has not been set. Simple alternative to dependency injection
            // when the actual generator is executed by Roslyn.
            if (DateTimeNowProvider == null)
                DateTimeNowProvider = new RealDateTimeNow();

            // Find the syntax contexts (i.e., interface declarations decorated with LazinatorAttribute)
            IncrementalValuesProvider<GeneratorAttributeSyntaxContext> syntaxContexts = context.SyntaxProvider
                          .ForAttributeWithMetadataName("Lazinator.Attributes.LazinatorAttribute", IsSyntaxTargetForGeneration, (ctx, cancellationToken) => ctx); // Note: We stick with the GeneratorAttributeSyntaxContext for now, so that we can combine with the LazinatorConfig.

            // For each syntax context, we need the appropriate LazinatorConfig. We will use the config file in the same directory as the source file, if it exists, or in the closest parent/ancestor directory, or the default config. 
            // Find our LazinatorConfig.json and Lazinatorconfig.json files.
            IncrementalValuesProvider<AdditionalText> additionalTextsProvider = context.AdditionalTextsProvider
                .Where((Func<AdditionalText, bool>)(x => PathMatches(x)));

            static bool PathMatches(AdditionalText x)
            {
                return x.Path.EndsWith(LazinatorConfigLoader.ConfigFileName) || x.Path.EndsWith(LazinatorConfigLoader.AltConfigFileName);
            }
            /// Extract the path and text from each of these. 
            IncrementalValuesProvider<(string path, string text)> additionalTextsPathAndContents = additionalTextsProvider.Select<AdditionalText, (string path, string text)>((additionalText, cancellationToken) =>
            {
                string filePath = additionalText.Path.TrimEnd(Path.DirectorySeparatorChar);
                string fileText = additionalText.GetText(cancellationToken).ToString();
                return (filePath, fileText);
            });
            // Parse the JSON to generate LazinatorConfig objects, placed into an ImmutableArray. Note that we have designed LazinatorConfig to have fast hashing, so that the generator can use appropriate caching efficiently.
            IncrementalValueProvider<ImmutableArray<LazinatorConfig>> configLocationsAndContents = additionalTextsPathAndContents.Where(x => x.path != null).Select((x, cancellationToken) => new LazinatorConfig(x.path, x.text)).Collect();
            // Now, we store the SyntaxContext and the applicable configuration file. It may seem tempting to defer choosing the appropriate source until we know that we need to generate the source, to save time early in the pipeline, but then changing any config file would force regeneration of every file in the project.
            IncrementalValuesProvider<(GeneratorAttributeSyntaxContext SyntaxContext, ImmutableArray<LazinatorConfig> ConfigFiles)> sourcesConfigsAndID = syntaxContexts.Combine(configLocationsAndContents).Select((x, cancellationToken) => (x.Left, x.Right));
            IncrementalValuesProvider<LazinatorPreGenerationInfo> preGenerationInfos = sourcesConfigsAndID.Select((x, cancellationToken) => new LazinatorPreGenerationInfo(x.SyntaxContext, ChooseAppropriateConfig(Path.GetDirectoryName(x.SyntaxContext.TargetNode.SyntaxTree.FilePath), x.ConfigFiles)));
            // Create a LazinatorPostGenerationInfo for each preGenerationInfo that hasn't been cached.
            IncrementalValuesProvider<LazinatorPostGenerationInfo> postGenerationInfos = preGenerationInfos.Select((x, cancellationToken) => new LazinatorPostGenerationInfo(x, x.ExecuteSourceGeneration(DateTimeNowProvider))).Where(x => !x.AlreadyGeneratedCode.IsEmpty);
            IncrementalValueProvider<ImmutableArray<LazinatorPostGenerationInfo>> postGenerationInfosCollected = postGenerationInfos.Collect();
            IncrementalValuesProvider<LazinatorPostGenerationInfo> postGenerationInfosSeparated = postGenerationInfosCollected.SelectMany((x, cancellationToken) => LazinatorPostGenerationInfo.SeparateForNextPipelineStep(x));
            // Generate the source using the compilation and enums
            context.RegisterSourceOutput(postGenerationInfosSeparated,
                (spc, postGenerationInfo) => postGenerationInfo.GenerateSource(spc, DateTimeNowProvider));
        }

        static LazinatorConfig ChooseAppropriateConfig(string path, ImmutableArray<LazinatorConfig> candidateConfigs)
        {
            LazinatorConfig? bestConfigSoFar = null;
            for (int i = 0; i < candidateConfigs.Length; i++)
            {
                string candidateConfigPath = candidateConfigs[i].ConfigFilePath;
                if (candidateConfigPath.Length >= (bestConfigSoFar?.ConfigFilePath.Length ?? 0) && path.StartsWith(candidateConfigPath))
                    bestConfigSoFar = candidateConfigs[i];
            }
            return bestConfigSoFar ?? new LazinatorConfig();
        }

        static bool IsSyntaxTargetForGeneration(SyntaxNode node, CancellationToken cancellationToken)
        {
            return node is InterfaceDeclarationSyntax m && m.AttributeLists.Count > 0;
        }


    }

    
}