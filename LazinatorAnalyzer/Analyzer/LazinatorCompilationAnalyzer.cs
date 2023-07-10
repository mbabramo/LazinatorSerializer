using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
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

namespace LazinatorAnalyzer.Analyzer
{


    public class LazinatorCompilationAnalyzer
    {
        #region Creation

        public const string LazinatorAttributeName = "Lazinator.Attributes.LazinatorAttribute";
        public const string LazinatorInterfaceName = "Lazinator.Core.ILazinator";

        public static LazinatorCompilationAnalyzer CreateCompilationAnalyzer(Compilation compilation,
            CancellationToken cancellationToken, ImmutableArray<AdditionalText> additionalFiles)
        {
            // Check if the attribute type LazinatorAttribute is defined.
            INamedTypeSymbol lazinatorAttributeType = compilation.GetTypeByMetadataName(LazinatorAttributeName);
            if (lazinatorAttributeType == null)
            {
                return null;
            }

            // Check if the interface type ILazinator is defined.
            INamedTypeSymbol lazinatorInterfaceType = compilation.GetTypeByMetadataName(LazinatorInterfaceName);
            if (lazinatorInterfaceType == null)
            {
                return null;
            }

            (string configPath, string configString) = LazinatorConfigLoader.GetConfigPathAndText(additionalFiles, cancellationToken);
            LazinatorConfig config = new LazinatorConfig(configPath, configString);

            // Initialize state in the start action.
            var analyzer = new LazinatorCompilationAnalyzer(lazinatorAttributeType, lazinatorInterfaceType, configPath, configString, config);

            return analyzer;
        }

        public static async Task<LazinatorCompilationAnalyzer> CreateCompilationAnalyzer(Compilation compilation,
            CancellationToken cancellationToken, ImmutableArray<TextDocument> additionalFiles)
        {
            // Check if the attribute type LazinatorAttribute is defined.
            INamedTypeSymbol lazinatorAttributeType = compilation.GetTypeByMetadataName(LazinatorAttributeName);
            if (lazinatorAttributeType == null)
            {
                return null;
            }

            // Check if the interface type ILazinator is defined.
            INamedTypeSymbol lazinatorInterfaceType = compilation.GetTypeByMetadataName(LazinatorInterfaceName);
            if (lazinatorInterfaceType == null)
            {
                return null;
            }

            (string configPath, string configString) = await LazinatorConfigLoader.GetConfigPathAndText(additionalFiles, cancellationToken);
            LazinatorConfig config = new LazinatorConfig(configPath, configString);

            // Initialize state in the start action.
            var analyzer = new LazinatorCompilationAnalyzer(lazinatorAttributeType, lazinatorInterfaceType, configPath, configString, config);

            return analyzer;
        }

        #endregion

        #region Per-Compilation immutable state

        private readonly INamedTypeSymbol _lazinatorAttributeType;
        private readonly INamedTypeSymbol _lazinatorInterfaceType;
        private readonly string _configPath, _configString;
        private LazinatorConfig? _config;
        public LazinatorConfig? Config => _config;

        #endregion

        #region Per-Compilation mutable state

        private ConcurrentDictionary<Location, LazinatorPairInformation> CompilationInformation = new ConcurrentDictionary<Location, LazinatorPairInformation>();

        #endregion

        #region State intialization

        public LazinatorCompilationAnalyzer(INamedTypeSymbol lazinatorAttributeType, INamedTypeSymbol lazinatorInterfaceType, string configPath, string configString, LazinatorConfig config)
        {
            _lazinatorAttributeType = lazinatorAttributeType;
             _lazinatorInterfaceType = lazinatorInterfaceType;
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

        #region Registered analyzer actions

        public void SyntaxTreeStartAction(SyntaxTreeAnalysisContext context)
        {
        }

        public void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            try
            {
                switch (context.Symbol.Kind)
                {
                    case SymbolKind.NamedType:
                        var namedType = (INamedTypeSymbol)context.Symbol;
                        Compilation compilation = context.Compilation;
                        AnalyzeNamedType(namedType, compilation);
                        break;
                }
            }

            catch (Exception ex)
            { // catch exceptions so that we can get a more useful error in the Visual Studio consumer of this analyzer.
                throw new Exception($"Lazinator analyzer exception encountered in AnalyzeSymbol. Message {ex.Message} Stack trace: {ex.StackTrace}");
            }
        }

