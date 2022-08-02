#nullable enable

using Lazinator.Attributes;
using Lazinator.Collections;
using LazinatorTests.Examples.Structs;
using System;
using System.Collections.Generic;

namespace LazinatorTests.Examples.ExampleHierarchy
{
    [Lazinator((int)ExampleUniqueIDs.NullableEnabledContext)]
    public interface INullableEnabledContext
    {
        Example? ExplicitlyNullable { get; set; }
        Example NonNullableClass { get; set; }
        IExample? ExplicitlyNullableInterface { get; set; }
        IExample NonNullableInterface {get; set;}
        Example[]? NullableArrayOfNonNullables { get; set; }
        Example?[]? NullableArrayOfNullables { get; set; }
        Example[] NonNullableArrayOfNonNullables { get; set; }
        Example?[] NonNullableArrayOfNullables { get; set; }
        List<Example> NonNullableListOfNonNullables { get; set; }
        List<Example>? NullableListOfNonNullables { get; set; }
        List<Example?> NonNullableListOfNullables { get; set; }
        List<Example?>? NullableListOfNullables { get; set; }
        (Example, int) ValueTupleWithNonNullable { get; set; }
        (Example?, int) ValueTupleWithNullable { get; set; }
        (Example, int)? NullableValueTupleWithNonNullable { get; set; }
        (Example?, int)? NullableValueTupleWithNullable { get; set; }
        Tuple<Example, int> NonNullableRegularTupleWithNonNullable { get; set; }
        Tuple<Example?, int> NonNullableRegularTupleWithNullable { get; set; }
        Tuple<Example, int>? NullableRegularTupleWithNonNullable { get; set; }
        Tuple<Example?, int>? NullableRegularTupleWithNullable { get; set; }
        Dictionary<int, Example> NonNullableDictionaryWithNonNullable { get; set; }
        Dictionary<int, Example?> NonNullableDictionaryWithNullable { get; set; }
        Dictionary<int, Example>? NullableDictionaryWithNonNullable { get; set; }
        Dictionary<int, Example?>? NullableDictionaryWithNullable { get; set; }
        LazinatorList<Example> NonNullableLazinatorListNonNullable { get; set; }
        LazinatorList<Example?> NonNullableLazinatorListNullable { get; set; }
        LazinatorList<Example>? NullableLazinatorListNonNullable { get; set; }
        LazinatorList<Example?>? NullableLazinatorListNullable { get; set; }
        string? NullableString { get; set; }
        string NonNullableString { get; set; }
        ReadOnlySpan<byte> ByteReadOnlySpan { get; set; }
        Memory<byte> NonNullableMemoryOfBytes { get; set; }
        ReadOnlyMemory<byte> NonNullableReadOnlyMemoryOfBytes { get; set; }
        Memory<byte>? NullableMemoryOfBytes { get; set; }
        ReadOnlyMemory<byte>? NullableReadOnlyMemoryOfBytes { get; set; }
        Queue<Example> NonNullableQueueOfNonNullables { get; set; }
        Queue<Example?> NonNullableQueueOfNullables { get; set; }
        Queue<Example>? NullableQueueOfNonNullables { get; set; }
        Queue<Example?>? NullableQueueOfNullables { get; set; }
        Stack<Example> NonNullableStackOfNonNullables { get; set; }
        Stack<Example?> NonNullableStackOfNullables { get; set; }
        Stack<Example>? NullableStackOfNonNullables { get; set; }
        Stack<Example?>? NullableStackOfNullables { get; set; }
        int MyInt { get; set; }
        int? MyNullableInt { get; set; }
        ExampleStructWithoutClass NonNullableStruct { get; set; }
        ExampleStructWithoutClass? NullableStruct { get; set; }
        RecordLikeStruct NonNullableRecordLikeStruct { get; set; }
        RecordLikeStruct? NullableRecordLikeStruct { get; set; }
        RecordLikeClass NonNullableRecordLikeClass { get; set; }
        RecordLikeClass? NullableRecordLikeClass { get; set; }
    }
}