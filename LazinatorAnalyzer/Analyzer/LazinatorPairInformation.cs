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
            string generatedCodeFileExtension = config?.GeneratedCodeFileExtension ?? ".laz.cs";
            SymbolsDictionary = symbolsDictionary;
            LazinatorObject = semanticModel.Compilation.GetTypeByMetadataName(symbolsDictionary["object"]);
            LazinatorInterface = semanticModel.Compilation.GetTypeByMetadataName(symbolsDictionary["interface"]);
            bool codeBehindExists = symbolsDictionary["codeBehindExists"] == "true";
            if (codeBehindExists)
            {
                bool useFullyQualifiedNames = (config?.UseFullyQualifiedNames ?? false) || LazinatorObject.ContainingType != null || LazinatorObject.IsGenericType;
                string correctCodeBehindName = RoslynHelpers.GetEncodableVersionOfIdentifier(LazinatorObject, useFullyQualifiedNames) + generatedCodeFileExtension;
                IEnumerable<Location> probablyCorrectNames = additionalLocations.Where(x => x.SourceTree.FilePath.EndsWith(correctCodeBehindName));
                CodeBehindLocation = probablyCorrectNames.FirstOrDefault();
                IncorrectCodeBehindLocations = additionalLocations.Where(x => !x.SourceTree.FilePath.EndsWith(correctCodeBehindName)).ToList();
                if (probablyCorrectNames.Count() > 1)
                    IncorrectCodeBehindLocations.AddRange(probablyCorrectNames.Skip(1));

                LazinatorObjectLocationsExcludingCodeBehind = additionalLocations.Skip(1).ToList();
            }
            else
            {
                CodeBehindLocation = null;
                LazinatorObjectLocationsExcludingCodeBehind = additionalLocations.ToList();
            }
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