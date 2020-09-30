using System.Collections.Generic;
using Lazinator.Attributes;
using Lazinator.Wrappers;

namespace LazinatorTests.Examples
{
    [Lazinator((int) ExampleUniqueIDs.ExampleStructContainer)]
    public interface IExampleContainerStructContainingClasses
    {
        ExampleStructContainingClasses MyExampleStructContainingClasses { get; set; }
        HashSet<ExampleStructContainingClasses> MyHashSetExampleStruct { get; set; }
        List<ExampleStructContainingClasses> MyListExampleStruct { get; set; }
        List<WNullableStruct<ExampleStructContainingClasses>> MyListNullableExampleStruct { get; set; }
        List<ExampleStructContainingClasses?> MyListUnwrappedNullableExampleStruct { get; set; }
        WInt32 IntWrapper { get; set; }
    }
}
