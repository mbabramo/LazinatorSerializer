using Lazinator.Attributes;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)ExampleUniqueIDs.ConcreteFromBase)]
    internal interface IConcreteFromBase
    {
        int IntInConcreteFromBase { get; set; }
    }
}