using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using LazinatorCodeGen.AttributeClones;
using LazinatorCodeGen.Roslyn;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
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
        private static readonly LocalizableString LazinatorOutOfDateTitle = new LocalizableResourceString(nameof(Resources.OutOfDateTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString LazinatorOutOfDateMessageFormat = new LocalizableResourceString(nameof(Resources.OutOfDateMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString LazinatorOutOfDateDescription = new LocalizableResourceString(nameof(Resources.OutOfDateDescription), Resources.ResourceManager, typeof(Resources));
        private static readonly DiagnosticDescriptor LazinatorOutOfDateRule = new DiagnosticDescriptor(Lazin001, LazinatorOutOfDateTitle, LazinatorOutOfDateMessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: LazinatorOutOfDateDescription);
        internal static DiagnosticDescriptor OutOfDateRule = new DiagnosticDescriptor(Lazin001, LazinatorOutOfDateTitle.ToString(), LazinatorOutOfDateMessageFormat.ToString(), Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: LazinatorOutOfDateDescription);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(LazinatorOutOfDateRule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterCompilationStartAction(compilationContext =>
            {
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
                var analyzer = new CompilationAnalyzer(lazinatorAttributeType, lazinatorInterfaceType);

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

            #endregion

            #region Per-Compilation mutable state
            
            private Dictionary<string, SourceFileInformation> CompilationInformation = new Dictionary<string, SourceFileInformation>();

            #endregion

            #region State intialization

            public CompilationAnalyzer(INamedTypeSymbol lazinatorAttributeType, INamedTypeSymbol lazinatorInterfaceType)
            {
                _lazinatorAttributeType = lazinatorAttributeType;
                _lazinatorInterfaceType = lazinatorInterfaceType;
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
                        var namedInterfaceType = namedType.GetTopLevelInterfaceImplementingAttribute(_lazinatorAttributeType);

                        if (namedInterfaceType != null)
                        {
                            var lazinatorAttribute = RoslynHelpers.GetKnownAttributes<LazinatorAttribute>(namedInterfaceType).First();
                            if (lazinatorAttribute.Autogenerate == false)
                                return;
                            var locationsExcludingCodeBehind = namedType.Locations.Where(x => !x.SourceTree.FilePath.EndsWith(".g.cs")).ToList();
                            string locationToIndexBy = locationsExcludingCodeBehind
                                .Select(x => x.SourceTree.FilePath)
                                .OrderBy(x => x)
                                .FirstOrDefault(x => CompilationInformation.ContainsKey(x));
                            if (locationToIndexBy != null)
                            {
                                SourceFileInformation sourceFileInfo = CompilationInformation[locationToIndexBy];
                                sourceFileInfo.LazinatorObject = namedType;
                                sourceFileInfo.LazinatorInterface = namedInterfaceType;
                                sourceFileInfo.LazinatorObjectLocationsExcludingCodeBehind =
                                    locationsExcludingCodeBehind
                                        .OrderByDescending(x => x.SourceTree.FilePath == locationToIndexBy)
                                        .ToList(); // place indexed location first
                                sourceFileInfo.CodeBehindLocation =
                                    namedType.Locations
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
                if (!CompilationInformation.Any())
                {
                    // No types to consider
                    return;
                }

                // Analyze types
                foreach (var compilationInfoEntry in CompilationInformation)
                {
                    var sourceFileInfo = compilationInfoEntry.Value;
                    if (sourceFileInfo.LazinatorInterface != null 
                        && sourceFileInfo.LazinatorObject != null 
                        && sourceFileInfo.LazinatorObjectLocationsExcludingCodeBehind != null 
                        && sourceFileInfo.LazinatorObjectLocationsExcludingCodeBehind.Any())
                    {
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
                        if (needsGeneration)
                        {
                            var locationOfImplementingType = sourceFileInfo.LazinatorObjectLocationsExcludingCodeBehind[0];
                            var implementingTypeRoot = locationOfImplementingType.SourceTree.GetRoot();
                            TypeDeclarationSyntax implementingTypeSyntaxNode = (TypeDeclarationSyntax) implementingTypeRoot.FindNode(locationOfImplementingType.SourceSpan);
                            Location interfaceSpecificationLocation = Location.Create(
                                locationOfImplementingType.SourceTree,
                                implementingTypeSyntaxNode.BaseList.Types.First(x => (x.Type as IdentifierNameSyntax)?.Identifier.Text.Contains(sourceFileInfo.LazinatorInterface.Name) ?? false).Span);
                            var additionalLocations = new List<Location>();
                            if (sourceFileInfo.CodeBehindLocation != null)
                                additionalLocations.Add(sourceFileInfo.CodeBehindLocation);
                            additionalLocations.AddRange(sourceFileInfo.LazinatorObjectLocationsExcludingCodeBehind);
                            var diagnostic = Diagnostic.Create(OutOfDateRule, interfaceSpecificationLocation, additionalLocations, sourceFileInfo.GetDictionaryForSymbols());
                            context.ReportDiagnostic(diagnostic);
                        }
                    }
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
