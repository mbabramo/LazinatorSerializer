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
        const string InitialVarName = "v";

        public LazinatorMutator(Random r, LazinatorObjectType objectType)
        {
            this.R = r;
            InitialObject = (LazinatorObjectContents)objectType.GetRandomObjectContents(r, null /* main object can't be nullable, though its properties can be */);
        }

        public string GetAndTestSequenceOfMutations(int numMutations, bool checkOnlyAfterAll)
        {
            StringBuilder sb = new StringBuilder();
            objectNamesAndContents.Add(InitialVarName, InitialObject);
            sb.AppendLine($"{InitialObject.TheLazinatorObjectType.Name} {InitialVarName} = {InitialObject.CodeToReplicateContents};");
            int tempVarCounter = 0;
            if (!checkOnlyAfterAll)
                AppendCodeToTestAllObjectValues(sb, ref tempVarCounter);
            for (int i = 0; i < numMutations; i++)
            {
                int numCurrentObjects = objectNamesAndContents.Count;
                KeyValuePair<string, LazinatorObjectContents> randomObject = objectNamesAndContents.ElementAt(R.Next(numCurrentObjects));
                (string codeForMutation, (IObjectContents objectContents, string objectName)? additionalObject) = MutateAndReturnCodeForMutation(R, randomObject.Key, ref tempVarCounter);
                sb.AppendLine(codeForMutation);
                if (i == numMutations - 1 || !checkOnlyAfterAll)
                {
                    AppendCodeToTestAllObjectValues(sb, ref tempVarCounter);
                }
            }
            if (R.Next(3) != 0)
                sb.AppendLine($"{InitialVarName} = {InitialVarName}.CloneLazinatorTyped();");
            sb.AppendLine($"{InitialVarName} = {InitialVarName}.CloneLazinatorTyped();");
            return sb.ToString();
        }


        public (string codeForMutation, (IObjectContents objectContents, string objectName)? additionalObject) MutateAndReturnCodeForMutation(Random r, string varName, ref int tempVarCounter)
        {
            var propertiesWithContents = InitialObject.GetPathToRandomPropertyInHierarchy(r).ToList();
            IObjectContents randomContents;
            if (propertiesWithContents.Any())
            {
                var lastProperty = propertiesWithContents.Last().property;
                randomContents = lastProperty.supportedType.GetRandomObjectContents(r, lastProperty.nullable ? 4 : null);
                var properties = propertiesWithContents.Select(x => x.property).ToList();
                string codeToReplicateContents = randomContents.CodeToReplicateContents;
                if (lastProperty.supportedType is LazinatorObjectType && r.Next(3) == 0)
                    codeToReplicateContents = $"({codeToReplicateContents}).CloneLazinatorTyped()";
                string codeForMutation = GetCodeToMutateProperty(r, varName, properties, codeToReplicateContents, ref tempVarCounter);
                return (codeForMutation, null);
            }
            else
                return ("", null); // nothing to do, since there are no properties
        }

        public string GetCodeToMutateProperty(Random r, string containingVariableName, List<LazinatorObjectProperty> propertyPath, string codeToSetLastProperty, ref int tempVarCounter)
        {
            if (!propertyPath.Any())
                return $"{containingVariableName} = {codeToSetLastProperty};";
            var lastProperty = propertyPath.Last();
            string lastPropertyName = lastProperty.propertyName;
            propertyPath = propertyPath.Take(propertyPath.Count - 1).ToList();
            int lastNonStructIndex = propertyPath.FindLastIndex(p => p.supportedType is not LazinatorStructType);
            var directAccessPath = propertyPath.Take(lastNonStructIndex + 1).Select(x => x.propertyName).ToList();
            var structsPath = propertyPath.Skip(lastNonStructIndex + 1).Take(propertyPath.Count - (lastNonStructIndex + 1)).Select(x => x.propertyName).ToList(); // skip last property, along with everything in direct access path
            string pathWithContainingVariableAndDirectAccess = containingVariableName + (directAccessPath.Any() ? "." : "") + String.Join(".", directAccessPath);
            if (structsPath.Any())
            {
                int tempVarCounterInit = tempVarCounter;
                tempVarCounter += structsPath.Count;
                return CodeForMutatingStructs(pathWithContainingVariableAndDirectAccess, structsPath, lastPropertyName, codeToSetLastProperty, tempVarCounterInit);
            }
            else
                return pathWithContainingVariableAndDirectAccess + "." + lastPropertyName + " = " + codeToSetLastProperty + ";";
        }

        public string CodeForMutatingStructs(string pathExcludingFinalStructs, List<string> structsPath, string lastPropertyName, string codeToGetValue, int firstTemporaryVariableIndex)
        {
            // Suppose our property path is A.B.C.D.E, and C.D.E are all strcturs, thus in structsPath. Further, suppose the initial counter is at 2. We want to generate code like this:
            // tempVar2 = A.B.C;
            // tempVar3 = A.B.C.D;
            // tempVar4 = A.B.C.D.E;
            // tempVar4.lastPropertyName = {codeToGetValue};
            // tempVar3.E = tempVar4;
            // tempVar2.D = tempVar3;
            // A.B.C = tempVar2;
            StringBuilder structCodeBuilder = new StringBuilder();
            // define temporary variables for each struct at the end
            string GetStructsPathThroughIndex(int i)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(pathExcludingFinalStructs);
                for (int j = 0; j <= i; j++)
                {
                    sb.Append(".");
                    sb.Append(structsPath[j]);
                }
                return sb.ToString();
            }
            Stack<(string tempVar, string path, string next)> tempVarAndPathNames = new Stack<(string tempVar, string path, string next)>();
            for (int i = 0; i < structsPath.Count; i++)
            {
                string tempVarName = "temp" + (firstTemporaryVariableIndex + i);
                string pathEquivalent = $"{GetStructsPathThroughIndex(i)}";
                string next = i == structsPath.Count - 1 ? "" : "." + structsPath[i + 1];
                tempVarAndPathNames.Push((tempVarName, pathEquivalent, next));
                structCodeBuilder.AppendLine($"var {tempVarName} = {pathEquivalent};");
            }
            var mostRecentPop = tempVarAndPathNames.Pop();
            structCodeBuilder.AppendLine($"{mostRecentPop.tempVar}.{lastPropertyName} = {codeToGetValue};");
            while (tempVarAndPathNames.Any())
            {
                var peek = tempVarAndPathNames.Peek();
                structCodeBuilder.AppendLine($"{peek.tempVar}{peek.next} = {mostRecentPop.tempVar};");
                mostRecentPop = tempVarAndPathNames.Pop();
            }
            return structCodeBuilder.ToString() + pathExcludingFinalStructs + "." + structsPath[0] + " = " + mostRecentPop.tempVar + ";";
        }

        public void AppendCodeToTestAllObjectValues(StringBuilder sb, ref int tempVarCounter)
        {
            foreach (var keyValuePair in objectNamesAndContents)
            {
                string varName = keyValuePair.Key;
                LazinatorObjectContents objectContents = keyValuePair.Value;
                sb.AppendLine($"bool verify{tempVarCounter} = {objectContents.CodeToTestValue(varName)};");
                sb.AppendLine($@"if (!verify{tempVarCounter++}) throw new Exception();");
            }
        }

    }
}
