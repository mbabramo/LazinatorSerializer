using Lazinator.Attributes;
using Lazinator.Core;

namespace LazinatorTests.Examples.NonAbstractGenerics
{
    [Lazinator((int)ExampleUniqueIDs.ConstrainedGeneric)]
    public interface IConstrainedGeneric<T, U>
        where T : struct, ILazinator
        where U : ILazinator, new()
    {
        T MyT { get; set; }
        U MyU { get; set; }
    }
}