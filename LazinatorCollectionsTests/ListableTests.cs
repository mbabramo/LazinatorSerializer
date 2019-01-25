﻿using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using LazinatorCollections;
using LazinatorTests.Examples;
using LazinatorTests.Examples.Collections;
using Lazinator.Core;
using LazinatorTests.Examples.Tuples;
using Xunit;
using Lazinator.Wrappers;
using LazinatorTests.Examples.Structs;
using System.Diagnostics;
using LazinatorCollections.Factories;
using System.Collections;
using LazinatorCollections.Interfaces;
using LazinatorTests.Tests;

namespace LazinatorCollectionsTests
{
    public class ListableTests
    {
        public enum ListFactoryToUse
        {
            LazinatorList,
            LazinatorLinkedList,
            LazinatorSortedList,
            LazinatorSortedListAllowDuplicates,
            LazinatorSortedLinkedList,
            LazinatorSortedLinkedListAllowDuplicates,
            UnbalancedAvlList,
            AvlList,
            AvlListWithAvlIndexableListTree,
            AvlSortedList,
            AvlSortedListAllowDuplicates,
            AvlSortedListWithAvlSortedIndexableListTree,
            AvlSortedListWithAvlSortedIndexableListTreeAllowDuplicates,
            UnbalancedAvlSortedList,
        }

