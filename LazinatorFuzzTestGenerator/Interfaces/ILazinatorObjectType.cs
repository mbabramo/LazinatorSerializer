using LazinatorFuzzTestGenerator.ObjectTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazinatorFuzzTestGenerator.Interfaces
{
    public interface ILazinatorObjectType : ISupportedType
    {
        int UniqueID { get; }
        abstract string DefinitionWord { get; }
        string Name { get; }
        List<LazinatorObjectProperty> Properties { get; }
        abstract bool Inherits { get; }
        abstract bool Inheritable { get; }
        abstract bool Instantiable { get; }
        abstract int ObjectDepth { get; }
    }
}
