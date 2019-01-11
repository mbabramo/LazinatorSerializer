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
    public enum ValueContainerType
    {
        AvlTree,
        AvlIndexableTree,
        AvlSortedTree,
        AvlSortedIndexableTree
    }

    public class ValueContainerTests<T> : SerializationDeserializationTestBase where T : ILazinator, IComparable<T>
    {

        public IValueContainer<T> GetValueContainer(ValueContainerType containerType)
        {
            switch (containerType)
            {
                case ValueContainerType.AvlTree:
                    return new AvlTree<T>();
                case ValueContainerType.AvlIndexableTree:
                    return new AvlIndexableTree<T>();
                case ValueContainerType.AvlSortedTree:
                    return new AvlSortedTree<T>();
                case ValueContainerType.AvlSortedIndexableTree:
                    return new AvlSortedIndexableTree<T>();
                default:
                    throw new NotSupportedException();
            }
        }

        Random ran;

        [Theory]
        [InlineData(ValueContainerType.AvlTree, 100, 100)]
        public void VerifyValueContainer(ValueContainerType containerType, int numRepetitions, int numInstructions)
        {
            for (int rep = 0; rep < numRepetitions; rep++)
            {
                List<T> list = new List<T>();
                IValueContainer<T> container = GetValueContainer(containerType);
                for (int i = 0; i < numInstructions; i++)
                {
                    int r = ran.Next(100);
                    RandomInstruction instruction;
                    if (r < 25)
                        instruction = new GetValueInstruction();
                    else
                        instruction = new InsertValueInstruction();
                    instruction.Execute(this, container, list);
                }
            }
        }

        public (T item, int firstIndex, int lastIndex)? GetRandomItem(List<T> list)
        {
            if (!list.Any())
                return null;
            int index = ran.Next(0, list.Count());
            T item = list[index];
            int firstIndex, lastIndex;
            GetIndexRange(list, index, out firstIndex, out lastIndex);
            return (item, firstIndex, lastIndex);
        }

        private static void GetIndexRange(List<T> list, int index, out int firstIndex, out int lastIndex)
        {
            T item = list[index];
            firstIndex = index;
            lastIndex = index;
            while (firstIndex > 0 && list[firstIndex - 1].Equals(item))
                firstIndex--;
            while (lastIndex < list.Count() - 1 && list[lastIndex + 1].Equals(item))
                lastIndex++;
        }

        public MultivalueLocationOptions ChooseInsertOption()
        {
            int i = ran.Next(0, 5);
            switch (i)
            {
                case 0:
                    return MultivalueLocationOptions.Any;
                case 2:
                    return MultivalueLocationOptions.First;
                case 3:
                    return MultivalueLocationOptions.Last;
                case 4:
                    return MultivalueLocationOptions.InsertBeforeFirst;
                case 5:
                    return MultivalueLocationOptions.InsertAfterLast;
                default:
                    throw new NotImplementedException();
            }
        }

        public (int index, bool insertedNotReplaced) InsertOrReplaceItem(List<T> list, T item, bool sorted, MultivalueLocationOptions whichOne)
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
                            index = ran.Next(firstIndex, lastIndex + 1);
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
                if (whichOne == MultivalueLocationOptions.InsertAfterLast || whichOne == MultivalueLocationOptions.InsertBeforeFirst)
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

        public int MaxIntValue;

        public T GetRandomValue()
        {
            if (typeof(T) == typeof(WInt))
            {
                int random = ran.Next(0, MaxIntValue);
                WInt w = new WInt(random);
                return (T)(object)w;
            }
            throw new Exception();
        }

        public abstract class RandomInstruction
        {
            public virtual void Execute(ValueContainerTests<T> testClass, IValueContainer<T> container, List<T> list)
            {
                // find a valid container type, and execute
                bool done = false;
                while (!done)
                {
                    int i = testClass.ran.Next(8);
                    switch (i)
                    {
                        case 0:
                            Execute_Value(testClass, container, list);
                            done = true;
                            break;
                        case 1:
                            if (container is IIndexableContainer<T> indexableContainer)
                            {
                                Execute_Indexable(testClass, indexableContainer, list);
                                done = true;
                            }
                            break;
                        case 2:
                            if (container is ISortedContainer<T> sortedContainer)
                            {
                                Execute_Sorted(testClass, sortedContainer, list);
                                done = true;
                            }
                            break;
                        case 3:
                            if (container is IMultivalueContainer<T> multivalueContainer)
                            {
                                Execute_Multivalue(testClass, multivalueContainer, list);
                                done = true;
                            }
                            break;
                        case 4:
                            if (container is ISortedMultivalueContainer<T> sortedMultivalueContainer)
                            {
                                Execute_SortedMultivalue(testClass, sortedMultivalueContainer, list);
                                done = true;
                            }
                            break;
                        case 5:
                            if (container is ISortedIndexableContainer<T> sortedIndexableContainer)
                            {
                                Execute_SortedIndexable(testClass, sortedIndexableContainer, list);
                                done = true;
                            }
                            break;
                        case 6:
                            if (container is IIndexableMultivalueContainer<T> indexableMultivalueContainer)
                            {
                                Execute_IndexableMultivalue(testClass, indexableMultivalueContainer, list);
                                done = true;
                            }
                            break;
                        case 7:
                            if (container is ISortedIndexableMultivalueContainer<T> sortedIndexableMultivalueContainer)
                            {
                                Execute_SortedIndexableMultivalue(testClass, sortedIndexableMultivalueContainer, list);
                                done = true;
                            }
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }
            public abstract void Execute_Value(ValueContainerTests<T> testClass, IValueContainer<T> container, List<T> list);
            public abstract void Execute_Indexable(ValueContainerTests<T> testClass, IIndexableContainer<T> container, List<T> list);
            public abstract void Execute_Sorted(ValueContainerTests<T> testClass, ISortedContainer<T> container, List<T> list);
            public abstract void Execute_Multivalue(ValueContainerTests<T> testClass, IMultivalueContainer<T> container, List<T> list);
            public abstract void Execute_SortedMultivalue(ValueContainerTests<T> testClass, ISortedMultivalueContainer<T> container, List<T> list);
            public abstract void Execute_SortedIndexable(ValueContainerTests<T> testClass, ISortedIndexableContainer<T> container, List<T> list);
            public abstract void Execute_IndexableMultivalue(ValueContainerTests<T> testClass, IIndexableMultivalueContainer<T> container, List<T> list);
            public abstract void Execute_SortedIndexableMultivalue(ValueContainerTests<T> testClass, ISortedIndexableMultivalueContainer<T> container, List<T> list);
            public IComparer<T> C => Comparer<T>.Default;
            public bool Eq(T item, T other)
            {
                return EqualityComparer<T>.Default.Equals(item, other);
            }
            public void AssertEqual(T item, T other)
            {
                Eq(item, other).Should().BeTrue();
            }
            public void AssertNotEqual(T item, T other)
            {
                Eq(item, other).Should().BeFalse();
            }
            public void VerifyExpectedIndex(MultivalueLocationOptions whichOne, (T item, int firstIndex, int lastIndex) listResult, long indexableContainerResult)
            {
                if (whichOne == MultivalueLocationOptions.First || whichOne == MultivalueLocationOptions.InsertBeforeFirst)
                    indexableContainerResult.Should().Be(listResult.firstIndex);
                else if (whichOne == MultivalueLocationOptions.Last)
                    indexableContainerResult.Should().Be(listResult.lastIndex);
                else if (whichOne == MultivalueLocationOptions.InsertAfterLast)
                    indexableContainerResult.Should().Be(listResult.lastIndex + 1);
                else if (whichOne == MultivalueLocationOptions.Any)
                {
                    indexableContainerResult.Should().BeGreaterOrEqualTo(listResult.firstIndex);
                    indexableContainerResult.Should().BeLessOrEqualTo(listResult.lastIndex);
                }
            }
        }

        public class GetValueInstruction : RandomInstruction
        {
            public override void Execute_Indexable(ValueContainerTests<T> testClass, IIndexableContainer<T> container, List<T> list)
            {
                var listResultOrNull = testClass.GetRandomItem(list);
                if (listResultOrNull == null)
                {
                    var findResult = container.Find(default, C);
                    findResult.index.Should().Be(-1);
                    findResult.exists.Should().BeFalse();
                }
                else
                {
                    var listResult = listResultOrNull.Value;
                    var findResult = container.Find(listResult.item, C);
                    VerifyExpectedIndex(MultivalueLocationOptions.Any, listResult, findResult.index);
                    findResult.exists.Should().BeTrue();
                    for (int i = listResult.firstIndex; i <= listResult.lastIndex; i++)
                    {
                        T getAtResult = container.GetAt(i);
                        AssertEqual(getAtResult, listResult.item);
                    }
                }
            }

            public override void Execute_IndexableMultivalue(ValueContainerTests<T> testClass, IIndexableMultivalueContainer<T> container, List<T> list)
            {
                var listResultOrNull = testClass.GetRandomItem(list);
                if (listResultOrNull == null)
                {
                    Execute_Indexable(testClass, container, list);
                }
                else
                {
                    var listResult = listResultOrNull.Value;
                    foreach (MultivalueLocationOptions whichOne in new MultivalueLocationOptions[] { MultivalueLocationOptions.First, MultivalueLocationOptions.Any, MultivalueLocationOptions.Last }) // other options are undefined
                    {
                        var findResult = container.Find(listResult.item, whichOne, C);
                        VerifyExpectedIndex(whichOne, listResult, findResult.index);
                        findResult.exists.Should().BeTrue();
                    }
                }
            }

            public override void Execute_Multivalue(ValueContainerTests<T> testClass, IMultivalueContainer<T> container, List<T> list)
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
                }
            }

            public override void Execute_Sorted(ValueContainerTests<T> testClass, ISortedContainer<T> container, List<T> list)
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

            public override void Execute_SortedIndexable(ValueContainerTests<T> testClass, ISortedIndexableContainer<T> container, List<T> list)
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

            public override void Execute_SortedIndexableMultivalue(ValueContainerTests<T> testClass, ISortedIndexableMultivalueContainer<T> container, List<T> list)
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

            public override void Execute_SortedMultivalue(ValueContainerTests<T> testClass, ISortedMultivalueContainer<T> container, List<T> list)
            {
                var listResultOrNull = testClass.GetRandomItem(list);
                if (listResultOrNull == null)
                {
                    Execute_Multivalue(testClass, container, list);
                }
                else
                {
                    var listResult = listResultOrNull.Value;
                }
            }

            public override void Execute_Value(ValueContainerTests<T> testClass, IValueContainer<T> container, List<T> list)
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
            bool ContainerIsSorted;
            ISortedContainer<T> SortedContainer;
            int Index;
            bool InsertedNotReplaced;

            public override void Execute(ValueContainerTests<T> testClass, IValueContainer<T> container, List<T> list)
            {
                Item = testClass.GetRandomValue();
                WhichOne = testClass.ChooseInsertOption();
                SortedContainer = container as ISortedContainer<T>;
                ContainerIsSorted = container is ISortedContainer<T>;
                (Index, InsertedNotReplaced) = testClass.InsertOrReplaceItem(list, Item, ContainerIsSorted, WhichOne);
                base.Execute(testClass, container, list);
            }

            public override void Execute_Indexable(ValueContainerTests<T> testClass, IIndexableContainer<T> container, List<T> list)
            {
                if (ContainerIsSorted)
                    Execute_SortedIndexable(testClass, (ISortedIndexableContainer<T>)SortedContainer, list);
                else
                {
                    if (InsertedNotReplaced)
                        container.InsertAt(Index, Item);
                    else
                        container.SetAt(Index, Item);
                }
            }

            public override void Execute_IndexableMultivalue(ValueContainerTests<T> testClass, IIndexableMultivalueContainer<T> container, List<T> list)
            {
                if (ContainerIsSorted)
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

            public override void Execute_Multivalue(ValueContainerTests<T> testClass, IMultivalueContainer<T> container, List<T> list)
            {
                if (ContainerIsSorted)
                {
                    bool insertedNotReplaced = container.TryInsert(Item, WhichOne, C);
                    insertedNotReplaced.Should().Be(InsertedNotReplaced);
                }
                else
                {
                    Execute_Value(testClass, container, list);
                }
            }

            public override void Execute_Sorted(ValueContainerTests<T> testClass, ISortedContainer<T> container, List<T> list)
            {
                bool insertedNotReplaced = container.TryInsert(Item);
                insertedNotReplaced.Should().Be(InsertedNotReplaced);
            }

            public override void Execute_SortedIndexable(ValueContainerTests<T> testClass, ISortedIndexableContainer<T> container, List<T> list)
            {
                (long index, bool insertedNotReplaced) = container.InsertGetIndex(Item);
                index.Should().Be(Index);
                insertedNotReplaced.Should().Be(InsertedNotReplaced);
            }

            public override void Execute_SortedIndexableMultivalue(ValueContainerTests<T> testClass, ISortedIndexableMultivalueContainer<T> container, List<T> list)
            {
                (long index, bool insertedNotReplaced) = container.InsertGetIndex(Item, WhichOne);
                index.Should().Be(Index);
                insertedNotReplaced.Should().Be(InsertedNotReplaced);
            }

            public override void Execute_SortedMultivalue(ValueContainerTests<T> testClass, ISortedMultivalueContainer<T> container, List<T> list)
            {
                bool insertedNotReplaced = container.TryInsert(Item, WhichOne);
                insertedNotReplaced.Should().Be(InsertedNotReplaced);
            }

            public override void Execute_Value(ValueContainerTests<T> testClass, IValueContainer<T> container, List<T> list)
            {
                bool insertedNotReplaced = container.TryInsert(Item, C);
                insertedNotReplaced.Should().Be(InsertedNotReplaced);
            }
        }





    }
}
