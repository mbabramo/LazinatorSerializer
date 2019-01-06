using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorKeyMultivalueTree)]
    public interface ILazinatorKeyMultivalueTree<TKey, TValue> where TKey : ILazinator where TValue : ILazinator
    {
        bool AllowDuplicates { get; set; }
        void AddValue(TKey key, TValue value);
        IEnumerable<TValue> GetAllValues(TKey key);
        bool RemoveAll(TKey item);
    }
}