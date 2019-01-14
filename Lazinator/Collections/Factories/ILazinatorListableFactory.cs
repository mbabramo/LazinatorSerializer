﻿using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace Lazinator.Collections.Factories
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorListableFactory)]
    public interface ILazinatorListableFactory<T> : ILazinator where T : ILazinator
    {
        ILazinatorListable<T> CreateListable();
    }
}
