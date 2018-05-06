using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Lazinator.Collections.Avl;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace Lazinator.Collections.AvlTree
{
    public class AvlSet<TKey> : AvlTree<TKey, LazinatorWrapperByte>, IEnumerable<TKey> where TKey : ILazinator, new()
    {
        public AvlSet(IComparer<TKey> comparer) : base(comparer)
        {
        }

        public AvlSet() : base()
        {

        }

        public bool Contains(TKey key)
        {
            return Search(key, out _);
        }

        public (bool valueFound, TKey valueIfFound) GetMatchOrNext(TKey key)
        {
            var matchOrNext = SearchMatchOrNext(key);
            return (matchOrNext != null, (matchOrNext == null) ? default(TKey) : matchOrNext.Key);
        }

        public bool Insert(TKey key)
        {
            return Insert(key, 0);
        }

        IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator()
        {
            return AsKeyEnumerator();
        }

        private IEnumerator<TKey> AsKeyEnumerator()
        {
            var baseEnumerator = base.GetEnumerator() as AvlNodeEnumerator<TKey, LazinatorWrapperByte>;
            return new AvlNodeKeyEnumerator<TKey>(baseEnumerator);
        }

        public IEnumerable<TKey> AsEnumerable()
        {
            var iterator = AsKeyEnumerator();
            while (iterator.MoveNext())
                yield return iterator.Current;
        }
    }
}
