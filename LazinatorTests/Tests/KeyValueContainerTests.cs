using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Lazinator.Collections;
using LazinatorTests.Examples;
using Lazinator.Core;
using Xunit;
using Lazinator.Wrappers;
using LazinatorTests.Examples.Hierarchy;
using LazinatorTests.Examples.Structs;
using LazinatorTests.Examples.NonAbstractGenerics;
using Lazinator.Collections.Tuples;
using Lazinator.Collections.Avl;
using Lazinator.Collections.Interfaces;

namespace LazinatorTests.Tests
{
    public enum KeyValueContainerType
    {
        AvlKeyValueTree,
        AvlIndexableKeyValueTree,
        AvlSortedKeyValueTree,
        AvlSortedIndexableKeyValueTree
    }

    public class KeyValueContainerTests_WInt : KeyValueContainerTests<WInt, WInt>
    {
        [Fact]
        public void VerifyKeyValueContainerDEBUG()
        {
            VerifyKeyValueContainer(KeyValueContainerType.AvlIndexableKeyValueTree, true, 100, 100);
        }

        [Theory]
        [InlineData(KeyValueContainerType.AvlKeyValueTree, false, 100, 100)]
        [InlineData(KeyValueContainerType.AvlIndexableKeyValueTree, false, 100, 100)]
        [InlineData(KeyValueContainerType.AvlSortedKeyValueTree, false, 100, 100)]
        [InlineData(KeyValueContainerType.AvlSortedIndexableKeyValueTree, false, 100, 100)]
        [InlineData(KeyValueContainerType.AvlKeyValueTree, true, 100, 100)]
        [InlineData(KeyValueContainerType.AvlIndexableKeyValueTree, true, 100, 100)]
        [InlineData(KeyValueContainerType.AvlSortedKeyValueTree, true, 100, 100)]
        [InlineData(KeyValueContainerType.AvlSortedIndexableKeyValueTree, true, 100, 100)]
        public void VerifyKeyValueContainer(KeyValueContainerType containerType, bool allowDuplicates, int numRepetitions, int numInstructions) => VerifyKeyValueContainerHelper(containerType, allowDuplicates, numRepetitions, numInstructions);

        public override WInt GetRandomValue()
        {
            return ran.Next(100);
        }

    }

    public abstract class KeyValueContainerTests<TKey, TValue> : SerializationDeserializationTestBase where TKey : ILazinator, IComparable<TKey>, IComparable where TValue : ILazinator
    {

        public IKeyValueContainer<TKey, TValue> GetKeyValueContainer(KeyValueContainerType containerType)
        {
            switch (containerType)
            {
                case KeyValueContainerType.AvlKeyValueTree:
                    return new AvlKeyValueTree<TKey, TValue>();
                case KeyValueContainerType.AvlIndexableKeyValueTree:
                    return new AvlIndexableKeyValueTree<TKey, TValue>();
                case KeyValueContainerType.AvlSortedKeyValueTree:
                    return new AvlSortedKeyValueTree<TKey, TValue>();
                case KeyValueContainerType.AvlSortedIndexableKeyValueTree:
                    return new AvlSortedIndexableKeyValueTree<TKey, TValue>();
                default:
                    throw new NotSupportedException();
            }
        }

        public Random ran = new Random(0);
        bool AllowDuplicates;


        public void VerifyKeyValueContainerHelper(KeyValueContainerType containerType, bool allowDuplicates, int numRepetitions, int numInstructions)
        {
            AllowDuplicates = allowDuplicates;
            for (int rep = 0; rep < numRepetitions; rep++)
            {
                List<LazinatorComparableKeyValue<TKey,TValue>> list = new List<LazinatorComparableKeyValue<TKey,TValue>>();
                IKeyValueContainer<TKey, TValue> container = GetKeyValueContainer(containerType);
                container.AllowDuplicates = AllowDuplicates;
                for (int i = 0; i < numInstructions; i++)
                {
                    int r = ran.Next(100);
                    RandomInstruction instruction;
                    if (r < 25)
                        instruction = new GetValueInstruction();
                    else
                        if (r < 75)
                        instruction = new InsertValueInstruction();
                    else
                        instruction = new RemoveInstruction();
                    instruction.Execute(this, container, list);
                }
                VerifyEntireList(container, list);
                VerifyEnumerableSkipAndReverse(container, list);
            }
        }

