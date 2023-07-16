using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LazinatorGenerator.Settings;
using LazinatorGenerator.AttributeClones;
using LazinatorCodeGen.Roslyn;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Diagnostics;

namespace LazinatorGenerator.Generator
{


    public class LazinatorPairFinder
    {
        #region Fields

        public const string LazinatorAttributeName = "Lazinator.Attributes.LazinatorAttribute";
        public const string LazinatorInterfaceName = "Lazinator.Core.ILazinator";

        Compilation Compilation;
        LazinatorConfig Config;
        string GeneratedCodeFileExtension => Config.GeneratedCodeFileExtension;

        #endregion

        #region Construction

        public LazinatorPairFinder(Compilation compilation, LazinatorConfig config)
        {
            Compilation = compilation;
            Config = config;
            
            // Check if the attribute type LazinatorAttribute is defined.
            INamedTypeSymbol lazinatorAttributeType = compilation.GetTypeByMetadataName(LazinatorAttributeName);

            // Check if the interface type ILazinator is defined.
            INamedTypeSymbol lazinatorInterfaceType = compilation.GetTypeByMetadataName(LazinatorInterfaceName);

            _lazinatorAttributeType = lazinatorAttributeType;
            _lazinatorInterfaceType = lazinatorInterfaceType;
        }

        #endregion

        #region Per-Compilation immutable state

        private readonly INamedTypeSymbol _lazinatorAttributeType;
        private readonly INamedTypeSymbol _lazinatorInterfaceType;

        #endregion

        #region Analyzing type

        internal LazinatorPairInformation GetLazinatorPairInfo(Compilation compilation, INamedTypeSymbol namedType)
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
                                            .Where(x => !x.SourceTree.FilePath.EndsWith(GeneratedCodeFileExtension))
                                            .ToList();
            var codeBehindLocations = lazinatorObjectType.Locations
                                            .Where(x => x.SourceTree.FilePath.EndsWith(GeneratedCodeFileExtension))
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
        
        private static bool disableStartingFromInterface = false;

        private void SearchForLazinatorObjectAndNamedInterface(Compilation compilation, INamedTypeSymbol namedType, out INamedTypeSymbol lazinatorObjectType, out INamedTypeSymbol namedInterfaceType)
        {
            // We want to be able to present a diagnostic starting either with the Lazinator class or vice-versa.
            if (namedType.TypeKind == TypeKind.Interface)
            {
                if (disableStartingFromInterface)
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

    }
}
