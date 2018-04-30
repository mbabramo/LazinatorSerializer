using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.CodeDescription;
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
                try
                {
                    JsonObject json = JsonValue.Parse(configString).AsJsonObject;
                    JsonObject interchangeMappings = json["InterchangeMappings"];
                    if (interchangeMappings != null)
                        foreach (var pair in interchangeMappings)
                            InterchangeMappings.Add(pair.Key, pair.Value.AsString);
                }
                catch
                {
                    throw new LazinatorCodeGenException("Error parsing LazinatorConfig.json file. Make sure that the json is valid.");
                }
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