        public void VerifyEntireList(IKeyValueContainer<TKey, TValue> valueKeyValueContainer, List<LazinatorComparableKeyValue<TKey,TValue>> list)
        {
            var values = valueKeyValueContainer.AsEnumerable().ToList();
            values.SequenceEqual(list).Should().BeTrue();
        }

        public void VerifyEnumerableSkipAndReverse(IKeyValueContainer<TKey, TValue> valueKeyValueContainer, List<LazinatorComparableKeyValue<TKey,TValue>> list)
        {
            var list2 = list.ToList();
            list2.Reverse();
            int numToSkip = ran.Next(0, list.Count + 1);
            var withSkips = list2.Skip(numToSkip).ToList();
            var values = valueKeyValueContainer.AsEnumerable(true, numToSkip).ToList();
            values.SequenceEqual(withSkips).Should().BeTrue();
        }

        public (T item, int firstIndex, int lastIndex)? GetRandomItem(List<LazinatorComparableKeyValue<TKey,TValue>> list)
        {
            if (!list.Any())
                return null;
            int index = ran.Next(0, list.Count());
            T item = list[index];
            int firstIndex, lastIndex;
            GetIndexRange(list, index, out firstIndex, out lastIndex);
            return (item, firstIndex, lastIndex);
        }

        public static void GetIndexRange(List<LazinatorComparableKeyValue<TKey,TValue>> list, int index, out int firstIndex, out int lastIndex)
        {
            T item = list[index];
            firstIndex = index;
            lastIndex = index;
            while (firstIndex > 0 && list[firstIndex - 1].Equals(item))
                firstIndex--;
            while (lastIndex < list.Count() - 1 && list[lastIndex + 1].Equals(item))
                lastIndex++;
        }

        public MultivalueLocationOptions ChooseMultivalueInsertOption()
        {
            int i = ran.Next(0, 5);
            switch (i)
            {
                case 0:
                    return MultivalueLocationOptions.Any;
                case 1:
                    return MultivalueLocationOptions.First;
                case 2:
                    return MultivalueLocationOptions.Last;
                case 3:
                    return MultivalueLocationOptions.InsertBeforeFirst;
                case 4:
                    return MultivalueLocationOptions.InsertAfterLast;
                default:
                    throw new NotSupportedException();
            }
        }

        public MultivalueLocationOptions ChooseMultivalueDeleteOption()
        {
            int i = ran.Next(0, 3);
            switch (i)
            {
                case 0:
                    return MultivalueLocationOptions.Any;
                case 1:
                    return MultivalueLocationOptions.First;
                case 2:
                    return MultivalueLocationOptions.Last;
                default:
                    throw new NotSupportedException();
            }
        }

        public (int index, bool insertedNotReplaced) InsertOrReplaceItem(List<LazinatorComparableKeyValue<TKey,TValue>> list, T item, bool sorted, MultivalueLocationOptions whichOne)
        {
            if (sorted)
            {
                int index = list.BinarySearch(item);
                bool found = index >= 0;
                if (!found)
                    index = ~index;
                bool replace = false;
                if (found)
                {
                    GetIndexRange(list, index, out int firstIndex, out int lastIndex);
                    switch (whichOne)
                    {
                        case MultivalueLocationOptions.Any:
                            replace = true;
                            break;
                        case MultivalueLocationOptions.First:
                            index = firstIndex;
                            replace = true;
                            break;
                        case MultivalueLocationOptions.Last:
                            index = lastIndex;
                            replace = true;
                            break;
                        case MultivalueLocationOptions.InsertBeforeFirst:
                            index = firstIndex;
                            replace = false;
                            break;
                        case MultivalueLocationOptions.InsertAfterLast:
                            index = lastIndex + 1;
                            replace = false;
                            break;
                    }
                }
                if (replace)
                {
                    list[index] = item;
                    return (index, false);
                }
                else
                {
                    list.Insert(index, item);
                    return (index, true);
                }
            }
            else
            {
                if (whichOne == MultivalueLocationOptions.InsertAfterLast || whichOne == MultivalueLocationOptions.InsertBeforeFirst || list.Count == 0)
                {
                    int index = ran.Next(0, list.Count + 1);
                    list.Insert(index, item);
                    return (index, true);
                }
                else
                {
                    int index = ran.Next(0, list.Count);
                    list[index] = item;
                    return (index, false);
                }
            }
        }

