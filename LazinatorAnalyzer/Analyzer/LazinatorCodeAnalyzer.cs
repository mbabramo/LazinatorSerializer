using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(LazinatorOutOfDateRule, LazinatorOptionalRegenerationRule); } }

        public override void Initialize(AnalysisContext context)
        {
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

                // Initialize state in the start action.
                var analyzer = new CompilationAnalyzer(lazinatorAttributeType, lazinatorInterfaceType, additionalFiles);

                // Register intermediate non-end actions that access and modify the state.
                compilationContext.RegisterSyntaxNodeAction(analyzer.AnalyzeSyntaxNode, SyntaxKind.StructDeclaration, SyntaxKind.ClassDeclaration);
                compilationContext.RegisterSymbolAction(analyzer.AnalyzeSymbol, SymbolKind.NamedType, SymbolKind.Method);

                // Register an end action to report diagnostics based on the final state.
                // 
                compilationContext.RegisterSyntaxTreeAction(analyzer.SyntaxTreeStartAction);
                compilationContext.RegisterSemanticModelAction(analyzer.SemanticModelEndAction);

                // NOTE: We could register a compilation end action, but this will not execute at all if full solution analysis is disabled. 
            });
        }

        private class CompilationAnalyzer
        {
            #region Per-Compilation immutable state

            private readonly INamedTypeSymbol _lazinatorAttributeType;
            private readonly INamedTypeSymbol _lazinatorInterfaceType;
            private readonly ImmutableArray<AdditionalText> _additionalFiles;

            #endregion

            #region Per-Compilation mutable state
            
            private Dictionary<string, SourceFileInformation> CompilationInformation = new Dictionary<string, SourceFileInformation>();

            #endregion

            #region State intialization

            public CompilationAnalyzer(INamedTypeSymbol lazinatorAttributeType, INamedTypeSymbol lazinatorInterfaceType, ImmutableArray<AdditionalText> additionalFiles)
            {
                _lazinatorAttributeType = lazinatorAttributeType;
                _lazinatorInterfaceType = lazinatorInterfaceType;
                _additionalFiles = additionalFiles;
            }

            #endregion

            #region Intermediate actions

            // These actions will execute in the following order.

            public void SyntaxTreeStartAction(SyntaxTreeAnalysisContext context)
            {
                string filePath = context.Tree.FilePath;
                if (!filePath.EndsWith(".g.cs"))
                {
                    CompilationInformation[filePath] = new SourceFileInformation();
                }
            }

            public void AnalyzeSymbol(SymbolAnalysisContext context)
            {
                switch (context.Symbol.Kind)
                {
                    case SymbolKind.NamedType:
                        var namedType = (INamedTypeSymbol)context.Symbol;
                        INamedTypeSymbol lazinatorObjectType;
                        INamedTypeSymbol namedInterfaceType;
                        // We want to be able to present a diagnostic starting either with the Lazinator class or vice-versa.
                        if (namedType.TypeKind == TypeKind.Interface)
                        {
                            // If this is a Lazinator interface and we can find the Lazinator class, we'll consider doing an analysis.
                            if (namedType.HasAttributeOfType<CloneLazinatorAttribute>())
                            {
                                // find candidate matching classes
                                // maybe another approach would be to use SymbolFinder, but we can't load the Solution in the code analyzer. var implementations = SymbolFinder.FindImplementationsAsync(namedType, ... See https://stackoverflow.com/questions/23203206/roslyn-current-workspace-in-diagnostic-with-code-fix-project for a possible workaround
                                IEnumerable<ISymbol> candidates = context.Compilation.GetSymbolsWithName(name => RoslynHelpers.GetNameWithoutGenericArity(name) == RoslynHelpers.GetNameWithoutGenericArity(namedType.MetadataName).Substring(1), SymbolFilter.Type);
                                lazinatorObjectType = candidates.OfType<INamedTypeSymbol>().FirstOrDefault(x => namedType.GetFullyQualifiedName() == x.GetTopLevelInterfaceImplementingAttribute(_lazinatorAttributeType).GetFullyQualifiedName());
                                if (lazinatorObjectType == null)
                                    return;
                                namedInterfaceType = namedType;
                                // Note: A more comprehensive, but slower approach would be to use context.Compilation.GlobalNamespace...
                            }
                            return;
                        }
                        else
                        {
                            // This is not an interface. It may be a Lazinator object with a corresponding Lazinator interface.
                            lazinatorObjectType = namedType;
                            namedInterfaceType = namedType.GetTopLevelInterfaceImplementingAttribute(_lazinatorAttributeType);
                        }
                        if (namedInterfaceType != null)
                        {
                            var lazinatorAttribute = RoslynHelpers.GetKnownAttributes<CloneLazinatorAttribute>(namedInterfaceType).FirstOrDefault();
                            if (lazinatorAttribute == null)
                                return;
                            if (lazinatorAttribute.Autogenerate == false)
                                return;
                            var locationsExcludingCodeBehind = lazinatorObjectType.Locations.Where(x => !x.SourceTree.FilePath.EndsWith(".g.cs")).ToList();
                            string locationToIndexBy = locationsExcludingCodeBehind
                                .Select(x => x.SourceTree.FilePath)
                                .OrderBy(x => x)
                                .FirstOrDefault(x => CompilationInformation.ContainsKey(x));
                            if (locationToIndexBy != null)
                            {
                                SourceFileInformation sourceFileInfo = CompilationInformation[locationToIndexBy];
                                sourceFileInfo.LazinatorObject = lazinatorObjectType;
                                sourceFileInfo.LazinatorInterface = namedInterfaceType;
                                sourceFileInfo.LazinatorObjectLocationsExcludingCodeBehind =
                                    locationsExcludingCodeBehind
                                        .OrderByDescending(x => x.SourceTree.FilePath == locationToIndexBy)
                                        .ToList(); // place indexed location first
                                sourceFileInfo.CodeBehindLocation =
                                    lazinatorObjectType.Locations
                                        .FirstOrDefault(x => x.SourceTree.FilePath.EndsWith(".g.cs"));
                            }
                        }

                        break;
                }
            }

            public void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
            {
            }

            #endregion

            #region End action

            public void SemanticModelEndAction(SemanticModelAnalysisContext context)
            {
                try
                {

                    if (!CompilationInformation.Any())
                    {
                        // No types to consider
                        return;
                    }

                    // Analyze types
                    foreach (var compilationInfoEntry in CompilationInformation)
                    {
                        bool couldBeGenerated = false;
                        var sourceFileInfo = compilationInfoEntry.Value;
                        if (sourceFileInfo.LazinatorInterface != null 
                            && sourceFileInfo.LazinatorObject != null 
                            && sourceFileInfo.LazinatorObjectLocationsExcludingCodeBehind != null 
                            && sourceFileInfo.LazinatorObjectLocationsExcludingCodeBehind.Any())
                        {
                            couldBeGenerated = true;
                            bool needsGeneration = sourceFileInfo.CodeBehindLocation == null;
                            if (!needsGeneration)
                            {
                                var success = sourceFileInfo.CodeBehindLocation.SourceTree.TryGetRoot(out SyntaxNode root);
                                if (success)
                                {
                                    SyntaxTrivia possibleComment = root.DescendantTrivia().FirstOrDefault();
                                    if (possibleComment.IsKind(SyntaxKind.SingleLineCommentTrivia))
                                    {
                                        string commentContent = possibleComment.ToString().Substring(2);
                                        bool parse = Guid.TryParse(commentContent, out Guid codeBehindGuid);
                                        if (parse)
                                        {
                                            var interfaceLocations = sourceFileInfo.LazinatorInterface.Locations;
                                            if (interfaceLocations.Count() != 1)
                                                return;
                                            var hash = LazinatorCompilation.GetHashForInterface(sourceFileInfo.LazinatorInterface, sourceFileInfo.LazinatorObject);
                                            if (hash != codeBehindGuid)
                                                needsGeneration = true;
                                        }
                                        else
                                            needsGeneration = true;
                                    }
                                    else
                                        needsGeneration = true;
                                }
                                else
                                    needsGeneration = true;
                            }
                            if (needsGeneration || couldBeGenerated)
                            {
                                var locationOfImplementingType = sourceFileInfo.LazinatorObjectLocationsExcludingCodeBehind[0];
                                var implementingTypeRoot = locationOfImplementingType.SourceTree.GetRoot();
                                TypeDeclarationSyntax implementingTypeSyntaxNode = (TypeDeclarationSyntax) implementingTypeRoot.FindNode(locationOfImplementingType.SourceSpan);
                                Location interfaceSpecificationLocation = Location.Create(
                                    locationOfImplementingType.SourceTree,
                                    implementingTypeSyntaxNode.BaseList.Types.First(x => (x.Type as IdentifierNameSyntax)?.Identifier.Text.Contains(sourceFileInfo.LazinatorInterface.Name) ?? (x.Type as GenericNameSyntax)?.Identifier.Text.Contains(sourceFileInfo.LazinatorInterface.Name) ?? false).Span);
                                var additionalLocations = new List<Location>();
                                if (sourceFileInfo.CodeBehindLocation != null)
                                    additionalLocations.Add(sourceFileInfo.CodeBehindLocation);
                                additionalLocations.AddRange(sourceFileInfo.LazinatorObjectLocationsExcludingCodeBehind);
                                (string configString, string configPath) = ConfigLoader.GetConfigTextAndPath(_additionalFiles, context.CancellationToken);
                                var diagnostic = Diagnostic.Create(needsGeneration ? OutOfDateRule : OptionalRegenerationRule, interfaceSpecificationLocation, additionalLocations, sourceFileInfo.GetSourceFileDictionary(configString, configPath));
                                context.ReportDiagnostic(diagnostic);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

        #endregion
    }

        private static TypeDeclarationSyntax GetTypeDeclarationContainingAttribute(SyntaxNode syntaxNode)
        {
            return syntaxNode.AncestorsAndSelf().OfType<TypeDeclarationSyntax>().FirstOrDefault();
            // return attribute?.Parent?.Parent?.Parent as TypeDeclarationSyntax;
        }

        private static List<AttributeListSyntax> GetAttributesModifyingField(FieldDeclarationSyntax syntaxNode)
        {
            return syntaxNode.AncestorsAndSelf().OfType<AttributeListSyntax>().ToList();
        }
    }
}
