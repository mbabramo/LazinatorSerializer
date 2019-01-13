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
            VerifyKeyValueContainer(KeyValueContainerType.AvlSortedIndexableKeyValueTree, true, 100, 100);
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

        public override WInt GetRandomKey()
        {
            return ran.Next(100);
        }

        public override WInt GetRandomValue()
        {
            return ran.Next(100);
        }

    }

    public abstract class KeyValueContainerTests<TKey, TValue> : SerializationDeserializationTestBase where TKey : ILazinator, IComparable<TKey>, IComparable where TValue : ILazinator
    {

        public IKeyValueContainer<TKey, TValue> GetKeyValueContainer(KeyValueContainerType containerType, bool allowDuplicates)
        {
            switch (containerType)
            {
                case KeyValueContainerType.AvlKeyValueTree:
                    return new AvlKeyValueTree<TKey, TValue>(allowDuplicates);
                case KeyValueContainerType.AvlIndexableKeyValueTree:
                    return new AvlIndexableKeyValueTree<TKey, TValue>(allowDuplicates);
                case KeyValueContainerType.AvlSortedKeyValueTree:
                    return new AvlSortedKeyValueTree<TKey, TValue>(allowDuplicates);
                case KeyValueContainerType.AvlSortedIndexableKeyValueTree:
                    return new AvlSortedIndexableKeyValueTree<TKey, TValue>(allowDuplicates);
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
                List<LazinatorComparableKeyValue<TKey, TValue>> list = new List<LazinatorComparableKeyValue<TKey, TValue>>();
                IKeyValueContainer<TKey, TValue> container = GetKeyValueContainer(containerType, allowDuplicates);
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
                    if (rep == 0 && i == 75)
                    {
                        var DEBUG = 0;
                    }
                    instruction.Execute(this, container, list);
                    VerifyEntireList(container, list); // DEBUG
                }
                VerifyEntireList(container, list);
                VerifyEnumerableSkipAndReverse(container, list);
            }
        }

        public void VerifyEntireList(IKeyValueContainer<TKey, TValue> keyValueContainer, List<LazinatorComparableKeyValue<TKey, TValue>> list)
        {
            var pairsEnumerator = keyValueContainer.GetKeyValuePairEnumerator();
            List<LazinatorComparableKeyValue<TKey, TValue>> pairsList = new List<LazinatorComparableKeyValue<TKey, TValue>>();
            while (pairsEnumerator.MoveNext())
                pairsList.Add(new LazinatorComparableKeyValue<TKey, TValue>(pairsEnumerator.Current.Key, pairsEnumerator.Current.Value));
            pairsList.SequenceEqual(list).Should().BeTrue();
        }

        public void VerifyEnumerableSkipAndReverse(IKeyValueContainer<TKey, TValue> keyValueContainer, List<LazinatorComparableKeyValue<TKey, TValue>> list)
        {

            var list2 = list.ToList();
            list2.Reverse();
            int numToSkip = ran.Next(0, list.Count + 1);
            var withSkips = list2.Skip(numToSkip).ToList();

            var pairsEnumerator = keyValueContainer.GetKeyValuePairEnumerator(true, numToSkip);
            List<LazinatorComparableKeyValue<TKey, TValue>> pairsList = new List<LazinatorComparableKeyValue<TKey, TValue>>();
            while (pairsEnumerator.MoveNext())
                pairsList.Add(new LazinatorComparableKeyValue<TKey, TValue>(pairsEnumerator.Current.Key, pairsEnumerator.Current.Value));
            pairsList.SequenceEqual(withSkips).Should().BeTrue();
        }

        public (LazinatorComparableKeyValue<TKey, TValue> item, int index, int firstIndex, int lastIndex)? GetRandomItem(List<LazinatorComparableKeyValue<TKey, TValue>> list)
        {
            if (!list.Any())
                return null;
            int index = ran.Next(0, list.Count());
            var item = list[index];
            int firstIndex, lastIndex;
            GetIndexRange(list, index, out firstIndex, out lastIndex);
            return (item, index, firstIndex, lastIndex);
        }


        public static void GetIndexRange(List<LazinatorComparableKeyValue<TKey, TValue>> list, int index, out int firstIndex, out int lastIndex)
        {
            var item = list[index];
            firstIndex = index;
            lastIndex = index;
            while (firstIndex > 0 && list[firstIndex - 1].Key.Equals(item.Key))
                firstIndex--;
            while (lastIndex < list.Count() - 1 && list[lastIndex + 1].Key.Equals(item.Key))
                lastIndex++;
        }

        public MultivalueLocationOptions ChooseMultivalueInsertOption()
        {
            int i = ran.Next(0, 5);
            switch (i)
            {
                case 0:
                    return MultivalueLocationOptions.Last; // for now, at least, we won't test Any when inserting in a multivalue store; it is difficult to test, because its behavior is undefined. But we are still testing it in the single value scenario.
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

        public IComparer<LazinatorComparableKeyValue<TKey, TValue>> KeyOnlyComparer => LazinatorComparableKeyValue<TKey, TValue>.GetKeyOnlyComparer();

        public (int index, bool insertedNotReplaced) InsertOrReplaceItem(List<LazinatorComparableKeyValue<TKey, TValue>> list, LazinatorComparableKeyValue<TKey, TValue> item, bool sorted, MultivalueLocationOptions whichOne)
        {
            if (sorted)
            {
                int index = list.BinarySearch(item, KeyOnlyComparer);
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

        public abstract TKey GetRandomKey();
        public abstract TValue GetRandomValue();

        public abstract class RandomInstruction
        {
            protected bool KeyValueContainerIsSorted;
            protected ISortedKeyValueContainer<TKey, TValue> SortedKeyValueContainer;
            protected IKeyMultivalueContainer<TKey, TValue> KeyMultivalueContainer;
            protected bool ContainerIsMultivalue;

            public virtual void Execute(KeyValueContainerTests<TKey, TValue> testClass, IKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey, TValue>> list)
            {
                EstablishSorted(container);
                EstablishMultivalue(container);
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

            protected void EstablishMultivalue(IKeyValueContainer<TKey, TValue> container)
            {
                KeyMultivalueContainer = container as IKeyMultivalueContainer<TKey, TValue>;
                ContainerIsMultivalue = container is IKeyMultivalueContainer<TKey, TValue>;
            }


            public abstract void Execute_Value(KeyValueContainerTests<TKey, TValue> testClass, IKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey, TValue>> list);
            public abstract void Execute_Indexable(KeyValueContainerTests<TKey, TValue> testClass, IIndexableKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey, TValue>> list);
            public abstract void Execute_Sorted(KeyValueContainerTests<TKey, TValue> testClass, ISortedKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey, TValue>> list);
            public abstract void Execute_Multivalue(KeyValueContainerTests<TKey, TValue> testClass, IKeyMultivalueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey, TValue>> list);
            public abstract void Execute_SortedMultivalue(KeyValueContainerTests<TKey, TValue> testClass, ISortedKeyMultivalueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey, TValue>> list);
            public abstract void Execute_SortedIndexable(KeyValueContainerTests<TKey, TValue> testClass, ISortedIndexableKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey, TValue>> list);
            public abstract void Execute_IndexableMultivalue(KeyValueContainerTests<TKey, TValue> testClass, IIndexableKeyMultivalueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey, TValue>> list);
            public abstract void Execute_SortedIndexableMultivalue(KeyValueContainerTests<TKey, TValue> testClass, ISortedIndexableKeyMultivalueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey, TValue>> list);
            public IComparer<TKey> C => Comparer<TKey>.Default;
            public bool Eq(TValue item, TValue other)
            {
                return EqualityComparer<TValue>.Default.Equals(item, other);
            }
            public void AssertEqual(TValue item, TValue other)
            {
                Eq(item, other).Should().BeTrue();
            }
            public void AssertNotEqual(TValue item, TValue other)
            {
                Eq(item, other).Should().BeFalse();
            }
            public bool Eq(LazinatorComparableKeyValue<TKey, TValue> item, LazinatorComparableKeyValue<TKey, TValue> other)
            {
                return EqualityComparer<LazinatorComparableKeyValue<TKey, TValue>>.Default.Equals(item, other);
            }
            public void AssertEqual(LazinatorComparableKeyValue<TKey, TValue> item, LazinatorComparableKeyValue<TKey, TValue> other)
            {
                Eq(item, other).Should().BeTrue();
            }
            public void AssertNotEqual(LazinatorComparableKeyValue<TKey, TValue> item, LazinatorComparableKeyValue<TKey, TValue> other)
            {
                Eq(item, other).Should().BeFalse();
            }
            public void VerifyExpectedIndex(MultivalueLocationOptions whichOne, (LazinatorComparableKeyValue<TKey, TValue> item, int index, int firstIndex, int lastIndex) listResult, long indexableKeyValueContainerResult)
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
            public override void Execute_Indexable(KeyValueContainerTests<TKey, TValue> testClass, IIndexableKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey, TValue>> list)
            {
                var listResultOrNull = testClass.GetRandomItem(list);
                if (listResultOrNull == null)
                {
                    if (KeyValueContainerIsSorted)
                    {
                        var findKeyResult = container.Find(default, C);
                        findKeyResult.index.Should().Be(-1);
                        findKeyResult.found.Should().BeFalse();
                        var findKeyValueResult = container.Find(default, default, C);
                        findKeyValueResult.index.Should().Be(-1);
                        findKeyValueResult.found.Should().BeFalse();
                    }
                }
                else
                {
                    var listResult = listResultOrNull.Value;
                    for (int i = listResult.firstIndex; i <= listResult.lastIndex; i++)
                    {
                        LazinatorComparableKeyValue<TKey, TValue> getAtResult = new LazinatorComparableKeyValue<TKey, TValue>(container.GetKeyAt(i), container.GetValueAt(i));
                        AssertEqual(getAtResult, listResult.item);
                    }
                }
            }

            public override void Execute_IndexableMultivalue(KeyValueContainerTests<TKey, TValue> testClass, IIndexableKeyMultivalueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey, TValue>> list)
            {
                foreach (MultivalueLocationOptions whichOne in new MultivalueLocationOptions[] { MultivalueLocationOptions.First, MultivalueLocationOptions.Any, MultivalueLocationOptions.Last }) // other options are undefined
                {
                    Execute_IndexableMultivalueHelper(testClass, container, list, whichOne);
                }
            }

            private void Execute_IndexableMultivalueHelper(KeyValueContainerTests<TKey, TValue> testClass, IIndexableKeyMultivalueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey, TValue>> list, MultivalueLocationOptions whichOne)
            {
                var listResultOrNull = testClass.GetRandomItem(list);
                if (listResultOrNull == null)
                {
                    if (KeyValueContainerIsSorted)
                    {
                        var findKeyResult = container.Find(default, whichOne, C);
                        findKeyResult.index.Should().Be(0);
                        findKeyResult.found.Should().BeFalse();
                    }
                }
                else
                {
                    var listResult = listResultOrNull.Value;

                    LazinatorComparableKeyValue<TKey, TValue> getAtResult = new LazinatorComparableKeyValue<TKey, TValue>(container.GetKeyAt(listResult.index), container.GetValueAt(listResult.index));
                    AssertEqual(getAtResult, listResult.item);
                    for (int i = listResult.firstIndex; i <= listResult.lastIndex; i++)
                    {
                        var keyResult = container.GetKeyAt(i);
                        EqualityComparer<TKey>.Default.Equals(keyResult, listResult.item.Key).Should().BeTrue();
                        AssertEqual(getAtResult, listResult.item);
                    }
                }
            }

            public override void Execute_Multivalue(KeyValueContainerTests<TKey, TValue> testClass, IKeyMultivalueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey, TValue>> list)
            {
                var listResultOrNull = testClass.GetRandomItem(list);
                if (listResultOrNull == null)
                {
                    Execute_Value(testClass, container, list);
                }
                else
                {
                    var listResult = listResultOrNull.Value;
                    foreach (MultivalueLocationOptions whichOne in new MultivalueLocationOptions[] { MultivalueLocationOptions.First, MultivalueLocationOptions.Any, MultivalueLocationOptions.Last })
                    {
                        var getValueResult = container.GetValueForKey(listResult.item.Key, whichOne, C);
                        if (whichOne == MultivalueLocationOptions.First)
                        {
                            AssertEqual(getValueResult, list[listResult.firstIndex].Value);
                        }
                        if (whichOne == MultivalueLocationOptions.Last)
                        {
                            AssertEqual(getValueResult, list[listResult.lastIndex].Value);
                        }
                    }
                    var getAllValuesResult = container.GetAllValues(listResult.item.Key, C).ToList();
                    for (int i = listResult.firstIndex; i <= listResult.lastIndex; i++)
                    {
                        var fromList = list[i];
                        var fromGetAllValues = getAllValuesResult[i - listResult.firstIndex];
                        AssertEqual(fromList, new LazinatorComparableKeyValue<TKey, TValue>(listResult.item.Key, fromGetAllValues));
                    }
                }
            }

            public override void Execute_Sorted(KeyValueContainerTests<TKey, TValue> testClass, ISortedKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey, TValue>> list)
            {
                var listResultOrNull = testClass.GetRandomItem(list);
                if (listResultOrNull == null)
                {
                    bool keyPresent = container.ContainsKey(default);
                    bool keyValuePresent = container.ContainsKeyValue(default, default);
                    keyPresent.Should().BeFalse();
                    keyValuePresent.Should().BeFalse();
                }
                else
                {
                    var listResult = listResultOrNull.Value;
                    bool keyPresent = container.ContainsKey(listResult.item.Key);
                    bool keyValuePresent = container.ContainsKeyValue(listResult.item.Key, listResult.item.Value, C);
                    keyPresent.Should().BeTrue();
                    keyValuePresent.Should().BeTrue();

                    TValue value = container.GetValueForKey(listResult.item.Key);
                    AssertEqual(value, listResult.item.Value);
                }
            }

            public override void Execute_SortedIndexable(KeyValueContainerTests<TKey, TValue> testClass, ISortedIndexableKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey, TValue>> list)
            {
                var listResultOrNull = testClass.GetRandomItem(list);
                if (listResultOrNull == null)
                {
                    Execute_Indexable(testClass, container, list);
                }
                else
                {
                    var listResult = listResultOrNull.Value;
                    var findKeyResult = container.Find(listResult.item.Key);
                    VerifyExpectedIndex(MultivalueLocationOptions.Any, listResult, findKeyResult.index);
                    findKeyResult.found.Should().BeTrue();
                    AssertEqual(findKeyResult.valueIfFound, listResult.item.Value);
                    var findKeyValueResult = container.Find(listResult.item.Key, listResult.item.Value);
                    VerifyExpectedIndex(MultivalueLocationOptions.Any, listResult, findKeyValueResult.index);
                    findKeyValueResult.found.Should().BeTrue();
                }
            }

            public override void Execute_SortedIndexableMultivalue(KeyValueContainerTests<TKey, TValue> testClass, ISortedIndexableKeyMultivalueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey, TValue>> list)
            {
                var listResultOrNull = testClass.GetRandomItem(list);
                if (listResultOrNull == null)
                {
                    Execute_Indexable(testClass, container, list);
                }
                else
                {
                    var listResult = listResultOrNull.Value;
                    MultivalueLocationOptions? whichOne;
                    if (listResult.index == listResult.firstIndex)
                        whichOne = MultivalueLocationOptions.First;
                    else if (listResult.index == listResult.lastIndex)
                        whichOne = MultivalueLocationOptions.Last;
                    else
                        whichOne = null;
                    if (whichOne != null)
                    {
                        var findKeyResult = container.Find(listResult.item.Key, whichOne.Value);
                        VerifyExpectedIndex(whichOne.Value, listResult, findKeyResult.index);
                        findKeyResult.found.Should().BeTrue();
                        AssertEqual(findKeyResult.valueIfFound, listResult.item.Value);
                    }
                }
            }

            public override void Execute_SortedMultivalue(KeyValueContainerTests<TKey, TValue> testClass, ISortedKeyMultivalueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey, TValue>> list)
            {
                Execute_Multivalue(testClass, container, list);
                var listResultOrNull = testClass.GetRandomItem(list);
                if (listResultOrNull != null)
                {
                    var listResult = listResultOrNull.Value;
                    foreach (MultivalueLocationOptions whichOne in new MultivalueLocationOptions[] { MultivalueLocationOptions.First, MultivalueLocationOptions.Any, MultivalueLocationOptions.Last })
                    {
                        var getValueResult = container.GetValueForKey(listResult.item.Key, whichOne);
                        if (whichOne == MultivalueLocationOptions.First)
                        {
                            AssertEqual(getValueResult, list[listResult.firstIndex].Value);
                        }
                        if (whichOne == MultivalueLocationOptions.Last)
                        {
                            AssertEqual(getValueResult, list[listResult.lastIndex].Value);
                        }
                    }
                    var getAllValuesResult = container.GetAllValues(listResult.item.Key).ToList();
                    for (int i = listResult.firstIndex; i <= listResult.lastIndex; i++)
                    {
                        var fromList = list[i];
                        var fromGetAllValues = getAllValuesResult[i - listResult.firstIndex];
                        AssertEqual(fromList, new LazinatorComparableKeyValue<TKey, TValue>(listResult.item.Key, fromGetAllValues));
                    }
                }
            }

            public override void Execute_Value(KeyValueContainerTests<TKey, TValue> testClass, IKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey, TValue>> list)
            {
                var listResultOrNull = testClass.GetRandomItem(list);
                if (listResultOrNull == null)
                {
                    bool keyPresent = container.ContainsKey(default, C);
                    bool keyValuePresent = container.ContainsKeyValue(default, default, C);
                    keyPresent.Should().BeFalse();
                    keyValuePresent.Should().BeFalse();
                }
                else
                {
                    var listResult = listResultOrNull.Value;
                    bool keyPresent = container.ContainsKey(listResult.item.Key, C);
                    bool keyValuePresent = container.ContainsKeyValue(listResult.item.Key, listResult.item.Value, C);
                    keyPresent.Should().BeTrue();
                    keyValuePresent.Should().BeTrue();

                    TValue value = container.GetValueForKey(listResult.item.Key, C);
                    AssertEqual(value, listResult.item.Value);
                }
            }
        }

        public class InsertValueInstruction : RandomInstruction
        {

            TKey Key;
            TValue Value;
            LazinatorComparableKeyValue<TKey, TValue> ComparableKeyValue;
            MultivalueLocationOptions WhichOne;
            int Index;
            bool InsertedNotReplaced;

            public override void Execute(KeyValueContainerTests<TKey, TValue> testClass, IKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey, TValue>> list)
            {
                Key = testClass.GetRandomKey();
                Value = testClass.GetRandomValue();
                ComparableKeyValue = new LazinatorComparableKeyValue<TKey, TValue>(Key, Value);
                WhichOne = testClass.AllowDuplicates ? testClass.ChooseMultivalueInsertOption() : MultivalueLocationOptions.Any; // if not multivalue, just replace the value associated with this key
                EstablishSorted(container);
                // If we are using the base container type, then we can only add items with a comparer, so we treat it as a sorted container.
                (Index, InsertedNotReplaced) = testClass.InsertOrReplaceItem(list, ComparableKeyValue, KeyValueContainerIsSorted || !(container is IIndexableKeyValueContainer<TKey, TValue>), WhichOne);
                base.Execute(testClass, container, list);
            }

            public override void Execute_Indexable(KeyValueContainerTests<TKey, TValue> testClass, IIndexableKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey, TValue>> list)
            {
                if (InsertedNotReplaced)
                    container.InsertAt(Index, Key, Value);
                else
                { // we set key and value separately to make sure these work; these will in turn call SetKeyValueAt
                    container.SetKeyAt(Index, Key);
                    container.SetValueAt(Index, Value);
                }
            }

            public override void Execute_IndexableMultivalue(KeyValueContainerTests<TKey, TValue> testClass, IIndexableKeyMultivalueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey, TValue>> list)
            {
                Execute_Indexable(testClass, container, list);
            }

            public override void Execute_Multivalue(KeyValueContainerTests<TKey, TValue> testClass, IKeyMultivalueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey, TValue>> list)
            {
                bool insertedNotReplaced = true;
                if (WhichOne == MultivalueLocationOptions.InsertAfterLast && testClass.ran.Next(2) == 0)
                    container.AddValueForKey(Key, Value, C); // note that this just calls SetValueForKey anyway.
                else
                {
                    insertedNotReplaced = container.SetValueForKey(Key, Value, WhichOne, C);
                    insertedNotReplaced.Should().Be(InsertedNotReplaced);
                }
            }

            public override void Execute_Sorted(KeyValueContainerTests<TKey, TValue> testClass, ISortedKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey, TValue>> list)
            {
                bool insertedNotReplaced = container.SetValueForKey(Key, Value);
                insertedNotReplaced.Should().Be(InsertedNotReplaced);
            }

            public override void Execute_SortedIndexable(KeyValueContainerTests<TKey, TValue> testClass, ISortedIndexableKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey, TValue>> list)
            {
                (long index, bool insertedNotReplaced) = container.InsertGetIndex(Key, Value);
                index.Should().Be(Index);
                insertedNotReplaced.Should().Be(InsertedNotReplaced);
            }

            public override void Execute_SortedIndexableMultivalue(KeyValueContainerTests<TKey, TValue> testClass, ISortedIndexableKeyMultivalueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey, TValue>> list)
            {
                (long index, bool insertedNotReplaced) = container.InsertGetIndex(Key, Value, WhichOne);
                // if WhichOne == Any, then exact location is undefined, so we don't verify it. The behavior may be different from our list implementation because which one is selected may be based on the ordering of the binary tree. The list binary search algorithm always starts from the middle element, while a tree search will start from the top of the tree, which may not be the exact middle element.
                if (WhichOne != MultivalueLocationOptions.Any)
                    index.Should().Be(Index);
                insertedNotReplaced.Should().Be(InsertedNotReplaced);
            }

            public override void Execute_SortedMultivalue(KeyValueContainerTests<TKey, TValue> testClass, ISortedKeyMultivalueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey, TValue>> list)
            {
                bool insertedNotReplaced = true;
                if (WhichOne == MultivalueLocationOptions.InsertAfterLast && testClass.ran.Next(2) == 0)
                    container.AddValueForKey(Key, Value); // note that this just calls SetValueForKey anyway.
                else
                {
                    insertedNotReplaced = container.SetValueForKey(Key, Value, WhichOne);
                    insertedNotReplaced.Should().Be(InsertedNotReplaced);
                }
            }

            public override void Execute_Value(KeyValueContainerTests<TKey, TValue> testClass, IKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey, TValue>> list)
            {
                bool insertedNotReplaced = container.SetValueForKey(Key, Value, C);
                insertedNotReplaced.Should().Be(InsertedNotReplaced);
            }
        }

        public class RemoveInstruction : RandomInstruction
        {
            bool RemoveSpecificKeyValue;
            TKey KeyToTryToRemove;
            TValue ValueToTryToRemove;
            bool KeyValueExisted;
            int IndexBeforeRemove;
            MultivalueLocationOptions WhichOne;
            bool RemoveAll;
            int FirstIndex, LastIndex;

            public override void Execute(KeyValueContainerTests<TKey, TValue> testClass, IKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey, TValue>> list)
            {
                EstablishSorted(container);
                EstablishMultivalue(container);
                RemoveSpecificKeyValue = testClass.ran.Next(2) == 0;
                if (testClass.AllowDuplicates)
                {
                    if (!RemoveSpecificKeyValue && KeyValueContainerIsSorted && testClass.ran.Next(0, 5) == 0)
                    {
                        RemoveAll = true; // overrides remaining settings
                        WhichOne = MultivalueLocationOptions.InsertAfterLast; // invalid -- but we won't use it in a call; we use something invalid to ensure that if we do, we'll get an error
                    }
                    else if (RemoveSpecificKeyValue)
                        WhichOne = MultivalueLocationOptions.InsertAfterLast; // again invalid, designed to cause error if we use it
                    else
                        WhichOne = testClass.ChooseMultivalueDeleteOption();
                }
                else
                    WhichOne = MultivalueLocationOptions.Any;
                PlanRemoval(testClass, container, list);
                if (KeyValueExisted)
                {
                    if (RemoveAll)
                    {
                        for (int i = FirstIndex; i <= LastIndex; i++)
                            list.RemoveAt(FirstIndex);
                    }
                    else if (RemoveSpecificKeyValue)
                    {
                        bool removed = false;
                        for (int i = FirstIndex; i <= LastIndex; i++)
                            if (EqualityComparer<TValue>.Default.Equals(list[i].Value, ValueToTryToRemove))
                            {
                                list.RemoveAt(i);
                                removed = true;
                                break;
                            }
                        if (!removed)
                            throw new Exception("Logic error.");
                    }
                    else
                    {
                        switch (WhichOne)
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
                }
                base.Execute(testClass, container, list);
            }

            public void PlanRemoval(KeyValueContainerTests<TKey, TValue> testClass, IKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey, TValue>> list)
            {
                if (!KeyValueContainerIsSorted)
                {
                    // Removing by index
                    if (list.Any())
                    {
                        ChooseItemInListToRemove(testClass, container, list);
                    }
                    else
                    {
                        KeyValueExisted = false;
                        KeyToTryToRemove = default;
                        ValueToTryToRemove = default;
                        IndexBeforeRemove = -1;
                    }
                    return;
                }
                if (!list.Any() || testClass.ran.Next(5) == 0)
                { // try to find a key NOT in list to try (and fail) to remove (note that we can't search easily for value, since we're not ordering by that)
                    const int maxTriesToFindNotIncludedItem = 10;
                    for (int i = 0; i < maxTriesToFindNotIncludedItem; i++)
                    {
                        TKey randomKey = testClass.GetRandomKey();
                        TValue randomValue = testClass.GetRandomValue();
                        int index = list.BinarySearch(new LazinatorComparableKeyValue<TKey, TValue>(randomKey, default), testClass.KeyOnlyComparer);
                        if (index < 0)
                        {
                            KeyToTryToRemove = randomKey;
                            ValueToTryToRemove = randomValue;
                            KeyValueExisted = false;
                            IndexBeforeRemove = -1;
                            return;
                        }
                        else if (i == maxTriesToFindNotIncludedItem - 1)
                        { // We'll go with something in the list
                            KeyToTryToRemove = randomKey;
                            ValueToTryToRemove = list[index].Value;
                            IndexBeforeRemove = index;
                            KeyValueExisted = true;
                            KeyValueContainerTests<TKey, TValue>.GetIndexRange(list, IndexBeforeRemove, out FirstIndex, out LastIndex);
                            return;
                        }
                    }
                }
                else
                {
                    ChooseItemInListToRemove(testClass, container, list);
                }
            }

            private void ChooseItemInListToRemove(KeyValueContainerTests<TKey, TValue> testClass, IKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey, TValue>> list)
            {
                KeyValueExisted = true;
                IndexBeforeRemove = testClass.ran.Next(list.Count);
                KeyValueContainerTests<TKey, TValue>.GetIndexRange(list, IndexBeforeRemove, out FirstIndex, out LastIndex);
                KeyToTryToRemove = list[IndexBeforeRemove].Key;
                if (ContainerIsMultivalue)
                {
                    if (WhichOne == MultivalueLocationOptions.Any)
                    {
                        if (container is ISortedIndexableKeyMultivalueContainer<TKey, TValue> sortedIndexableContainer)
                        {
                            // we need to figure out which one will be removed.
                            var result = sortedIndexableContainer.Find(KeyToTryToRemove, MultivalueLocationOptions.Any, Comparer<TKey>.Default);
                            IndexBeforeRemove = (int)result.index;
                        }
                        else
                        {
                            // there is no easy way to figure out which one will be removed, so we just remove the first instead
                            WhichOne = MultivalueLocationOptions.First;
                        }
                    }
                    if (WhichOne == MultivalueLocationOptions.First)
                        IndexBeforeRemove = FirstIndex;
                    else if (WhichOne == MultivalueLocationOptions.Last)
                        IndexBeforeRemove = LastIndex;
                }
                KeyToTryToRemove = list[IndexBeforeRemove].Key; // potentially updated
                ValueToTryToRemove = list[IndexBeforeRemove].Value;
            }

            public override void Execute_Indexable(KeyValueContainerTests<TKey, TValue> testClass, IIndexableKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey, TValue>> list)
            {
                if (IndexBeforeRemove != -1)
                {
                    container.RemoveAt(IndexBeforeRemove);
                }
            }

            public override void Execute_IndexableMultivalue(KeyValueContainerTests<TKey, TValue> testClass, IIndexableKeyMultivalueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey, TValue>> list)
            {
                if (RemoveAll)
                {
                    bool result = container.TryRemoveAll(KeyToTryToRemove, Comparer<TKey>.Default);
                    VerifySuccess(result);
                }
                else
                    Execute_Indexable(testClass, container, list);
            }

            public override void Execute_Multivalue(KeyValueContainerTests<TKey, TValue> testClass, IKeyMultivalueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey, TValue>> list)
            {
                if (RemoveAll)
                    container.TryRemoveAll(KeyToTryToRemove, Comparer<TKey>.Default);
                else
                {
                    bool result = RemoveSpecificKeyValue ? container.TryRemoveKeyValue(KeyToTryToRemove, ValueToTryToRemove, Comparer<TKey>.Default) : container.TryRemove(KeyToTryToRemove, WhichOne, Comparer<TKey>.Default);
                    VerifySuccess(result);
                }
            }

            public override void Execute_Sorted(KeyValueContainerTests<TKey, TValue> testClass, ISortedKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey, TValue>> list)
            {
                bool result = container.TryRemove(KeyToTryToRemove);
                VerifySuccess(result);
            }

            private void VerifySuccess(bool result)
            {
                if (KeyValueExisted)
                    result.Should().BeTrue();
                else
                    result.Should().BeFalse();
            }

            public override void Execute_SortedIndexable(KeyValueContainerTests<TKey, TValue> testClass, ISortedIndexableKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey, TValue>> list)
            {
                if (testClass.ran.Next(2) == 0)
                    Execute_Sorted(testClass, container, list);
                else
                    Execute_Indexable(testClass, container, list);
            }

            public override void Execute_SortedIndexableMultivalue(KeyValueContainerTests<TKey, TValue> testClass, ISortedIndexableKeyMultivalueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey, TValue>> list)
            {
                if (testClass.ran.Next(2) == 0)
                    Execute_SortedMultivalue(testClass, container, list);
                else
                    Execute_IndexableMultivalue(testClass, container, list);
            }

            public override void Execute_SortedMultivalue(KeyValueContainerTests<TKey, TValue> testClass, ISortedKeyMultivalueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey, TValue>> list)
            {
                if (RemoveAll)
                    container.TryRemoveAll(KeyToTryToRemove);
                else
                {
                    bool result = RemoveSpecificKeyValue ? container.TryRemoveKeyValue(KeyToTryToRemove, ValueToTryToRemove) : container.TryRemove(KeyToTryToRemove, WhichOne);
                    VerifySuccess(result);
                }
            }

            public override void Execute_Value(KeyValueContainerTests<TKey, TValue> testClass, IKeyValueContainer<TKey, TValue> container, List<LazinatorComparableKeyValue<TKey, TValue>> list)
            {
                bool result = RemoveSpecificKeyValue ? container.TryRemoveKeyValue(KeyToTryToRemove, ValueToTryToRemove, Comparer<TKey>.Default) : container.TryRemove(KeyToTryToRemove, Comparer<TKey>.Default);
                VerifySuccess(result);
            }
        }
    }
}