        public abstract T GetRandomValue();

        public abstract class RandomInstruction
        {
            protected bool KeyValueContainerIsSorted;
            protected ISortedKeyValueContainer<TKey, TValue> SortedKeyValueContainer;

            public virtual void Execute(KeyValueContainerTests<TKey, TValue> testClass, IKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey,TValue>> list)
            {
                EstablishSorted(container);
                switch (container)
                {
                    case AvlSortedIndexableKeyValueTree<TKey, TValue> sortedIndexableKeyValueContainer when sortedIndexableKeyValueContainer.AllowDuplicates == true:
                        Execute_SortedIndexableMultivalue(testClass, sortedIndexableKeyValueContainer, list);
                        break;
                    case AvlSortedKeyValueTree<TKey, TValue> sortedKeyValueContainer when sortedKeyValueContainer.AllowDuplicates == true:
                        Execute_SortedMultivalue(testClass, sortedKeyValueContainer, list);
                        break;
                    case AvlIndexableKeyValueTree<TKey, TValue> indexableKeyValueContainer when indexableKeyValueContainer.AllowDuplicates == true:
                        Execute_IndexableMultivalue(testClass, indexableKeyValueContainer, list);
                        break;
                    case AvlKeyValueTree<TKey, TValue> basicKeyValueContainer when basicKeyValueContainer.AllowDuplicates == true:
                        Execute_Multivalue(testClass, basicKeyValueContainer, list);
                        break;
                    case AvlSortedIndexableKeyValueTree<TKey, TValue> sortedIndexableKeyValueContainer when sortedIndexableKeyValueContainer.AllowDuplicates == false:
                        Execute_SortedIndexable(testClass, sortedIndexableKeyValueContainer, list);
                        break;
                    case AvlSortedKeyValueTree<TKey, TValue> sortedKeyValueContainer when sortedKeyValueContainer.AllowDuplicates == false:
                        Execute_Sorted(testClass, sortedKeyValueContainer, list);
                        break;
                    case AvlIndexableKeyValueTree<TKey, TValue> indexableKeyValueContainer when indexableKeyValueContainer.AllowDuplicates == false:
                        Execute_Indexable(testClass, indexableKeyValueContainer, list);
                        break;
                    case AvlKeyValueTree<TKey, TValue> basicKeyValueContainer when basicKeyValueContainer.AllowDuplicates == false:
                        Execute_Value(testClass, basicKeyValueContainer, list);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }


            protected void EstablishSorted(IKeyValueContainer<TKey, TValue> container)
            {
                SortedKeyValueContainer = container as ISortedKeyValueContainer<TKey, TValue>;
                KeyValueContainerIsSorted = container is ISortedKeyValueContainer<TKey, TValue>;
            }

            public abstract void Execute_Value(KeyValueContainerTests<TKey, TValue> testClass, IKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey,TValue>> list);
            public abstract void Execute_Indexable(KeyValueContainerTests<TKey, TValue> testClass, IIndexableKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey,TValue>> list);
            public abstract void Execute_Sorted(KeyValueContainerTests<TKey, TValue> testClass, ISortedKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey,TValue>> list);
            public abstract void Execute_Multivalue(KeyValueContainerTests<TKey, TValue> testClass, IKeyMultivalueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey,TValue>> list);
            public abstract void Execute_SortedMultivalue(KeyValueContainerTests<TKey, TValue> testClass, ISortedKeyMultivalueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey,TValue>> list);
            public abstract void Execute_SortedIndexable(KeyValueContainerTests<TKey, TValue> testClass, ISortedIndexableKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey,TValue>> list);
            public abstract void Execute_IndexableMultivalue(KeyValueContainerTests<TKey, TValue> testClass, IIndexableKeyMultivalueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey,TValue>> list);
            public abstract void Execute_SortedIndexableMultivalue(KeyValueContainerTests<TKey, TValue> testClass, ISortedIndexableKeyMultivalueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey,TValue>> list);
            public IComparer<TKey> C => Comparer<TKey>.Default;
            public bool Eq(T item, T other)
            {
                return EqualityComparer<TKey>.Default.Equals(item, other);
            }
            public void AssertEqual(T item, T other)
            {
                Eq(item, other).Should().BeTrue();
            }
            public void AssertNotEqual(T item, T other)
            {
                Eq(item, other).Should().BeFalse();
            }
            public void VerifyExpectedIndex(MultivalueLocationOptions whichOne, (T item, int firstIndex, int lastIndex) listResult, long indexableKeyValueContainerResult)
            {
                if (whichOne == MultivalueLocationOptions.First || whichOne == MultivalueLocationOptions.InsertBeforeFirst)
                    indexableKeyValueContainerResult.Should().Be(listResult.firstIndex);
                else if (whichOne == MultivalueLocationOptions.Last)
                    indexableKeyValueContainerResult.Should().Be(listResult.lastIndex);
                else if (whichOne == MultivalueLocationOptions.InsertAfterLast)
                    indexableKeyValueContainerResult.Should().Be(listResult.lastIndex + 1);
                else if (whichOne == MultivalueLocationOptions.Any)
                {
                    indexableKeyValueContainerResult.Should().BeGreaterOrEqualTo(listResult.firstIndex);
                    indexableKeyValueContainerResult.Should().BeLessOrEqualTo(listResult.lastIndex);
                }
            }
        }

