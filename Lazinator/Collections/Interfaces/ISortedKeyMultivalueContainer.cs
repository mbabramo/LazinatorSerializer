﻿using Lazinator.Core;
using Lazinator.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ISortedKeyMultivalueContainer]
    public interface ISortedKeyMultivalueContainer<TKey, TValue> : ISortedKeyValueContainer<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        IEnumerable<TValue> GetAllValues(TKey key);
        bool RemoveAll(TKey item);

        TValue ValueForKey(TKey key, MultivalueLocationOptions whichOne);
        bool SetValueForKey(TKey key, TValue value, MultivalueLocationOptions whichOne);
        bool TryRemove(TKey key, MultivalueLocationOptions whichOne);
        bool TryRemoveKeyValue(TKey key, TValue value, MultivalueLocationOptions whichOne);
    }
}
