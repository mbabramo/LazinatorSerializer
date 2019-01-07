using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorSortedMultivalueTree)]
    public interface ILazinatorSortedMultivalueTree<TKey, TValue> : ILazinatorSortedKeyValueTree<TKey, TValue>, ILazinatorKeyMultivalueTree<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
    }
}
