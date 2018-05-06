using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace Lazinator.Collections.AvlTree
{
    public partial class AvlMultiset<T> : IAvlMultiset<T> where T : ILazinator, new()
    {
        public AvlMultiset()
        {
            UnderlyingSet = new AvlSet<LazinatorTuple<T, LazinatorWrapperInt>>();
        }

        public bool Contains(T key)
        {
            var result = UnderlyingSet.GetMatchOrNext(new LazinatorTuple<T, LazinatorWrapperInt>(key, 0));
            if (!result.valueFound)
                return false;
            return result.valueIfFound.Item1.Equals(key);
        }

        public (bool valueFound, T valueIfFound) GetMatchOrNext(T key)
        {
            var matchOrNext = UnderlyingSet.GetMatchOrNext(new LazinatorTuple<T, LazinatorWrapperInt>(key, 0));
            return (matchOrNext.valueFound, matchOrNext.valueFound ? matchOrNext.valueIfFound.Item1 : default(T));
        }

        public bool Insert(T key)
        {
            return UnderlyingSet.Insert(new LazinatorTuple<T, LazinatorWrapperInt>(key, NumItemsAdded++));
        }

        public void RemoveFirstMatchIfExists(T key)
        {
            var matchOrNext = UnderlyingSet.GetMatchOrNext(new LazinatorTuple<T, LazinatorWrapperInt>(key, 0));
            if (matchOrNext.valueFound)
                UnderlyingSet.Delete(matchOrNext.valueIfFound);
        }


    }
}
