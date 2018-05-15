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
        public Dictionary<string, string> InterchangeConverters;
        public Dictionary<string, string> DirectConverters;
        public List<string> IgnoreRecordLikeTypes;
        public string ConfigFilePath;
        public string ConversionCodePath;
        public bool DefineConversionCodeOutsideClass => ConversionCodePath != null;

        public LazinatorConfig()
        {

        }

        public LazinatorConfig(string configPath, string configString)
        {
            InterchangeConverters = new Dictionary<string, string>(); // default
            DirectConverters = new Dictionary<string, string>();
            IgnoreRecordLikeTypes = new List<string>();
            if (configString != null)
            {
                try
                {
                    JsonObject json = JsonValue.Parse(configString).AsJsonObject;
                    const string InterchangeConvertersString = "InterchangeConverters";
                    LoadDictionary(json, InterchangeConvertersString, InterchangeConverters);
                    const string DirectConvertersString = "DirectConverters";
                    LoadDictionary(json, DirectConvertersString, DirectConverters);
                    LoadIgnoreRecordLikeTypes(json);
                    ConfigFilePath = configPath;
                    if (ConfigFilePath != null)
                        ConversionCodePath = ConfigFilePath + "\\" + (json["ConversionCodePath"]);
                }
                catch
                {
                    throw new LazinatorCodeGenException("Error parsing LazinatorConfig.json file. Make sure that the json is valid.");
                }
            }
        }

        private void LoadIgnoreRecordLikeTypes(JsonObject json)
        {
            JsonArray typeList = json["IgnoreRecordLikeTypes"];
            if (typeList != null)
                foreach (var item in typeList)
                    IgnoreRecordLikeTypes.Add(item.AsString);
        }

        private void LoadDictionary(JsonObject json, string mappingPropertyName, Dictionary<string, string> dictionary)
        {
            JsonObject mappings = json[mappingPropertyName];
            if (mappings != null)
                foreach (var pair in mappings)
                    dictionary.Add(pair.Key, pair.Value.AsString);
        }

        public string GetInterchangeConverterTypeName(INamedTypeSymbol type)
        {
            string fullyQualifiedName = type.GetFullyQualifiedName();
            if (InterchangeConverters.ContainsKey(fullyQualifiedName))
                return InterchangeConverters[fullyQualifiedName];
            return null;
        }

        public string GetDirectConverterTypeName(INamedTypeSymbol type)
        {
            string fullyQualifiedName = type.GetFullyQualifiedName();
            if (DirectConverters.ContainsKey(fullyQualifiedName))
                return DirectConverters[fullyQualifiedName];
            return null;
        }
    }
}
