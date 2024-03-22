using LazinatorFuzzTestGenerator.Interfaces;
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
            PropertyValues = new List<IObjectContents?>();
            var properties = TheLazinatorObjectType.PropertiesIncludingInherited;
            foreach (var property in properties)
            {
                var propertyType = property.supportedType;
                var isNullable = property.nullable;
                var value = propertyType.GetRandomObjectContents(r, property.nullable ? 3 : null);
                PropertyValues.Add(value);
            }
        }


        public string GetAndTestSequenceOfMutations(Random r, int numMutations, bool checkOnlyAfterAll)
        {
            Initialize(r);
            StringBuilder sb = new StringBuilder();
            string varName = "v1";
            sb.AppendLine($"{TheLazinatorObjectType.Name} {varName} = {CodeToGetValue};");
            if (!checkOnlyAfterAll)
                sb.AppendLine($"Debug.Assert({CodeToTestValue(varName)});");
            for (int i = 0; i < numMutations; i++)
            {
                sb.AppendLine(MutateAndReturnCodeForMutation(r, varName));
                if (i == numMutations - 1 || !checkOnlyAfterAll)
                {
                    sb.AppendLine($"Debug.Assert({CodeToTestValue(varName)});");
                }
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
                if (numProperties > 0)
                {
                    int propertyToMutate = r.Next(numProperties);
                    var property = TheLazinatorObjectType.PropertiesIncludingInherited[propertyToMutate];
                    var originalValue = PropertyValues[propertyToMutate];
                    var value = property.supportedType.GetRandomObjectContents(r, property.nullable ? 4 : null);
                    PropertyValues[propertyToMutate] = value;
                    return $@"{containerName}.{property.propertyName} = {value.CodeToGetValue};";
                }
                return "";
                // DEBUG -- we'd like to be able to change just one field of nested objects. With structs, that will be tricky, because we have to back out of the innermost property using temporary variables. (Fields could be set directly.)
                // DEBUG2 -- with classes, we'd like to be able to copy from one object hierarchy to another. 
                // DEBUG3 -- we actually need to use CloneLazinatorTyped etc. to make sure that Lazinating works.
            }
        }

        public string CodeToGetValue => GetPropertyValuesAsString();

        private string GetPropertyValuesAsString()
        {
            if (PropertyValues is null)
                return "null";
            int numProperties = PropertyValues.Count;
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (var property in TheLazinatorObjectType.PropertiesIncludingInherited)
            {
                var value = PropertyValues[i];
                sb.Append($"{property.propertyName} = {value?.CodeToGetValue ?? "null"}");
                if (i < numProperties - 1)
                    sb.AppendLine(",");
                else
                    sb.AppendLine();
                i++;
            }
            string propertyValuesString = sb.ToString();
            return $@"new {TheLazinatorObjectType.Name}()
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
