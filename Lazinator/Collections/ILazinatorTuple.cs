using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections
{
    [Lazinator((int) LazinatorCollectionUniqueIDs.ILazinatorTuple)]
    interface ILazinatorTuple<T, U> : ILazinator where T : ILazinator where U : ILazinator
    {
        T Item1 { get; set; }
        U Item2 { get; set; }
    }
}
