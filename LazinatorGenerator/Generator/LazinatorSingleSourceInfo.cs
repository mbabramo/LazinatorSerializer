using Lazinator.CodeDescription;
using LazinatorCodeGen.Roslyn;
using LazinatorGenerator.Settings;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace LazinatorGenerator.Generator
{
    internal struct LazinatorSingleSourceInfo
    {
        internal GeneratorAttributeSyntaxContext SyntaxContext;
        internal LazinatorConfig Config;
        internal SemanticModel SemanticModel => SyntaxContext.SemanticModel;
        internal Compilation Compilation => SemanticModel.Compilation;
        internal INamedTypeSymbol InterfaceSymbol => (INamedTypeSymbol) SyntaxContext.TargetSymbol;
        internal LazinatorSingleSourceInfo(GeneratorAttributeSyntaxContext syntaxContext, LazinatorConfig config)
        {
            SyntaxContext = syntaxContext;
            Config = config;
        }
        
        internal void GenerateSource(SourceProductionContext spc)
        {
            LazinatorPairInformation pairInfo = GetLazinatorPairInformation();
            var x = pairInfo;
            LazinatorCompilation lazinatorCompilation = new LazinatorCompilation(Compilation, pairInfo.LazinatorObject, Config);
            var d = new ObjectDescription(lazinatorCompilation.ImplementingTypeSymbol, lazinatorCompilation, Config, true);
            var resultingSource = d.GetCodeBehind();
            spc.AddSource(d.ObjectNameEncodable + Config.GeneratedCodeFileExtension, resultingSource);
        }

        private LazinatorPairInformation GetLazinatorPairInformation()
        {
            LazinatorCompilationAnalyzer analyzer = LazinatorCompilationAnalyzer.CreateCompilationAnalyzer_WithoutConfig(Compilation); // we're not running the analyzer, but the analyzer code can help us get the LazinatorPairInfo.
            LazinatorPairInformation pairInfo = analyzer.GetLazinatorPairInfo(Compilation, InterfaceSymbol);
            return pairInfo;
        }
    }
}
