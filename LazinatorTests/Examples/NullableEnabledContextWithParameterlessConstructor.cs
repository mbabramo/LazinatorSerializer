#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazinatorTests.Examples
{
    public partial class NullableEnabledContextWithParameterlessConstructor : INullableEnabledContextWithParameterlessConstructor
    {
        // We assume that the developer using the library creates a parameterless constructor and ignores the warning.
#pragma warning disable 8618
        public NullableEnabledContextWithParameterlessConstructor() 
        { 
        }
#pragma warning restore 8618
    }
}

#nullable restore