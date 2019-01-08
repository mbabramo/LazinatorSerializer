﻿using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IKeyMultivalueContainer)]
    public interface IKeyMultivalueContainer<TKey, TValue> where TKey : ILazinator where TValue : ILazinator
    {
        bool AllowDuplicates { get; set; }
        void AddValue(TKey key, TValue value, IComparer<TKey> comparer);
        IEnumerable<TValue> GetAllValues(TKey key, IComparer<TKey> comparer);
        bool RemoveAll(TKey key, IComparer<TKey> comparer);
        
        TValue GetValueForKey(TKey key, MultivalueLocationOptions whichOne, IComparer<TKey> comparer);
        bool SetValueForKey(TKey key, MultivalueLocationOptions whichOne, TValue value, IComparer<TKey> comparer);
        
        bool TryRemove(TKey key, MultivalueLocationOptions whichOne, IComparer<TKey> comparer);
        bool TryRemoveKeyValue(TKey key, TValue value, MultivalueLocationOptions whichOne, IComparer<TKey> comparer);
    }
}