using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Dictionary
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.DictionaryBucket)]
    interface IDictionaryBucket<TKey, TValue> where TKey : ILazinator where TValue : ILazinator
    {
        bool Initialized { get; set; }
        LazinatorList<TKey> Keys { get; set; }
        LazinatorList<TValue> Values { get; set; }
    }
}
