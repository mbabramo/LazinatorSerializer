using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using FluentAssertions;
using Lazinator.Collections.Dictionary;
using Lazinator.Wrappers;
using Lazinator.Core;
using Lazinator.Collections;
using Lazinator.Collections.Avl;
using LazinatorTests.Examples;
using Lazinator.Collections.Tuples;

namespace LazinatorTests.Tests
{

    public class DictionaryTests
    {
        public enum DictionaryToUse
        {
            LazinatorDictionary,
            AvlDictionary,
            AvlDictionaryMultiValue,
            AvlSortedDictionary,
            AvlSortedDictionaryMultiValue
        }

        public ILazinatorKeyable<TKey, TValue> GetDictionary<TKey, TValue>(DictionaryToUse dictionaryToUse) where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
        {
            switch (dictionaryToUse)
            {
                case DictionaryToUse.LazinatorDictionary:
                    return new LazinatorDictionary<TKey, TValue>();
                case DictionaryToUse.AvlDictionary:
                    return new AvlDictionary<TKey, TValue>(false);
                case DictionaryToUse.AvlDictionaryMultiValue:
                    return new AvlDictionary<TKey, TValue>(true);
                case DictionaryToUse.AvlSortedDictionary:
                    return new AvlSortedDictionary<TKey, TValue>(false);
                case DictionaryToUse.AvlSortedDictionaryMultiValue:
                    return new AvlSortedDictionary<TKey, TValue>(true);
            }
            throw new InvalidOperationException();
        }
        
