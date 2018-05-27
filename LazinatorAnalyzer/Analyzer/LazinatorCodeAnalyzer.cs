using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using LazinatorAnalyzer.Settings;
using LazinatorCodeGen.AttributeClones;
using LazinatorCodeGen.Roslyn;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.FindSymbols;
using Location = Microsoft.CodeAnalysis.Location;

namespace LazinatorAnalyzer.Analyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class LazinatorCodeAnalyzer : DiagnosticAnalyzer
    {

        // Analyses 

        public const string LazinatorAttributeName = "Lazinator.Attributes.LazinatorAttribute";
        public const string LazinatorInterfaceName = "Lazinator.Core.ILazinator";
        public const string Category = "Lazinator";

        // 1. If the Lazinator code behind does not exist or is out of date, then it must be generated.
        public const string Lazin001 = "Lazin001";
        private static readonly string LazinatorOutOfDateTitle = "Lazinator out-of-date";
        private static readonly string LazinatorOutOfDateMessageFormat = "Generate the code behind for this Lazinator object";
        private static readonly string LazinatorOutOfDateDescription =
            "This object implements an interface with the Lazinator attribute, but it is out of date.";
        private static readonly DiagnosticDescriptor LazinatorOutOfDateRule = new DiagnosticDescriptor(Lazin001, LazinatorOutOfDateTitle, LazinatorOutOfDateMessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: LazinatorOutOfDateDescription);
        internal static DiagnosticDescriptor OutOfDateRule = new DiagnosticDescriptor(Lazin001, LazinatorOutOfDateTitle.ToString(), LazinatorOutOfDateMessageFormat.ToString(), Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: LazinatorOutOfDateDescription);
        // 2. Otherwise, we can create an option to regenerate the Lazinator code.
        public const string Lazin002 = "Lazin002";
        private static readonly string LazinatorOptionalRegenerationTitle = "Lazinator regeneration";
        private static readonly string LazinatorOptionalRegenerationMessageFormat = "Regenerate the code behind for this Lazinator object";
        private static readonly string LazinatorOptionalRegenerationDescription =
            "This object implements an interface with the Lazinator attribute.";
        private static readonly DiagnosticDescriptor LazinatorOptionalRegenerationRule = new DiagnosticDescriptor(Lazin002, LazinatorOptionalRegenerationTitle, LazinatorOptionalRegenerationMessageFormat, Category, DiagnosticSeverity.Info, isEnabledByDefault: true, description: LazinatorOptionalRegenerationDescription);
        internal static DiagnosticDescriptor OptionalRegenerationRule = new DiagnosticDescriptor(Lazin002, LazinatorOptionalRegenerationTitle.ToString(), LazinatorOptionalRegenerationMessageFormat.ToString(), Category, DiagnosticSeverity.Info, isEnabledByDefault: true, description: LazinatorOptionalRegenerationDescription);

        internal DateTime configLastLoaded = new DateTime(1900, 1, 1);
        internal string configPath, configString;
        internal LazinatorConfig config;

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(LazinatorOutOfDateRule, LazinatorOptionalRegenerationRule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.RegisterCompilationStartAction(compilationContext =>
            {
                var additionalFiles = compilationContext.Options.AdditionalFiles;

                // Check if the attribute type LazinatorAttribute is defined.
                INamedTypeSymbol lazinatorAttributeType = compilationContext.Compilation.GetTypeByMetadataName(LazinatorAttributeName);
                if (lazinatorAttributeType == null)
                {
                    return;
                }

                // Check if the interface type ILazinator is defined.
                INamedTypeSymbol lazinatorInterfaceType = compilationContext.Compilation.GetTypeByMetadataName(LazinatorInterfaceName);
                if (lazinatorInterfaceType == null)
                {
                    return;
                }

                if (DateTime.Now - configLastLoaded > TimeSpan.FromSeconds(5))
                { // we don't want to reload this too often, although it shouldn't involve a disk operation
                    (configPath, configString) = LazinatorConfigLoader.GetConfigPathAndText(additionalFiles, compilationContext.CancellationToken);
                    config = new LazinatorConfig(configPath, configString);
                    configLastLoaded = DateTime.Now;
                }

                // Initialize state in the start action.
                var analyzer = new LazinatorCompilationAnalyzer(lazinatorAttributeType, lazinatorInterfaceType, additionalFiles, configPath, configString, config);

                // Register intermediate non-end actions that access and modify the state.
                compilationContext.RegisterSyntaxNodeAction(analyzer.AnalyzeSyntaxNode, SyntaxKind.StructDeclaration, SyntaxKind.ClassDeclaration);
                compilationContext.RegisterSymbolAction(analyzer.AnalyzeSymbol, SymbolKind.NamedType, SymbolKind.Method);

                // Register an end action to report diagnostics based on the final state.
                compilationContext.RegisterSyntaxTreeAction(analyzer.SyntaxTreeStartAction);
                compilationContext.RegisterSemanticModelAction(analyzer.SemanticModelEndAction);

                // NOTE: We could register a compilation end action, but this will not execute at all if full solution analysis is disabled. 
            });
        }
    }
}