        public class GetValueInstruction : RandomInstruction
        {
            public override void Execute_Indexable(KeyValueContainerTests<TKey, TValue> testClass, IIndexableKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey,TValue>> list)
            {
                Execute_IndexableHelper(testClass, container, list, MultivalueLocationOptions.Any);
            }

            private void Execute_IndexableHelper(KeyValueContainerTests<TKey, TValue> testClass, IIndexableKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey,TValue>> list, MultivalueLocationOptions whichOne)
            {
                var listResultOrNull = testClass.GetRandomItem(list);
                if (listResultOrNull == null)
                {
                    if (KeyValueContainerIsSorted)
                    {
                        var findResult = container.Find(default, C);
                        findResult.index.Should().Be(0);
                        findResult.exists.Should().BeFalse();
                    }
                }
                else
                {
                    var listResult = listResultOrNull.Value;
                    if (KeyValueContainerIsSorted)
                    {
                        var findResult = container.Find(listResult.item, C);
                        VerifyExpectedIndex(MultivalueLocationOptions.Any, listResult, findResult.index);
                        findResult.exists.Should().BeTrue();
                    }
                    for (int i = listResult.firstIndex; i <= listResult.lastIndex; i++)
                    {
                        T getAtResult = container.GetAt(i);
                        AssertEqual(getAtResult, listResult.item);
                    }
                }
            }

            public override void Execute_IndexableMultivalue(KeyValueContainerTests<TKey, TValue> testClass, IIndexableKeyMultivalueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey,TValue>> list)
            {
                foreach (MultivalueLocationOptions whichOne in new MultivalueLocationOptions[] { MultivalueLocationOptions.First, MultivalueLocationOptions.Any, MultivalueLocationOptions.Last }) // other options are undefined
                {
                    Execute_IndexableHelper(testClass, container, list, whichOne);
                }
            }

