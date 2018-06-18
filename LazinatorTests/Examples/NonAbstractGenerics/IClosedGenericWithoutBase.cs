using Lazinator.Attributes;

namespace LazinatorTests.Examples
{
    [Lazinator((int)ExampleUniqueIDs.ClosedGenericWithoutBase)]
    public interface IClosedGenericWithoutBase : IOpenGenericWithoutAttribute<int, ExampleChild>
    {
    }
}
