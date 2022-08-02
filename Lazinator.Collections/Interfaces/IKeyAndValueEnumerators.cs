using System.Collections.Generic;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    /// <summary>
    /// A nonexclusive interface that makes it possible to obtain keys, values, or key-value pairs as enumerables or enumerators.
    /// </summary>
    /// <typeparam name="TKey">The key type</typeparam>
    /// <typeparam name="TValue">The value type</typeparam>
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IKeyAndValueEnumerators)]
    public interface IKeyAndValueEnumerators<TKey, TValue> where TKey : ILazinator where TValue : ILazinator
    {
        IEnumerable<TKey> KeysAsEnumerable(bool reverse = false, long skip = 0);
        IEnumerable<TValue> ValuesAsEnumerable(bool reverse = false, long skip = 0);
        IEnumerable<KeyValuePair<TKey, TValue>> KeyValuePairsAsEnumerable(bool reverse = false, long skip = 0);
        IEnumerable<TKey> KeysAsEnumerable(bool reverse, TKey startKey, IComparer<TKey> comparer);
        IEnumerable<TValue> ValuesAsEnumerable(bool reverse, TKey startKey, IComparer<TKey> comparer);
        IEnumerable<KeyValuePair<TKey, TValue>> KeyValuePairsAsEnumerable(bool reverse, TKey startKey, IComparer<TKey> comparer);
        IEnumerator<TKey> GetKeyEnumerator(bool reverse = false, long skip = 0);
        IEnumerator<TValue> GetValueEnumerator(bool reverse = false, long skip = 0);
        IEnumerator<KeyValuePair<TKey, TValue>> GetKeyValuePairEnumerator(bool reverse = false, long skip = 0);
        IEnumerator<TKey> GetKeyEnumerator(bool reverse, TKey startKey, IComparer<TKey> comparer);
        IEnumerator<TValue> GetValueEnumerator(bool reverse, TKey startKey, IComparer<TKey> comparer);
        IEnumerator<KeyValuePair<TKey, TValue>> GetKeyValuePairEnumerator(bool reverse, TKey startKey, IComparer<TKey> comparer);
    }
}
