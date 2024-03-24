using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LazinatorFuzzTestGenerator.Interfaces;

namespace LazinatorFuzzTestGenerator.ObjectTypes
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
            // the order of properties is defined by ExclusiveInterfaceDescription.
            // all lower-level properties are ordered by name (regardless of level), followed by the top level's properties (ordered by name).
            List<LazinatorObjectProperty> thisLevelProperties = Properties.OrderBy(x => x.propertyName).ToList();
            List<LazinatorObjectProperty> lowerLevelProperties = new List<LazinatorObjectProperty>();
            LazinatorClassType? c = this.InheritsFrom;
            while (c != null)
            {
                lowerLevelProperties.AddRange(c.Properties);
                c = c.InheritsFrom;
            }
            _PropertiesIncludingInherited = lowerLevelProperties.OrderBy(x => x.propertyName).ToList();
            _PropertiesIncludingInherited.AddRange(thisLevelProperties);
            _PropertiesIncludingInherited = _PropertiesIncludingInherited.OrderBy(x => x.propertyName).ToList(); // just make all of them alphabetical (consistent with ExclusiveInterfaceDescription)
        }


        private List<LazinatorObjectProperty> _PropertiesIncludingInherited = new List<LazinatorObjectProperty>();
        public override List<LazinatorObjectProperty> PropertiesIncludingInherited => _PropertiesIncludingInherited;

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
using System;
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


        public override string GetObjectDeclaration_Top(bool nullableEnabledContext) => $"public {(IsSealed ? "sealed " : "")}{(IsAbstract ? "abstract " : "")}partial class {Name} : {(InheritsFrom != null ? InheritsFrom.Name + "," : "")} I{Name}";

    }
}
