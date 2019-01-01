using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorKeyable)]
    public interface ILazinatorKeyable<TKey, TValue> where TKey : ILazinator where TValue : ILazinator
    {
        bool ContainsKey(TKey key);
        TValue GetValue(TKey key);
        void SetValue(TKey key, TValue value);
    }
}