            public override void Execute_Multivalue(KeyValueContainerTests<TKey, TValue> testClass, IKeyMultivalueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey,TValue>> list)
            {
                var listResultOrNull = testClass.GetRandomItem(list);
                if (listResultOrNull == null)
                {
                    var getValueResult = container.GetValue(default, MultivalueLocationOptions.Any, C, out T getValueMatch);
                    getValueResult.Should().BeFalse();
                    AssertEqual(getValueMatch, default(T));
                }
                else
                {
                    var listResult = listResultOrNull.Value;
                    foreach (MultivalueLocationOptions whichOne in new MultivalueLocationOptions[] { MultivalueLocationOptions.First, MultivalueLocationOptions.Any, MultivalueLocationOptions.Last })
                    {
                        var getValueResult = container.GetValue(listResult.item, whichOne, C, out T getValueMatch);
                        getValueResult.Should().BeTrue();
                        AssertEqual(getValueMatch, listResult.item);
                    }
                    foreach (MultivalueLocationOptions whichOne in new MultivalueLocationOptions[] { MultivalueLocationOptions.InsertBeforeFirst, MultivalueLocationOptions.InsertAfterLast })
                    {
                        var getValueResult = container.GetValue(listResult.item, whichOne, C, out T getValueMatch);
                        getValueResult.Should().BeFalse();
                        AssertEqual(getValueMatch, default);
                    }
                    long count = container.Count(listResult.item, Comparer<TKey>.Default);
                    count.Should().Be(listResult.lastIndex - listResult.firstIndex + 1);
                }
            }

            public override void Execute_Sorted(KeyValueContainerTests<TKey, TValue> testClass, ISortedKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey,TValue>> list)
            {
                var listResultOrNull = testClass.GetRandomItem(list);
                if (listResultOrNull == null)
                {
                    bool containsResult = container.Contains(default);
                    containsResult.Should().BeFalse();
                }
                else
                {
                    var listResult = listResultOrNull.Value;
                    bool containsResult = container.Contains(listResult.item);
                    containsResult.Should().BeTrue();
                }
            }

            public override void Execute_SortedIndexable(KeyValueContainerTests<TKey, TValue> testClass, ISortedIndexableKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey,TValue>> list)
            {
                var listResultOrNull = testClass.GetRandomItem(list);
                if (listResultOrNull == null)
                {
                    Execute_Indexable(testClass, container, list);
                }
                else
                {
                    var listResult = listResultOrNull.Value;
                    var findResult = container.Find(listResult.item);
                    VerifyExpectedIndex(MultivalueLocationOptions.Any, listResult, findResult.index);
                    findResult.exists.Should().BeTrue();
                    for (int i = listResult.firstIndex; i <= listResult.lastIndex; i++)
                    {
                        T getAtResult = container.GetAt(i);
                        AssertEqual(getAtResult, listResult.item);
                    }
                }
            }

            public override void Execute_SortedIndexableMultivalue(KeyValueContainerTests<TKey, TValue> testClass, ISortedIndexableKeyMultivalueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey,TValue>> list)
            {
                var listResultOrNull = testClass.GetRandomItem(list);
                if (listResultOrNull == null)
                {
                    Execute_IndexableMultivalue(testClass, container, list);
                }
                else
                {
                    var listResult = listResultOrNull.Value;
                    foreach (MultivalueLocationOptions whichOne in new MultivalueLocationOptions[] { MultivalueLocationOptions.First, MultivalueLocationOptions.Any, MultivalueLocationOptions.Last }) // other options are undefined
                    {
                        var findResult = container.Find(listResult.item, whichOne);
                        VerifyExpectedIndex(whichOne, listResult, findResult.index);
                        findResult.exists.Should().BeTrue();
                    }
                }
            }

            public override void Execute_SortedMultivalue(KeyValueContainerTests<TKey, TValue> testClass, ISortedKeyMultivalueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey,TValue>> list)
            {
                Execute_Multivalue(testClass, container, list);
                var listResultOrNull = testClass.GetRandomItem(list);
                if (listResultOrNull != null)
                {
                    var listResult = listResultOrNull.Value;
                    long count = container.Count(listResult.item, Comparer<TKey>.Default);
                    count.Should().Be(listResult.lastIndex - listResult.firstIndex + 1);
                }
            }

