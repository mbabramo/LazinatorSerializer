using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Lazinator.Core;
using Lazinator.Support;
using Lazinator.Wrappers;

namespace Lazinator.Collections.Avl
{
    public partial class AvlMultiset<T> : IAvlMultiset<T>, IEnumerable<T> where T : ILazinator, new()
    {
        public AvlMultiset(IComparer<T> comparer)
        {
            // Given comparer, we must define a custom comparer for tuples containing T and an int that indicates the item number added.
            var customComparer = GetComparerForWrapper(comparer);

            UnderlyingSet = new AvlSet<LazinatorTuple<T, WInt>>(customComparer);
        }

        public void SetComparer(IComparer<T> comparer)
        {
            UnderlyingSet.UnderlyingTree.SetComparer(GetComparerForWrapper(comparer));
        }

        private static CustomComparer<LazinatorTuple<T, WInt>> GetComparerForWrapper(IComparer<T> comparer)
        {
            var c2 = new CustomComparer<LazinatorTuple<T, WInt>>((p0, p1) =>
            {
                var r = comparer.Compare(p0.Item1, p1.Item1);
                if (r == 0)
                {
                    return p0.Item2.CompareTo(p1.Item2);
                }
                else return r;
            });
            return c2;
        }

        public AvlMultiset()
        {
            UnderlyingSet = new AvlSet<LazinatorTuple<T, WInt>>();
        }

        public int Count => UnderlyingSet.Count;

        public bool Contains(T key)
        {
            var result = UnderlyingSet.GetMatchOrNext(new LazinatorTuple<T, WInt>(key, 0));
            if (!result.valueFound)
                return false;
            return result.valueIfFound.Item1.Equals(key);
        }

        public (bool valueFound, T valueIfFound) GetMatchOrNext(T key)
        {
            var matchOrNext = UnderlyingSet.GetMatchOrNext(new LazinatorTuple<T, WInt>(key, 0));
            return (matchOrNext.valueFound, matchOrNext.valueFound ? matchOrNext.valueIfFound.Item1 : default(T));
        }

        public bool Insert(T key)
        {
            return UnderlyingSet.Insert(new LazinatorTuple<T, WInt>(key, NumItemsAdded++));
        }

        public void RemoveFirstMatchIfExists(T key)
        {
            var matchOrNext = UnderlyingSet.GetMatchOrNext(new LazinatorTuple<T, WInt>(key, 0));
            if (matchOrNext.valueFound)
                UnderlyingSet.Delete(matchOrNext.valueIfFound);
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
