using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using LazinatorCodeGen.Roslyn;
using LightJson;

namespace LazinatorAnalyzer.Settings
{

    public class LazinatorConfig
    {
        public Dictionary<string, string> InterchangeMappings;

        public LazinatorConfig()
        {

        }

        public LazinatorConfig(string configString)
        {
            InterchangeMappings = new Dictionary<string, string>(); // default
            if (configString != null)
            {
                JsonObject json = JsonValue.Parse(configString).AsJsonObject;
                JsonObject interchangeMappings = json["InterchangeMappings"];
                if (interchangeMappings != null)
                    foreach (var pair in interchangeMappings)
                        InterchangeMappings.Add(pair.Key, pair.Value.AsString);
            }
        }

        public string GetInterchangeTypeName(INamedTypeSymbol type)
        {
            string fullyQualifiedName = type.GetFullyQualifiedName();
            if (InterchangeMappings.ContainsKey(fullyQualifiedName))
                return InterchangeMappings[fullyQualifiedName];
            return null;
        }
    }
}