        static ContainerFactory GetListFactory(ListFactoryToUse l)
        {
            switch (l)
            {
                case ListFactoryToUse.LazinatorList:
                    return new ContainerFactory(new ContainerLevel(ContainerType.LazinatorList));
                case ListFactoryToUse.LazinatorLinkedList:
                    return new ContainerFactory(new ContainerLevel(ContainerType.LazinatorLinkedList));
                case ListFactoryToUse.LazinatorSortedList:
                    return new ContainerFactory(new ContainerLevel(ContainerType.LazinatorSortedList));
                case ListFactoryToUse.LazinatorSortedListAllowDuplicates:
                    return new ContainerFactory(new ContainerLevel(ContainerType.LazinatorSortedList, true));
                case ListFactoryToUse.LazinatorSortedLinkedList:
                    return new ContainerFactory(new ContainerLevel(ContainerType.LazinatorSortedLinkedList));
                case ListFactoryToUse.LazinatorSortedLinkedListAllowDuplicates:
                    return new ContainerFactory(new ContainerLevel(ContainerType.LazinatorSortedLinkedList, true));
                case ListFactoryToUse.UnbalancedAvlList:
                    return new ContainerFactory(new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlList, false, long.MaxValue, true),
                        new ContainerLevel(ContainerType.AvlIndexableTree)
                    });
                case ListFactoryToUse.AvlList:
                    return new ContainerFactory(new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlList),
                        new ContainerLevel(ContainerType.AvlIndexableTree)
                    });
                case ListFactoryToUse.AvlListWithAvlIndexableListTree:
                    return new ContainerFactory(new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlList),
                        new ContainerLevel(ContainerType.AvlIndexableListTree),
                        new ContainerLevel(ContainerType.LazinatorList, false, 3)
                    });
                case ListFactoryToUse.UnbalancedAvlSortedList:
                    return new ContainerFactory(new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlSortedList, false, long.MaxValue, true),
                        new ContainerLevel(ContainerType.AvlSortedIndexableTree)
                    });
                case ListFactoryToUse.AvlSortedList:
                    return new ContainerFactory(new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlSortedList),
                        new ContainerLevel(ContainerType.AvlSortedIndexableTree)
                    });
                case ListFactoryToUse.AvlSortedListAllowDuplicates:
                    return new ContainerFactory(new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlSortedList, true),
                        new ContainerLevel(ContainerType.AvlSortedIndexableTree, true)
                    });
                case ListFactoryToUse.AvlSortedListWithAvlSortedIndexableListTree:
                    return new ContainerFactory(new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlSortedList),
                        new ContainerLevel(ContainerType.AvlSortedIndexableListTree),
                        new ContainerLevel(ContainerType.LazinatorSortedList, false, 3)
                    });
                case ListFactoryToUse.AvlSortedListWithAvlSortedIndexableListTreeAllowDuplicates:
                    return new ContainerFactory(new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlSortedList, true),
                        new ContainerLevel(ContainerType.AvlSortedIndexableListTree, true),
                        new ContainerLevel(ContainerType.LazinatorSortedList, true, 3)
                    });
                default:
                    throw new NotImplementedException();
            }
        }

        static (bool isSorted, bool allowsDuplicates) GetSortedInfo(ListFactoryToUse l)
        {
            switch (l)
            {
                case ListFactoryToUse.LazinatorList:
                    return (false, false);
                case ListFactoryToUse.LazinatorLinkedList:
                    return (false, false);
                case ListFactoryToUse.LazinatorSortedList:
                    return (true, false);
                case ListFactoryToUse.LazinatorSortedListAllowDuplicates:
                    return (true, true);
                case ListFactoryToUse.LazinatorSortedLinkedList:
                    return (true, false);
                case ListFactoryToUse.LazinatorSortedLinkedListAllowDuplicates:
                    return (true, true);
                case ListFactoryToUse.AvlList:
                case ListFactoryToUse.AvlListWithAvlIndexableListTree:
                case ListFactoryToUse.UnbalancedAvlList:
                    return (false, false);
                case ListFactoryToUse.AvlSortedList:
                case ListFactoryToUse.AvlSortedListWithAvlSortedIndexableListTree:
                case ListFactoryToUse.UnbalancedAvlSortedList:
                    return (true, false);
                case ListFactoryToUse.AvlSortedListAllowDuplicates:
                case ListFactoryToUse.AvlSortedListWithAvlSortedIndexableListTreeAllowDuplicates:
                    return (true, true);
                default:
                    throw new NotImplementedException();
            }
        }

        [Theory]
        [InlineData(ListFactoryToUse.LazinatorList)]
        [InlineData(ListFactoryToUse.LazinatorLinkedList)]
        [InlineData(ListFactoryToUse.LazinatorSortedList)]
        [InlineData(ListFactoryToUse.LazinatorSortedListAllowDuplicates)]
        [InlineData(ListFactoryToUse.LazinatorSortedLinkedList)]
        [InlineData(ListFactoryToUse.LazinatorSortedLinkedListAllowDuplicates)]
        [InlineData(ListFactoryToUse.AvlList)]
        [InlineData(ListFactoryToUse.AvlListWithAvlIndexableListTree)]
        [InlineData(ListFactoryToUse.UnbalancedAvlList)]
        [InlineData(ListFactoryToUse.AvlSortedList)]
        [InlineData(ListFactoryToUse.UnbalancedAvlSortedList)]
        [InlineData(ListFactoryToUse.AvlSortedListAllowDuplicates)]
        [InlineData(ListFactoryToUse.AvlSortedListWithAvlSortedIndexableListTree)]
        [InlineData(ListFactoryToUse.AvlSortedListWithAvlSortedIndexableListTreeAllowDuplicates)]
        public void Listable_AddingAtEnd(ListFactoryToUse listFactoryToUse)
        {
            var factory = GetListFactory(listFactoryToUse);
            ILazinatorListable<WInt> l = factory.CreatePossiblySortedLazinatorListable<WInt>();
            int numItems = (listFactoryToUse == ListFactoryToUse.UnbalancedAvlList || listFactoryToUse == ListFactoryToUse.UnbalancedAvlSortedList) ? 20 : 1000;
            for (int i = 0; i < numItems; i++)
            {
                l.Add(i);
            }
            var result = l.Select(x => x.WrappedValue).ToList();
            result.SequenceEqual(Enumerable.Range(0, numItems)).Should().BeTrue();
        }

        [Fact]
        public void ListableDEBUG() => Listable_AddingAtBeginning(ListFactoryToUse.AvlListWithAvlIndexableListTree);

        [Theory]
        [InlineData(ListFactoryToUse.LazinatorList)]
        [InlineData(ListFactoryToUse.LazinatorLinkedList)]
        [InlineData(ListFactoryToUse.LazinatorSortedList)]
        [InlineData(ListFactoryToUse.LazinatorSortedListAllowDuplicates)]
        [InlineData(ListFactoryToUse.LazinatorSortedLinkedList)]
        [InlineData(ListFactoryToUse.LazinatorSortedLinkedListAllowDuplicates)]
        [InlineData(ListFactoryToUse.AvlList)]
        [InlineData(ListFactoryToUse.AvlListWithAvlIndexableListTree)]
        [InlineData(ListFactoryToUse.UnbalancedAvlList)]
        [InlineData(ListFactoryToUse.AvlSortedList)]
        [InlineData(ListFactoryToUse.UnbalancedAvlSortedList)]
        [InlineData(ListFactoryToUse.AvlSortedListAllowDuplicates)]
        [InlineData(ListFactoryToUse.AvlSortedListWithAvlSortedIndexableListTree)]
        [InlineData(ListFactoryToUse.AvlSortedListWithAvlSortedIndexableListTreeAllowDuplicates)]
        public void Listable_AddingAtBeginning(ListFactoryToUse listFactoryToUse)
        {
            var factory = GetListFactory(listFactoryToUse);
            ILazinatorListable<WInt> l = factory.CreatePossiblySortedLazinatorListable<WInt>();
            int numItems = (listFactoryToUse == ListFactoryToUse.UnbalancedAvlList || listFactoryToUse == ListFactoryToUse.UnbalancedAvlSortedList) ? 20 : 500;
            for (int i = 0; i < numItems; i++)
                l.InsertAtIndex(0, i);
            l.Any().Should().BeTrue();
            l.First().Should().Be(numItems - 1);
            l.FirstOrDefault().Should().Be(numItems - 1);
            l.Last().Should().Be(0);
            l.LastOrDefault().Should().Be(0);
            var result = l.Select(x => x.WrappedValue).ToList();
            result.Reverse();
            result.SequenceEqual(Enumerable.Range(0, numItems)).Should().BeTrue();
        }

        [Theory]
        [InlineData(ListFactoryToUse.LazinatorList)]
        [InlineData(ListFactoryToUse.LazinatorLinkedList)]
        [InlineData(ListFactoryToUse.LazinatorSortedList)]
        [InlineData(ListFactoryToUse.LazinatorSortedListAllowDuplicates)]
        [InlineData(ListFactoryToUse.LazinatorSortedLinkedList)]
        [InlineData(ListFactoryToUse.LazinatorSortedLinkedListAllowDuplicates)]
        [InlineData(ListFactoryToUse.AvlList)]
        [InlineData(ListFactoryToUse.AvlListWithAvlIndexableListTree)]
        [InlineData(ListFactoryToUse.UnbalancedAvlList)]
        [InlineData(ListFactoryToUse.AvlSortedList)]
        [InlineData(ListFactoryToUse.UnbalancedAvlSortedList)]
        [InlineData(ListFactoryToUse.AvlSortedListAllowDuplicates)]
        [InlineData(ListFactoryToUse.AvlSortedListWithAvlSortedIndexableListTree)]
        [InlineData(ListFactoryToUse.AvlSortedListWithAvlSortedIndexableListTreeAllowDuplicates)]
        public void Listable_Empty(ListFactoryToUse listFactoryToUse)
        {
            var factory = GetListFactory(listFactoryToUse);
            ILazinatorListable<WInt> l = factory.CreatePossiblySortedLazinatorListable<WInt>();
            l.Any().Should().BeFalse();
            l.FirstOrDefault().Should().Be(default(WInt));
            l.LastOrDefault().Should().Be(default(WInt));
        }

        [Theory]
        [InlineData(ListFactoryToUse.LazinatorList)]
        [InlineData(ListFactoryToUse.LazinatorLinkedList)]
        [InlineData(ListFactoryToUse.LazinatorSortedList)]
        [InlineData(ListFactoryToUse.LazinatorSortedListAllowDuplicates)]
        [InlineData(ListFactoryToUse.LazinatorSortedLinkedList)]
        [InlineData(ListFactoryToUse.LazinatorSortedLinkedListAllowDuplicates)]
        [InlineData(ListFactoryToUse.AvlList)]
        [InlineData(ListFactoryToUse.AvlListWithAvlIndexableListTree)]
        [InlineData(ListFactoryToUse.UnbalancedAvlList)]
        [InlineData(ListFactoryToUse.AvlSortedList)]
        [InlineData(ListFactoryToUse.UnbalancedAvlSortedList)]
        [InlineData(ListFactoryToUse.AvlSortedListAllowDuplicates)]
        [InlineData(ListFactoryToUse.AvlSortedListWithAvlSortedIndexableListTree)]
        [InlineData(ListFactoryToUse.AvlSortedListWithAvlSortedIndexableListTreeAllowDuplicates)]
        public void Listable_EmptyAfterNotEmpty(ListFactoryToUse listFactoryToUse)
        {
            var factory = GetListFactory(listFactoryToUse);
            ILazinatorListable<WInt> l = factory.CreatePossiblySortedLazinatorListable<WInt>();
            l.Add(1);
            l.RemoveAtIndex(0);
            l.Any().Should().BeFalse();
            l.FirstOrDefault().Should().Be(default(WInt));
            l.LastOrDefault().Should().Be(default(WInt));
        }

        [Theory]
        [InlineData(ListFactoryToUse.LazinatorList)]
        [InlineData(ListFactoryToUse.LazinatorLinkedList)]
        [InlineData(ListFactoryToUse.LazinatorSortedList)]
        [InlineData(ListFactoryToUse.LazinatorSortedListAllowDuplicates)]
        [InlineData(ListFactoryToUse.LazinatorSortedLinkedList)]
        [InlineData(ListFactoryToUse.LazinatorSortedLinkedListAllowDuplicates)]
        [InlineData(ListFactoryToUse.AvlList)]
        [InlineData(ListFactoryToUse.AvlListWithAvlIndexableListTree)]
        [InlineData(ListFactoryToUse.UnbalancedAvlList)]
        [InlineData(ListFactoryToUse.AvlSortedList)]
        [InlineData(ListFactoryToUse.UnbalancedAvlSortedList)]
        [InlineData(ListFactoryToUse.AvlSortedListAllowDuplicates)]
        [InlineData(ListFactoryToUse.AvlSortedListWithAvlSortedIndexableListTree)]
        [InlineData(ListFactoryToUse.AvlSortedListWithAvlSortedIndexableListTreeAllowDuplicates)]
        public void Listable_CopyToArray(ListFactoryToUse listFactoryToUse)
        {

            var factory = GetListFactory(listFactoryToUse);
            ILazinatorListable<WInt> l = factory.CreatePossiblySortedLazinatorListable<WInt>();
            int numItems = (listFactoryToUse == ListFactoryToUse.UnbalancedAvlList || listFactoryToUse == ListFactoryToUse.UnbalancedAvlSortedList) ? 20 : 1000;
            for (int i = 0; i < numItems; i++)
                l.Add(i);
            WInt[] x = new WInt[numItems + 10];
            l.CopyTo(x, 10);
            for (int i = 0; i < numItems; i++)
                x[i + 10].Should().Be(i);
        }

        [Theory]
        [InlineData(ListFactoryToUse.LazinatorList)]
        [InlineData(ListFactoryToUse.LazinatorLinkedList)]
        [InlineData(ListFactoryToUse.LazinatorSortedList)]
        [InlineData(ListFactoryToUse.LazinatorSortedListAllowDuplicates)]
        [InlineData(ListFactoryToUse.LazinatorSortedLinkedList)]
        [InlineData(ListFactoryToUse.LazinatorSortedLinkedListAllowDuplicates)]
        [InlineData(ListFactoryToUse.AvlList)]
        [InlineData(ListFactoryToUse.AvlListWithAvlIndexableListTree)]
        [InlineData(ListFactoryToUse.UnbalancedAvlList)]
        [InlineData(ListFactoryToUse.AvlSortedList)]
        [InlineData(ListFactoryToUse.UnbalancedAvlSortedList)]
        [InlineData(ListFactoryToUse.AvlSortedListAllowDuplicates)]
        [InlineData(ListFactoryToUse.AvlSortedListWithAvlSortedIndexableListTree)]
        [InlineData(ListFactoryToUse.AvlSortedListWithAvlSortedIndexableListTreeAllowDuplicates)]
        public void Listable_InsertAndRemoveWork(ListFactoryToUse listFactoryToUse)
        {
            var factory = GetListFactory(listFactoryToUse);
            (int numModifications, double removalProbability)[] phases = new (int numModifications, double removalProbability)[] { (2, 0.25), (4, 0.6), (8, 0.25), (16, 0.6), (32, 0.25), (64, 0.6), };
            Random r = new Random(0);
            const int numRepetitions = 10;
            for (int repetition = 0; repetition < numRepetitions; repetition++)
            {
                int i = 0;
                List<int> list = new List<int>();
                ILazinatorListable<WInt> l = factory.CreatePossiblySortedLazinatorListable<WInt>();
                foreach (var phase in phases)
                {
                    for (int m = 0; m < phase.numModifications; m++)
                    {
                        ListModificationInstruction instruction = new ListModificationInstruction(r, list, i++, phase.removalProbability);
                        instruction.Execute(list);
                        instruction.Execute(l);
                        if (!list.SequenceEqual(l.Select(x => x.WrappedValue)))
                            throw new Exception("Match failure.");
                    }
                }
            }
        }

        class ListModificationInstruction
        {
            public bool Remove;
            public int Index;
            public int ValueToAdd;

            public ListModificationInstruction(Random r, IList<int> list, int valueToAdd, double removalProbability)
            {
                if (!list.Any())
                {
                    Remove = false;
                    Index = 0;
                }
                else
                {
                    Remove = r.NextDouble() < removalProbability;
                    Index = r.Next(list.Count());
                    if (!Remove)
                        ValueToAdd = valueToAdd;
                }
            }

            public void Execute(IList<int> list)
            {
                if (Remove)
                    list.RemoveAt(Index);
                else
                    list.Insert(Index, ValueToAdd);
            }

            public void Execute(IList<WInt> list)
            {
                if (Remove)
                    list.RemoveAt(Index);
                else
                    list.Insert(Index, ValueToAdd);
            }
        }

        [Theory]
        [InlineData(ListFactoryToUse.LazinatorSortedList, 200, 1)]
        [InlineData(ListFactoryToUse.LazinatorSortedListAllowDuplicates, 200, 1)]
        [InlineData(ListFactoryToUse.LazinatorSortedLinkedList, 200, 1)]
        [InlineData(ListFactoryToUse.LazinatorSortedLinkedListAllowDuplicates, 200, 1)]
        [InlineData(ListFactoryToUse.AvlSortedList, 200, 1)]
        [InlineData(ListFactoryToUse.UnbalancedAvlSortedList, 200, 1)]
        [InlineData(ListFactoryToUse.AvlSortedListAllowDuplicates, 200, 1)]
        [InlineData(ListFactoryToUse.LazinatorList, 200, 1)]
        [InlineData(ListFactoryToUse.LazinatorLinkedList, 200, 1)]
        [InlineData(ListFactoryToUse.AvlList, 200, 1)]
        [InlineData(ListFactoryToUse.AvlListWithAvlIndexableListTree, 200, 1)]
        [InlineData(ListFactoryToUse.UnbalancedAvlList, 200, 1)]
        [InlineData(ListFactoryToUse.AvlSortedListWithAvlSortedIndexableListTree, 200, 1)]
        [InlineData(ListFactoryToUse.AvlSortedListWithAvlSortedIndexableListTreeAllowDuplicates, 200, 1)]
        [InlineData(ListFactoryToUse.LazinatorSortedList, 15, 20)]
        [InlineData(ListFactoryToUse.LazinatorSortedListAllowDuplicates, 15, 20)]
        [InlineData(ListFactoryToUse.LazinatorSortedLinkedList, 15, 20)]
        [InlineData(ListFactoryToUse.LazinatorSortedLinkedListAllowDuplicates, 15, 20)]
        [InlineData(ListFactoryToUse.AvlSortedList, 15, 20)]
        [InlineData(ListFactoryToUse.UnbalancedAvlSortedList, 15, 20)]
        [InlineData(ListFactoryToUse.AvlSortedListAllowDuplicates, 15, 20)]
        [InlineData(ListFactoryToUse.LazinatorList, 15, 20)]
        [InlineData(ListFactoryToUse.LazinatorLinkedList, 15, 20)]
        [InlineData(ListFactoryToUse.AvlList, 15, 20)]
        [InlineData(ListFactoryToUse.AvlListWithAvlIndexableListTree, 15, 20)]
        [InlineData(ListFactoryToUse.UnbalancedAvlList, 15, 20)]
        [InlineData(ListFactoryToUse.AvlSortedListWithAvlSortedIndexableListTree, 15, 20)]
        [InlineData(ListFactoryToUse.AvlSortedListWithAvlSortedIndexableListTreeAllowDuplicates, 15, 20)]
        public void Listable_Sorted(ListFactoryToUse listFactoryToUse, int totalChanges, int repetitions)
        {
            (bool isSorted, bool allowsDuplicates) = GetSortedInfo(listFactoryToUse);
            var factory = GetListFactory(listFactoryToUse);
            bool trace = false;
            bool testIntermediateValues = true;  // DEBUG
            Random r = new Random(0);
            ILazinatorListable<WInt> l = factory.CreatePossiblySortedLazinatorListable<WInt>();
            List<int> o = new List<int>();
            for (int repetition = 0; repetition < repetitions; repetition++)
            {
                l.Clear();
                o.Clear();
                int switchToMoreDeletionsAfter = (int) (totalChanges * (r.Next(2) == 0 ? .4 : .6));

                int maxValue = isSorted ? totalChanges / 2 : 999999; // fewer possibilities if sorted so we can get some duplicates
                for (int i = 0; i < totalChanges; i++)
                {
                    if (o.Count == 0 || r.Next(0, 100) < (i > switchToMoreDeletionsAfter ? 30 : 70))
                    {
                        int j;
                        if (r.Next(0, 10) == 0)
                            j = 0;
                        else if (r.Next(0, 10) == 0 && o.Count() != 0)
                            j = o.Count() - 1;
                        else
                            j = r.Next(0, o.Count()); // insert at a valid location
                        int k = r.Next(0, maxValue);
                        if (trace)
                            Debug.WriteLine($"Inserting {k} at index {j}");
                        if (isSorted)
                        {
                            if (allowsDuplicates || !o.Contains(k))
                            {
                                int index = o.BinarySearch(k);
                                if (index < 0)
                                    o.Insert(~index, k); // bitwise complement
                                else
                                    o.Insert(index, k);
                            }
                            (l as ILazinatorSorted<WInt>).InsertOrReplace(k);
                        }
                        else
                        {
                            o.Insert(j, k);
                            l.InsertAtIndex(j, k);
                        }
                    }
                    else
                    {
                        int j = r.Next(0, o.Count() - 1); // delete at valid location
                        if (trace)
                            Debug.WriteLine($"Deleting at index {j}");
                        if (isSorted)
                        {
                            int value = o[j];
                            o.RemoveAt(j);
                            (l as ILazinatorSorted<WInt>).TryRemove(value);
                        }
                        else
                        {
                            o.RemoveAt(j);
                            l.RemoveAtIndex(j);
                        }
                    }
                    if ((testIntermediateValues || i == totalChanges - 1) && o.Any())
                    {
                        var result = l.Select(x => x.WrappedValue).ToList();
                        result.SequenceEqual(o).Should().BeTrue();
                        if (isSorted)
                        {
                            (long location, bool exists) findResult;
                            for (int index = 0; index < o.Count(); index++)
                            {
                                int value = o[index];
                                int expectedIndex = index;
                                while (expectedIndex > 0 && o[expectedIndex - 1] == value)
                                    expectedIndex--; // first key with same value

                                findResult = (l as ILazinatorSorted<WInt>).FindIndex(value);
                                findResult.exists.Should().BeTrue();
                                findResult.location.Should().Be(expectedIndex);
                                // previous value, if not in list, should produce same location
                                if (!o.Contains(value - 1))
                                {
                                    findResult = (l as ILazinatorSorted<WInt>).FindIndex(value - 1);
                                    findResult.exists.Should().BeFalse();
                                    findResult.location.Should().Be(expectedIndex);
                                }
                            }
                            findResult = (l as ILazinatorSorted<WInt>).FindIndex(o.Max() + 1);
                            findResult.exists.Should().BeFalse();
                            findResult.location.Should().Be(o.Count()); // i.e., location after last one
                        }
                    }
                }
            }
        }
    }
}