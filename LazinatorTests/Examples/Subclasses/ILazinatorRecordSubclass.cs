using Lazinator.Attributes;

namespace LazinatorTests.Examples.Subclasses
{
    [Lazinator((int)ExampleUniqueIDs.LazinatorRecordSubclass)]
    public interface ILazinatorRecordSubclass : ILazinatorRecord
    {
        int MySubclassInt { get; set; }
    }
}