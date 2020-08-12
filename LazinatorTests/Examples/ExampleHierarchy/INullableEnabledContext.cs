#nullable enable

using Lazinator.Attributes;
using System.Collections.Generic;

namespace LazinatorTests.Examples.ExampleHierarchy
{
    [Lazinator((int)ExampleUniqueIDs.INullableEnabledContext)]
    public interface INullableEnabledContext
    {
        Example? ExplicitlyNullable { get; set; }
        Example NonNullableClass { get; set; }
        IExample? ExplicitlyNullableInterface { get; set; }
        IExample NonNullableInterface {get; set;}
        List<Example> NonNullableListOfNonNullables { get; set; }
        List<Example>? NullableListOfNonNullables { get; set; }
        List<Example?> NonNullableListOfNullables { get; set; }
        List<Example?>? NullableListOfNullables { get; set; }
    }
}