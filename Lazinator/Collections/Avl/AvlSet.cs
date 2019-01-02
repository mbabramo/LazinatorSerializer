using System;
using System.Collections;
using System.Collections.Generic;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace Lazinator.Collections.Avl
{
    public partial class AvlSet<T> : IAvlSet<T>, IEnumerable<T> where T : ILazinator, IComparable<T>
    {
        public AvlSet() : base()
        {
            // DEBUG -- don't use this constructor
            UnderlyingTree = new AvlTree<T, Placeholder>();
        }

        public bool Contains(T value)
        {
            return UnderlyingTree.Search(value, out _);
        }

        public (bool valueFound, bool valueOrNextExists, T valueOrNext) GetMatchOrNext(T value)
        {
            var matchOrNext = UnderlyingTree.SearchMatchOrNext(value);
            if (matchOrNext.found)
                return (true, true, value);
            if (matchOrNext.node == null)
                return (false, false, default);
            return (false, true, matchOrNext.node.Key);
        }

        public bool Insert(T value)
        {
            (bool inserted, long location) = UnderlyingTree.Insert(value, default);
            if (inserted)
                Count++;
            return inserted;
        }

        public bool Remove(T value)
        {
            bool deleted = UnderlyingTree.Remove(value);
            if (deleted)
                Count--;
            return deleted;
        }

        public IEnumerator GetEnumerator()
        {
            return AsKeyEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return AsKeyEnumerator();
        }

        public AvlNodeKeyEnumerator<T> AsKeyEnumerator()
        {
            var underlyingEnumerator = UnderlyingTree.GetEnumerator() as AvlNodeEnumerator<T, Placeholder>;
            return new AvlNodeKeyEnumerator<T>(underlyingEnumerator);
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
