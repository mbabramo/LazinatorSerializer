using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazinatorFuzzTestWriter
{
    public class LazinatorClassType : LazinatorObjectType, ISupportedType
    {
        public override string DefinitionWord => "class";
        public bool IsAbstract { get; init; }
        public bool IsSealed { get; init; }
        public LazinatorClassType? InheritsFrom { get; init; }
        public override bool Inherits => InheritsFrom != null;
        public override bool CanInheritFrom => !IsSealed;

        public LazinatorClassType(int uniqueID, string name, bool isAbstract, bool isSealed, LazinatorClassType inheritsFrom, List<LazinatorObjectProperty> properties) : base(uniqueID, name, properties)
        {
            IsAbstract = isAbstract;
            IsSealed = isSealed;
            InheritsFrom = inheritsFrom;
        }

        public override string TypeDeclaration(bool nullable, bool nullableEnabledContext)
        {
            string declaration = Name;
            if (nullable && nullableEnabledContext)
                declaration += "?";
            if (!nullable && !nullableEnabledContext)
                throw new NotImplementedException();
            return declaration;
        }

        public override string ILazinatorDeclaration(bool nullableEnabledContext)
        {
            string inheritString = InheritsFrom == null ? "" : $" : I{InheritsFrom.Name}";
            return
$@"
using Lazinator.Attributes;
using Lazinator.Wrappers;

namespace FuzzTests
{{
    [Lazinator((int){UniqueID})]
    public interface I{Name}
    {{
        {PropertyDeclarations(nullableEnabledContext)}
    }}
}}
";
        }
        public override string ObjectDeclaration(bool nullableEnabledContext)
        {
            return
$@"
namespace FuzzTests
{{
    public partial class {Name} : I{Name}
    {{
    }}
}}
";
        }
    }
}