        [Theory]
        [InlineData(DictionaryToUse.LazinatorDictionary)]
        [InlineData(DictionaryToUse.AvlDictionary)]
        [InlineData(DictionaryToUse.AvlSortedDictionary)]
        public void DictionaryEnumerableWorks(DictionaryToUse dictionaryToUse)
        {
            ILazinatorKeyable<WLong, WString> d = GetDictionary<WLong, WString>(dictionaryToUse);
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

        private static void ConfirmDictionary(ILazinatorKeyable<WLong, WString> d)
        {
            List<int> l = Enumerable.Range(0, 100).ToList();
            foreach (int i in l)
                d[new WLong(i)].Should().Be(i.ToString());
            var results = d.ToList().Select(x => x.Key.WrappedValue).ToList();
            if (!(d is ILazinatorOrderedKeyable<WLong, WString>))
                results = results.OrderBy(x => x).ToList();
            foreach (int i in l)
                results[i].Should().Be(i);
        }

        [Theory]
        [InlineData(DictionaryToUse.LazinatorDictionary)]
        [InlineData(DictionaryToUse.AvlDictionary)]
        [InlineData(DictionaryToUse.AvlSortedDictionary)]
        public void DictionaryLongStringWorks(DictionaryToUse dictionaryToUse)
        {
            ILazinatorKeyable<WLong, WString> d = GetDictionary<WLong, WString>(dictionaryToUse);
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
            bool result = d.Remove(new KeyValuePair<WLong, WString>(17, "seventeen"));
            result.Should().Be(true);
            d.Count.Should().Be(0);
            d.ContainsKey(17).Should().BeFalse();
            d[17] = "seventeen";
            result = d.Remove(new KeyValuePair<WLong, WString>(17, "not a match"));
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
            d.Values.OrderBy(x => x).ToList().SequenceEqual((new WString[] { "negative one", "fifteen", "seventeen", "eighteen", "twenty-three" }).OrderBy(x => x)).Should().BeTrue();
            d.Select(x => x.Key).OrderBy(x => x).ToArray().SequenceEqual(new WLong[] { -1, 15, 17, 18, 23 }).Should().BeTrue();
            d.Select(x => x.Value).OrderBy(x => x).ToArray().SequenceEqual((new WString[] { "negative one", "fifteen", "seventeen", "eighteen", "twenty-three" }).OrderBy(x => x)).Should().BeTrue();
            d.Count.Should().Be(5);
            result = d.TryGetValue(17, out WString s);
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
            d.Values.ToList().OrderBy(x => x).SequenceEqual(new WString[] { }).Should().BeTrue();
            d.ToList().OrderBy(x => x).Select(x => x.Key).ToArray().SequenceEqual(new WLong[] { }).Should().BeTrue();
            d.ToList().OrderBy(x => x).Select(x => x.Value).ToArray().SequenceEqual(new WString[] { }).Should().BeTrue();
        }


        [Theory]
        [InlineData(DictionaryToUse.LazinatorDictionary)]
        [InlineData(DictionaryToUse.AvlDictionary)]
        [InlineData(DictionaryToUse.AvlSortedDictionary)]
        public void DictionaryCanGrowAndShrink(DictionaryToUse dictionaryToUse)
        {
            ILazinatorKeyable<WLong, WString> d = GetDictionary<WLong, WString>(dictionaryToUse);
            const int numItems = 25;
            for (long i = 0; i < numItems; i++)
            {
                d[i] = i.ToString();
            }
            d.Count.Should().Be(numItems);
            RemoveAllItemsFromDictionary(d);
            d.Count().Should().Be(0);
        }

        [Theory]
        [InlineData(DictionaryToUse.LazinatorDictionary)]
        [InlineData(DictionaryToUse.AvlDictionary)]
        [InlineData(DictionaryToUse.AvlSortedDictionary)]
        public void DictionaryRecognizesKeyEquivalent(DictionaryToUse dictionaryToUse)
        {
            ILazinatorKeyable<WString, WLong> s = GetDictionary<WString, WLong>(dictionaryToUse);
            s["mykey"] = 34;
            s["mykey"] = 56;
            s.Count().Should().Be(1);
        }

        [Theory]
        [InlineData(DictionaryToUse.LazinatorDictionary)]
        [InlineData(DictionaryToUse.AvlDictionary)]
        [InlineData(DictionaryToUse.AvlSortedDictionary)]
        public void EmptyDictionary(DictionaryToUse dictionaryToUse)
        {
            ILazinatorKeyable<WLong, WString> d = GetDictionary<WLong, WString>(dictionaryToUse);
            bool result = d.TryGetValue(10, out WString value);
            result.Should().BeFalse();
        }

        private static void RemoveAllItemsFromDictionary(ILazinatorKeyable<WLong, WString> d)
        {
            int numItems = d.Count;
            RemoveItemsFromDictionary(d, numItems);
        }

        private static void RemoveItemsFromDictionary(ILazinatorKeyable<WLong, WString> d, int numItems)
        {
            for (long i = 0; i < numItems; i++)
            {
                d.Remove(i);
            }
        }

        [Theory]
        [InlineData(DictionaryToUse.LazinatorDictionary)]
        [InlineData(DictionaryToUse.AvlDictionary)]
        [InlineData(DictionaryToUse.AvlSortedDictionary)]
        public void SingleItemDictionary(DictionaryToUse dictionaryToUse)
        {
            // we have some optimizations for a single-item dictionary, so this tests them
            ILazinatorKeyable<WLong, WString> d = GetDictionary<WLong, WString>(dictionaryToUse);
            d[234] = "something";
            d.ContainsKey(123).Should().BeFalse();
            d.ContainsKey(234).Should().BeTrue();
            d[234].WrappedValue.Should().Be("something");

            // try with a zero key
            d = new LazinatorDictionary<WLong, WString>();
            d[0] = "something";
            d.ContainsKey(123).Should().BeFalse();
            d.ContainsKey(0).Should().BeTrue();
            d[0].WrappedValue.Should().Be("something");

            // try after initially adding items
            d = new LazinatorDictionary<WLong, WString>();
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
        [InlineData(DictionaryToUse.AvlSortedDictionary)]
        public void DictionaryWithBadHashFunction(DictionaryToUse dictionaryToUse)
        {
            // With some dictionary implementations, all items with same hash are put in hash bucket. So, this ensures that dictionary will work in this case.
            ILazinatorKeyable<StructWithBadHashFunction, WString> d = GetDictionary<StructWithBadHashFunction, WString>(dictionaryToUse);
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
                    d.Remove(new KeyValuePair<StructWithBadHashFunction, WString>(i, i.ToString()));

            for (int i = 0; i < 100; i++)
                d.ContainsKey(i).Should().BeFalse();
        }

        [Theory]
        [InlineData(DictionaryToUse.LazinatorDictionary)]
        [InlineData(DictionaryToUse.AvlDictionary)]
        [InlineData(DictionaryToUse.AvlSortedDictionary)]
        public void DictionarySearchWorksEvenIfLastKeyDisposed(DictionaryToUse dictionaryToUse)
        {
            // The concern here is that the dictionary remembers the last key searched as a shortcut. What happens if that is disposed? With a struct or class that contains only primitive properties, that is not a problem, because equality can be determined solely by looking at primitive properties. But it can be an issue with a Lazinator object that has child objects that need to be examined.
            
            ILazinatorKeyable<LazinatorTuple<WLong, WInt>, WString> d = GetDictionary<LazinatorTuple<WLong, WInt>, WString>(dictionaryToUse);
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
            ILazinatorKeyableMultivalue<WLong, WInt> d = (ILazinatorKeyableMultivalue<WLong, WInt>)GetDictionary<WLong, WInt>(dictionaryToUse);
            const int numKeys = 100;
            const int numEntries = 150;
            List<int>[] itemsForNode = new List<int>[numKeys];
            for (int i = 0; i < numKeys; i++)
                itemsForNode[i] = new List<int>();
            Random r = new Random(0);
            for (int i = 0; i < numEntries; i++)
            {
                int j = r.Next(numKeys);
                int k = r.Next();
                itemsForNode[j].Add(k);
                d.AddValue(j, k);
            }
            for (int i = 0; i < numKeys; i++)
            {
                var key = new WLong(i);
                var items = d.GetAllValues(key).Select(x => x.WrappedValue).ToList();
                d.ContainsKey(key).Should().Be(items.Any());
                items.SequenceEqual(itemsForNode[i]).Should().BeTrue();
                d.RemoveAll(key);
                d.GetAllValues(key).Any().Should().BeFalse();
            }
        }


    }
}