            public override void Execute_Value(KeyValueContainerTests<TKey, TValue> testClass, IKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey,TValue>> list)
            {
                var listResultOrNull = testClass.GetRandomItem(list);
                if (listResultOrNull == null)
                {
                    bool present = container.GetValue(default, C, out T match);
                    present.Should().BeFalse();
                    AssertEqual(match, default(T));
                }
                else
                {
                    var listResult = listResultOrNull.Value;
                    bool present = container.GetValue(listResult.item, C, out T match);
                    present.Should().BeTrue();
                    AssertEqual(match, listResultOrNull.Value.item);
                }
            }
        }

        public class InsertValueInstruction : RandomInstruction
        {

            T Item;
            MultivalueLocationOptions WhichOne;
            int Index;
            bool InsertedNotReplaced;

            public override void Execute(KeyValueContainerTests<TKey, TValue> testClass, IKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey,TValue>> list)
            {
                Item = testClass.GetRandomValue();
                WhichOne = testClass.AllowDuplicates ? testClass.ChooseMultivalueInsertOption() : MultivalueLocationOptions.Any; // if not multivalue, just replace the item
                EstablishSorted(container);
                // If we are using the base container type, then we can only add items with a comparer, so we treat it as a sorted container.
                (Index, InsertedNotReplaced) = testClass.InsertOrReplaceItem(list, Item, KeyValueContainerIsSorted || !(container is IIndexableKeyValueContainer<TKey, TValue>), WhichOne);
                base.Execute(testClass, container, list);
            }

            public override void Execute_Indexable(KeyValueContainerTests<TKey, TValue> testClass, IIndexableKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey,TValue>> list)
            {
                if (KeyValueContainerIsSorted)
                    Execute_SortedIndexable(testClass, (ISortedIndexableKeyValueContainer<TKey, TValue>)SortedKeyValueContainer, list);
                else
                {
                    if (InsertedNotReplaced)
                        container.InsertAt(Index, Item);
                    else
                        container.SetAt(Index, Item);
                }
            }

            public override void Execute_IndexableMultivalue(KeyValueContainerTests<TKey, TValue> testClass, IIndexableKeyMultivalueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey,TValue>> list)
            {
                if (KeyValueContainerIsSorted)
                {
                    (long index, bool insertedNotReplaced) = container.InsertGetIndex(Item, WhichOne, C);
                    index.Should().Be(Index);
                    insertedNotReplaced.Should().Be(InsertedNotReplaced);
                }
                else
                {
                    Execute_Indexable(testClass, container, list);
                }
            }

            public override void Execute_Multivalue(KeyValueContainerTests<TKey, TValue> testClass, IKeyMultivalueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey,TValue>> list)
            {
                if (KeyValueContainerIsSorted)
                {
                    bool insertedNotReplaced = container.TryInsert(Item, WhichOne, C);
                    insertedNotReplaced.Should().Be(InsertedNotReplaced);
                }
                else
                {
                    bool insertedNotReplaced = container.TryInsert(Item, WhichOne, C);
                    insertedNotReplaced.Should().Be(InsertedNotReplaced);
                }
            }

            public override void Execute_Sorted(KeyValueContainerTests<TKey, TValue> testClass, ISortedKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey,TValue>> list)
            {
                bool insertedNotReplaced = container.TryInsert(Item);
                insertedNotReplaced.Should().Be(InsertedNotReplaced);
            }

            public override void Execute_SortedIndexable(KeyValueContainerTests<TKey, TValue> testClass, ISortedIndexableKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey,TValue>> list)
            {
                (long index, bool insertedNotReplaced) = container.InsertGetIndex(Item);
                index.Should().Be(Index);
                insertedNotReplaced.Should().Be(InsertedNotReplaced);
            }

