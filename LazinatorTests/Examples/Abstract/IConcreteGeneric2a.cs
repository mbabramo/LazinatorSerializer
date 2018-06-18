using Lazinator.Attributes;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)ExampleUniqueIDs.ConcreteGeneric2a)]
    interface IConcreteGeneric2a : IAbstractGeneric1<int>
    {
        string AnotherProperty { get; set; }
        Example LazinatorExample { get; set; }
    }
}
