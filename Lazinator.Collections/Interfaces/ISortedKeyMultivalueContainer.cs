﻿using Lazinator.Core;
using Lazinator.Attributes;
using System;
using System.Collections.Generic;

namespace Lazinator.Collections.Interfaces
{
    /// <summary>
    /// A nonexclusive Lazinator interface for sorted containers of key-multivalue pairs
    /// </summary>
    /// <typeparam name="TKey">The key type</typeparam>
    /// <typeparam name="TValue">The value type</typeparam>
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ISortedKeyMultivalueContainer)]
    public interface ISortedKeyMultivalueContainer<TKey, TValue> : ISortedKeyValueContainer<TKey, TValue>, IKeyMultivalueContainer<TKey, TValue>, ILazinator where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        IEnumerable<TValue> GetAllValues(TKey key);
        bool TryRemoveAll(TKey key);

        TValue GetValueForKey(TKey key, MultivalueLocationOptions whichOne);
        bool SetValueForKey(TKey key, TValue value, MultivalueLocationOptions whichOne);
        bool TryRemove(TKey key, MultivalueLocationOptions whichOne);
        void AddValueForKey(TKey key, TValue value);
    }
}
