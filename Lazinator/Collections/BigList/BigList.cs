using Lazinator.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.BigList
{
    public partial class BigList<T> : IBigList<T>, IList<T> where T : ILazinator
    {
        public BigList(int branchingFactor) : this(branchingFactor, false)
        {
        }

        public BigList(int branchingFactor, bool isAppendOnly)
        {
            UnderlyingTree = new BigListTree<T>(new BigListLeafContainer<T>(branchingFactor, null));
            IsAppendOnly = isAppendOnly;
        }

        public T this[int index] { get => ((IList<T>)UnderlyingTree)[index]; set => ((IList<T>)UnderlyingTree)[index] = value; }

        public int Count => ((IList<T>)UnderlyingTree).Count;

        public bool IsReadOnly => ((IList<T>)UnderlyingTree).IsReadOnly;

        public int BranchingFactor => UnderlyingTree.BranchingFactor;

        public void Add(T item)
        {
            ((IList<T>)UnderlyingTree).Add(item);
        }

        public void Clear()
        {
            if (IsAppendOnly)
                throw new BigListUnsupportedOperationWithAppendOnlyException();
            ((IList<T>)UnderlyingTree).Clear();
        }

        public bool Contains(T item)
        {
            return ((IList<T>)UnderlyingTree).Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            ((IList<T>)UnderlyingTree).CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IList<T>)UnderlyingTree).GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return ((IList<T>)UnderlyingTree).IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            if (IsAppendOnly & index != Count)
                throw new BigListUnsupportedOperationWithAppendOnlyException();
            if (IsAppendOnly && IsPowerOfBranchingFactorGreaterThan1())
            {
                UnderlyingTree.DemoteInteriorContainer();
            }
            ((IList<T>)UnderlyingTree).Insert(index, item);
        }

        private bool IsPowerOfBranchingFactorGreaterThan1()
        {
            long n = Count;
            int branchingFactor = BranchingFactor;
            if (n <= branchingFactor)
                return false;
            while (n % branchingFactor == 0)
            {
                n /= branchingFactor;
            }
            return n == 1;
        }

        public bool Remove(T item)
        {
            if (IsAppendOnly)
                throw new BigListUnsupportedOperationWithAppendOnlyException();
            return ((IList<T>)UnderlyingTree).Remove(item);
        }

        public void RemoveAt(int index)
        {
            if (IsAppendOnly)
                throw new BigListUnsupportedOperationWithAppendOnlyException();
            ((IList<T>)UnderlyingTree).RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<T>)UnderlyingTree).GetEnumerator();
        }
    }
}
