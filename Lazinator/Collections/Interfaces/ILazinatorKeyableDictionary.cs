using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorKeyableDictionary)]
    public interface ILazinatorKeyableDictionary<TKey, TValue> : ILazinatorKeyable<TKey, TValue>, IDictionary<TKey, TValue> where TKey : ILazinator where TValue : ILazinator
    {
    }
}