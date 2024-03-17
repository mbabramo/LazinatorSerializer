using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazinatorFuzzTestGenerator
{
    public class LazinatorClassType : LazinatorObjectType, ISupportedType
    {
        public override int ObjectDepth { get; }
        public override string DefinitionWord => "class";
        public bool IsAbstract { get; init; }
        public bool IsSealed { get; init; }
        public LazinatorClassType? InheritsFrom { get; init; }
        public override bool Inherits => InheritsFrom != null;
        public override bool Instantiable => !IsAbstract;
        public override bool Inheritable => !IsSealed;

        public LazinatorClassType(int uniqueID, string name, bool isAbstract, bool isSealed, LazinatorClassType? inheritsFrom, List<LazinatorObjectProperty> properties) : base(uniqueID, name, properties)
        {
            IsAbstract = isAbstract;
            IsSealed = isSealed;
            InheritsFrom = inheritsFrom;
            if (InheritsFrom == null)
                ObjectDepth = 1;
            else
                ObjectDepth = InheritsFrom.ObjectDepth + 1;
        }

        public override bool UnannotatedIsNullable(bool nullableEnabledContext)
        {
            if (nullableEnabledContext)
                return false;
            else
                return true;
        }

        public override string UnannotatedTypeDeclaration() => Name;

        public override string ILazinatorDeclaration(string namespaceString, bool nullableEnabledContext)
        {
            string inheritString = InheritsFrom == null ? "" : $" : I{InheritsFrom.Name}";
            return
$@"
using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace FuzzTests.{namespaceString}
{{
    [Lazinator((int){UniqueID})]
    public interface I{Name}{(InheritsFrom == null ? "" : $" : I{InheritsFrom.Name}")}
    {{
{PropertyDeclarations(nullableEnabledContext)}
    }}
}}
";
        }
        public override string ObjectDeclaration(string namespaceString, bool nullableEnabledContext)
        {
            return
$@"
namespace FuzzTests.{namespaceString}
{{
    public {(IsSealed ? "sealed " : "")}{(IsAbstract ? "abstract " : "")}partial class {Name} : {(InheritsFrom != null ? InheritsFrom.Name + "," : "")} I{Name}
    {{
    }}
}}
";
        }
    }
}