            public override void Execute_SortedIndexableMultivalue(KeyValueContainerTests<TKey, TValue> testClass, ISortedIndexableKeyMultivalueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey,TValue>> list)
            {
                (long index, bool insertedNotReplaced) = container.InsertGetIndex(Item, WhichOne);
                // if WhichOne == Any, then exact location is undefined, so we don't verify it. The behavior may be different from our list implementation because which one is selected may be based on the ordering of the binary tree. The list binary search algorithm always starts from the middle element, while a tree search will start from the top of the tree, which may not be the exact middle element.
                if (WhichOne != MultivalueLocationOptions.Any)
                    index.Should().Be(Index);
                insertedNotReplaced.Should().Be(InsertedNotReplaced);
            }

            public override void Execute_SortedMultivalue(KeyValueContainerTests<TKey, TValue> testClass, ISortedKeyMultivalueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey,TValue>> list)
            {
                bool insertedNotReplaced = container.TryInsert(Item, WhichOne);
                insertedNotReplaced.Should().Be(InsertedNotReplaced);
            }

            public override void Execute_Value(KeyValueContainerTests<TKey, TValue> testClass, IKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey,TValue>> list)
            {
                bool insertedNotReplaced = container.TryInsert(Item, C);
                insertedNotReplaced.Should().Be(InsertedNotReplaced);
            }
        }

        public class RemoveInstruction : RandomInstruction
        {
            T ValueToTryToRemove;
            bool ValueExisted;
            int IndexBeforeRemove;
            MultivalueLocationOptions WhichOne;
            bool RemoveAll;
            int FirstIndex, LastIndex;

            public override void Execute(KeyValueContainerTests<TKey, TValue> testClass, IKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey,TValue>> list)
            {
                EstablishSorted(container);
                if (testClass.AllowDuplicates)
                {
                    if (KeyValueContainerIsSorted && testClass.ran.Next(0, 5) == 0)
                    {
                        RemoveAll = true; // overrides remaining settings
                        WhichOne = MultivalueLocationOptions.InsertAfterLast; // invalid -- but we won't use it in a call; we use something invalid to ensure that if we do, we'll get an error
                    }
                    else
                        WhichOne = testClass.ChooseMultivalueDeleteOption();
                }
                else
                    WhichOne = MultivalueLocationOptions.Any;
                PlanRemoval(testClass, list);
                if (ValueExisted)
                {
                    if (RemoveAll)
                    {
                        for (int i = FirstIndex; i <= LastIndex; i++)
                            list.RemoveAt(FirstIndex);
                    }
                    else switch (WhichOne)
                        {
                            case MultivalueLocationOptions.Any:
                                list.RemoveAt(IndexBeforeRemove);
                                break;
                            case MultivalueLocationOptions.First:
                                list.RemoveAt(FirstIndex);
                                break;
                            case MultivalueLocationOptions.Last:
                                list.RemoveAt(LastIndex);
                                break;
                            default: throw new NotSupportedException();
                        }
                }
                base.Execute(testClass, container, list);
            }

            public void PlanRemoval(KeyValueContainerTests<TKey, TValue> testClass, List<LazinatorComparableKeyValue<TKey,TValue>> list)
            {
                if (!KeyValueContainerIsSorted)
                {
                    // Removing by index
                    if (list.Any())
                    {
                        ChooseItemInListToRemove(testClass, list);
                    }
                    else
                    {
                        ValueExisted = false;
                        ValueToTryToRemove = default;
                        IndexBeforeRemove = -1;
                    }
                    return;
                }
                if (!list.Any() || testClass.ran.Next(5) == 0)
                { // try to find something NOT in list to try (and fail) to remove
                    const int maxTriesToFindNotIncludedItem = 10;
                    for (int i = 0; i < maxTriesToFindNotIncludedItem; i++)
                    {
                        T randomValue = testClass.GetRandomValue();
                        int index = list.BinarySearch(randomValue, Comparer<TKey>.Default);
                        if (index < 0)
                        {
                            ValueToTryToRemove = randomValue;
                            ValueExisted = false;
                            IndexBeforeRemove = -1;
                            return;
                        }
                        else if (i == maxTriesToFindNotIncludedItem - 1)
                        { // We'll go with something in the list
                            ValueToTryToRemove = randomValue;
                            IndexBeforeRemove = index;
                            ValueExisted = true;
                            KeyValueContainerTests<TKey, TValue>.GetIndexRange(list, IndexBeforeRemove, out FirstIndex, out LastIndex);
                            return;
                        }
                    }
                }
                else
                {
                    ChooseItemInListToRemove(testClass, list);
                }
            }

