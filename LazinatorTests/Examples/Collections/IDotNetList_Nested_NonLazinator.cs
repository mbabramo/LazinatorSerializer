using System.Collections.Generic;
using Lazinator.Attributes;

namespace LazinatorTests.Examples.Collections
{
    [Lazinator((int)ExampleUniqueIDs.DotNetList_Nested_NonLazinator)]
    public interface IDotNetList_Nested_NonLazinator
    {
        List<List<NonLazinatorClass>> MyListNestedNonLazinatorType { get; set; }
    }
}