﻿using Lazinator.Attributes;
using System;

namespace Lazinator.Collections.OffsetList
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorFastReadListInt16, -1, true)]
    internal interface ILazinatorFastReadListInt16 : ILazinatorFastReadList<Int16>
    {
    }
}