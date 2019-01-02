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

namespace LazinatorTests.Tests
{
    public class ListableTests : SerializationDeserializationTestBase
    {
        public enum ListFactoryToUse
        {
            LazinatorList,
            SortedLazinatorList,
            SortedLazinatorListWithDuplicates,
            AvlList,
            AvlSortedList,
            AvlSortedListWithDuplicates,
            
        }

        static ILazinatorCountableListableFactory<WInt> GetListFactory(ListFactoryToUse l)
        {
            switch (l)
            {
                case ListFactoryToUse.LazinatorList:
                    return new LazinatorListFactory<WInt>();
                case ListFactoryToUse.SortedLazinatorList:
                    return new SortedLazinatorListFactory<WInt>();
                case ListFactoryToUse.SortedLazinatorListWithDuplicates:
                    return new SortedLazinatorListWithDuplicatesFactory<WInt>();
                case ListFactoryToUse.AvlList:
                    return new AvlListFactory<WInt>();
                case ListFactoryToUse.AvlSortedList:
                    return new AvlSortedListFactory<WInt>();
                case ListFactoryToUse.AvlSortedListWithDuplicates:
                    return new AvlSortedListWithDuplicatesFactory<WInt>();
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
                case ListFactoryToUse.SortedLazinatorList:
                    return (true, false);
                case ListFactoryToUse.SortedLazinatorListWithDuplicates:
                    return (true, true);
                case ListFactoryToUse.AvlList:
                    return (false, true);
                case ListFactoryToUse.AvlSortedList:
                    return (true, false);
                case ListFactoryToUse.AvlSortedListWithDuplicates:
                    return (true, true);
                default:
                    throw new NotImplementedException();
            }
        }

        [Theory]
        [InlineData(ListFactoryToUse.LazinatorList)]
        [InlineData(ListFactoryToUse.SortedLazinatorList)]
        [InlineData(ListFactoryToUse.SortedLazinatorListWithDuplicates)]
        [InlineData(ListFactoryToUse.AvlList)]
        [InlineData(ListFactoryToUse.AvlSortedList)]
        [InlineData(ListFactoryToUse.AvlSortedListWithDuplicates)]
        public void Listable_AddingAtEnd(ListFactoryToUse listFactoryToUse)
        {
            var factory = GetListFactory(listFactoryToUse);
            ILazinatorCountableListable<WInt> l = factory.CreateCountableListable();
            const int numItems = 500;
            for (int i = 0; i < numItems; i++)
            {
                l.Add(i);
            }
            var result = l.Select(x => x.WrappedValue).ToList();
            result.SequenceEqual(Enumerable.Range(0, numItems)).Should().BeTrue();
        }

        [Theory]
        [InlineData(ListFactoryToUse.LazinatorList)]
        [InlineData(ListFactoryToUse.SortedLazinatorList)]
        [InlineData(ListFactoryToUse.SortedLazinatorListWithDuplicates)]
        [InlineData(ListFactoryToUse.AvlList)]
        [InlineData(ListFactoryToUse.AvlSortedList)]
        [InlineData(ListFactoryToUse.AvlSortedListWithDuplicates)]
        public void Listable_AddingAtBeginning(ListFactoryToUse listFactoryToUse)
        {
            var factory = GetListFactory(listFactoryToUse);
            ILazinatorCountableListable<WInt> l = factory.CreateCountableListable();
            const int numItems = 2; // DEBUG 500 ;
            for (int i = 0; i < numItems; i++)
                l.InsertAt(0, i);
            var result = l.Select(x => x.WrappedValue).ToList();
            result.Reverse();
            result.SequenceEqual(Enumerable.Range(0, numItems)).Should().BeTrue();
        }


        [Theory]
        [InlineData(ListFactoryToUse.LazinatorList)]
        [InlineData(ListFactoryToUse.SortedLazinatorList)]
        [InlineData(ListFactoryToUse.SortedLazinatorListWithDuplicates)]
        [InlineData(ListFactoryToUse.AvlList)]
        [InlineData(ListFactoryToUse.AvlSortedList)]
        [InlineData(ListFactoryToUse.AvlSortedListWithDuplicates)]
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
                ILazinatorCountableListable<WInt> l = factory.CreateCountableListable();
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
        [InlineData(ListFactoryToUse.SortedLazinatorList, 200, 1)]
        [InlineData(ListFactoryToUse.SortedLazinatorListWithDuplicates, 200, 1)]
        [InlineData(ListFactoryToUse.AvlSortedList, 200, 1)]
        [InlineData(ListFactoryToUse.AvlSortedListWithDuplicates, 200, 1)]
        [InlineData(ListFactoryToUse.LazinatorList, 200, 1)]
        [InlineData(ListFactoryToUse.AvlList, 200, 1)]
        [InlineData(ListFactoryToUse.SortedLazinatorList, 15, 20)]
        [InlineData(ListFactoryToUse.SortedLazinatorListWithDuplicates, 15, 20)]
        [InlineData(ListFactoryToUse.AvlSortedList, 15, 20)]
        [InlineData(ListFactoryToUse.AvlSortedListWithDuplicates, 15, 20)]
        [InlineData(ListFactoryToUse.LazinatorList, 15, 20)]
        [InlineData(ListFactoryToUse.AvlList, 15, 20)]
        public void Listable_Sortable(ListFactoryToUse listFactoryToUse, int totalChanges, int repetitions)
        {
            (bool isSorted, bool allowsDuplicates) = GetSortedInfo(listFactoryToUse);
            var factory = GetListFactory(listFactoryToUse);
            bool trace = false;
            bool testIntermediateValues = false;
            Random r = new Random(0);
            ILazinatorCountableListable<WInt> l = factory.CreateCountableListable();
            List<int> o = new List<int>();
            for (int repetition = 0; repetition < repetitions; repetition++)
            {
                l.Clear();
                o.Clear();
                int switchToMoreDeletionsAfter = (int) (totalChanges * (r.Next(2) == 0 ? .4 : .6));

                int maxValue = isSorted ? totalChanges / 2 : 999999; // fewer possibilities if sorted so we can get some duplicates
                for (int i = 0; i < totalChanges; i++)
                {
                    //if (i > 0 && l is Lazinator.Collections.Avl.AvlList<WInt> DEBUG)
                    //{
                    //    DEBUG.UnderlyingTree.Root.Print("", false);
                    //    Debug.WriteLine($"");
                    //}
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
                            (l as ILazinatorSortable<WInt>).InsertSorted(k);
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
                            (l as ILazinatorSortable<WInt>).RemoveSorted(value);
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
                                    expectedIndex--; // earlier key with same value
                                findResult = (l as ILazinatorSortable<WInt>).FindSorted(value);
                                findResult.exists.Should().BeTrue();
                                findResult.location.Should().Be(expectedIndex);
                                // previous value, if not in list, should produce same location
                                if (!o.Contains(value - 1))
                                {
                                    findResult = (l as ILazinatorSortable<WInt>).FindSorted(value - 1);
                                    findResult.exists.Should().BeFalse();
                                    findResult.location.Should().Be(expectedIndex);
                                }
                            }
                            findResult = (l as ILazinatorSortable<WInt>).FindSorted(o.Max() + 1);
                            findResult.exists.Should().BeFalse();
                            findResult.location.Should().Be(o.Count()); // i.e., location after last one
                        }
                    }
                }
            }
        }
    }
}
