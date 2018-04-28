using System.Collections.Generic;
using Lazinator.Attributes;
using LazinatorTests.Examples;

namespace LazinatorTests.Examples.Collections
{
    [Lazinator((int)ExampleUniqueIDs.DotNetList_Nested_NonSelfSerializable)]
    public interface IDotNetList_Nested_NonSelfSerializable
    {
        List<List<NonLazinatorClass>> MyListNestedNonLazinatorType { get; set; }
    }
}