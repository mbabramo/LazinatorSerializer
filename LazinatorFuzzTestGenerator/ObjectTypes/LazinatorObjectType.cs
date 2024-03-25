using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lazinator.CodeDescription;
using LazinatorFuzzTestGenerator.Interfaces;
using LazinatorFuzzTestGenerator.ObjectValues;

namespace LazinatorFuzzTestGenerator.ObjectTypes
{
    public abstract class LazinatorObjectType : ILazinatorObjectType, ISupportedType
    {
        public int UniqueID { get; init; }
        public abstract string DefinitionWord { get; }
        public string Name { get; init; }
        public List<LazinatorObjectProperty> Properties { get; init; } = new List<LazinatorObjectProperty>();
        public abstract bool Inherits { get; }
        public abstract bool Inheritable { get; }
        public abstract bool Instantiable { get; }
        public abstract int ObjectDepth { get; }

        public virtual List<LazinatorObjectProperty> PropertiesIncludingInherited => Properties.OrderBy(x => x.propertyName).ToList();

        public abstract bool UnannotatedIsNullable(bool nullableEnabledContext);

        public LazinatorObjectType(int uniqueID, string name, List<LazinatorObjectProperty> properties)
        {
            UniqueID = uniqueID;
            Name = name;
            Properties = properties;
        }

        public string PropertyDeclarations(bool nullableEnabledContext)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var property in Properties)
            {
                sb.Append("        ");
                sb.AppendLine(property.Declaration(nullableEnabledContext));
            }
            return sb.ToString();
        }

        public abstract string ILazinatorDeclaration(string namespaceString, bool nullableEnabledContext);

        public abstract string UnannotatedTypeDeclaration();
        public virtual string ObjectDeclaration(string namespaceString, bool nullableEnabledContext)
        {
            return
$@"{(nullableEnabledContext ? "using System.Diagnostics.CodeAnalysis;" : "")}
namespace FuzzTests.{namespaceString}
{{
    {GetObjectDeclaration_Top(nullableEnabledContext)}
    {{
{EqualsAndGetHashCodeString(nullableEnabledContext)}
    }}
}}
";
        }

        public abstract string GetObjectDeclaration_Top(bool nullableEnabledContext);

        public string EqualsAndGetHashCodeString(bool nullableContextEnabled)
        {
            return $@"
       public override bool Equals({(nullableContextEnabled ? "[NotNullWhen(true)] " : "")}object{(nullableContextEnabled ? "?" : "")} obj)
        {{
            if (obj == null)
                return false;
            if (obj.GetType() != GetType())
                return false;
            var other = ({Name}) obj;
            return {(PropertiesIncludingInherited.Count() == 0 ? "true" : $"{PropertiesAsTupleString("other.")}.Equals({PropertiesAsTupleString("")})")};
        }}

        public override int GetHashCode()
        {{
            return {(Properties.Count == 0 ? "0" : $"{PropertiesAsTupleString("")}.GetHashCode()")};
        }}
";
        }
    
        public string PropertiesAsTupleString(string prefix)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            string plist = String.Join(",", PropertiesIncludingInherited.Select(x => $"{prefix}{x.propertyName}"));
            sb.Append(plist);
            if (PropertiesIncludingInherited.Count == 1)
                sb.Append(", 0"); // to make it a tuple, we need an additional property
            sb.Append(")");
            return sb.ToString();
        }

        public IObjectContents GetRandomObjectContents(Random r, int? inverseProbabilityOfNull)
        {
            return new LazinatorObjectContents(this, r, inverseProbabilityOfNull);
        }

        public List<LazinatorObjectProperty> GetRandomPropertyPath(Random r)
        {
            List<LazinatorObjectProperty> l = new List<LazinatorObjectProperty>();
            var current = this;
            while (current != null)
            {
                var properties = PropertiesIncludingInherited;
                var randomProperty = properties[r.Next(properties.Count)];
                l.Add(randomProperty);
                if (randomProperty.supportedType is LazinatorObjectType)
                    current = (LazinatorObjectType)randomProperty.supportedType;
                else
                    current = null;
            }
            return l;
        }

        public string GetRandomPropertyPathString(Random r, string containingVariableName, ref int tempVarCounter)
        {
            var propertyPath = GetRandomPropertyPath(r);
            if (!propertyPath.Any())
                return "";
            var lastProperty = propertyPath.Last();
            string lastPropertyName = lastProperty.propertyName;
            int lastNonStructIndex = propertyPath.FindLastIndex(p => p.supportedType is not LazinatorStructType);
            var directAccessPath = propertyPath.Take(lastNonStructIndex + 1).Select(x => x.propertyName).ToList();
            var structsPath = propertyPath.Skip(lastNonStructIndex + 1).Take(propertyPath.Count - (lastNonStructIndex + 1) - 1).Select(x => x.propertyName).ToList(); // skip last property, along with everything in direct access path
            IObjectContents valueToMutateTo = lastProperty.supportedType.GetRandomObjectContents(r, lastProperty.nullable ? 3 : null);
            string codeToGetValue = valueToMutateTo.CodeToGetValue;
            string pathExcludingFinalStructs = containingVariableName + String.Join(".", directAccessPath);
            if (structsPath.Any())
            {
                int tempVarCounterInit = tempVarCounter;
                tempVarCounter += structsPath.Count;
                return CodeForMutatingStructs(pathExcludingFinalStructs, structsPath, lastPropertyName, codeToGetValue, tempVarCounterInit);
            }
            else
                return pathExcludingFinalStructs + lastPropertyName + " = " + codeToGetValue;
        }

        public static string CodeForMutatingStructs(string pathExcludingFinalStructs, List<string> structsPath, string lastPropertyName, string codeToGetValue, int firstTemporaryVariableIndex)
        {
            // Suppose our property path is A.B.C.D.E, and C.D.E are all strcturs, thus in structsPath. Further, suppose the initial counter is at 2. We want to generate code like this:
            // tempVar2 = A.B.C;
            // tempVar3 = A.B.C.D;
            // tempVar4 = A.B.C.D.E;
            // tempVar4.lastPropertyName = {codeToGetValue};
            // tempVar3.E = tempVar4;
            // tempVar2.D = tempVar3;
            // A.B = tempVar2;
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
            return structCodeBuilder.ToString() + pathExcludingFinalStructs + " = " + mostRecentPop.tempVar + ";";
        }
    }
}
