using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LazinatorAnalyzer.Settings;
using LazinatorAnalyzer.AttributeClones;
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

        public const string Category = "Lazinator";

        // 1. If the Lazinator code behind does not exist or is out of date, then it must be generated.
        public const string Lazin001 = "Lazin001";
        private static readonly string LazinatorOutOfDateTitle = "Lazinator out-of-date";
        private static readonly string LazinatorOutOfDateMessageFormat = "Generate the code behind for this out-of-date Lazinator object";
        private static readonly string LazinatorOutOfDateDescription =
            "This object implements an interface with the Lazinator attribute, but it is out of date.";
        private static readonly DiagnosticDescriptor LazinatorOutOfDateRule = new DiagnosticDescriptor(Lazin001, LazinatorOutOfDateTitle, LazinatorOutOfDateMessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: LazinatorOutOfDateDescription);
        internal static DiagnosticDescriptor OutOfDateRule = new DiagnosticDescriptor(Lazin001, LazinatorOutOfDateTitle.ToString(), LazinatorOutOfDateMessageFormat.ToString(), Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: LazinatorOutOfDateDescription);
        // 2. Otherwise, we can create an option to regenerate the Lazinator code.
        public const string Lazin002 = "Lazin002";
        private static readonly string LazinatorOptionalRegenerationTitle = "Lazinator Generation";
        private static readonly string LazinatorOptionalRegenerationMessageFormat = "Regenerate the code behind for this Lazinator object";
        private static readonly string LazinatorOptionalRegenerationDescription =
            "This object implements an interface with the Lazinator attribute.";
        private static readonly DiagnosticDescriptor LazinatorOptionalRegenerationRule = new DiagnosticDescriptor(Lazin002, LazinatorOptionalRegenerationTitle, LazinatorOptionalRegenerationMessageFormat, Category, DiagnosticSeverity.Info, isEnabledByDefault: true, description: LazinatorOptionalRegenerationDescription);
        internal static DiagnosticDescriptor OptionalRegenerationRule = new DiagnosticDescriptor(Lazin002, LazinatorOptionalRegenerationTitle.ToString(), LazinatorOptionalRegenerationMessageFormat.ToString(), Category, DiagnosticSeverity.Info, isEnabledByDefault: true, description: LazinatorOptionalRegenerationDescription);
        // 3. If fields exist in main code without ILazinator, report an error.
        public const string Lazin003 = "Lazin003";
        private static readonly string LazinatorUnaccountedForFieldTitle = "Lazinator Field Problem";
        private static readonly string LazinatorUnaccountedForFieldMessageFormat = "Mark field as NonSerialized or remove";
        private static readonly string LazinatorUnaccountedForFieldDescription =
            "A field in an object implementing a Lazinator interface must be marked NonSerialable. Consider marking as [NonSerialized].";
        private static readonly DiagnosticDescriptor LazinatorUnaccountedForFieldRule = new DiagnosticDescriptor(Lazin003, LazinatorUnaccountedForFieldTitle, LazinatorUnaccountedForFieldMessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: LazinatorUnaccountedForFieldDescription);
        internal static DiagnosticDescriptor UnaccountedForFieldRule = new DiagnosticDescriptor(Lazin003, LazinatorUnaccountedForFieldTitle.ToString(), LazinatorUnaccountedForFieldMessageFormat.ToString(), Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: LazinatorUnaccountedForFieldDescription);
        // 4. If there is an extra code behind file (for example, as a result of a refactoring), generate an error so that it can be deleted
        public const string Lazin004 = "Lazin004";
        private static readonly string LazinatorExtraFileTitle = "Lazinator Extra File";
        private static readonly string LazinatorExtraFileMessageFormat = "Remove extra code-behind file";
        private static readonly string LazinatorExtraFileDescription =
            "This code-behind file was generated but no longer belongs here. (This could be the result of a refactoring.)";
        private static readonly DiagnosticDescriptor LazinatorExtraFileRule = new DiagnosticDescriptor(Lazin004, LazinatorExtraFileTitle, LazinatorExtraFileMessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: LazinatorExtraFileDescription);
        internal static DiagnosticDescriptor ExtraFileRule = new DiagnosticDescriptor(Lazin004, LazinatorExtraFileTitle.ToString(), LazinatorExtraFileMessageFormat.ToString(), Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: LazinatorExtraFileDescription);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(LazinatorOutOfDateRule, LazinatorOptionalRegenerationRule, LazinatorUnaccountedForFieldRule, LazinatorExtraFileRule); } }

        public override void Initialize(AnalysisContext context)
        {
            try
            {
                context.EnableConcurrentExecution();
                context.RegisterCompilationStartAction(compilationContext =>
                {
                    var additionalFiles = compilationContext.Options.AdditionalFiles;
                    var compilation = compilationContext.Compilation;
                    var cancellationToken = compilationContext.CancellationToken;
                    var analyzer = LazinatorCompilationAnalyzer.CreateCompilationAnalyzer(compilation, cancellationToken, additionalFiles);
                    if (analyzer == null)
                        return;

                    // Register intermediate non-end actions that access and modify the state.
                    compilationContext.RegisterSyntaxNodeAction(analyzer.AnalyzeSyntaxNode, SyntaxKind.StructDeclaration, SyntaxKind.ClassDeclaration);
                    compilationContext.RegisterSymbolAction(analyzer.AnalyzeSymbol, SymbolKind.NamedType, SymbolKind.Method);

                    // Register an end action to report diagnostics based on the final state.
                    compilationContext.RegisterSyntaxTreeAction(analyzer.SyntaxTreeStartAction);
                    compilationContext.RegisterSemanticModelAction(analyzer.SemanticModelEndAction);

                    // NOTE: We could register a compilation end action, but this will not execute at all if full solution analysis is disabled. 
                });
            }
            catch (Exception ex)
            { // catch exceptions so that we can get a more useful error in the Visual Studio consumer of this analyzer.
                throw new Exception($"Lazinator analyzer exception encountered. Message {ex.Message} Stack trace: {ex.StackTrace}");
            }
        }
    }
}
