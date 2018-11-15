using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Dictionary
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.DictionaryUnofficial)]
    internal interface ILazinatorDictionaryUnofficial<TKey, TValue> where TKey : ILazinator, new() where TValue : ILazinator, new()
    {
        LazinatorList<DictionaryBucket<TKey, TValue>> Buckets { get; set; }
    }
}
