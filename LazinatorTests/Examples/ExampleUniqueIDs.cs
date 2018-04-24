﻿namespace LazinatorTests.Examples
{
    // IMPORTANT: Even if a self-serializing class is deleted and is no longer represented in an data, it must keep its place in enums like this, because otherwise all other IDs will change. But you can rename it so that it's clear that it refers to a class that doesn't exist anymore.

    public enum ExampleUniqueIDs
    {
        ArrayMultidimensionalAndJagged = 100,
        Array_Values,
        ClosedGeneric,
        DerivedLazinatorList,
        Dictionary_Values_SelfSerialized,
        DotNetHash_SelfSerialized,
        DotNetList_Nested_NonSelfSerializable,
        DotNetList_NonSelfSerializable,
        DotNetList_Serialized,
        DotNetList_Values,
        DotNetQueue_Values,
        DotNetStack_Values,
        Example,
        ExampleChild,
        ExampleChildInherited,
        ExampleNonexclusiveInterfaceImplementer,
        ExampleStruct,
        ExampleStructContainer,
        ExampleStructContainingStruct,
        IExampleInterface,
        KeyValuePair,
        LazinatorListContainer,
        ListContainer,
        ListContainerGeneric,
        NestedTuple,
        NonLazinatorContainer,
        RecordLikeType,
        RegularTuple,
        SpanAndMemory,
        StructTuple,
        UnofficialInterface,
        UnofficialInterfaceIncorporator,
    }
}
