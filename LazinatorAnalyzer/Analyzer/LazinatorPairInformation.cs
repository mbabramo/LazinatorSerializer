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
        public Location CodeBehindLocation;
        public List<Location> IncorrectCodeBehindLocations;
        public string Config;
        public string ConfigPath;
        public ImmutableDictionary<string, string> SymbolsDictionary;

        public LazinatorPairInformation()
        {

        }

        public LazinatorPairInformation(SemanticModel semanticModel, ImmutableDictionary<string, string> symbolsDictionary,
            IReadOnlyList<Location> additionalLocations)
        {
            Config = symbolsDictionary["configJsonString"];
            ConfigPath = symbolsDictionary["configPath"];
            LazinatorConfig config = LoadLazinatorConfig();
            SymbolsDictionary = symbolsDictionary;
            LazinatorObject = semanticModel.Compilation.GetTypeByMetadataName(symbolsDictionary["object"]);
            LazinatorInterface = semanticModel.Compilation.GetTypeByMetadataName(symbolsDictionary["interface"]);
            bool codeBehindExists = symbolsDictionary["codeBehindExists"] == "true";
            if (codeBehindExists)
            {
                (CodeBehindLocation, IncorrectCodeBehindLocations, LazinatorObjectLocationsExcludingCodeBehind) = CategorizeLocations(config, LazinatorObject, new List<Location>(additionalLocations));
            }
            else
            {
                CodeBehindLocation = null;
                LazinatorObjectLocationsExcludingCodeBehind = additionalLocations.ToList();
            }
        }

        public static (Location codeBehindLocation, List<Location> incorrectCodeBehindLocations, List<Location> lazinatorObjectLocationsExcludingCodeBehind) CategorizeLocations(LazinatorConfig config, INamedTypeSymbol lazinatorObject, List<Location> locationsBesidesMainTypeAndInterface)
        {
            string generatedCodeFileExtension = config?.GeneratedCodeFileExtension ?? ".laz.cs";
            bool useFullyQualifiedNames = (config?.UseFullyQualifiedNames ?? false) || lazinatorObject.ContainingType != null || lazinatorObject.IsGenericType;
            string correctCodeBehindName = RoslynHelpers.GetEncodableVersionOfIdentifier(lazinatorObject, useFullyQualifiedNames) + generatedCodeFileExtension;
            IEnumerable<Location> probablyCorrectNames = locationsBesidesMainTypeAndInterface.Where(x => x.SourceTree.FilePath.EndsWith(generatedCodeFileExtension));
            Location codeBehindLocation = probablyCorrectNames.FirstOrDefault();
            List<Location> incorrectCodeBehindLocations = locationsBesidesMainTypeAndInterface.Where(x => !x.SourceTree.FilePath.EndsWith(correctCodeBehindName)).ToList();
            if (probablyCorrectNames.Count() > 1)
                incorrectCodeBehindLocations.AddRange(probablyCorrectNames.Skip(1));
            var lazinatorObjectLocationsExcludingCodeBehind = locationsBesidesMainTypeAndInterface.Where(x => !x.Equals(codeBehindLocation) && !incorrectCodeBehindLocations.Any(y => y.Equals(x))).ToList();
            return (codeBehindLocation, incorrectCodeBehindLocations, lazinatorObjectLocationsExcludingCodeBehind);
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
            builder.Add("codeBehindExists", CodeBehindLocation == null ? "false" : "true");
            builder.Add("configJsonString", configJsonString);
            builder.Add("configPath", configPath);
            return builder.ToImmutable();
        }
    }
}