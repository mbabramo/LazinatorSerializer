using Lazinator.Attributes;

namespace LazinatorTests.Examples.Tuples
{
    [Lazinator((int) ExampleUniqueIDs.LazinatorRecord)]
    public interface ILazinatorRecord
    {
        int MyInt { get; set; }
        string MyString { get; set; }
    }
}