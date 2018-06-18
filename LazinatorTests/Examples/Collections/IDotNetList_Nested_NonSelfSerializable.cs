using System.Collections.Generic;
using Lazinator.Attributes;

namespace LazinatorTests.Examples.Collections
{
    [Lazinator((int)ExampleUniqueIDs.DotNetList_Nested_NonSelfSerializable)]
    public interface IDotNetList_Nested_NonSelfSerializable
    {
        List<List<NonLazinatorClass>> MyListNestedNonLazinatorType { get; set; }
    }
}