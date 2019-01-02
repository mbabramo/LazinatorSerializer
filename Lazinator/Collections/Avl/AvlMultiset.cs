using System.Collections;
using System.Collections.Generic;
using Lazinator.Core;
using Lazinator.Support;
using Lazinator.Wrappers;

namespace Lazinator.Collections.Avl
{
    public partial class AvlMultiset<T> : IAvlMultiset<T>, IEnumerable<T> where T : ILazinator
    {
        public AvlMultiset(IComparer<T> comparer)
        {
            UnderlyingSet = new AvlSet<LazinatorTuple<T, WInt>>();
        }

        public AvlMultiset()
        {
            UnderlyingSet = new AvlSet<LazinatorTuple<T, WInt>>();
        }

        public int Count => UnderlyingSet.Count;

        public bool Contains(T key)
        {
            var result = UnderlyingSet.GetMatchOrNext(new LazinatorTuple<T, WInt>(key, 0));
            if (result.valueFound)
                return true;
            if (result.valueOrNextExists)
                return result.valueOrNext.Item1.Equals(key); // the result
            return false;
        }

        public bool Insert(T key)
        {
            return UnderlyingSet.Insert(new LazinatorTuple<T, WInt>(key, NumItemsAdded++));
        }

        public void RemoveFirstMatchIfExists(T key)
        {
            var result = UnderlyingSet.GetMatchOrNext(new LazinatorTuple<T, WInt>(key, 0));
            if (result.valueFound || (result.valueOrNextExists && result.valueOrNext.Item1.Equals(key)))
                UnderlyingSet.Remove(result.valueOrNext);
        }

        public IEnumerator GetEnumerator()
        {
            return AsKeyEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return AsKeyEnumerator();
        }

        private AvlMultisetEnumerator<T> AsKeyEnumerator()
        {
            AvlNodeKeyEnumerator<LazinatorTuple<T,WInt>> underlyingEnumerator = UnderlyingSet.AsKeyEnumerator();
            return new AvlMultisetEnumerator<T>(underlyingEnumerator);
        }

        public IEnumerable<T> AsEnumerable()
        {
            var iterator = AsKeyEnumerator();
            while (iterator.MoveNext())
                yield return iterator.Current;
        }

        public IEnumerable<T> Skip(int i)
        {
            var enumerator = AsKeyEnumerator();
            enumerator.Skip(i);
            while (enumerator.MoveNext())
                yield return enumerator.Current;
        }

    }
}
