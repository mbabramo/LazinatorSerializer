using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Dictionary
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.DictionaryBucket)]
    interface IDictionaryBucket<TKey, TValue> where TKey : ILazinator, new() where TValue : ILazinator, new()
    {
        LazinatorList<TKey> Keys { get; set; }
        LazinatorList<TValue> Values { get; set; }
    }
}
