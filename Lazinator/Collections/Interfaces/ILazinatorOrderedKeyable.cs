using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorOrderedKeyable)]
    public interface ILazinatorOrderedKeyable<TKey, TValue> : ILazinatorKeyable<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
    }
}
