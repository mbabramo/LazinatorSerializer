using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Dictionary
{
    [UnofficiallyIncorporateInterface("Lazinator.Collections.Dictionary.ILazinatorDictionaryUnofficial", "internal")]
    [Lazinator((int)LazinatorCollectionUniqueIDs.Dictionary)]
    interface ILazinatorDictionary<TKey, TValue> where TKey : ILazinator where TValue : ILazinator
    {
        [SetterAccessibility("private")]
        int Count { get; }
    }
}
