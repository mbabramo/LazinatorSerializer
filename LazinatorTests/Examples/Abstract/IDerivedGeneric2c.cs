using Lazinator.Attributes;
using Lazinator.Core;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)ExampleUniqueIDs.DerivedGeneric2c)]
    interface IDerivedGeneric2c<T> : IAbstractGeneric1<T> where T : ILazinator
    {
    }
}