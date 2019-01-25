using Lazinator.Attributes;
using Lazinator.Core;

namespace LazinatorCollections.Dictionary
{
    [UnofficiallyIncorporateInterface("LazinatorCollections.Dictionary.ILazinatorDictionaryUnofficial`2", "internal")]
    [Lazinator((int)LazinatorCollectionUniqueIDs.IDictionary)]
    interface ILazinatorDictionary<TKey, TValue> where TKey : ILazinator where TValue : ILazinator
    {
        [SetterAccessibility("private")]
        int Count { get; }
    }
}
