using Lazinator.Attributes;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)ExampleUniqueIDs.ContainerWithAbstract1)]
    interface IContainerWithAbstract1 
    {
        Abstract1 AbstractProperty { get; set; }
    }
}
