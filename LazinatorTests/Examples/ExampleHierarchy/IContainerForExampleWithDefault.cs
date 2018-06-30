using Lazinator.Attributes;

namespace LazinatorTests.Examples
{
    [Lazinator((int)ExampleUniqueIDs.ContainerForExampleWithDefault)]
    public interface IContainerForExampleWithDefault
    {
        Example Example { get; set; }
    }
}