using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Dictionary
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.Dictionary)]
    interface ILazinatorDictionary<TKey, TValue> where TKey : ILazinator where TValue : ILazinator
    {
        LazinatorList<DictionaryBucket<TKey, TValue>> Buckets { get; set; }
    }
}