        public void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
        {
        }

        public void SemanticModelEndAction(SemanticModelAnalysisContext context)
        {
            try
            {
                var diagnostics = GetDiagnosticsToReport(false);
                foreach (var diagnostic in diagnostics)
                    context.ReportDiagnostic(diagnostic);
            }

            catch (Exception ex)
            { // catch exceptions so that we can get a more useful error in the Visual Studio consumer of this analyzer.
                throw new Exception($"Lazinator analyzer exception encountered in SemanticModelEndAction. Message {ex.Message} Stack trace: {ex.StackTrace}");
            }
        }

        #endregion

        #region Analyzing type

        public void AnalyzeNamedType(INamedTypeSymbol namedType, Compilation compilation)
        {
            var lazinatorPairInfo = GetLazinatorPairInfo(compilation, namedType);
            RememberLazinatorPair(lazinatorPairInfo);
        }

        private void RememberLazinatorPair(LazinatorPairInformation lazinatorPairInfo)
        {
            if (lazinatorPairInfo != null)
            {
                Location key = lazinatorPairInfo.PrimaryLocation;
                CompilationInformation.AddOrUpdate(key, lazinatorPairInfo, (k, v) => lazinatorPairInfo);
            }
        }

        private LazinatorPairInformation GetLazinatorPairInfo(Compilation compilation, INamedTypeSymbol namedType)
        {
            INamedTypeSymbol lazinatorObjectType;
            INamedTypeSymbol namedInterfaceType;

            SearchForLazinatorObjectAndNamedInterface(compilation, namedType, out lazinatorObjectType, out namedInterfaceType);
            if (namedInterfaceType != null && lazinatorObjectType != null)
            {
                var lazinatorAttribute = RoslynHelpers.GetKnownAttributes<CloneLazinatorAttribute>(namedInterfaceType).FirstOrDefault();
                if (lazinatorAttribute != null && lazinatorAttribute.Autogenerate)
                {
                    var lazinatorPairInfo = GetLazinatorPairInfo(compilation, lazinatorObjectType, namedInterfaceType);
                    return lazinatorPairInfo;
                }
            }

            return null;
        }

        private LazinatorPairInformation GetLazinatorPairInfo(Compilation compilation, INamedTypeSymbol lazinatorObjectType, INamedTypeSymbol namedInterfaceType)
        {

            var locationsExcludingCodeBehind =
                                        lazinatorObjectType.Locations
                                            .Where(x => !x.SourceTree.FilePath.EndsWith(GetGeneratedCodeFileExtension()))
                                            .ToList();
            var codeBehindLocations = lazinatorObjectType.Locations
                                            .Where(x => x.SourceTree.FilePath.EndsWith(GetGeneratedCodeFileExtension()))
                                            .ToList();
            // Complication: we should exclude code behind locations that are just for partial classes.
            // Similarly, if this is a partial class, we should exclude code behind locations for the main class.
            // We can figure this out by looking at the last partial class declared in the syntax tree and seeing if it matches.
            HashSet<Location> excludedCodeBehindLocations = null;
            foreach (Location location in codeBehindLocations)
            {
                var lastClassNode = location.SourceTree.GetRoot().DescendantNodes().Where(x => x is ClassDeclarationSyntax or StructDeclarationSyntax or RecordDeclarationSyntax).Select(x => (TypeDeclarationSyntax)x).LastOrDefault();
                SemanticModel semanticModel = compilation.GetSemanticModel(location.SourceTree);
                INamedTypeSymbol mainClass = lastClassNode == null ? null : semanticModel.GetDeclaredSymbol(lastClassNode);
                if (!SymbolEqualityComparer.Default.Equals(mainClass, lazinatorObjectType))
                {
                    if (excludedCodeBehindLocations == null)
                        excludedCodeBehindLocations = new HashSet<Location>();
                    excludedCodeBehindLocations.Add(location);
                }
            }
            // Now, make a list of all non-excluded locations
            List<Location> allLocations = lazinatorObjectType.Locations.Where(x => excludedCodeBehindLocations == null || !excludedCodeBehindLocations.Contains(x)).ToList();

            var primaryLocation = locationsExcludingCodeBehind
                .FirstOrDefault();

            if (primaryLocation != null)
            {
                LazinatorPairInformation lazinatorPairInfo = new LazinatorPairInformation();
                lazinatorPairInfo.LazinatorObject = lazinatorObjectType;
                lazinatorPairInfo.LazinatorInterface = namedInterfaceType;
                (lazinatorPairInfo.CodeBehindLocation, lazinatorPairInfo.IncorrectCodeBehindLocations, lazinatorPairInfo.LazinatorObjectLocationsExcludingCodeBehind) = LazinatorPairInformation.CategorizeLocations(Config, lazinatorObjectType, allLocations);
                return lazinatorPairInfo;
            }

            return null;
        }

