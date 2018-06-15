using Lazinator.Attributes;
using Lazinator.Wrappers;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)ExampleUniqueIDs.ConcreteFromGenericFromBase)]
    internal interface IConcreteFromGenericFromBase : IGenericFromBase<WNullableDecimal>
    {
    }
}