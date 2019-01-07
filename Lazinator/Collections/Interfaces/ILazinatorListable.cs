﻿using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorListable)]
    public interface ILazinatorListable<T> : IList<T>, ILazinatorSplittable where T : ILazinator
    {
        void InsertAt(long index, T item);
        void RemoveAt(long index);
        IEnumerable<T> AsEnumerable(bool reverse = false, long skip = 0);
    }
}
