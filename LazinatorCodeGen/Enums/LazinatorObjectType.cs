using System;
using System.Collections.Generic;
using System.Text;
using LazinatorCodeGen.Support;

namespace Lazinator.CodeDescription
{
    public enum LazinatorObjectType
    {
        [StringValue("class")]
        Class,
        [StringValue("struct")]
        Struct
    }
}
