using Lazinator.CodeDescription;
using LazinatorFuzzTestGenerator.Interfaces;
using LazinatorFuzzTestGenerator.ObjectTypes;
using LazinatorFuzzTestGenerator.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazinatorFuzzTestGenerator.ObjectValues
{
    internal class LazinatorMutator
    {
        Random R;

        Dictionary<string, LazinatorObjectContents> objectNamesAndContents = new Dictionary<string, LazinatorObjectContents>();

        UniqueCSharpNameGenerator nameGenerator = new UniqueCSharpNameGenerator();
        LazinatorObjectContents InitialObject;

        public LazinatorMutator(Random r, LazinatorObjectType objectType)
        {
            this.R = r;
            InitialObject = (LazinatorObjectContents)objectType.GetRandomObjectContents(r, 5);
        }

        public string GetAndTestSequenceOfMutations(int numMutations, bool checkOnlyAfterAll)
        {
            StringBuilder sb = new StringBuilder();
            string varName = nameGenerator.GetUniqueName(R, false);
            objectNamesAndContents.Add(varName, InitialObject);
            sb.AppendLine($"{InitialObject.TheLazinatorObjectType.Name} {varName} = {InitialObject.CodeToGetValue};");
            if (!checkOnlyAfterAll)
                AppendCodeToTestAllObjectValues(sb);
            for (int i = 0; i < numMutations; i++)
            {
                int numCurrentObjects = objectNamesAndContents.Count;
                KeyValuePair<string, LazinatorObjectContents> randomObject = objectNamesAndContents.ElementAt(R.Next(numCurrentObjects));
                (string codeForMutation, (IObjectContents objectContents, string objectName)? additionalObject) = randomObject.Value.MutateAndReturnCodeForMutation(R, randomObject.Key);
                sb.AppendLine();
                if (i == numMutations - 1 || !checkOnlyAfterAll)
                {
                    AppendCodeToTestAllObjectValues(sb);
                }
            }
            return sb.ToString();
        }

        public void AppendCodeToTestAllObjectValues(StringBuilder sb)
        {
            foreach (var keyValuePair in objectNamesAndContents)
            {
                string varName = keyValuePair.Key;
                LazinatorObjectContents objectContents = keyValuePair.Value;
                sb.AppendLine($"Debug.Assert({objectContents.CodeToTestValue(varName)});");
            }
        }

    }
}
