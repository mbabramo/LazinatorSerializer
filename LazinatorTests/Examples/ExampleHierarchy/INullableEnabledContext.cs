#nullable enable

using Lazinator.Attributes;
using LazinatorCollections;
using LazinatorTests.Examples.Structs;
using System;
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
        Example[] NonNullableArray { get; set; }
        Example?[] NullableArray { get; set; }
        List<Example> NonNullableListOfNonNullables { get; set; }
        List<Example>? NullableListOfNonNullables { get; set; }
        List<Example?> NonNullableListOfNullables { get; set; }
        List<Example?>? NullableListOfNullables { get; set; }
        (Example, int) ValueTupleWithNonNullable { get; set; }
        (Example?, int) ValueTupleWithNullable { get; set; }
        Tuple<Example, int> RegularTupleWithNonNullable { get; set; }
        Tuple<Example?, int> RegularTupleWithNullable { get; set; }
        Dictionary<int, Example> DictionaryWithNonNullable { get; set; }
        Dictionary<int, Example?> DictionaryWithNullable { get; set; }
        LazinatorList<Example> LazinatorListNonNullable { get; set; }
        LazinatorList<Example?> LazinatorListNullable { get; set; }
        string? NullableString { get; set; }
        string NonNullableString { get; set; }
        ReadOnlySpan<byte> ByteReadOnlySpan { get; set; }
        Memory<byte> ByteMemory { get; set; }
        ReadOnlyMemory<byte> ByteReadOnlyMemory { get; set; }
        Memory<byte>? ByteMemoryNullable { get; set; }
        ReadOnlyMemory<byte>? ByteReadOnlyMemoryNullable { get; set; }
        Queue<Example> NonNullableQueue { get; set; }
        Queue<Example?> NullableQueue { get; set; }
        Stack<Example> NonNullableStack { get; set; }
        Stack<Example?> NullableStack { get; set; }
        int MyInt { get; set; }
        int? MyNullableInt { get; set; }
        ExampleStructWithoutClass MyStruct { get; set; }
    }
}