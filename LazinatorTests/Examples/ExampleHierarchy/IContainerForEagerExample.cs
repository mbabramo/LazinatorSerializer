using Lazinator.Attributes;

namespace LazinatorTests.Examples
{
    [Lazinator((int)ExampleUniqueIDs.ContainerForEagerExample)]
    public interface IContainerForEagerExample
    {
        [Eager]
        Example EagerExample{ get; set; }
    }
}