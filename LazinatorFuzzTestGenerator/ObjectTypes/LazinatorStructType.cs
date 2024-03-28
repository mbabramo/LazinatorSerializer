using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LazinatorFuzzTestGenerator.Interfaces;

namespace LazinatorFuzzTestGenerator.ObjectTypes
{
    public class LazinatorStructType : LazinatorObjectType, ISupportedType
    {
        public override int ObjectDepth => 1;
        public override string DefinitionWord => "struct";
        public override bool Inherits => false;
        public override bool Instantiable => true;

        public override bool Inheritable => false;

        public override bool UnannotatedIsNullable()
        {
            return false;
        }

        public LazinatorStructType(int uniqueID, string name, List<LazinatorObjectProperty> properties, bool nullableContextEnabled) : base(uniqueID, name, properties)
        {
            NullableContextEnabled = nullableContextEnabled;
        }

        public override string UnannotatedTypeDeclaration() => Name;

        public override string ILazinatorDeclaration(string namespaceString)
        {
            return
$@"
using System;
using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace FuzzTests.{namespaceString}
{{
    [Lazinator((int){UniqueID})]
    public interface I{Name}
    {{
{PropertyDeclarations()}
    }}
}}
";
        }

        public override string GetObjectDeclaration_Top() => $"public partial struct {Name} : I{Name}";
        
    }
}
