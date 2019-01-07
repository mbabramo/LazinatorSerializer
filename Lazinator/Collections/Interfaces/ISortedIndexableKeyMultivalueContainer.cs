using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorSortedKeyMultivalueTree)]
    public interface ISortedIndexableKeyMultivalueContainer<TKey, TValue> : IIndexableKeyValueContainer<TKey, TValue>, ISortedMultivalueContainer<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
    }
}
