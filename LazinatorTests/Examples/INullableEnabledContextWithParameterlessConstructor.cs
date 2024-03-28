#nullable enable

using Lazinator.Attributes;
using System.Collections.Generic;

namespace LazinatorTests.Examples
{
    [Lazinator((int)ExampleUniqueIDs.NullableContextEnabledWithParameterlessConstructor)]
    [NullableContextSetting(true)]
    public interface INullableContextEnabledWithParameterlessConstructor
    {
        string MyString { get; set; }
        List<int> MyList { get; set; }
    }
}

#nullable restore