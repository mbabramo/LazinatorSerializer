using Lazinator.Attributes;

namespace LazinatorTests.AnotherNamespace
{
    [Lazinator((int) ExampleUniqueIDs.StructInAnotherNamespace)]
    public interface IStructInAnotherNamespace
    {
        int MyInt { get; set; }
    }
}