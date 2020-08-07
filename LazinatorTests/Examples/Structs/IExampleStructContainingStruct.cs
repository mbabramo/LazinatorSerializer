using Lazinator.Attributes;

namespace LazinatorTests.Examples
{
    [Lazinator((int) ExampleUniqueIDs.ExampleStructContainingStruct)]
    public interface IExampleStructContainingStruct
    {
        ExampleStructContainingClasses MyExampleStructContainingClasses { get; set; }
        ExampleStructContainingClasses? MyExampleNullableStruct { get; set; }
    }
}
