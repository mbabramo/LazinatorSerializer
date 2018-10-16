using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections
{
    [Lazinator((int) LazinatorCollectionUniqueIDs.LazinatorTuple)]
    interface ILazinatorTuple<T, U> : ILazinator where T : ILazinator, new() where U : ILazinator, new()
    {
        T Item1 { get; set; }
        U Item2 { get; set; }
    }
}
