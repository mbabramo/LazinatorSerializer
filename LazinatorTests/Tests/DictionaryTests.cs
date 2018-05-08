using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using FluentAssertions;
using Lazinator.Collections.Dictionary;
using Lazinator.Wrappers;

namespace LazinatorTests.Tests
{

    public class DictionaryTests
    {
        [Fact]
        public void DictionaryLongStringWorks()
        {
            LazinatorDictionary<LazinatorWrapperLong, LazinatorWrapperString> d = new LazinatorDictionary<LazinatorWrapperLong, LazinatorWrapperString>();
            d.Count.Should().Be(0);
            d.Add(17, "seventeen");
            d.Count.Should().Be(1);
            d[17].Value.Should().Be("seventeen");
            d[17] = "Seventeen";
            d[17].Value.Should().Be("Seventeen");
            Action a = () => d.Add(17, "seventeen");
            a.Should().Throw<ArgumentException>();
            d.Count.Should().Be(1);
            d.Remove(17);
            d.Count.Should().Be(0);
            d.ContainsKey(17).Should().BeFalse();
            d[17] = "seventeen";
            bool result = d.Remove(new KeyValuePair<LazinatorWrapperLong, LazinatorWrapperString>(17, "seventeen"));
            result.Should().Be(true);
            d.Count.Should().Be(0);
            d.ContainsKey(17).Should().BeFalse();
            d[17] = "seventeen";
            result = d.Remove(new KeyValuePair<LazinatorWrapperLong, LazinatorWrapperString>(17, "not a match"));
            result.Should().BeFalse();
            d.Add(15, "fifteen");
            d.Count.Should().Be(2);
            d.ContainsKey(14).Should().BeFalse();
            d.ContainsKey(15).Should().BeTrue();
            d.ContainsKey(16).Should().BeFalse();
            d.ContainsKey(17).Should().BeTrue();
            d.ContainsKey(18).Should().BeFalse();
            d[15].Value.Should().Be("fifteen");
            d[17].Value.Should().Be("seventeen");
            d[23] = "twenty-three";
            d[18] = "eighteen";
            d[-1] = "negative one";
            d.Keys.OrderBy(x => x).ToList().SequenceEqual(new LazinatorWrapperLong[] { -1, 15, 17, 18, 23 }).Should().BeTrue();
            d.Values.OrderBy(x => x).ToList().SequenceEqual((new LazinatorWrapperString[] { "negative one", "fifteen", "seventeen", "eighteen", "twenty-three" }).OrderBy(x => x)).Should().BeTrue();
            d.Select(x => x.Key).OrderBy(x => x).ToArray().SequenceEqual(new LazinatorWrapperLong[] { -1, 15, 17, 18, 23 }).Should().BeTrue();
            d.Select(x => x.Value).OrderBy(x => x).ToArray().SequenceEqual((new LazinatorWrapperString[] { "negative one", "fifteen", "seventeen", "eighteen", "twenty-three" }).OrderBy(x => x)).Should().BeTrue();
            d.Count.Should().Be(5);
            result = d.TryGetValue(17, out LazinatorWrapperString s);
            result.Should().BeTrue();
            s.Value.Should().Be("seventeen");
            result = d.TryGetValue(12354, out s);
            result.Should().BeFalse();
            s.Value.Should().Be(null);
            d.Clear();
            d.ContainsKey(15).Should().BeFalse();
            result = d.TryGetValue(15, out s);
            result.Should().BeFalse();
            s.Value.Should().Be(null);
            d.Count.Should().Be(0);
            d.Keys.ToList().OrderBy(x => x).SequenceEqual(new LazinatorWrapperLong[] { }).Should().BeTrue();
            d.Values.ToList().OrderBy(x => x).SequenceEqual(new LazinatorWrapperString[] { }).Should().BeTrue();
            d.ToList().OrderBy(x => x).Select(x => x.Key).ToArray().SequenceEqual(new LazinatorWrapperLong[] { }).Should().BeTrue();
            d.ToList().OrderBy(x => x).Select(x => x.Value).ToArray().SequenceEqual(new LazinatorWrapperString[] { }).Should().BeTrue();
        }
    }
}
