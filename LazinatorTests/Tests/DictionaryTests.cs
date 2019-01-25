using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using FluentAssertions;
using LazinatorCollections.Dictionary;
using Lazinator.Wrappers;
using Lazinator.Core;
using LazinatorCollections;
using LazinatorCollections.Avl;
using LazinatorTests.Examples;
using LazinatorCollections.Tuples;
using LazinatorCollections.Factories;
using Lazinator.Examples.Structs;
using LazinatorCollections.Interfaces;

namespace LazinatorTests.Tests
{

    public class DictionaryTests
    {
        public enum DictionaryToUse
        {
            LazinatorDictionary,
            AvlDictionary,
            AvlDictionaryMultiValue,
            AvlDictionaryWithUnderlyingIndexableListTree,
            AvlSortedDictionary,
            AvlSortedIndexableDictionary,
            AvlSortedDictionaryMultiValue,
            AvlSortedDictionaryWithUnderlyingIndexableListTree
        }

        public ContainerFactory GetDictionaryFactory<TKey, TValue>(DictionaryToUse dictionaryToUse) where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
        {
            switch (dictionaryToUse)
            {
                case DictionaryToUse.LazinatorDictionary:
                    return new ContainerFactory(new ContainerLevel(ContainerType.LazinatorDictionary));
                case DictionaryToUse.AvlDictionary:
                    return new ContainerFactory(new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlDictionary, false),
                        new ContainerLevel(ContainerType.AvlSortedKeyValueTree, true)
                    });
                case DictionaryToUse.AvlDictionaryMultiValue:
                    return new ContainerFactory(new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlDictionary, true),
                        new ContainerLevel(ContainerType.AvlSortedKeyValueTree, true)
                    });
                case DictionaryToUse.AvlDictionaryWithUnderlyingIndexableListTree:
                    return new ContainerFactory(new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlDictionary, false),
                        new ContainerLevel(ContainerType.AvlSortedKeyValueTree, true),
                        new ContainerLevel(ContainerType.AvlIndexableListTree, true),
                        new ContainerLevel(ContainerType.LazinatorList, true)
                    });
                case DictionaryToUse.AvlSortedDictionary:
                    return new ContainerFactory(new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlSortedDictionary),
                        new ContainerLevel(ContainerType.AvlSortedKeyValueTree)
                    });
                case DictionaryToUse.AvlSortedIndexableDictionary:
                    return new ContainerFactory(new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlSortedIndexableDictionary),
                        new ContainerLevel(ContainerType.AvlSortedKeyValueTree)
                    });
                case DictionaryToUse.AvlSortedDictionaryMultiValue:
                    return new ContainerFactory(new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlSortedDictionary, true),
                        new ContainerLevel(ContainerType.AvlSortedKeyValueTree, true)
                    });
                case DictionaryToUse.AvlSortedDictionaryWithUnderlyingIndexableListTree:
                    return new ContainerFactory(new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlSortedDictionary, false),
                        new ContainerLevel(ContainerType.AvlSortedKeyValueTree, false),
                        new ContainerLevel(ContainerType.AvlIndexableListTree, false),
                        new ContainerLevel(ContainerType.LazinatorList, false)
                    });
                default:
                    throw new NotSupportedException();
            }
        }

        public ILazinatorDictionaryable<TKey, TValue> GetDictionary<TKey, TValue>(DictionaryToUse dictionaryToUse) where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
        {
            var factory = GetDictionaryFactory<TKey, TValue>(dictionaryToUse);
            return factory.CreatePossiblySortedLazinatorDictionaryable<TKey, TValue>();
        }
        
        [Theory]
        [InlineData(DictionaryToUse.LazinatorDictionary)]
        [InlineData(DictionaryToUse.AvlDictionary)]
        [InlineData(DictionaryToUse.AvlSortedDictionary)]
        [InlineData(DictionaryToUse.AvlSortedIndexableDictionary)]
        [InlineData(DictionaryToUse.AvlDictionaryWithUnderlyingIndexableListTree)]
        [InlineData(DictionaryToUse.AvlSortedDictionaryWithUnderlyingIndexableListTree)]
        public void DictionaryEnumerableWorks(DictionaryToUse dictionaryToUse)
        {
            ILazinatorDictionaryable<WLong, NonComparableWrapperString> d = GetDictionary<WLong, NonComparableWrapperString>(dictionaryToUse);
            List<int> l = Enumerable.Range(0, 100).ToList();
            Shuffle(l);
            foreach (int i in l)
                d[i] = i.ToString();
            ConfirmDictionary(d);

            var c = d.CloneLazinatorTyped();
            ConfirmDictionary(c);
        }

        private static void Shuffle<T>(IList<T> list)
        {
            Random rng = new Random(0);
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        private static void ConfirmDictionary(ILazinatorDictionaryable<WLong, NonComparableWrapperString> d)
        {
            List<int> l = Enumerable.Range(0, 100).ToList();
            foreach (int i in l)
                d[new WLong(i)].Should().Be(i.ToString());
            var results = d.ToList().Select(x => x.Key.WrappedValue).ToList();
            if (!(d.IsSorted))
                results = results.OrderBy(x => x).ToList();
            foreach (int i in l)
                results[i].Should().Be(i);
            if (d is AvlSortedIndexableDictionary<WLong, NonComparableWrapperString> indexable)
            {
                var result = indexable.FindIndex(50, MultivalueLocationOptions.First);
                result.index.Should().Be(50);
                var enumeratedFrom = indexable.KeysAsEnumerable(false, (WLong)60).Skip(5).First().WrappedValue.Should().Be(65);
            }
        }

        [Theory]
        [InlineData(DictionaryToUse.LazinatorDictionary)]
        [InlineData(DictionaryToUse.AvlDictionary)]
        [InlineData(DictionaryToUse.AvlSortedDictionary)]
        [InlineData(DictionaryToUse.AvlSortedIndexableDictionary)]
        [InlineData(DictionaryToUse.AvlDictionaryWithUnderlyingIndexableListTree)]
        [InlineData(DictionaryToUse.AvlSortedDictionaryWithUnderlyingIndexableListTree)]
        public void DictionaryLongStringWorks(DictionaryToUse dictionaryToUse)
        {
            ILazinatorDictionaryable<WLong, NonComparableWrapperString> d = GetDictionary<WLong, NonComparableWrapperString>(dictionaryToUse);
            d.Count.Should().Be(0);
            d.Add(17, "seventeen");
            d.Count.Should().Be(1);
            d[17].WrappedValue.Should().Be("seventeen");
            d[17] = "Seventeen";
            d[17].WrappedValue.Should().Be("Seventeen");
            Action a = () => d.Add(17, "seventeen");
            a.Should().Throw<ArgumentException>();
            d.Count.Should().Be(1);
            d.Remove(17);
            d.Count.Should().Be(0);
            d.ContainsKey(17).Should().BeFalse();
            d[17] = "seventeen";
            bool result = d.Remove(new KeyValuePair<WLong, NonComparableWrapperString>(17, "seventeen"));
            result.Should().Be(true);
            d.Count.Should().Be(0);
            d.ContainsKey(17).Should().BeFalse();
            d[17] = "seventeen";
            result = d.Remove(new KeyValuePair<WLong, NonComparableWrapperString>(17, "not a match"));
            result.Should().BeFalse();
            d.Add(15, "fifteen");
            d.Count.Should().Be(2);
            d.ContainsKey(14).Should().BeFalse();
            d.ContainsKey(15).Should().BeTrue();
            d.ContainsKey(16).Should().BeFalse();
            d.ContainsKey(17).Should().BeTrue();
            d.ContainsKey(18).Should().BeFalse();
            d[15].WrappedValue.Should().Be("fifteen");
            d[17].WrappedValue.Should().Be("seventeen");
            d[23] = "twenty-three";
            d[18] = "eighteen";
            d[-1] = "negative one";
            d.Keys.OrderBy(x => x).ToList().SequenceEqual(new WLong[] { -1, 15, 17, 18, 23 }).Should().BeTrue();
            d.Values.Select(x => x.WrappedValue).OrderBy(x => x).ToList().SequenceEqual((new string[] { "negative one", "fifteen", "seventeen", "eighteen", "twenty-three" }).OrderBy(x => x)).Should().BeTrue();
            d.Select(x => x.Key).OrderBy(x => x).ToArray().SequenceEqual(new WLong[] { -1, 15, 17, 18, 23 }).Should().BeTrue();
            d.Select(x => x.Value).Select(x => x.WrappedValue).OrderBy(x => x).ToArray().SequenceEqual((new string[] { "negative one", "fifteen", "seventeen", "eighteen", "twenty-three" }).OrderBy(x => x)).Should().BeTrue();
            d.Count.Should().Be(5);
            result = d.TryGetValue(17, out NonComparableWrapperString s);
            result.Should().BeTrue();
            s.WrappedValue.Should().Be("seventeen");
            result = d.TryGetValue(12354, out s);
            result.Should().BeFalse();
            s.WrappedValue.Should().Be(null);
            d.Clear();
            d.ContainsKey(15).Should().BeFalse();
            result = d.TryGetValue(15, out s);
            result.Should().BeFalse();
            s.WrappedValue.Should().Be(null);
            d.Count.Should().Be(0);
            d.Keys.ToList().OrderBy(x => x).SequenceEqual(new WLong[] { }).Should().BeTrue();
            d.Values.ToList().Select(x => x.WrappedValue).OrderBy(x => x).SequenceEqual(new string[] { }).Should().BeTrue();
            d.ToList().OrderBy(x => x).Select(x => x.Key).ToArray().SequenceEqual(new WLong[] { }).Should().BeTrue();
            d.ToList().OrderBy(x => x).Select(x => x.Value).ToArray().SequenceEqual(new NonComparableWrapperString[] { }).Should().BeTrue();
        }


        [Theory]
        [InlineData(DictionaryToUse.LazinatorDictionary)]
        [InlineData(DictionaryToUse.AvlDictionary)]
        [InlineData(DictionaryToUse.AvlSortedIndexableDictionary)]
        [InlineData(DictionaryToUse.AvlSortedDictionary)]
        [InlineData(DictionaryToUse.AvlDictionaryWithUnderlyingIndexableListTree)]
        [InlineData(DictionaryToUse.AvlSortedDictionaryWithUnderlyingIndexableListTree)]
        public void DictionaryCanGrowAndShrink(DictionaryToUse dictionaryToUse)
        {
            ILazinatorDictionaryable<WLong, NonComparableWrapperString> d = GetDictionary<WLong, NonComparableWrapperString>(dictionaryToUse);
            const int numItems = 25;
            for (long i = 0; i < numItems; i++)
            {
                d[i] = i.ToString();
            }
            d.Count.Should().Be(numItems);
            if (d is ICountableContainer countable)
                countable.LongCount.Should().Be(numItems);
            RemoveAllItemsFromDictionary(d);
            d.Count().Should().Be(0);
        }

        [Theory]
        [InlineData(DictionaryToUse.LazinatorDictionary)]
        [InlineData(DictionaryToUse.AvlDictionary)]
        [InlineData(DictionaryToUse.AvlSortedIndexableDictionary)]
        [InlineData(DictionaryToUse.AvlSortedDictionary)]
        [InlineData(DictionaryToUse.AvlDictionaryWithUnderlyingIndexableListTree)]
        [InlineData(DictionaryToUse.AvlSortedDictionaryWithUnderlyingIndexableListTree)]
        public void DictionaryRecognizesKeyEquivalent(DictionaryToUse dictionaryToUse)
        {
            ILazinatorDictionaryable<WString, WLong> s = GetDictionary<WString, WLong>(dictionaryToUse);
            s["mykey"] = 34;
            s["mykey"] = 56;
            s.Count().Should().Be(1);
        }

        [Theory]
        [InlineData(DictionaryToUse.LazinatorDictionary)]
        [InlineData(DictionaryToUse.AvlDictionary)]
        [InlineData(DictionaryToUse.AvlSortedIndexableDictionary)]
        [InlineData(DictionaryToUse.AvlSortedDictionary)]
        [InlineData(DictionaryToUse.AvlDictionaryWithUnderlyingIndexableListTree)]
        [InlineData(DictionaryToUse.AvlSortedDictionaryWithUnderlyingIndexableListTree)]
        public void EmptyDictionary(DictionaryToUse dictionaryToUse)
        {
            ILazinatorDictionaryable<WLong, NonComparableWrapperString> d = GetDictionary<WLong, NonComparableWrapperString>(dictionaryToUse);
            bool result = d.TryGetValue(10, out NonComparableWrapperString value);
            result.Should().BeFalse();
        }

        private static void RemoveAllItemsFromDictionary(ILazinatorDictionaryable<WLong, NonComparableWrapperString> d)
        {
            int numItems = d.Count;
            RemoveItemsFromDictionary(d, numItems);
        }

        private static void RemoveItemsFromDictionary(ILazinatorDictionaryable<WLong, NonComparableWrapperString> d, int numItems)
        {
            for (long i = 0; i < numItems; i++)
            {
                d.Remove(i);
            }
        }

        [Theory]
        [InlineData(DictionaryToUse.LazinatorDictionary)]
        [InlineData(DictionaryToUse.AvlDictionary)]
        [InlineData(DictionaryToUse.AvlSortedIndexableDictionary)]
        [InlineData(DictionaryToUse.AvlSortedDictionary)]
        [InlineData(DictionaryToUse.AvlDictionaryWithUnderlyingIndexableListTree)]
        [InlineData(DictionaryToUse.AvlSortedDictionaryWithUnderlyingIndexableListTree)]
        public void SingleItemDictionary(DictionaryToUse dictionaryToUse)
        {
            // we have some optimizations for a single-item dictionary, so this tests them
            ILazinatorDictionaryable<WLong, NonComparableWrapperString> d = GetDictionary<WLong, NonComparableWrapperString>(dictionaryToUse);
            d[234] = "something";
            d.ContainsKey(123).Should().BeFalse();
            d.ContainsKey(234).Should().BeTrue();
            d[234].WrappedValue.Should().Be("something");

            // try with a zero key
            d = GetDictionary<WLong, NonComparableWrapperString>(dictionaryToUse);
            d[0] = "something";
            d.ContainsKey(123).Should().BeFalse();
            d.ContainsKey(0).Should().BeTrue();
            d[0].WrappedValue.Should().Be("something");

            // try after initially adding items
            d = GetDictionary<WLong, NonComparableWrapperString>(dictionaryToUse);
            for (long i = 0; i < 25; i++)
            {
                d[i] = i.ToString();
            }
            RemoveItemsFromDictionary(d, 24);
            d.ContainsKey(123).Should().BeFalse();
            var key = d.Keys.First();
            d.ContainsKey(key).Should().BeTrue();
            d[key].WrappedValue.Should().Be(key.ToString());
        }

        [Theory]
        [InlineData(DictionaryToUse.LazinatorDictionary)]
        [InlineData(DictionaryToUse.AvlDictionary)]
        [InlineData(DictionaryToUse.AvlSortedIndexableDictionary)]
        [InlineData(DictionaryToUse.AvlSortedDictionary)]
        [InlineData(DictionaryToUse.AvlDictionaryWithUnderlyingIndexableListTree)]
        [InlineData(DictionaryToUse.AvlSortedDictionaryWithUnderlyingIndexableListTree)]
        public void DictionaryWithBadHashFunction(DictionaryToUse dictionaryToUse)
        {
            // With some dictionary implementations, all items with same hash are put in hash bucket. So, this ensures that dictionary will work in this case.
            ILazinatorDictionaryable<StructWithBadHashFunction, NonComparableWrapperString> d = GetDictionary<StructWithBadHashFunction, NonComparableWrapperString>(dictionaryToUse);
            for (int i = 0; i < 100; i++)
                d[i] = i.ToString();

            for (int i = 0; i < 100; i++)
            {
                d.ContainsKey(i).Should().BeTrue();
                d[i].WrappedValue.Should().Be(i.ToString());
            }

            for (int i = 0; i < 100; i++)
                if (i % 3 == 0)
                    d.Remove(i);
                else
                    d.Remove(new KeyValuePair<StructWithBadHashFunction, NonComparableWrapperString>(i, i.ToString()));

            for (int i = 0; i < 100; i++)
                d.ContainsKey(i).Should().BeFalse();
        }

        [Theory]
        [InlineData(DictionaryToUse.LazinatorDictionary)]
        [InlineData(DictionaryToUse.AvlDictionary)]
        [InlineData(DictionaryToUse.AvlSortedIndexableDictionary)]
        [InlineData(DictionaryToUse.AvlSortedDictionary)]
        [InlineData(DictionaryToUse.AvlDictionaryWithUnderlyingIndexableListTree)]
        [InlineData(DictionaryToUse.AvlSortedDictionaryWithUnderlyingIndexableListTree)]
        public void DictionarySearchWorksEvenIfLastKeyDisposed(DictionaryToUse dictionaryToUse)
        {
            // The concern here is that the dictionary remembers the last key searched as a shortcut. What happens if that is disposed? With a struct or class that contains only primitive properties, that is not a problem, because equality can be determined solely by looking at primitive properties. But it can be an issue with a Lazinator object that has child objects that need to be examined.

            ILazinatorDictionaryable<LazinatorTuple<WLong, WInt>, NonComparableWrapperString> d = GetDictionary<LazinatorTuple<WLong, WInt>, NonComparableWrapperString>(dictionaryToUse);
            LazinatorTuple<WLong, WInt> a = new LazinatorTuple<WLong, WInt>(1, 2);
            LazinatorTuple<WLong, WInt> b = new LazinatorTuple<WLong, WInt>(3, 4);
            d[a] = "something";
            d[b] = "else";
            LazinatorTuple<WLong, WInt> a2 = new LazinatorTuple<WLong, WInt>(1, 2);
            a2.UpdateStoredBuffer();
            string s = d[a2];
            s.Should().Be("something");
            a2.FreeInMemoryObjects();
            a2.LazinatorMemoryStorage.Dispose();

            LazinatorTuple<WLong, WInt> a3 = new LazinatorTuple<WLong, WInt>(1, 2);
            s = d[a3];
            s.Should().Be("something");

        }

        [Theory]
        [InlineData(DictionaryToUse.AvlDictionaryMultiValue)]
        [InlineData(DictionaryToUse.AvlSortedDictionaryMultiValue)]
        public void SortedMultivalueDictionaryWorks(DictionaryToUse dictionaryToUse)
        {
            ILazinatorMultivalueDictionaryable<WLong, WInt> d = (ILazinatorMultivalueDictionaryable<WLong, WInt>)GetDictionary<WLong, WInt>(dictionaryToUse);
            const int numKeys = 100;
            const int numEntries = 150;
            List<int>[] itemsForNode = new List<int>[numKeys];
            for (int i = 0; i < numKeys; i++)
                itemsForNode[i] = new List<int>();
            Random r = new Random(0);
            for (int i = 0; i < numEntries; i++)
            {
                int key = r.Next(numKeys);
                int value = r.Next();
                itemsForNode[key].Add(value);
                d.AddValueForKey(key, value);
                d.Count.Should().Be(i + 1);
            }
            for (int i = 0; i < numKeys; i++)
            {
                var key = new WLong(i);
                var items = d.GetAllValues(key).Select(x => x.WrappedValue).ToList();
                d.ContainsKey(key).Should().Be(items.Any());
                items.SequenceEqual(itemsForNode[i]).Should().BeTrue();
                d.TryRemoveAll(key);
                d.GetAllValues(key).Any().Should().BeFalse();
            }
        }


    }
}
