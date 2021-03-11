using System.Collections.Generic;
using Lazinator.Attributes;

namespace LazinatorTests.Examples.Collections
{
    [Lazinator((int)ExampleUniqueIDs.Derived_DotNetList_Nested_NonLazinator)]
    internal interface IDerived_DotNetList_Nested_NonLazinator : IDotNetList_Nested_NonLazinator
    {
        int MyLevel2Int { get; set; }
        List<List<NonLazinatorClass>> MyLevel2ListNestedNonLazinatorType { get; set; }
    }
}