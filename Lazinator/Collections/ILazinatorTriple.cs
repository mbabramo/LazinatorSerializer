﻿using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorTriple)]
    interface ILazinatorTriple<T, U, V> : ILazinator where T : ILazinator, new() where U : ILazinator, new() where V : ILazinator, new()
    {
        [Autoclone]
        T Item1 { get; set; }
        [Autoclone]
        U Item2 { get; set; }
        [Autoclone]
        V Item3 { get; set; }
    }
}
