using Lazinator.CodeDescription;
using LazinatorCodeGen.Roslyn;
using LazinatorGenerator.CodeDescription;
using LazinatorGenerator.Settings;
using LazinatorGenerator.Support;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
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
        internal CSharpCompilation Compilation => (CSharpCompilation) SemanticModel.Compilation;
        internal INamedTypeSymbol InterfaceSymbol => (INamedTypeSymbol) SyntaxContext.TargetSymbol;
        internal LazinatorPreGenerationInfo(GeneratorAttributeSyntaxContext syntaxContext, LazinatorConfig config)
        {
            SyntaxContext = syntaxContext;
            Config = config;
        }

        public override int GetHashCode()
        {
            return (SyntaxContext, Config).GetHashCode();
        }
        
        public override bool Equals(object obj)
        {
            if (!(obj is LazinatorPreGenerationInfo other))
                return false;
            bool returnVal = Config.Equals(other.Config) && SyntaxContext.TargetNode.IsEquivalentTo(other.SyntaxContext.TargetNode, false);
            return returnVal;
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
                if (path == "BinaryNode_T.laz.cs")
                {
                    var DEBUG = 0;
                }
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
