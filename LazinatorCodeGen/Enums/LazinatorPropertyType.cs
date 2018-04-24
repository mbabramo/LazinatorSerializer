using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.CodeGeneration
{
    public enum LazinatorPropertyType : byte
    {
        PrimitiveType,
        PrimitiveTypeNullable,
        LazinatorClassOrInterface,
        LazinatorStruct,
        NonSelfSerializingType,
        SupportedCollection,
        SupportedTuple,
        OpenGenericParameter
    }
}
