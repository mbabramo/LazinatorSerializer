using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using LazinatorCodeGen.Roslyn;
using Microsoft.CodeAnalysis;

namespace LazinatorAnalyzer.Analyzer
{
    public class SourceFileInformation
    {
        public INamedTypeSymbol LazinatorObject;
        public INamedTypeSymbol LazinatorInterface;
        public List<Location> LazinatorObjectLocationsExcludingCodeBehind;
        public Location CodeBehindLocation;
        public string Config;
        public string ConfigPath;

        public SourceFileInformation()
        {

        }

        public SourceFileInformation(SemanticModel semanticModel, ImmutableDictionary<string, string> symbolsDictionary,
            IReadOnlyList<Location> additionalLocations)
        {
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
            Config = symbolsDictionary["config"];
            ConfigPath = symbolsDictionary["configPath"];
        }

        public ImmutableDictionary<string, string> GetSourceFileDictionary(string config, string configPath)
        {
            var builder = ImmutableDictionary.CreateBuilder<string, string>();
            builder.Add("object", LazinatorObject.GetFullNamespace() + "." + LazinatorObject.MetadataName);
            builder.Add("interface", LazinatorInterface.GetFullNamespace() + "." + LazinatorInterface.MetadataName);
            builder.Add("codeBehindExists", CodeBehindLocation == null ? "false" : "true");
            builder.Add("config", config);
            builder.Add("configPath", configPath);
            return builder.ToImmutable();
        }
    }
}