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
    public class ValueContainerTests<T> : SerializationDeserializationTestBase where T : ILazinator, IComparable<T>
    {
        Random r;

        public (T item, int firstIndex, int lastIndex)? GetRandomItem(List<T> list)
        {
            if (!list.Any())
                return null;
            int index = r.Next(0, list.Count());
            T item = list[index];
            int firstIndex = index, lastIndex = index;
            while (firstIndex > 0 && list[firstIndex - 1].Equals(item))
                firstIndex--;
            while (lastIndex < list.Count() - 1 && list[lastIndex + 1].Equals(item))
                lastIndex++;
            return (item, firstIndex, lastIndex);
        }



        public int MaxIntValue;

        public T GetRandomValue()
        {
            if (typeof(T) == typeof(WInt))
            {
                int random = r.Next(0, MaxIntValue);
                WInt w = new WInt(random);
                return (T)(object)w;
            }
            throw new Exception();
        }

        public abstract class RandomInstruction
        {
            public void Execute(ValueContainerTests<T> testClass, IValueContainer<T> container, List<T> list)
            {
                // find a valid container type, and execute
                bool done = false;
                while (!done)
                {
                    int i = testClass.r.Next(8);
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

        public class GetValue : RandomInstruction
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

    


    }
}
