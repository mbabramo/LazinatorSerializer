using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections
{
    [Lazinator((int) LazinatorCollectionUniqueIDs.LazinatorTuple)]
    interface ILazinatorTuple<T, U> : ILazinator where T : ILazinator, new() where U : ILazinator, new()
    {
        [Autoclone]
        T Item1 { get; set; }
        [Autoclone]
        U Item2 { get; set; }
    }
}
