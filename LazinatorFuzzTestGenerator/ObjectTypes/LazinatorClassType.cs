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
            LazinatorClassType? c = this;

            var classes = new Stack<LazinatorClassType>(); 
            while (c != null)
            {
                classes.Push(c);
                c = c.InheritsFrom;
            }

            while (classes.Count > 0)
            {
                c = classes.Pop();
                if (c.Properties != null)
                {
                    foreach (var property in c.Properties)
                    {
                        if (!PropertiesIncludingInherited.Any(x => x.propertyName == property.propertyName))
                            PropertiesIncludingInherited.Add(property);
                    }
                }
            }
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