        public bool DisableStartingFromInterface { get; set; }

        private void SearchForLazinatorObjectAndNamedInterface(Compilation compilation, INamedTypeSymbol namedType, out INamedTypeSymbol lazinatorObjectType, out INamedTypeSymbol namedInterfaceType)
        {
            // We want to be able to present a diagnostic starting either with the Lazinator class or vice-versa.
            if (namedType.TypeKind == TypeKind.Interface)
            {
                if (DisableStartingFromInterface)
                {
                    lazinatorObjectType = null;
                    namedInterfaceType = null;
                }
                else
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
                lazinatorObjectType = candidates.OfType<INamedTypeSymbol>().FirstOrDefault(x => namedType.GetFullNamespacePlusSimpleName() == x.GetTopLevelInterfaceImplementingAttribute(_lazinatorAttributeType).GetFullNamespacePlusSimpleName());
                
                if (lazinatorObjectType != null)
                    namedInterfaceType = namedType;
                // Note: A more comprehensive, but slower approach would be to use context.Compilation.GlobalNamespace...
            }
        }

        #endregion

        #region Diagnostics

        public List<Diagnostic> GetDiagnosticsToReport(bool suppressRegenerate)
        {
            if (!CompilationInformation.Any())
            {
                // No types to consider
                return new List<Diagnostic>();
            }
            List<Diagnostic> diagnostics = new List<Diagnostic>();
            // Analyze types
            foreach (var compilationInfoEntry in CompilationInformation)
            {
                List<Diagnostic> diagnosticsToAdd = null;
                var lazinatorPairInfo = compilationInfoEntry.Value;
                if (lazinatorPairInfo.IncorrectCodeBehindLocations?.Any() ?? false)
                {
                    diagnosticsToAdd = GetExtraFileDiagnosticsToReport(lazinatorPairInfo);
                }
                else if (lazinatorPairInfo.LazinatorInterface != null
                    && lazinatorPairInfo.LazinatorObject != null
                    && lazinatorPairInfo.LazinatorObjectLocationsExcludingCodeBehind != null
                    && lazinatorPairInfo.LazinatorObjectLocationsExcludingCodeBehind.Any())
                {
                    diagnosticsToAdd = GetDiagnosticsToReport(lazinatorPairInfo, suppressRegenerate);
                }
                if (diagnosticsToAdd != null)
                    foreach (var diagnostic in diagnosticsToAdd)
                        diagnostics.Add(diagnostic);
            }

            return diagnostics;
        }

        public void ClearDiagnostics()
        {
            CompilationInformation = new ConcurrentDictionary<Location, LazinatorPairInformation>();
        }



