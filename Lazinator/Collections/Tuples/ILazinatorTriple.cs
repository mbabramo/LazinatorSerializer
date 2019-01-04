using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Tuples
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorTriple)]
    interface ILazinatorTriple<T, U, V> : ILazinator where T : ILazinator where U : ILazinator where V : ILazinator
    {
        T Item1 { get; set; }
        U Item2 { get; set; }
        V Item3 { get; set; }
    }
}
