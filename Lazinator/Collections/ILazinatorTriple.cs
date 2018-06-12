using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorTriple)]
    interface ILazinatorTriple<T, U, V> : ILazinator where T : ILazinator, new() where U : ILazinator, new() where V : ILazinator, new()
    {
        [AutoChangeParent]
        T Item1 { get; set; }
        [AutoChangeParent]
        U Item2 { get; set; }
        [AutoChangeParent]
        V Item3 { get; set; }
    }
}
