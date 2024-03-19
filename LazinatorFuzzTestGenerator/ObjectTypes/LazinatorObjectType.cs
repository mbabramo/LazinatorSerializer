using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public virtual List<LazinatorObjectProperty> PropertiesIncludingInherited => Properties;

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
            return {(Properties.Count == 0 ? "0" : $"{PropertiesAsTupleString("")}.GetHashCode();")
        }";
               }
    
        public string PropertiesAsTupleString(string prefix)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            int propertyCount = PropertiesIncludingInherited.Count();
            int i = 0;
            foreach (var property in PropertiesIncludingInherited)
            {
                sb.Append($"{prefix}{property.propertyName}");
                if (i++ < propertyCount)
                    sb.Append(", ");
            }
            sb.Append(")");
            return sb.ToString();
        }

        public IObjectContents GetRandomObjectContents(Random r, int? inverseProbabilityOfNull)
        {
            return new LazinatorObjectContents(this, r, inverseProbabilityOfNull);
        }
    }
}
