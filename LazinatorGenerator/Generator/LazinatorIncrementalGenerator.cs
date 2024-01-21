﻿using LazinatorCodeGen.Roslyn;
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
    /// (1) IncrementalGeneratorInitializationContext -> LazinatorPreGenerationInfo: We initialize two incremental values providers, one for the syntax contexts for our interfaces decorated with the LazinatorAttribute and one for the config files. Then we choose the appropriate config file for each syntax context and combine these for each syntax context and its config file into a LazinatorPreGenerationInfo. Note that if there has been no change to a syntax context or its config file, the LazinatorPreGenerationInfo will be loaded from the cache, but that doesn't mean that the same code will eventually be produced.
    /// (2) LazinatorPreGenerationInfo -> LazinatorPostGenerationInfo. At this step, we actually perform the source generation. We also use the information generated by the source generation to create a list of dependencies for each generated Lazinator object, i.e. a list of other objects that, if relevantly changed, will necessitate regeneration of the Lazinator code. This is stored with the code in the LazinatorCodeGenerationResult field of the LazinatorPostGenerationInfo. At this point, the LazinatorPostGenerationInfo that is generated thus contains a list of dependencies, but it does not contain any information (i.e., hash codes) about the status of those dependencies, i.e. about whether any of those have changed. Note that if this has occurred in a previous run of the pipeline and nothing has changed in the LazinatorPreGenerationInfo (i.e., the syntax context and config are the same), then the LazinatorPostGenerationInfo will be loaded from the cache, thus potentially saving us the very demanding step of code generation. But we need to be careful, because we don't want to use the cached source generation if any of the config files or the type itself has changed. The rest of the pipeline is designed to ensure that we don't do that.
    /// (3) LazinatorPostGenerationInfo -> ImmutableArray<LazinatorPostGenerationInfo> -> multiple individual LazinatorPostGenerationInfo, with dependency hashes. We combine all LazinatorPostGenerationInfos into an immutable array. Note that if nothing at all has changed from the last pipeline, and thus Lazinator really has nothing to do, then the individual LazinatorPostGenerationInfos to be produced will be generated from the cache. If it is not cached, however, we will manually create the individual LazinatorPostGenerationInfos corresponding to each element of the immutable array, but containing hash codes for any dependencies included in the immutable array. Thus, whether Object X has changed or not, if it is dependent (directly or indirectly) on Object Y (perhaps a base class, for example), and Object Y's generated code has changed, then the new LazinatorPostGenerationInfo produced will be different from any past LazinatorPostGenerationInfo.
    /// (4) LazinatorPostGenerationInfo -> Generated code. If the LazinatorPostGenerationInfo with updated dependency information has not changed from a previous pipeline run, then the cached version will automatically be used. If LazinatorPostGenerationInfo is asked to generate code, meaning that it has changed in some way, then it must determine whether Step 2 actually ran on this step to create this object (in which case the generated code can just be returned, as it will reflect the state of all dependencies and that will help to avoid generating code twice) or was skipped in favor of a cached result (in which case the code must be generated at this point). It makes this determination by checking the PipelineRunUniqueID. If the PipelineRunUniqueID is the same as the PipelineRunUniqueID of the LazinatorPostGenerationInfo, then the code was already generated during this pipeline run and can be returned.  
    public class LazinatorIncrementalGenerator : IIncrementalGenerator
    {

        public static SourceProductionContext DEBUG = default;

        public IDateTimeNow DateTimeNowProvider { get; set; }
        
        public void Initialize(IncrementalGeneratorInitializationContext 
context)
        {
            // Set the DateTimeNowProvider if it has not been set. Simple alternative to dependency injection
            // when the actual generator is executed by Roslyn.
            if (DateTimeNowProvider == null)
                DateTimeNowProvider = new RealDateTimeNow();

            // Get the compilation and the start time for later use.
            IncrementalValueProvider<(Compilation compilation, long pipelineRunTimeStamp)> compilationAndpipelineRunTimeStamp = context.CompilationProvider.Select((comp, canc) => (comp, 0L)); // SUPERDEBUG DateTime.UtcNow.Ticks));
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
            // Now. We want to add the compilation and the pipeline run time stamp to each LazinatorPreGenerationInfo. But we don't want to mess up the caching. That is, once a preGenerationInfo has been cached,
            // then on the next run of the pipeline, we want to recover the LazinatorPostGenerationInfo previously generated. So, we use the struct WithIgnoredState, which will find equality even if the state is
            // different. The only purpose of this state is to be able to pass it to ExecuteSourceGeneration when we create the initial LazinatorPostGenerationInfo.
            IncrementalValuesProvider<WithIgnoredState<LazinatorPreGenerationInfo, (Compilation compilation, long pipelineRunTimeStamp)>> preGenerationInfosWithCompilation = preGenerationInfos.Combine(compilationAndpipelineRunTimeStamp).Select((x, cancellationToken) => new WithIgnoredState<LazinatorPreGenerationInfo, (Compilation compilation, long pipelineRunTimeStamp)>(x.Left, (x.Right.compilation, x.Right.pipelineRunTimeStamp)));
            // Create a LazinatorPostGenerationInfo for each preGenerationInfo that hasn't been cached.
            IncrementalValuesProvider<LazinatorPostGenerationInfo> postGenerationInfos = preGenerationInfosWithCompilation.Select((x, cancellationToken) => new LazinatorPostGenerationInfo(x.Item, x.Item.ExecuteSourceGeneration(DateTimeNowProvider, x.State.pipelineRunTimeStamp, x.State.compilation))).Where(x => !x.AlreadyGeneratedCode.IsEmpty);
            IncrementalValueProvider<ImmutableArray<LazinatorPostGenerationInfo>> postGenerationInfosCollected = postGenerationInfos.Collect();
            IncrementalValuesProvider<(LazinatorPostGenerationInfo postGenerationInfo, Compilation compilation, long pipelineRunTimeStamp)> postGenerationInfosSeparated = postGenerationInfosCollected.SelectMany((x, cancellationToken) => LazinatorPostGenerationInfo.SeparateForNextPipelineStep(x)).Combine(compilationAndpipelineRunTimeStamp).Select((x, cancellationToken) => (x.Left, x.Right.compilation, x.Right.pipelineRunTimeStamp));
            // Generate the source using the compilation and enums
            context.RegisterSourceOutput(postGenerationInfosSeparated,
                (spc, postGenerationInfoPlus) => postGenerationInfoPlus.postGenerationInfo.GenerateSource(spc, DateTimeNowProvider, postGenerationInfoPlus.pipelineRunTimeStamp, postGenerationInfoPlus.compilation));
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