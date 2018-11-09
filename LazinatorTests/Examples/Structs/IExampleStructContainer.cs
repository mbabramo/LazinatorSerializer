using System.Collections.Generic;
using Lazinator.Attributes;
using Lazinator.Wrappers;

namespace LazinatorTests.Examples
{
    [Lazinator((int) ExampleUniqueIDs.ExampleStructContainer)]
    public interface IExampleStructContainer
    {
        ExampleStructContainingClasses MyExampleStructContainingClasses { get; set; }
        List<ExampleStructContainingClasses> MyListExampleStruct { get; set; }
        List<WNullableStruct<ExampleStructContainingClasses>> MyListNullableExampleStruct { get; set; }
        WInt IntWrapper { get; set; }
    }
}
