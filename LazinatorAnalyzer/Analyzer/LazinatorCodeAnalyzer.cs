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
                var analyzer = new CompilationAnalyzer(lazinatorAttributeType, lazinatorInterfaceType, additionalFiles, configPath, configString, config);

                // Register intermediate non-end actions that access and modify the state.
                compilationContext.RegisterSyntaxNodeAction(analyzer.AnalyzeSyntaxNode, SyntaxKind.StructDeclaration, SyntaxKind.ClassDeclaration);
                compilationContext.RegisterSymbolAction(analyzer.AnalyzeSymbol, SymbolKind.NamedType, SymbolKind.Method);

                // Register an end action to report diagnostics based on the final state.
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
            private readonly string _configPath, _configString;
            private LazinatorConfig _config;

            #endregion

            #region Per-Compilation mutable state
            
            private ConcurrentDictionary<Location, LazinatorPairInformation> CompilationInformation = new ConcurrentDictionary<Location, LazinatorPairInformation>();

            #endregion

            #region State intialization

            public CompilationAnalyzer(INamedTypeSymbol lazinatorAttributeType, INamedTypeSymbol lazinatorInterfaceType, ImmutableArray<AdditionalText> additionalFiles, string configPath, string configString, LazinatorConfig config)
            {
                _lazinatorAttributeType = lazinatorAttributeType;
                _lazinatorInterfaceType = lazinatorInterfaceType;
                _additionalFiles = additionalFiles;
                // we pass information about the config as well as the config itself, since we need to use the config, but also need to pass information about it to the code fix provider
                _configPath = configPath;
                _configString = configString;
                _config = config;
            }

            private string GetGeneratedCodeFileExtension()
            {
                string fileExtension = _config?.GeneratedCodeFileExtension ?? ".laz.cs";
                return fileExtension;
            }

            #endregion

            #region Intermediate actions

            // These actions will execute in the following order.

            public void SyntaxTreeStartAction(SyntaxTreeAnalysisContext context)
            {
            }

            public void AnalyzeSymbol(SymbolAnalysisContext context)
            {
                switch (context.Symbol.Kind)
                {
                    case SymbolKind.NamedType:
                        var namedType = (INamedTypeSymbol)context.Symbol;
                        var lazinatorPairInfo = GetLazinatorPairInfo(context.Compilation, namedType);
                        if (lazinatorPairInfo != null)
                        {
                            Location key = lazinatorPairInfo.LazinatorObjectLocationsExcludingCodeBehind.First();
                            CompilationInformation.AddOrUpdate(key, lazinatorPairInfo, (k, v) => lazinatorPairInfo);
                        }

                        break;
                }
            }

            private LazinatorPairInformation GetLazinatorPairInfo(Compilation compilation, INamedTypeSymbol namedType)
            {
                INamedTypeSymbol lazinatorObjectType;
                INamedTypeSymbol namedInterfaceType;
                //Debug.WriteLine($"Considering {namedType}"); // DEBUG

                SearchForLazinatorObjectAndNamedInterface(compilation, namedType, out lazinatorObjectType, out namedInterfaceType);
                if (namedInterfaceType != null && lazinatorObjectType != null)
                {
                    var lazinatorAttribute = RoslynHelpers.GetKnownAttributes<CloneLazinatorAttribute>(namedInterfaceType).FirstOrDefault();
                    if (lazinatorAttribute != null && lazinatorAttribute.Autogenerate)
                    {
                        var lazinatorPairInfo = GetLazinatorPairInfo(lazinatorObjectType, namedInterfaceType);
                        return lazinatorPairInfo;
                    }
                }

                return null;
            }

            private LazinatorPairInformation GetLazinatorPairInfo(INamedTypeSymbol lazinatorObjectType, INamedTypeSymbol namedInterfaceType)
            {
                var locationsExcludingCodeBehind =
                                            lazinatorObjectType.Locations
                                                .Where(x => !x.SourceTree.FilePath.EndsWith(GetGeneratedCodeFileExtension()))
                                                .ToList();
                var primaryLocation = locationsExcludingCodeBehind
                    .FirstOrDefault();
                var possibleName1 =
                    RoslynHelpers.GetEncodableVersionOfIdentifier(lazinatorObjectType, true) +
                    (_config?.GeneratedCodeFileExtension ?? ".laz.cs");
                var possibleName2 =
                    RoslynHelpers.GetEncodableVersionOfIdentifier(lazinatorObjectType, false) +
                    (_config?.GeneratedCodeFileExtension ?? ".laz.cs");
                var codeBehindLocation =
                    lazinatorObjectType.Locations
                        .Where(x => x.SourceTree.FilePath.EndsWith(possibleName1) ||
                                    x.SourceTree.FilePath.EndsWith(possibleName2))
                        .FirstOrDefault();
                //Debug.WriteLine($"Primary location {primaryLocation}"); // DEBUG
                if (primaryLocation != null)
                {
                    LazinatorPairInformation lazinatorPairInfo = new LazinatorPairInformation();
                    lazinatorPairInfo.LazinatorObject = lazinatorObjectType;
                    lazinatorPairInfo.LazinatorInterface = namedInterfaceType;
                    lazinatorPairInfo.LazinatorObjectLocationsExcludingCodeBehind =
                        locationsExcludingCodeBehind;
                    lazinatorPairInfo.CodeBehindLocation =
                        codeBehindLocation;
                    return lazinatorPairInfo;
                }

                return null;
            }

            private void SearchForLazinatorObjectAndNamedInterface(Compilation compilation, INamedTypeSymbol namedType, out INamedTypeSymbol lazinatorObjectType, out INamedTypeSymbol namedInterfaceType)
            {
                // We want to be able to present a diagnostic starting either with the Lazinator class or vice-versa.
                if (namedType.TypeKind == TypeKind.Interface)
                {
                    SearchStartingFromInterface(compilation, namedType, out lazinatorObjectType, out namedInterfaceType);
                }
                else
                {
                    SearchStartingFromMainType(namedType, out lazinatorObjectType, out namedInterfaceType);
                }
            }

            private void SearchStartingFromMainType(INamedTypeSymbol namedType, out INamedTypeSymbol lazinatorObjectType, out INamedTypeSymbol namedInterfaceType)
            {
                // This is not an interface. It may be a Lazinator object with a corresponding Lazinator interface.
                lazinatorObjectType = namedType;
                namedInterfaceType = namedType.GetTopLevelInterfaceImplementingAttribute(_lazinatorAttributeType);
                //Debug.WriteLine($"Corresponding interface {namedInterfaceType}"); // DEBUG
            }

            private void SearchStartingFromInterface(Compilation compilation, INamedTypeSymbol namedType, out INamedTypeSymbol lazinatorObjectType, out INamedTypeSymbol namedInterfaceType)
            {
                lazinatorObjectType = null;
                namedInterfaceType = null;
                // If this is a Lazinator interface and we can find the Lazinator class, we'll consider doing an analysis.
                if (namedType.HasAttributeOfType<CloneLazinatorAttribute>())
                {
                    // find candidate matching classes
                    // maybe another approach would be to use SymbolFinder, but we can't load the Solution in the code analyzer. var implementations = SymbolFinder.FindImplementationsAsync(namedType, ... See https://stackoverflow.com/questions/23203206/roslyn-current-workspace-in-diagnostic-with-code-fix-project for a possible workaround
                    IEnumerable<ISymbol> candidates = compilation.GetSymbolsWithName(name => RoslynHelpers.GetNameWithoutGenericArity(name) == RoslynHelpers.GetNameWithoutGenericArity(namedType.MetadataName).Substring(1), SymbolFilter.Type);
                    lazinatorObjectType = candidates.OfType<INamedTypeSymbol>().FirstOrDefault(x => namedType.GetFullyQualifiedNameWithoutGlobal() == x.GetTopLevelInterfaceImplementingAttribute(_lazinatorAttributeType).GetFullyQualifiedNameWithoutGlobal());

                    //Debug.WriteLine($"Lazinator object {lazinatorObjectType}"); // DEBUG
                    if (lazinatorObjectType != null)
                        namedInterfaceType = namedType;
                    // Note: A more comprehensive, but slower approach would be to use context.Compilation.GlobalNamespace...
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
                        var lazinatorPairInfo = compilationInfoEntry.Value;
                        if (lazinatorPairInfo.LazinatorInterface != null 
                            && lazinatorPairInfo.LazinatorObject != null 
                            && lazinatorPairInfo.LazinatorObjectLocationsExcludingCodeBehind != null 
                            && lazinatorPairInfo.LazinatorObjectLocationsExcludingCodeBehind.Any())
                        {
                            //Debug.WriteLine($"end action {lazinatorPairInfo.LazinatorObject} {lazinatorPairInfo.LazinatorInterface}"); // DEBUG
                            var diagnostic = GetDiagnosticToReport(lazinatorPairInfo);
                            if (diagnostic != null)
                                context.ReportDiagnostic(diagnostic);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            public Diagnostic GetDiagnosticToReport(LazinatorPairInformation lazinatorPairInfo)
            {
                bool couldBeGenerated, needsGeneration;
                AssessGenerationFeasibility(lazinatorPairInfo, out couldBeGenerated, out needsGeneration);
                if (needsGeneration || couldBeGenerated)
                {
                    var locationOfImplementingType = lazinatorPairInfo.LazinatorObjectLocationsExcludingCodeBehind[0];
                    var implementingTypeRoot = locationOfImplementingType.SourceTree.GetRoot();
                    TypeDeclarationSyntax implementingTypeSyntaxNode = (TypeDeclarationSyntax)implementingTypeRoot.FindNode(locationOfImplementingType.SourceSpan);
                    Microsoft.CodeAnalysis.Text.TextSpan? textSpan =
                        implementingTypeSyntaxNode.BaseList.Types
                            .FirstOrDefault(x =>
                                (x.Type as IdentifierNameSyntax)?.Identifier.Text.Contains(lazinatorPairInfo.LazinatorInterface.Name)
                                ??
                                (x.Type as GenericNameSyntax)?.Identifier.Text.Contains(lazinatorPairInfo.LazinatorInterface.Name)
                                ??
                                false
                                )?.Span;
                    if (textSpan == null)
                    {
                        //Debug.WriteLine($"aborting -- textSpan is null"); // DEBUG
                        return null;
                    }
                    else
                    {
                        Location interfaceSpecificationLocation = Location.Create(
                            locationOfImplementingType.SourceTree,
                            textSpan.Value);
                        var additionalLocations = new List<Location>();
                        if (lazinatorPairInfo.CodeBehindLocation != null)
                            additionalLocations.Add(lazinatorPairInfo.CodeBehindLocation);
                        additionalLocations.AddRange(lazinatorPairInfo.LazinatorObjectLocationsExcludingCodeBehind);
                        var diagnostic = Diagnostic.Create(needsGeneration ? OutOfDateRule : OptionalRegenerationRule, interfaceSpecificationLocation, additionalLocations, lazinatorPairInfo.GetSourceFileDictionary(_configPath, _configString));
                        //Debug.WriteLine($"reporting diagnostic {(needsGeneration ? "out of date" : "regenerate")} for {lazinatorPairInfo.LazinatorObject}"); // DEBUG
                        return diagnostic;
                    }
                }
                else
                {
                    return null;
                }
            }

            private static void AssessGenerationFeasibility(LazinatorPairInformation lazinatorPairInfo, out bool couldBeGenerated, out bool needsGeneration)
            {
                couldBeGenerated = true;
                needsGeneration = lazinatorPairInfo.CodeBehindLocation == null;
                if (!needsGeneration)
                {
                    var success = lazinatorPairInfo.CodeBehindLocation.SourceTree.TryGetRoot(out SyntaxNode root);
                    if (success)
                    {
                        SyntaxTrivia possibleComment = root.DescendantTrivia().FirstOrDefault();
                        if (possibleComment.IsKind(SyntaxKind.SingleLineCommentTrivia))
                        {
                            string commentContent = possibleComment.ToString().Substring(2);
                            bool parse = Guid.TryParse(commentContent, out Guid codeBehindGuid);
                            if (parse)
                            {
                                var interfaceLocations = lazinatorPairInfo.LazinatorInterface.Locations;
                                if (interfaceLocations.Count() != 1)
                                {
                                    couldBeGenerated = false;
                                }
                                else
                                {
                                    var hash = LazinatorCompilation.GetHashForInterface(lazinatorPairInfo.LazinatorInterface, lazinatorPairInfo.LazinatorObject);

                                    //Debug.WriteLine($"hash {hash} code-behind guid {codeBehindGuid}"); // DEBUG
                                    if (hash != codeBehindGuid)
                                        needsGeneration = true;
                                }
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
