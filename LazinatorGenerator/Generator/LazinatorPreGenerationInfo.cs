using Lazinator.CodeDescription;
using LazinatorCodeGen.Roslyn;
using LazinatorGenerator.Settings;
using Microsoft.CodeAnalysis;
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
        internal LazinatorConfig Config;
        internal string AllPropertyDeclarations;
        internal ImmutableArray<string> NamesOfTypesReliedOn;
        internal SemanticModel SemanticModel => SyntaxContext.SemanticModel;
        internal Compilation Compilation => SemanticModel.Compilation;
        internal INamedTypeSymbol InterfaceSymbol => (INamedTypeSymbol) SyntaxContext.TargetSymbol;
        internal LazinatorPreGenerationInfo(GeneratorAttributeSyntaxContext syntaxContext, LazinatorConfig config, (string allPropertyDeclarations, ImmutableArray<string> namesOfTypesReliedOn) typeInfo)
        {
            SyntaxContext = syntaxContext;
            Config = config;
            AllPropertyDeclarations = typeInfo.allPropertyDeclarations;
            NamesOfTypesReliedOn = typeInfo.namesOfTypesReliedOn;
        }
        
        internal LazinatorCodeGenerationResult DoSourceGeneration(Guid pipelineRunUniqueID)
        {
            LazinatorPairInformation pairInfo = GetLazinatorPairInformation();
            if (pairInfo == null)
                return new LazinatorCodeGenerationResult(null, null, null, default, pipelineRunUniqueID);
            LazinatorCompilation lazinatorCompilation = new LazinatorCompilation(Compilation, pairInfo.LazinatorObject, Config);
            var d = new ObjectDescription(lazinatorCompilation.ImplementingTypeSymbol, lazinatorCompilation, Config, true);
            var generatedCode = d.GetCodeBehind();
            string path = d.ObjectNameEncodable + Config.GeneratedCodeFileExtension;
            return new LazinatorCodeGenerationResult(d.FullyQualifiedObjectName, generatedCode, path, d.GetDependencyInfo(), pipelineRunUniqueID);
        }

        private LazinatorPairInformation GetLazinatorPairInformation()
        {
            LazinatorCompilationAnalyzer analyzer = LazinatorCompilationAnalyzer.CreateCompilationAnalyzer_WithoutConfig(Compilation); // we're not running the analyzer, but the analyzer code can help us get the LazinatorPairInfo.
            LazinatorPairInformation pairInfo = analyzer.GetLazinatorPairInfo(Compilation, InterfaceSymbol);
            return pairInfo;
        }

    }
}
