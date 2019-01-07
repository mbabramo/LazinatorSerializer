using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorSortedKeyMultivalueTree)]
    public interface ILazinatorSortedIndexableKeyMultivalueTree<TKey, TValue> : ILazinatorIndexableKeyValueTree<TKey, TValue>, ILazinatorSortedMultivalueTree<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
    }
}
