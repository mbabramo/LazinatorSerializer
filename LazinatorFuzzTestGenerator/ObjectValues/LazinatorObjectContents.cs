﻿using LazinatorFuzzTestGenerator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazinatorFuzzTestGenerator.ObjectValues
{
    public class LazinatorObjectContents : IObjectContents
    {
        public ISupportedType TheType { get; init; }
        public ILazinatorObjectType TheLazinatorObjectType => (ILazinatorObjectType)TheType;
        private List<IObjectContents?>? PropertyValues { get; set; }

        public bool IsNull { get; set; }

        public LazinatorObjectContents(ISupportedType theType, Random r, int? inverseProbabilityOfNull)
        {
            TheType = theType;
            SetToRandom(r, inverseProbabilityOfNull);
        }

        private void SetToRandom(Random r, int? inverseProbabilityOfNull)
        {
            if (inverseProbabilityOfNull == null || r.Next((int)inverseProbabilityOfNull) != 0)
                Initialize(r);
            else
                PropertyValues = null;
        }

        public void Initialize(Random r)
        {
            var properties = TheLazinatorObjectType.Properties;
            PropertyValues = new List<IObjectContents?>(properties.Count);
            for (int i = 0; i < properties.Count; i++)
            {
                var property = properties[i];
                var propertyType = property.supportedType;
                var isNullable = property.nullable;
                var value = propertyType.GetRandomObjectContents(r, property.nullable ? 3 : null);
                PropertyValues.Add(value);
            }
        }


        public string GetAndTestSequenceOfMutations(Random r, int numMutations)
        {
            Initialize(r);
            StringBuilder sb = new StringBuilder();
            string varName = "v1";
            sb.AppendLine($"{TheLazinatorObjectType.Name} {varName} = {CodeToGetValue};");
            sb.AppendLine($"Debug.Assert({CodeToTestValue})");
            for (int i = 0; i < numMutations; i++)
            {
                sb.AppendLine(MutateAndReturnCodeForMutation(r, varName));
                sb.AppendLine($"Debug.Assert({CodeToTestValue})");
            }
            return sb.ToString();
        }

        public string MutateAndReturnCodeForMutation(Random r, string containerName)
        {
            if (r.Next(3) == 0 || PropertyValues == null)
            {
                Initialize(r);
                return $@"{containerName} = {CodeToGetValue};";
            }
            else
            {
                int numProperties = PropertyValues.Count;
                int propertyToMutate = r.Next(numProperties);
                var property = TheLazinatorObjectType.Properties[propertyToMutate];
                var originalValue = PropertyValues[propertyToMutate];
                var value = property.supportedType.GetRandomObjectContents(r, property.nullable ? 4 : null);
                PropertyValues[propertyToMutate] = value;
                return $@"{containerName}.{property.propertyName} = {value.CodeToGetValue};";
                // DEBUG -- we'd like to be able to change just one field of nested objects. With structs, that will be tricky, because we have to back out of the innermost property using temporary variables. (Fields could be set directly.)
                // DEBUG2 -- with classes, we'd like to be able to copy from one object hierarchy to another. 
            }
        }

        public string CodeToGetValue => GetPropertyValuesAsString();

        private string GetPropertyValuesAsString()
        {
            if (PropertyValues is null)
                return "null";
            int numProperties = PropertyValues.Count;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < numProperties; i++)
            {
                var property = TheLazinatorObjectType.Properties[i];
                var value = PropertyValues[i];
                sb.Append($"            {property.propertyName} = {value?.CodeToGetValue ?? "null"}");
                if (i < numProperties - 1)
                    sb.AppendLine(",");
                else
                    sb.AppendLine();
            }
            string propertyValuesString = sb.ToString();
            return $@"    new {TheLazinatorObjectType.Name}()
    {{
{propertyValuesString}
    }}
";
        }

        public string CodeToTestValue(string containerName)
        {
            if (PropertyValues is null)
                return $"{containerName} is null";
            return $"{containerName}.Equals({CodeToGetValue})";
        }
    }
}
