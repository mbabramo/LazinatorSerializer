using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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
        public string Config;
        public string ConfigPath;
        public ImmutableDictionary<string, string> SymbolsDictionary;

        public LazinatorPairInformation()
        {

        }

        public LazinatorPairInformation(SemanticModel semanticModel, ImmutableDictionary<string, string> symbolsDictionary,
            IReadOnlyList<Location> additionalLocations)
        {
            SymbolsDictionary = symbolsDictionary;
            LazinatorObject = semanticModel.Compilation.GetTypeByMetadataName(symbolsDictionary["object"]);
            LazinatorInterface = semanticModel.Compilation.GetTypeByMetadataName(symbolsDictionary["interface"]);
            bool codeBehindExists = symbolsDictionary["codeBehindExists"] == "true";
            if (codeBehindExists)
            {
                CodeBehindLocation = additionalLocations.First();
                LazinatorObjectLocationsExcludingCodeBehind = additionalLocations.Skip(1).ToList();
            }
            else
            {
                CodeBehindLocation = null;
                LazinatorObjectLocationsExcludingCodeBehind = additionalLocations.ToList();
            }
            Config = symbolsDictionary["configJsonString"];
            ConfigPath = symbolsDictionary["configPath"];
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