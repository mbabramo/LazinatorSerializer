using Lazinator.Attributes;
using Lazinator.Core;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)ExampleUniqueIDs.UnformedGeneric2c)]
    interface IUnformedGeneric2c<T> : IAbstractGeneric1<T> where T : ILazinator
    {
    }
}