using Lazinator.Attributes;
using Lazinator.Core;

namespace LazinatorCollections.Dictionary
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IDictionaryBucket)]
    interface IDictionaryBucket<TKey, TValue> where TKey : ILazinator where TValue : ILazinator
    {
        bool Initialized { get; set; }
        LazinatorList<TKey> Keys { get; set; }
        LazinatorList<TValue> Values { get; set; }
    }
}
