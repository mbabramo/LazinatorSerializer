#nullable enable

using Lazinator.Attributes;
using System.Collections.Generic;

namespace LazinatorTests.Examples
{
    [Lazinator((int)ExampleUniqueIDs.NullableEnabledContextWithParameterlessConstructor)]
    public interface INullableEnabledContextWithParameterlessConstructor
    {
        string MyString { get; set; }
        List<int> MyList { get; set; }
    }
}

#nullable restore