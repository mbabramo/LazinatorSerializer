using Lazinator.Attributes;

namespace LazinatorTests.Examples.Tuples
{
    [Lazinator((int)ExampleUniqueIDs.StructTuple)]
    public interface IStructTuple
    {
        (uint, ExampleChild, NonLazinatorClass) MyValueTupleSerialized { get; set; }
        (int, double)? MyNullableTuple { get; set; }
        (int MyFirstItem, double MySecondItem) MyNamedTuple { get; set; }
        (TestEnum firstEnum, TestEnum anotherEnum) EnumTuple { get; set; }
    }
}