using System.Collections.Generic;
using Lazinator.Attributes;
using LazinatorTests.Examples;

namespace LazinatorTests.Examples.Collections
{
    [Lazinator((int)ExampleUniqueIDs.Derived_DotNetList_Nested_NonSelfSerializable)]
    internal interface IDerived_DotNetList_Nested_NonSelfSerializable
    {
        int MyLevel2Int { get; set; }
        List<List<NonLazinatorClass>> MyLevel2ListNestedNonLazinatorType { get; set; }
    }
}