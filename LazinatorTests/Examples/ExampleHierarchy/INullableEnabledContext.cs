#nullable enable

using Lazinator.Attributes;

namespace LazinatorTests.Examples.ExampleHierarchy
{
    [Lazinator((int)ExampleUniqueIDs.INullableEnabledContext)]
    public interface INullableEnabledContext
    {
        Example? ExplicitlyNullable { get; set; }
        Example NonNullableClass { get; set; }
        IExample? ExplicitlyNullableInterface { get; set; }
        IExample NonNullableInterface {get; set;}
    }
}