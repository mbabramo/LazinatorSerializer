using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorSortedKeyMultivalueTree)]
    public interface ILazinatorSortedKeyMultivalueTree<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        TValue ValueAtKey(TKey key, MultivalueLocationOptions whichOne);
        (bool inserted, long index) Insert(TKey key, TValue value, MultivalueLocationOptions whichOne);
        bool Remove(TKey key, MultivalueLocationOptions whichOne);
    }
}
