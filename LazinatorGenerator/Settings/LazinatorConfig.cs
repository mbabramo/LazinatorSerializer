using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using Lazinator.CodeDescription;
using LazinatorCodeGen.Roslyn;
using LightJson;

namespace LazinatorGenerator.Settings
{
    public readonly struct LazinatorConfig
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
        private const string ProhibitLazinatorInNonLazinatorString = "ProhibitLazinatorInNonLazinator";
        private const string HideBackingFieldsString = "HideBackingFields";
        private const string HideMainPropertiesString = "HideMainProperties";
        private const string HideILazinatorPropertiesString = "HideILazinatorProperties";
        public readonly string GeneratedCodeFileExtension;
        public readonly bool UseFullyQualifiedNames;
        public readonly Dictionary<string, string> InterchangeConverters = new Dictionary<string, string>();
        public readonly Dictionary<string, string> DirectConverters = new Dictionary<string, string>();
        public readonly bool DefaultAutoChangeParent, DefaultAllowRecordLikeClasses, DefaultAllowRecordLikeRegularStructs, DefaultAllowRecordLikeReadOnlyStructs; // only read only structs allowed by default
        public readonly List<string> IgnoreRecordLikeTypes = new List<string>();
        public readonly List<string> IncludeRecordLikeTypes = new List<string>();
        public readonly string ConfigFilePath;
        public readonly string RelativeGeneratedCodePath, GeneratedCodePath;
        public readonly bool IncludeTracingCode, StepThroughProperties;
        public readonly bool ProhibitLazinatorInNonLazinator;
        public readonly bool HideBackingFields;
        public readonly bool HideMainProperties;
        public readonly bool HideILazinatorProperties;
        private readonly int CachedHashCode;

        public LazinatorConfig() 
        {
            StepThroughProperties = true;
            GeneratedCodeFileExtension = ".laz.cs";
            DefaultAutoChangeParent = true;
            DefaultAllowRecordLikeReadOnlyStructs = true;
            HideBackingFields = true;
            HideILazinatorProperties = true;
            CachedHashCode = 1;
        }

        public LazinatorConfig(string GeneratedCodeFileExtension, bool UseFullyQualifiedNames, Dictionary<string, string> InterchangeConverters, Dictionary<string, string> DirectConverters, bool DefaultAutoChangeParent, bool DefaultAllowRecordLikeClasses, bool DefaultAllowRecordLikeRegularStructs, bool DefaultAllowRecordLikeReadOnlyStructs, List<string> IgnoreRecordLikeTypes, List<string> IncludeRecordLikeTypes, string ConfigFilePath, string RelativeGeneratedCodePath, string GeneratedCodePath, bool IncludeTracingCode, bool StepThroughProperties, bool ProhibitLazinatorInNonLazinator, bool HideBackingFields, bool HideMainProperties, bool HideILazinatorProperties)
        {
            this.GeneratedCodeFileExtension = GeneratedCodeFileExtension;
            this.UseFullyQualifiedNames = UseFullyQualifiedNames;
            this.InterchangeConverters = InterchangeConverters;
            this.DirectConverters = DirectConverters;
            this.DefaultAutoChangeParent = DefaultAutoChangeParent;
            this.DefaultAllowRecordLikeClasses = DefaultAllowRecordLikeClasses;
            this.DefaultAllowRecordLikeRegularStructs = DefaultAllowRecordLikeRegularStructs;
            this.DefaultAllowRecordLikeReadOnlyStructs = DefaultAllowRecordLikeReadOnlyStructs;
            this.IgnoreRecordLikeTypes = IgnoreRecordLikeTypes;
            this.IncludeRecordLikeTypes = IncludeRecordLikeTypes;
            this.ConfigFilePath = ConfigFilePath;
            this.RelativeGeneratedCodePath = RelativeGeneratedCodePath;
            this.GeneratedCodePath = GeneratedCodePath;
            this.IncludeTracingCode = IncludeTracingCode;
            this.StepThroughProperties = StepThroughProperties;
            this.ProhibitLazinatorInNonLazinator = ProhibitLazinatorInNonLazinator;
            this.HideBackingFields = HideBackingFields;
            this.HideMainProperties = HideMainProperties;
            this.HideILazinatorProperties = HideILazinatorProperties;
            CachedHashCode = (GeneratedCodeFileExtension, UseFullyQualifiedNames, InterchangeConverters, DirectConverters, DefaultAutoChangeParent, DefaultAllowRecordLikeClasses, DefaultAllowRecordLikeRegularStructs, DefaultAllowRecordLikeReadOnlyStructs, IgnoreRecordLikeTypes, IncludeRecordLikeTypes, ConfigFilePath, RelativeGeneratedCodePath, GeneratedCodePath, IncludeTracingCode, StepThroughProperties, ProhibitLazinatorInNonLazinator, HideBackingFields, HideMainProperties, HideILazinatorProperties).GetHashCode();
        }

