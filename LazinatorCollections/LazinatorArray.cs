using Lazinator.Core;
using System;
using System.Collections.Generic;

namespace LazinatorCollections
{
    public partial class LazinatorArray<T> : ILazinatorArray<T> where T : ILazinator
    {
        public ILazinatorListable<T> UnderlyingList { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public LazinatorArray(int numItems)
        {
            UnderlyingList = new LazinatorList<T>();
            for (int i = 0; i < numItems; i++)
                UnderlyingList.Add(default);
        }

        public LazinatorArray(ILazinatorListable<T> underlyingList, int numItems)
        {
            UnderlyingList = underlyingList;
            for (int i = 0; i < numItems; i++)
                UnderlyingList.Add(default);
        }

        public LazinatorArray(IEnumerable<T> items)
        {
            UnderlyingList = new LazinatorList<T>();
            foreach (T item in items)
                UnderlyingList.Add(item);
        }

        public int Length => UnderlyingList.Count;
        public long LongLength => UnderlyingList.LongCount;

        public T GetAtIndex(long index) => UnderlyingList.GetAtIndex(index);
        public void SetAtIndex(long index, T value) => UnderlyingList.SetAtIndex(index, value);

        public T this[int index]
        {
            get => GetAtIndex(index);
            set => SetAtIndex(index, value);
        }
    }
}
