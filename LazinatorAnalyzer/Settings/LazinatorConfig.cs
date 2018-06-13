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
        private const string RelativeGeneratedCodePathString = "RelativeGeneratedCodePath";
        private const string GeneratedCodeFileExtensionString = "GeneratedCodeFileExtension";
        private const string UseFullyQualifiedNamesString = "UseFullyQualifiedNames";
        private const string InterchangeConvertersString = "InterchangeConverters";
        private const string DirectConvertersString = "DirectConverters";
        private const string IgnoreRecordLikeTypesString = "IgnoreRecordLikeTypes";
        private const string IncludeRecordLikeTypesString = "IncludeRecordLikeTypes";
        private const string IncludeTracingCodeString = "IncludeTracingCode";
        private const string StepThroughPropertiesString = "StepThroughProperties";
        private const string DefaultAutoChangeParentString = "DefaultAutoChangeParent";
        private const string DefaultAllowRecordLikeClassesString = "DefaultAllowRecordLikeClasses";
        private const string DefaultAllowRecordLikeRegularStructsString = "DefaultAllowRecordLikeRegularStructs";
        private const string DefaultAllowRecordLikeReadOnlyStructsString = "DefaultAllowRecordLikeReadOnlyStructs";
        public string GeneratedCodeFileExtension;
        public bool UseFullyQualifiedNames;
        public Dictionary<string, string> InterchangeConverters;
        public Dictionary<string, string> DirectConverters;
        public bool DefaultAutoChangeParent, DefaultAllowRecordLikeClasses, DefaultAllowRecordLikeRegularStructs, DefaultAllowRecordLikeReadOnlyStructs; // only read only structs allowed by default
        public List<string> IgnoreRecordLikeTypes;
        public List<string> IncludeRecordLikeTypes;
        public string ConfigFilePath;
        public string RelativeGeneratedCodePath, GeneratedCodePath;
        public bool IncludeTracingCode, StepThroughProperties;

        public LazinatorConfig()
        {

        }

        public LazinatorConfig(string configPath, string configString)
        {
            InterchangeConverters = new Dictionary<string, string>(); // default
            DirectConverters = new Dictionary<string, string>();
            IgnoreRecordLikeTypes = new List<string>();
            IncludeRecordLikeTypes = new List<string>();
            if (configString != null)
            {
                try
                {
                    JsonObject json = JsonValue.Parse(configString).AsJsonObject;
                    IncludeTracingCode = json.ContainsKey(IncludeTracingCodeString) ? json[IncludeTracingCodeString].AsBoolean : false;
                    StepThroughProperties = json.ContainsKey(StepThroughPropertiesString) ? json[StepThroughPropertiesString].AsBoolean : true;

                    UseFullyQualifiedNames = json.ContainsKey(UseFullyQualifiedNamesString) ? json[UseFullyQualifiedNamesString].AsBoolean : false;
                    GeneratedCodeFileExtension = json.ContainsKey(GeneratedCodeFileExtensionString) ? json[GeneratedCodeFileExtensionString].AsString : ".laz.cs";
                    LoadDictionary(json, InterchangeConvertersString, InterchangeConverters);
                    LoadDictionary(json, DirectConvertersString, DirectConverters);
                    LoadIgnoreRecordLikeTypes(json);
                    LoadIncludeRecordLikeTypes(json);
                    DefaultAllowRecordLikeClasses = json.ContainsKey(DefaultAllowRecordLikeClassesString) ? json[DefaultAllowRecordLikeClassesString].AsBoolean : false;
                    DefaultAutoChangeParent = json.ContainsKey(DefaultAutoChangeParentString) ? json[DefaultAutoChangeParentString].AsBoolean : true;
                    DefaultAllowRecordLikeRegularStructs = json.ContainsKey(DefaultAllowRecordLikeRegularStructsString) ? json[DefaultAllowRecordLikeRegularStructsString].AsBoolean : false;
                    DefaultAllowRecordLikeReadOnlyStructs = json.ContainsKey(DefaultAllowRecordLikeReadOnlyStructsString) ? json[DefaultAllowRecordLikeReadOnlyStructsString].AsBoolean : true;
                    ConfigFilePath = configPath;
                    if (ConfigFilePath != null)
                    {
                        RelativeGeneratedCodePath = json[RelativeGeneratedCodePathString];
                        if (RelativeGeneratedCodePath == null)
                            GeneratedCodePath = null;
                        else
                            GeneratedCodePath = ConfigFilePath + "\\" + RelativeGeneratedCodePath;
                    }
                }
                catch
                {
                    throw new LazinatorCodeGenException("Error parsing LazinatorConfig.json file. Make sure that the json is valid.");
                }
            }
        }

        private void LoadIgnoreRecordLikeTypes(JsonObject json)
        {
            JsonArray typeList = json[IgnoreRecordLikeTypesString];
            if (typeList != null)
                foreach (var item in typeList)
                    IgnoreRecordLikeTypes.Add(item.AsString);
        }

        private void LoadIncludeRecordLikeTypes(JsonObject json)
        {
            JsonArray typeList = json[IncludeRecordLikeTypesString];
            if (typeList != null)
                foreach (var item in typeList)
                    IncludeRecordLikeTypes.Add(item.AsString);
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
            string name = UseFullyQualifiedNames ? type.GetFullyQualifiedNameWithoutGlobal() : type.GetMinimallyQualifiedName();
            if (InterchangeConverters.ContainsKey(name))
                return InterchangeConverters[name];
            return null;
        }

        public string GetDirectConverterTypeName(INamedTypeSymbol type)
        {
            string name = UseFullyQualifiedNames ? type.GetFullyQualifiedNameWithoutGlobal() : type.GetMinimallyQualifiedName();
            if (DirectConverters.ContainsKey(name))
                return DirectConverters[name];
            return null;
        }
    }
}