        public List<Diagnostic> GetDiagnosticsToReport(LazinatorPairInformation lazinatorPairInfo, bool suppressRegenerate)
        {
            bool couldBeGenerated, needsGeneration;
            AssessGenerationFeasibility(lazinatorPairInfo, out couldBeGenerated, out needsGeneration);
            if (needsGeneration || couldBeGenerated) 
            {
                var locationOfImplementingType = lazinatorPairInfo.PrimaryLocation;
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
                    return null;
                }
                else
                {
                    List<Diagnostic> diagnosticsToReturn;
                    if (UnaccountedForFieldExists(lazinatorPairInfo, out var problemFields))
                    {
                        diagnosticsToReturn = problemFields.Select(x => Diagnostic.Create(LazinatorCodeAnalyzer.UnaccountedForFieldRule, x.GetLocation())).ToList();
                    }
                    else
                        diagnosticsToReturn = new List<Diagnostic>();
                    Diagnostic diagnosticForGeneratable = GetDiagnosticForGeneratable(lazinatorPairInfo, needsGeneration, locationOfImplementingType, textSpan, suppressRegenerate);
                    if (diagnosticForGeneratable != null)
                        diagnosticsToReturn.Add(diagnosticForGeneratable);
                    return diagnosticsToReturn;
                }
            }
            else
            {
                return null;
            }
        }

        private List<Diagnostic> GetExtraFileDiagnosticsToReport(LazinatorPairInformation lazinatorPairInfo)
        {
            List<Diagnostic> diagnosticsToReturn = new List<Diagnostic>();
            List<Location> additionalLocations = GetAdditionalLocations(lazinatorPairInfo);
            additionalLocations.AddRange(lazinatorPairInfo.IncorrectCodeBehindLocations);
            diagnosticsToReturn.Add(Diagnostic.Create(LazinatorCodeAnalyzer.ExtraFileRule, lazinatorPairInfo.PrimaryLocation, additionalLocations, lazinatorPairInfo.GetSourceFileDictionary(_configPath, _configString)));
            return diagnosticsToReturn;
        }

        private static List<Location> GetAdditionalLocations(LazinatorPairInformation lazinatorPairInfo)
        {
            var additionalLocations = new List<Location>();
            if (lazinatorPairInfo.CodeBehindLocation != null)
                additionalLocations.Add(lazinatorPairInfo.CodeBehindLocation);
            additionalLocations.AddRange(lazinatorPairInfo.LazinatorObjectLocationsExcludingCodeBehind);
            return additionalLocations;
        }

        private Diagnostic GetDiagnosticForGeneratable(LazinatorPairInformation lazinatorPairInfo, bool needsGeneration, Location locationOfImplementingType, Microsoft.CodeAnalysis.Text.TextSpan? textSpan, bool suppressRegenerate)
        {
            Location interfaceSpecificationLocation = Location.Create(
                locationOfImplementingType.SourceTree,
                textSpan.Value);
            List<Location> additionalLocations = GetAdditionalLocations(lazinatorPairInfo);
            if (suppressRegenerate && !needsGeneration)
                return null;
            var diagnostic = Diagnostic.Create(needsGeneration ? LazinatorCodeAnalyzer.OutOfDateRule : LazinatorCodeAnalyzer.OptionalRegenerationRule, interfaceSpecificationLocation, additionalLocations, lazinatorPairInfo.GetSourceFileDictionary(_configPath, _configString));
            return diagnostic;
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

        private static bool UnaccountedForFieldExists(LazinatorPairInformation lazinatorPairInformation, out List<FieldDeclarationSyntax> problemFields)
        {
            var fields = lazinatorPairInformation.LazinatorObjectLocationsExcludingCodeBehind.Select(x =>
                {
                    bool success = x.SourceTree.TryGetRoot(out SyntaxNode root);
                    if (success)
                        return root;
                    return null;
                }
                )
                .Where(x => x != null)
                .SelectMany(x => x.DescendantNodes().OfType<FieldDeclarationSyntax>());
            var fieldsWithoutNonserializedAttribute = fields
                .Where(x => !x.Modifiers.Any(y => y.Kind() == SyntaxKind.StaticKeyword || y.Kind() == SyntaxKind.ConstKeyword))
                .Where(x => !x.AttributeLists.Any(
                    y => y.Attributes.Any(
                        z => (z.Name as IdentifierNameSyntax)?.Identifier.Text == "NonSerialized"
                        )
                    )
                    )
                .ToList();
            if (fieldsWithoutNonserializedAttribute.Any())
            {
                problemFields = fieldsWithoutNonserializedAttribute;
                return true;
            }
            problemFields = null;
            return false;
        }

        #endregion
    }
}
