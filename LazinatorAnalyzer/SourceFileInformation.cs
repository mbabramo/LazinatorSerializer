using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using LazinatorCodeGen.Roslyn;
using Microsoft.CodeAnalysis;

namespace LazinatorAnalyzer
{
    public class SourceFileInformation
    {
        public INamedTypeSymbol LazinatorObject;
        public INamedTypeSymbol LazinatorInterface;
        public List<Location> LazinatorObjectLocationsExcludingCodeBehind;
        public Location CodeBehindLocation;

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
        }

        public ImmutableDictionary<string, string> GetDictionaryForSymbols()
        {
            var builder = ImmutableDictionary.CreateBuilder<string, string>();
            builder.Add("object", LazinatorObject.GetFullyQualifiedName());
            builder.Add("interface", LazinatorInterface.GetFullyQualifiedName());
            builder.Add("codeBehindExists", CodeBehindLocation == null ? "false" : "true");
            return builder.ToImmutable();
        }
    }
}