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

        public (bool isPastEnd, TKey valueIfNotPastEnd) GetMatchOrNext(TKey key)
        {
            var matchOrNext = SearchMatchOrNext(key);
            return (matchOrNext == null, (matchOrNext == null) ? default(TKey) : matchOrNext.Key);
        }

        public bool Insert(TKey key)
        {
            return Insert(key, 0);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
