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
        public bool NullableContextEnabled { get; init; }
        public int UniqueID { get; init; }
        public abstract string DefinitionWord { get; }
        public string Name { get; init; }
        public List<LazinatorObjectProperty> Properties { get; init; } = new List<LazinatorObjectProperty>();
        public abstract bool Inherits { get; }
        public abstract bool Inheritable { get; }
        public abstract bool Instantiable { get; }
        public abstract int ObjectDepth { get; }

        public virtual List<LazinatorObjectProperty> PropertiesIncludingInherited => Properties.OrderBy(x => x.propertyName).ToList();

        public abstract bool UnannotatedIsNullable();

        public LazinatorObjectType(int uniqueID, string name, List<LazinatorObjectProperty> properties)
        {
            UniqueID = uniqueID;
            Name = name;
            Properties = properties;
        }

        public string PropertyDeclarations()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var property in Properties)
            {
                sb.Append("        ");
                sb.AppendLine(property.Declaration());
            }
            return sb.ToString();
        }

        public abstract string ILazinatorDeclaration(string namespaceString);

        public abstract string UnannotatedTypeDeclaration();
        public virtual string ObjectDeclaration(string namespaceString)
        {
            return
$@"{(NullableContextEnabled ? "using System.Diagnostics.CodeAnalysis;" : "")}
namespace FuzzTests.{namespaceString}
{{
    {GetObjectDeclaration_Top()}
    {{
{EqualsAndGetHashCodeString()}
    }}
}}
";
        }

        public abstract string GetObjectDeclaration_Top();

        public string EqualsAndGetHashCodeString()
        {
            return $@"
       public override bool Equals({(NullableContextEnabled ? "[NotNullWhen(true)] " : "")}object{(NullableContextEnabled ? "?" : "")} obj)
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
    }
}
