using CountedTree.Core;
using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CountedTree.Rebuild
{
    /// <summary>
    /// An interface for a producer of data to be used to rebuild a tree. 
    /// </summary>
    /// <typeparam name="TKey">The key type</typeparam>
    [NonexclusiveLazinator((int)CountedTreeLazinatorUniqueIDs.RebuildSource)]
    public interface IRebuildSource<TKey> : ILazinator where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {
        Task<WUInt32> GetNumberItems();
        Task<KeyAndID<TKey>?> GetFirstExclusive();
        Task<KeyAndID<TKey>?> GetLastInclusive();
        Task<List<KeyAndID<TKey>>> GetNextItems(int numValues);
        Task ReportComplete();
    }
}
