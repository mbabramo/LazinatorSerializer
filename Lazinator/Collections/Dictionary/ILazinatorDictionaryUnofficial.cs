﻿using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Dictionary
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.DictionaryUnofficial)]
    internal interface ILazinatorDictionaryUnofficial<TKey, TValue> where TKey : ILazinator where TValue : ILazinator
    {
        LazinatorList<DictionaryBucket<TKey, TValue>> Buckets { get; set; }
    }
}
