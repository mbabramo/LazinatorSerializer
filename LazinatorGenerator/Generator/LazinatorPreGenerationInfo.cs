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
        internal SemanticModel SemanticModel => SyntaxContext.SemanticModel;
        internal Compilation Compilation => SemanticModel.Compilation;
        internal INamedTypeSymbol InterfaceSymbol => (INamedTypeSymbol) SyntaxContext.TargetSymbol;
        internal LazinatorPreGenerationInfo(GeneratorAttributeSyntaxContext syntaxContext, LazinatorConfig config)
        {
            SyntaxContext = syntaxContext;
            Config = config;
        }
        
        internal LazinatorCodeGenerationResult ExecuteSourceGeneration(Guid pipelineRunUniqueID)
        {
            LazinatorPairInformation pairInfo = GetLazinatorPairInformation();
            if (pairInfo == null)
                return new LazinatorCodeGenerationResult(null, null, null, default, pipelineRunUniqueID, default);
            LazinatorCompilation lazinatorCompilation = new LazinatorCompilation(Compilation, pairInfo.LazinatorObject, Config);

            try
            {
                var objectDescription = new ObjectDescription(lazinatorCompilation.ImplementingTypeSymbol, lazinatorCompilation, Config, true);
                var generatedCode = objectDescription.GetCodeBehind();
                string path = objectDescription.ObjectNameEncodable + Config.GeneratedCodeFileExtension;
                return new LazinatorCodeGenerationResult(objectDescription.FullyQualifiedObjectName_InNullableMode, path, generatedCode, lazinatorCompilation.GetDependencyInfo(), pipelineRunUniqueID, null);
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
                return new LazinatorCodeGenerationResult(lazinatorCompilation.ImplementingTypeSymbol.GetFullyQualifiedNameWithoutGlobal(true), null, null, lazinatorCompilation.GetDependencyInfo(), pipelineRunUniqueID, diagnostic);
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
