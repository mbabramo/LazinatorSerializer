using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Support;
using LazinatorCodeGen.Support;

namespace Lazinator.CodeGeneration
{
    public enum LazinatorObjectType
    {
        [StringValue("class")]
        Class,
        [StringValue("struct")]
        Struct
    }
}
