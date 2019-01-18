using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Lazinator.Collections;
using LazinatorTests.Examples;
using LazinatorTests.Examples.Collections;
using Lazinator.Core;
using LazinatorTests.Examples.Tuples;
using Xunit;
using Lazinator.Wrappers;
using LazinatorTests.Examples.Structs;
using System.Diagnostics;
using Lazinator.Collections.Factories;
using System.Collections;
using Lazinator.Collections.Interfaces;

namespace LazinatorTests.Tests
{
    public class ListableTests : SerializationDeserializationTestBase
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
            AvlSortedList,
            AvlSortedListAllowDuplicates,
            UnbalancedAvlSortedList,
        }

        static ILazinatorListableFactory<WInt> GetListFactory(ListFactoryToUse l)
        {
            switch (l)
            {
                case ListFactoryToUse.LazinatorList:
                    return new LazinatorListFactory<WInt>();
                case ListFactoryToUse.LazinatorLinkedList:
                    return new LazinatorLinkedListFactory<WInt>();
                case ListFactoryToUse.LazinatorSortedList:
                    return new LazinatorSortedListFactory<WInt>();
                case ListFactoryToUse.LazinatorSortedListAllowDuplicates:
                    return new LazinatorSortedListFactory<WInt>() { AllowDuplicates = true };
                case ListFactoryToUse.LazinatorSortedLinkedList:
                    return new LazinatorSortedLinkedListFactory<WInt>();
                case ListFactoryToUse.LazinatorSortedLinkedListAllowDuplicates:
                    return new LazinatorSortedLinkedListFactory<WInt>() { AllowDuplicates = true };
                case ListFactoryToUse.UnbalancedAvlList:
                    return new AvlListFactory<WInt>(new AvlIndexableTreeFactory<WInt>() { Unbalanced = true });
                case ListFactoryToUse.AvlList:
                    return new AvlListFactory<WInt>(new AvlIndexableTreeFactory<WInt>());
                case ListFactoryToUse.UnbalancedAvlSortedList:
                    return new AvlSortedListFactory<WInt>(false, new AvlSortedIndexableTreeFactory<WInt>(false, true));
                case ListFactoryToUse.AvlSortedList:
                    return new AvlSortedListFactory<WInt>(false, new AvlSortedIndexableTreeFactory<WInt>(false, false));
                case ListFactoryToUse.AvlSortedListAllowDuplicates:
                    return new AvlSortedListFactory<WInt>(true, new AvlSortedIndexableTreeFactory<WInt>(true, false));
                default:
                    throw new NotImplementedException();
            }
        }

        static (bool isSorted, bool allowsDuplicates) GetSortedInfo(ListFactoryToUse l)
        {
            switch (l)
            {
                case ListFactoryToUse.LazinatorList:
                    return (false, true);
                case ListFactoryToUse.LazinatorLinkedList:
                    return (false, true);
                case ListFactoryToUse.LazinatorSortedList:
                    return (true, false);
                case ListFactoryToUse.LazinatorSortedListAllowDuplicates:
                    return (true, true);
                case ListFactoryToUse.LazinatorSortedLinkedList:
                    return (true, false);
                case ListFactoryToUse.LazinatorSortedLinkedListAllowDuplicates:
                    return (true, true);
                case ListFactoryToUse.AvlList:
                case ListFactoryToUse.UnbalancedAvlList:
                    return (false, true);
                case ListFactoryToUse.AvlSortedList:
                case ListFactoryToUse.UnbalancedAvlSortedList:
                    return (true, false);
                case ListFactoryToUse.AvlSortedListAllowDuplicates:
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
        [InlineData(ListFactoryToUse.UnbalancedAvlList)]
        [InlineData(ListFactoryToUse.AvlSortedList)]
        [InlineData(ListFactoryToUse.UnbalancedAvlSortedList)]
        [InlineData(ListFactoryToUse.AvlSortedListAllowDuplicates)]
        public void Listable_AddingAtEnd(ListFactoryToUse listFactoryToUse)
        {
            var factory = GetListFactory(listFactoryToUse);
            ILazinatorListable<WInt> l = factory.CreateListable();
            int numItems = (listFactoryToUse == ListFactoryToUse.UnbalancedAvlList || listFactoryToUse == ListFactoryToUse.UnbalancedAvlSortedList) ? 20 : 1000;
            for (int i = 0; i < numItems; i++)
            {
                l.Add(i);
            }
            var result = l.Select(x => x.WrappedValue).ToList();
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
        [InlineData(ListFactoryToUse.UnbalancedAvlList)]
        [InlineData(ListFactoryToUse.AvlSortedList)]
        [InlineData(ListFactoryToUse.UnbalancedAvlSortedList)]
        [InlineData(ListFactoryToUse.AvlSortedListAllowDuplicates)]
        public void Listable_AddingAtBeginning(ListFactoryToUse listFactoryToUse)
        {
            var factory = GetListFactory(listFactoryToUse);
            ILazinatorListable<WInt> l = factory.CreateListable();
            int numItems = (listFactoryToUse == ListFactoryToUse.UnbalancedAvlList || listFactoryToUse == ListFactoryToUse.UnbalancedAvlSortedList) ? 20 : 500;
            for (int i = 0; i < numItems; i++)
                l.InsertAt(0, i);
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
        [InlineData(ListFactoryToUse.UnbalancedAvlList)]
        [InlineData(ListFactoryToUse.AvlSortedList)]
        [InlineData(ListFactoryToUse.UnbalancedAvlSortedList)]
        [InlineData(ListFactoryToUse.AvlSortedListAllowDuplicates)]
        public void Listable_Empty(ListFactoryToUse listFactoryToUse)
        {
            var factory = GetListFactory(listFactoryToUse);
            ILazinatorListable<WInt> l = factory.CreateListable();
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
        [InlineData(ListFactoryToUse.UnbalancedAvlList)]
        [InlineData(ListFactoryToUse.AvlSortedList)]
        [InlineData(ListFactoryToUse.UnbalancedAvlSortedList)]
        [InlineData(ListFactoryToUse.AvlSortedListAllowDuplicates)]
        public void Listable_EmptyAfterNotEmpty(ListFactoryToUse listFactoryToUse)
        {
            var factory = GetListFactory(listFactoryToUse);
            ILazinatorListable<WInt> l = factory.CreateListable();
            l.Add(1);
            l.RemoveAt(0);
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
        [InlineData(ListFactoryToUse.UnbalancedAvlList)]
        [InlineData(ListFactoryToUse.AvlSortedList)]
        [InlineData(ListFactoryToUse.UnbalancedAvlSortedList)]
        [InlineData(ListFactoryToUse.AvlSortedListAllowDuplicates)]
        public void Listable_CopyToArray(ListFactoryToUse listFactoryToUse)
        {

            var factory = GetListFactory(listFactoryToUse);
            ILazinatorListable<WInt> l = factory.CreateListable();
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
        [InlineData(ListFactoryToUse.UnbalancedAvlList)]
        [InlineData(ListFactoryToUse.AvlSortedList)]
        [InlineData(ListFactoryToUse.UnbalancedAvlSortedList)]
        [InlineData(ListFactoryToUse.AvlSortedListAllowDuplicates)]
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
                ILazinatorListable<WInt> l = factory.CreateListable();
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
        [InlineData(ListFactoryToUse.UnbalancedAvlList, 200, 1)]
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
        [InlineData(ListFactoryToUse.UnbalancedAvlList, 15, 20)]
        public void Listable_Sorted(ListFactoryToUse listFactoryToUse, int totalChanges, int repetitions)
        {
            (bool isSorted, bool allowsDuplicates) = GetSortedInfo(listFactoryToUse);
            var factory = GetListFactory(listFactoryToUse);
            bool trace = false;
            bool testIntermediateValues = false; 
            Random r = new Random(0);
            ILazinatorListable<WInt> l = factory.CreateListable();
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
                            (l as ILazinatorSorted<WInt>).InsertGetIndex(k);
                        }
                        else
                        {
                            o.Insert(j, k);
                            l.InsertAt(j, k);
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
                            l.RemoveAt(j);
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

                                findResult = (l as ILazinatorSorted<WInt>).Find(value);
                                findResult.exists.Should().BeTrue();
                                findResult.location.Should().Be(expectedIndex);
                                // previous value, if not in list, should produce same location
                                if (!o.Contains(value - 1))
                                {
                                    findResult = (l as ILazinatorSorted<WInt>).Find(value - 1);
                                    findResult.exists.Should().BeFalse();
                                    findResult.location.Should().Be(expectedIndex);
                                }
                            }
                            findResult = (l as ILazinatorSorted<WInt>).Find(o.Max() + 1);
                            findResult.exists.Should().BeFalse();
                            findResult.location.Should().Be(o.Count()); // i.e., location after last one
                        }
                    }
                }
            }
        }
    }
}
