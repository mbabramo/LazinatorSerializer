using Lazinator.Attributes;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)ExampleUniqueIDs.ConcreteGeneric2b)]
    interface IConcreteGeneric2b : IAbstractGeneric1<Example>
    {
        string AnotherProperty { get; set; }
        Example LazinatorExample { get; set; }
    }
}