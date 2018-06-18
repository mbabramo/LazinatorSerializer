using Lazinator.Attributes;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)ExampleUniqueIDs.ConcreteGenericContainer)]
    interface IConcreteGenericContainer : IAbstractGenericContainer<int>
    {
    }
}
