using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorOrderedKeyableDictionary)]
    public interface ILazinatorOrderedKeyableDictionary<TKey, TValue> : ILazinatorOrderedKeyable<TKey, TValue>, IDictionary<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
    }
}
