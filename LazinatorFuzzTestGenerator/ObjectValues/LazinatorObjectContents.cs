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

        public string InitializationCode(string varName)
        {
            return $@"{varName} = {CodeToGetValue};";
        }

        private PropertyWithContents? GetRandomProperty(Random r)
        {
            if (PropertyValues is null || !PropertyValues!.Any())
                return null;
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
                for (int i = 0; i <= indexAmongNonNull; i++)
                {
                    if (PropertyValues![numNonNullFound] != null)
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

        public IEnumerable<PropertyWithContents> GetRandomPropertiesMovingInward(Random r)
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
                            foreach (var lowerLevelProperty in objectContents.GetRandomPropertiesMovingInward(r))
                                yield return lowerLevelProperty;
                        }
                    }
                }
            }
        }

        public string CodeToGetValue => GetPropertyValuesAsString();

        private string GetPropertyValuesAsString()
        {
            if (PropertyValues is null)
                return "null";
            string propertyValuesNonNullableReferencesString = GetPropertyValuesString(true, false, false);
            string propertyValuesOtherString = GetPropertyValuesString(false, true, true);
            return $@"new {TheLazinatorObjectType.Name}({propertyValuesNonNullableReferencesString})
    {{
{propertyValuesOtherString}
    }}
";
        }

        private string GetPropertyValuesString(bool nonNullableReferenceTypes, bool other, bool includePropertyNameAndEquals)
        {
            int numProperties = PropertyValues!.Count;
            StringBuilder sb = new StringBuilder();
            int i = 0;
            bool isFirst = true;
            var properties = TheLazinatorObjectType.PropertiesIncludingInherited;
            foreach (var property in properties)
            {
                bool isNonNullableReferenceType = !property.nullable && property.supportedType is LazinatorClassType;
                bool include = (nonNullableReferenceTypes && isNonNullableReferenceType) || (!nonNullableReferenceTypes && !isNonNullableReferenceType);
                if (include)
                {
                    var value = PropertyValues[i];
                    if (!isFirst)
                        sb.AppendLine(", ");
                    else
                    {
                        sb.AppendLine("");
                        isFirst = false;
                    }
                    if (includePropertyNameAndEquals) 
                        sb.Append($"{property.propertyName} = ");
                    sb.Append($"/* {property.propertyName} */ {value?.CodeToGetValue ?? "null"}");
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
            return $"{containerName}.Equals({CodeToGetValue})";
        }
    }
}