            private void ChooseItemInListToRemove(KeyValueContainerTests<TKey, TValue> testClass, List<LazinatorComparableKeyValue<TKey,TValue>> list)
            {
                ValueExisted = true;
                IndexBeforeRemove = testClass.ran.Next(list.Count);
                ValueToTryToRemove = list[IndexBeforeRemove];
                KeyValueContainerTests<TKey, TValue>.GetIndexRange(list, IndexBeforeRemove, out FirstIndex, out LastIndex);
            }

            public override void Execute_Indexable(KeyValueContainerTests<TKey, TValue> testClass, IIndexableKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey,TValue>> list)
            {
                if (IndexBeforeRemove != -1)
                {
                    container.RemoveAt(IndexBeforeRemove);
                }
            }

            public override void Execute_IndexableMultivalue(KeyValueContainerTests<TKey, TValue> testClass, IIndexableKeyMultivalueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey,TValue>> list)
            {
                Execute_Indexable(testClass, container, list);
            }

            public override void Execute_Multivalue(KeyValueContainerTests<TKey, TValue> testClass, IKeyMultivalueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey,TValue>> list)
            {
                if (RemoveAll)
                    container.TryRemoveAll(ValueToTryToRemove, Comparer<TKey>.Default);
                else
                {
                    bool result = container.TryRemove(ValueToTryToRemove, WhichOne, Comparer<TKey>.Default);
                    VerifySuccess(result);
                }
            }

            public override void Execute_Sorted(KeyValueContainerTests<TKey, TValue> testClass, ISortedKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey,TValue>> list)
            {
                bool result = container.TryRemove(ValueToTryToRemove);
                VerifySuccess(result);
            }

            private void VerifySuccess(bool result)
            {
                if (IndexBeforeRemove == -1)
                    result.Should().BeFalse();
                else
                    result.Should().BeTrue();
            }

            public override void Execute_SortedIndexable(KeyValueContainerTests<TKey, TValue> testClass, ISortedIndexableKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey,TValue>> list)
            {
                if (testClass.ran.Next(2) == 0)
                    Execute_Sorted(testClass, container, list);
                else
                    Execute_Indexable(testClass, container, list);
            }

            public override void Execute_SortedIndexableMultivalue(KeyValueContainerTests<TKey, TValue> testClass, ISortedIndexableKeyMultivalueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey,TValue>> list)
            {
                if (RemoveAll)
                    container.TryRemoveAll(ValueToTryToRemove);
                else
                {
                    if (testClass.ran.Next(2) == 0)
                        Execute_SortedMultivalue(testClass, container, list);
                    else
                        Execute_Indexable(testClass, container, list);
                }
            }

            public override void Execute_SortedMultivalue(KeyValueContainerTests<TKey, TValue> testClass, ISortedKeyMultivalueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey,TValue>> list)
            {
                if (RemoveAll)
                    container.TryRemoveAll(ValueToTryToRemove);
                else
                {
                    bool result = container.TryRemove(ValueToTryToRemove, WhichOne);
                    VerifySuccess(result);
                }
            }

            public override void Execute_Value(KeyValueContainerTests<TKey, TValue> testClass, IKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey,TValue>> list)
            {
                bool result = container.TryRemove(ValueToTryToRemove, Comparer<TKey>.Default);
                if (IndexBeforeRemove == -1)
                    result.Should().BeFalse();
                else
                    result.Should().BeTrue();
            }
        }
    }
}
