using Lazinator.Core;
using System;
using System.Threading.Tasks;

namespace CountedTree.Updating
{
    public interface ITreeHistoryManagerAccess
    {
        Task<ITreeHistoryManager<TKey>> Get<TKey>(Guid key) where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>;
        Task Set<TKey>(Guid key, ITreeHistoryManager<TKey> item) where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>;
    }
}
