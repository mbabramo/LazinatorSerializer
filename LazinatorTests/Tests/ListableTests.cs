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
using Lazinator.Collections.BigList;
using System.Diagnostics;
using Lazinator.Collections.Factories;
using System.Collections;

namespace LazinatorTests.Tests
{
    public class ListableTests : SerializationDeserializationTestBase
    {
        public class ListFactoryTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { new LazinatorListFactory<WInt>() };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Theory]
        [ClassData(typeof(ListFactoryTestData))]
        public void Listable_AddingAtEnd(ILazinatorCountableListableFactory<WInt> factory)
        {
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
        [ClassData(typeof(ListFactoryTestData))]
        public void Listable_AddingAtBeginning(ILazinatorCountableListableFactory<WInt> factory)
        {
            ILazinatorCountableListable<WInt> l = factory.CreateCountableListable();
            const int numItems = 500;
            for (int i = 0; i < numItems; i++)
                l.InsertAt(0, i);
            var result = l.Select(x => x.WrappedValue).ToList();
            result.Reverse();
            result.SequenceEqual(Enumerable.Range(0, 100)).Should().BeTrue();
        }

        [Theory]
        [ClassData(typeof(ListFactoryTestData))]
        public void Listable_RandomInsertionsAndDeletions(ILazinatorCountableListableFactory<WInt> factory)
        {
            bool trace = false;
            Random r = new Random(0);
            List<int> o = new List<int>();
            ILazinatorCountableListable<WInt> l = factory.CreateCountableListable();
            const int totalChanges = 1000;
            const int switchToMoreDeletionsAfter = 400; // we'll start mostly with insertions, and then switch to mostly deletions, so that we can delete the entire tree.
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
                    int k = r.Next(0, 999999);
                    if (trace)
                        Debug.WriteLine($"Inserting {k} at index {j}");
                    o.Insert(j, k);
                    l.InsertAt(j, k);
                }
                else
                {
                    int j = r.Next(0, o.Count() - 1); // delete at valid location
                    if (trace)
                        Debug.WriteLine($"Deleting at index {j}");
                    o.RemoveAt(j);
                    l.RemoveAt(j);
                }
                var result = l.Select(x => x.WrappedValue).ToList();
                result.SequenceEqual(o).Should().BeTrue();
            }
        }
    }
}
