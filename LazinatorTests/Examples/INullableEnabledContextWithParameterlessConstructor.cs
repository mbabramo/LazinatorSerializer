#nullable enable

using Lazinator.Attributes;

namespace LazinatorTests.Examples
{
    [Lazinator((int)ExampleUniqueIDs.NullableEnabledContextWithParameterlessConstructor)]
    public interface INullableEnabledContextWithParameterlessConstructor
    {
        string MyString { get; set; }
    }
}

#nullable restore