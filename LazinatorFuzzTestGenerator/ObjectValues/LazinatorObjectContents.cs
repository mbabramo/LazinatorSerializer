using LazinatorFuzzTestGenerator.Interfaces;
using LazinatorFuzzTestGenerator.ObjectTypes;
using LazinatorFuzzTestGenerator.Utility;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazinatorFuzzTestGenerator.ObjectValues
{
    using PropertyWithContents = (LazinatorObjectProperty property, int indexInParent, IObjectContents? contents);
    public class LazinatorObjectContents : IObjectContents
    {
        public ISupportedType TheType { get; init; }
        public ILazinatorObjectType TheLazinatorObjectType => (ILazinatorObjectType)TheType;
        public List<IObjectContents?>? PropertyValues { get; set; }

        public bool IsNull { get; set; }

        public LazinatorObjectContents(ISupportedType theType, Random r, int? inverseProbabilityOfNull)
        {
            TheType = theType;
            Initialize(r);
        }

        public void Initialize(Random r)
        {
            // Initialize all non-nullable properties. Set other property values to null.
            PropertyValues = new List<IObjectContents?>();
            var properties = TheLazinatorObjectType.PropertiesIncludingInherited;
            foreach (var property in properties)
            {
                var propertyType = property.supportedType;
                var isNullable = property.nullable;
                if (isNullable)
                    PropertyValues.Add(null);
                else
                    PropertyValues.Add(propertyType.GetRandomObjectContents(r, null)); 
            }
        }

        public IEnumerable<PropertyWithContents> GetPathToRandomPropertyInHierarchy(Random r)
        {
            const double probabilityStopBeforeEnd = 0.15;
            if (r.NextDouble() < probabilityStopBeforeEnd)
            {
                yield break;
            }
            else
            {
                var propertyIndexAndContents = GetRandomProperty(r);
                if (propertyIndexAndContents != null)
                {
                    yield return propertyIndexAndContents.Value;
                    var property = propertyIndexAndContents.Value.property;
                    if (property.supportedType is LazinatorObjectType objectType)
                    {
                        if (propertyIndexAndContents.Value.contents is LazinatorObjectContents objectContents)
                        {
                            foreach (var lowerLevelProperty in objectContents.GetPathToRandomPropertyInHierarchy(r))
                                yield return lowerLevelProperty;
                        }
                    }
                }
            }
        }

        private PropertyWithContents? GetRandomProperty(Random r)
        {
            if (PropertyValues is null || !PropertyValues!.Any())
                return null;
            int numPropertiesOverall = PropertyValues?.Count() ?? 0;
            int numNonNullProperties = PropertyValues?.Count(x => x != null) ?? 0;
            int indexOverall = -1;
            if (numNonNullProperties == 0 || r.NextDouble() < 0.3)
            {
                // choose a random property, regardless of whether it's null
                indexOverall = r.Next(PropertyValues!.Count);
            }
            else
            {
                int indexAmongNonNull = r.Next(numNonNullProperties);
                int numNonNullFound = 0;
                for (int i = 0; i < numPropertiesOverall; i++)
                {
                    if (PropertyValues![i] != null)
                        numNonNullFound++;
                    if (numNonNullFound == indexAmongNonNull + 1)
                    {
                        indexOverall = i;
                        break;
                    }
                }
            }
            var property = TheLazinatorObjectType.PropertiesIncludingInherited[indexOverall];
            return (property, indexOverall, PropertyValues![indexOverall]);
        }

        public string CodeToReplicateContents => GetPropertyValuesAsString(true);

        private string GetPropertyValuesAsString(bool omitPropertiesAtDefaultValues)
        {
            if (PropertyValues is null)
                return "null";
            string propertyValuesNonNullableReferencesString = GetPropertyValuesString(true, false, false, omitPropertiesAtDefaultValues);
            string propertyValuesOtherString = GetPropertyValuesString(false, true, true, omitPropertiesAtDefaultValues);
            return $@"new {TheLazinatorObjectType.Name}({propertyValuesNonNullableReferencesString})
    {{
{propertyValuesOtherString}
    }}
";
        }

        private string GetPropertyValuesString(bool nullableEnabledContext, bool other, bool includePropertyNameAndEquals, bool omitPropertiesAtDefaultValues)
        {
            int numProperties = PropertyValues!.Count;
            StringBuilder sb = new StringBuilder();
            int i = 0;
            bool isFirst = true;
            var properties = TheLazinatorObjectType.PropertiesIncludingInherited;
            foreach (var property in properties)
            {
                bool isNonNullableReferenceType = !property.nullable && property.supportedType is LazinatorClassType;
                bool include = (nullableEnabledContext && isNonNullableReferenceType) || (!nullableEnabledContext && !isNonNullableReferenceType);
                if (include && omitPropertiesAtDefaultValues)
                {
                    var value = PropertyValues[i];
                    // if this is already set to default value, then we shouldn't need to include it
                    if ((property.nullable && value is null) || (value is not null && !property.nullable && property.supportedType is PrimitiveType pt && ((PrimitiveObjectContents) value).Value!.Equals(pt.GetDefaultValueIfNotNullable())))
                        include = false;
                }
                if (include)
                {
                    var value = PropertyValues[i];
                    string getValueCode = value?.CodeToReplicateContents ?? "null";
                    if (getValueCode != "")
                    {
                        if (!isFirst)
                            sb.AppendLine(", ");
                        else
                        {
                            isFirst = false;
                        }
                        if (includePropertyNameAndEquals)
                            sb.Append($"{property.propertyName} = ");
                        bool includeTypeInfo = true; // not necessary but can make code more readable 
                        string typeInfo = includeTypeInfo ? $"({property.supportedType.AnnotatedTypeDeclaration(property.nullable, nullableEnabledContext)})" : "";
                        sb.Append($"{typeInfo} {getValueCode}");
                    }
                }
                i++;
            }
            string propertyValuesString = sb.ToString();
            return propertyValuesString;
        }

        public string CodeToTestValue(string containerName)
        {
            if (PropertyValues is null)
                return $"{containerName} is null";
            return $"{containerName}.Equals({CodeToReplicateContents})";
        }
    }
}
