using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Dictionary
{
    [UnofficiallyIncorporateInterface("Lazinator.Collections.Dictionary.ILazinatorDictionaryUnofficial`2", "internal")]
    [Lazinator((int)LazinatorCollectionUniqueIDs.IDictionary)]
    interface ILazinatorDictionary<TKey, TValue> where TKey : ILazinator where TValue : ILazinator
    {
        [SetterAccessibility("private")]
        int Count { get; }
    }
}
