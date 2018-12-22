using Lazinator.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lazinator.Collections.BigList
{
    public partial class BigListTree<T> : LazinatorGeneralTree<BigListContainer<T>>, IList<T>, IBigListTree<T> where T : ILazinator
    {
        #region Constructor and initialization

        public BigListTree() : base()
        {
        }

        public BigListTree(BigListContainer<T> container)
        {
            Item = container;
            Item.CorrespondingTree = this;
        }

        public override LazinatorGeneralTree<BigListContainer<T>> CreateTree(BigListContainer<T> item)
        {
            return new BigListTree<T>(item);
        }

        protected internal void InitializeWithChildLeaf()
        {
            Children = new LazinatorList<LazinatorGeneralTree<BigListContainer<T>>>();
            BigListLeafContainer<T> leaf = new BigListLeafContainer<T>();
            BigListTree<T> treeForLeaf = new BigListTree<T>(leaf);
            leaf.CorrespondingTree = treeForLeaf;
            base.Children.Add(treeForLeaf);
        }

        #endregion

        #region Container access

        public BigListContainer<T> BigListContainer
        {
            get
            {
                var item = (BigListContainer<T>)Item;
                if (item == null)
                    return item;
                item.CorrespondingTree = this;
                return item;
            }
        }

        public IEnumerable<BigListTree<T>> BigListChildTrees => Children.Select(x => (BigListTree<T>)x);

        public IEnumerable<BigListContainer<T>> BigListChildContainers => BigListChildTrees.Select(x => x.BigListContainer);

        public BigListTree<T> BigListChildTree(int childIndex) => BigListChildTrees.Skip(childIndex).FirstOrDefault();

        public BigListContainer<T> BigListChildContainer(int childIndex) => BigListChildContainers.Skip(childIndex).FirstOrDefault();

        public BigListTree<T> BigListParentTree => (BigListTree<T>)ParentTree;

        public BigListContainer<T> BigListParentNode => BigListParentTree.BigListContainer;

        #endregion

        #region Tree modification

        protected internal void InsertChildContainer(BigListContainer<T> childContainer, int childIndex)
        {
            BigListInteriorContainer<T> containerToInsertUnder = (BigListInteriorContainer<T>)BigListContainer;
            containerToInsertUnder.ChildContainerCounts.Insert(childIndex, childContainer.Count);
            containerToInsertUnder.Count += childContainer.Count;
            InsertChild(childContainer, childIndex);
            BigListChildContainer(childIndex).CorrespondingTree = BigListChildTree(childIndex);
        }

        protected internal BigListInteriorContainer<T> DemoteLeafContainer(bool separateItemsIntoSeparateLeaves)
        {
            BigListLeafContainer<T> originalContainer = (BigListLeafContainer<T>) BigListContainer;
            BigListInteriorContainer<T> interiorContainer = new BigListInteriorContainer<T>(originalContainer.BranchingFactor, this);
            Item = interiorContainer;
            if (!separateItemsIntoSeparateLeaves)
            {
                InsertChildContainer(originalContainer, 0);
            }
            else
            {
                int childIndex = 0;
                foreach (T t in originalContainer.Items)
                {
                    BigListLeafContainer<T> newLeafNode = new BigListLeafContainer<T>(originalContainer.BranchingFactor, null);
                    newLeafNode.Items.Add(t); // only item for this container for now
                    InsertChildContainer(newLeafNode, childIndex++);
                }
            }
            return interiorContainer;
        }

        #endregion

        #region Methods with long indices

        public long LongCount => (long)(BigListContainer?.Count ?? 0);

        public void Insert(long index, T item)
        {
            BigListContainer.Insert(index, item);
        }

        public void RemoveAt(long index)
        {
            BigListContainer.RemoveAt(index);
        }

        public T Get(long index)
        {
            return BigListContainer.Get(index);
        }

        public void Set(long index, T value)
        {
            BigListContainer.Set(index, value);
        }

        public long LongIndexOf(T item)
        {
            long i = 0;
            foreach (T existingItem in this)
            {
                if (item == null)
                {
                    if (existingItem == null)
                        return i;
                }
                else
                {
                    if (item.Equals(existingItem))
                        return i;
                }
                i++;
            }
            return -1;
        }

        #endregion

        #region Implement IList

        public int Count
        {
            get
            {
                long longCount = LongCount;
                if (longCount > int.MaxValue)
                    throw new Exception($"More than {int.MaxValue} items in the list. Use LongCount instead.");
                return (int) longCount;
            }
        }

        public bool IsReadOnly => false;

        public T this[int index]
        {
            get => Get(index);
            set => Set(index, value);
        }

        public int IndexOf(T item)
        {
            long longIndex = LongIndexOf(item);
            if (longIndex > int.MaxValue)
                throw new Exception($"More than {int.MaxValue} items in the list. Use LongIndexOf instead.");
            return (int)longIndex;
        }

        public void Insert(int index, T item)
        {
            Insert((long)index, item);
        }

        public void RemoveAt(int index)
        {
            RemoveAt((long)index);
        }

        public void Add(T item)
        {
            Insert(LongCount, item);
        }

        public void Clear()
        {
            InitializeWithChildLeaf();
        }

        public bool Contains(T item)
        {
            return LongIndexOf(item) != -1;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException();
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException();
            if (Count > array.Length - arrayIndex + 1)
                throw new ArgumentException();
            foreach (var item in this)
            {
                array[arrayIndex++] = item;
            }
        }

        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index == -1)
                return false;
            RemoveAt(index);
            return true;
        }

        private IEnumerable<BigListTree<T>> EnumerateTreeNodes => TraverseTree().Select(x => (BigListTree<T>)x);

        private IEnumerable<BigListLeafContainer<T>> EnumerateLeafNodes => EnumerateTreeNodes.Where(x => x.Item is BigListLeafContainer<T>).Select(x => (BigListLeafContainer<T>)x.Item);

        private IEnumerable<T> EnumerateItems => EnumerateLeafNodes.SelectMany(x => x.Items);

        public IEnumerator<T> GetEnumerator()
        {
            return (IEnumerator<T>)EnumerateItems;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)EnumerateItems;
        }

        #endregion
    }
}