        public LazinatorConfig WithDefaultAllowRecordLikeReadOnlyStructs(bool value) => new LazinatorConfig(GeneratedCodeFileExtension, UseFullyQualifiedNames, InterchangeConverters, DirectConverters, DefaultAutoChangeParent, DefaultAllowRecordLikeClasses, DefaultAllowRecordLikeRegularStructs, value, IgnoreRecordLikeTypes, IncludeRecordLikeTypes, ConfigFilePath, RelativeGeneratedCodePath, GeneratedCodePath, IncludeTracingCode, StepThroughProperties, ProhibitLazinatorInNonLazinator, HideBackingFields, HideMainProperties, HideILazinatorProperties);
        public LazinatorConfig WithDefaultAllowRecordLikeStructs(bool value) => new LazinatorConfig(GeneratedCodeFileExtension, UseFullyQualifiedNames, InterchangeConverters, DirectConverters, DefaultAutoChangeParent, DefaultAllowRecordLikeClasses, value, DefaultAllowRecordLikeReadOnlyStructs, IgnoreRecordLikeTypes, IncludeRecordLikeTypes, ConfigFilePath, RelativeGeneratedCodePath, GeneratedCodePath, IncludeTracingCode, StepThroughProperties, ProhibitLazinatorInNonLazinator, HideBackingFields, HideMainProperties, HideILazinatorProperties);

        public LazinatorConfig WithDefaultAllowRecordLikeClasses(bool value) => new LazinatorConfig(GeneratedCodeFileExtension, UseFullyQualifiedNames, InterchangeConverters, DirectConverters, DefaultAutoChangeParent, value, DefaultAllowRecordLikeRegularStructs, DefaultAllowRecordLikeReadOnlyStructs, IgnoreRecordLikeTypes, IncludeRecordLikeTypes, ConfigFilePath, RelativeGeneratedCodePath, GeneratedCodePath, IncludeTracingCode, StepThroughProperties, ProhibitLazinatorInNonLazinator, HideBackingFields, HideMainProperties, HideILazinatorProperties);

        public LazinatorConfig(string configPath, string configString)
        {
            CachedHashCode = (configPath, configString).GetHashCode();
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
                    ProhibitLazinatorInNonLazinator = json.ContainsKey(ProhibitLazinatorInNonLazinatorString) ? json[ProhibitLazinatorInNonLazinatorString].AsBoolean : false;
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
                    HideBackingFields = json.ContainsKey(HideBackingFieldsString) ? json[HideBackingFieldsString].AsBoolean : true;
                    HideMainProperties = json.ContainsKey(HideMainPropertiesString) ? json[HideMainPropertiesString].AsBoolean : false;
                    HideILazinatorProperties = json.ContainsKey(HideILazinatorPropertiesString) ? json[HideILazinatorPropertiesString].AsBoolean : true;
                    ConfigFilePath = configPath?.Replace("\\LazinatorConfig.json", "");
                    if (ConfigFilePath != null)
                    {
                        ConfigFilePath = ConfigFilePath.TrimEnd(Path.DirectorySeparatorChar); // normalize
                        RelativeGeneratedCodePath = json[RelativeGeneratedCodePathString];
                        if (RelativeGeneratedCodePath == null)
                            RelativeGeneratedCodePath = "LazinatorCode"; // needed for test project
                        if (RelativeGeneratedCodePath == null)
                            GeneratedCodePath = null;
                        else
                            GeneratedCodePath = ConfigFilePath + "\\" + RelativeGeneratedCodePath;
                        System.IO.Directory.CreateDirectory(GeneratedCodePath); // create if it doesn't exist
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
            string name = UseFullyQualifiedNames ? type.GetFullNamespacePlusSimpleName() : type.GetMinimallyQualifiedName(false);
            if (InterchangeConverters.ContainsKey(name))
                return InterchangeConverters[name];
            return null;
        }

        public string GetDirectConverterTypeName(INamedTypeSymbol type)
        {
            string name = UseFullyQualifiedNames ? type.GetFullNamespacePlusSimpleName() : type.GetMinimallyQualifiedName(false);
            if (DirectConverters.ContainsKey(name))
                return DirectConverters[name];
            return null;
        }

        public override int GetHashCode()
        {
            // We're using a cached hash code so that we don't need to redo a cache calculation every time this config is hashed in source generation.
            return CachedHashCode;
        }
    }
}
