using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using FluentAssertions;
using Lazinator.Collections.Dictionary;
using Lazinator.Wrappers;
using Lazinator.Core;
using LazinatorTests.Examples;
using Lazinator.Collections;

namespace LazinatorTests.Tests
{

    public class DictionaryTests
    {
        [Fact]
        public void DictionaryEnumerableWorks()
        {
            LazinatorDictionary<WLong, WString> d = new LazinatorDictionary<WLong, WString>();
            d.Add(1, "one");
            d.Add(2, "two");
            ConfirmDictionary(d);

            var c = d.CloneLazinatorTyped();
            ConfirmDictionary(c);
        }

        private static void ConfirmDictionary(LazinatorDictionary<WLong, WString> d)
        {
            d[new WLong(1)].Should().Be("one");
            d[new WLong(2)].Should().Be("two");
            var results = d.ToList().Select(x => x.Key).OrderBy(x => x).ToList();
            results[0].WrappedValue.Should().Be(1);
            results[1].WrappedValue.Should().Be(2);
            var results2 = d.AsEnumerable().Select(x => x.Key).OrderBy(x => x).ToList();
            results2[0].WrappedValue.Should().Be(1);
            results2[1].WrappedValue.Should().Be(2);
        }

        [Fact]
        public void DictionaryLongStringWorks()
        {
            LazinatorDictionary<WLong, WString> d = new LazinatorDictionary<WLong, WString>();
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


        [Fact]
        public void DictionaryCanGrowAndShrink()
        {
            LazinatorDictionary<WLong, WString> d = new LazinatorDictionary<WLong, WString>();
            const int numItems = 25;
            for (long i = 0; i < numItems; i++)
            {
                d[i] = i.ToString();
            }
            d.Count.Should().Be(numItems);
            RemoveAllItemsFromDictionary(d);
            d.Count().Should().Be(0);
        }

        [Fact]
        public void EmptyDictionary()
        {
            LazinatorDictionary<WLong, WString> d = new LazinatorDictionary<WLong, WString>();
            bool result = d.TryGetValue(10, out WString value);
            result.Should().BeFalse();
        }

        private static void RemoveAllItemsFromDictionary(LazinatorDictionary<WLong, WString> d)
        {
            int numItems = d.Count;
            RemoveItemsFromDictionary(d, numItems);
        }

        private static void RemoveItemsFromDictionary(LazinatorDictionary<WLong, WString> d, int numItems)
        {
            for (long i = 0; i < numItems; i++)
            {
                d.Remove(i);
            }
        }

        [Fact]
        public void SingleItemDictionary()
        {
            // we have some optimizations for a single-item dictionary, so this tests them
            LazinatorDictionary<WLong, WString> d = new LazinatorDictionary<WLong, WString>();
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

        [Fact]
        public void DictionarySearchWorksEvenIfLastKeyDeleted()
        {
            // The concern here is that the dictionary remembers the last key searched as a shortcut. What happens if that is disposed? With a struct or class that contains only primitive properties, that is not a problem, because equality can be determined solely by looking at primitive properties. But it can be an issue with a Lazinator object that has child objects that need to be examined.

            LazinatorDictionary<LazinatorTuple<WLong, WInt>, WString> d = new LazinatorDictionary<LazinatorTuple<WLong, WInt>, WString>();
            LazinatorTuple<WLong, WInt> a = new LazinatorTuple<WLong, WInt>(1, 2);
            LazinatorTuple<WLong, WInt> b = new LazinatorTuple<WLong, WInt>(3, 4);
            d[a] = "something";
            d[b] = "else";
            LazinatorTuple<WLong, WInt> a2 = new LazinatorTuple<WLong, WInt>(1, 2);
            a2.EnsureLazinatorMemoryUpToDate();
            string s = d[a2];
            s.Should().Be("something");
            a2.FreeInMemoryObjects();
            a2.LazinatorMemoryStorage.Dispose();

            LazinatorTuple<WLong, WInt> a3 = new LazinatorTuple<WLong, WInt>(1, 2);
            s = d[a3];
            s.Should().Be("something");

        }


    }
}
