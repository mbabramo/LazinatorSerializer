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

        public LazinatorStructType(int uniqueID, string name, List<LazinatorObjectProperty> properties) : base(uniqueID, name, properties)
        { 
        }

        public override string TypeDeclaration(bool nullable, bool nullableEnabledContext)
        {
            string declaration = Name;
            if (nullable && nullableEnabledContext)
                declaration += "?";
            return declaration;
        }

        public override string ILazinatorDeclaration(bool nullableEnabledContext)
        {
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
    public partial struct {Name} : I{Name}
    {{
    }}
}}
";
        }
    }
}
