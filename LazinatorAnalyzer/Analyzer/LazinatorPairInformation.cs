using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Lazinator.CodeDescription;
using LazinatorAnalyzer.Settings;
using LazinatorCodeGen.Roslyn;
using Microsoft.CodeAnalysis;

namespace LazinatorAnalyzer.Analyzer
{
    public class LazinatorPairInformation
    {
        public INamedTypeSymbol LazinatorObject;
        public INamedTypeSymbol LazinatorInterface;
        public List<Location> LazinatorObjectLocationsExcludingCodeBehind;
        public Location PrimaryLocation => LazinatorObjectLocationsExcludingCodeBehind[0];
        public Location CodeBehindLocation;
        public List<Location> IncorrectCodeBehindLocations;
        public string Config;
        public string ConfigPath;
        public ImmutableDictionary<string, string> SymbolsDictionary;

        public LazinatorPairInformation()
        {

        }

        public LazinatorPairInformation(SemanticModel semanticModel, ImmutableDictionary<string, string> symbolsDictionary,
            IReadOnlyList<Location> locations)
        {
            Config = symbolsDictionary["configJsonString"];
            ConfigPath = symbolsDictionary["configPath"];
            SymbolsDictionary = symbolsDictionary;
            LazinatorObject = semanticModel.Compilation.GetTypeByMetadataName(symbolsDictionary["object"]);
            LazinatorInterface = semanticModel.Compilation.GetTypeByMetadataName(symbolsDictionary["interface"]);
            bool codeBehindExists = symbolsDictionary["codeBehindExists"] == "true";
            if (codeBehindExists)
            {
                CategorizeLocations(locations);
            }
            else
            {
                CodeBehindLocation = null;
                LazinatorObjectLocationsExcludingCodeBehind = locations.ToList();
            }
        }

        private void CategorizeLocations(IReadOnlyList<Location> locations)
        {
            LazinatorConfig config = LoadLazinatorConfig();
            (CodeBehindLocation, IncorrectCodeBehindLocations, LazinatorObjectLocationsExcludingCodeBehind) = CategorizeLocations(config, LazinatorObject, new List<Location>(locations));
        }

        public static (Location codeBehindLocation, List<Location> incorrectCodeBehindLocations, List<Location> lazinatorObjectLocationsExcludingCodeBehind) CategorizeLocations(LazinatorConfig config, INamedTypeSymbol lazinatorObject, List<Location> mainTypeLocations)
        {
            string generatedCodeFileExtension = config?.GeneratedCodeFileExtension ?? ".laz.cs";
            bool useFullyQualifiedNames = (config?.UseFullyQualifiedNames ?? false) || lazinatorObject.ContainingType != null || lazinatorObject.IsGenericType;
            string correctCodeBehindName = RoslynHelpers.GetEncodableVersionOfIdentifier(lazinatorObject, useFullyQualifiedNames) + generatedCodeFileExtension;
            var nonGeneratedLocations = mainTypeLocations.Where(x => !x.SourceTree.FilePath.EndsWith(generatedCodeFileExtension)).ToList();
            var codeBehindLocations = mainTypeLocations.Where(x => x.SourceTree.FilePath.EndsWith(generatedCodeFileExtension)).ToList();
            IEnumerable<Location> possiblyCorrectCodeBehindLocations = mainTypeLocations.Where(x => x.SourceTree.FilePath.EndsWith(correctCodeBehindName));
            Location codeBehindLocation = possiblyCorrectCodeBehindLocations.FirstOrDefault();
            List<Location> incorrectCodeBehindLocations = codeBehindLocations.Where(x => x != codeBehindLocation).ToList();
            return (codeBehindLocation, incorrectCodeBehindLocations, nonGeneratedLocations);
        }



        public LazinatorConfig LoadLazinatorConfig()
        {
            LazinatorConfig config = null;
            if (Config != null && Config != "")
            {
                try
                {
                    config = new LazinatorConfig(ConfigPath, Config);
                }
                catch
                {
                    throw new LazinatorCodeGenException("Lazinator.config is not a valid JSON file.");
                }
            }

            return config;
        }

        public ImmutableDictionary<string, string> GetSourceFileDictionary(string configPath, string configJsonString)
        {
            var builder = ImmutableDictionary.CreateBuilder<string, string>();
            builder.Add("object", LazinatorObject.GetFullMetadataName());
            builder.Add("interface", LazinatorInterface.GetFullMetadataName());
            if (LazinatorObject.GetFullMetadataName()?.Contains("StatCollectorArrayInterchange") ?? false)
            {
                var DEBUG = 0;
            }
            builder.Add("codeBehindExists", CodeBehindLocation == null && (IncorrectCodeBehindLocations == null || !IncorrectCodeBehindLocations.Any()) ? "false" : "true");
            builder.Add("configJsonString", configJsonString);
            builder.Add("configPath", configPath);
            return builder.ToImmutable();
        }
    }
}