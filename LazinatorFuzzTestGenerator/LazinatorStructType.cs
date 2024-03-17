using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazinatorFuzzTestGenerator
{
    public class LazinatorStructType : LazinatorObjectType, ISupportedType
    {
        public override int ObjectDepth => 1;
        public override string DefinitionWord => "struct";
        public override bool Inherits => false;
        public override bool Instantiable => true;

        public override bool Inheritable => false;

        public override bool UnannotatedIsNullable(bool nullableEnabledContext)
        {
            return false;
        }

        public LazinatorStructType(int uniqueID, string name, List<LazinatorObjectProperty> properties) : base(uniqueID, name, properties)
        { 
        }

        public override string UnannotatedTypeDeclaration() => Name;

        public override string ILazinatorDeclaration(string namespaceString, bool nullableEnabledContext)
        {
            return
$@"
using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace FuzzTests.{namespaceString}
{{
    [Lazinator((int){UniqueID})]
    public interface I{Name}
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
    public partial struct {Name} : I{Name}
    {{
    }}
}}
";
        }
    }
}
