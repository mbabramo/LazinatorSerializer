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

namespace LazinatorTests.Tests
{
    public class FreeInMemoryObjectsTests : SerializationDeserializationTestBase
    {
        [Fact]
        public void FreeInMemoryObjects_SetsUnserializedObjectToNull()
        {
            Example e = GetTypicalExample();
            e.MyChild1.Should().NotBeNull();
            e.FreeInMemoryObjects();
            e.MyChild1.Should().Be(null);
        }

        [Fact]
        public void FreeInMemoryObjects_OriginalValueReverted()
        {
            Example e = GetTypicalExample();
            e.EnsureLazinatorMemoryUpToDate();
            var origValue = e.MyChild1.MyLong;
            const long revisedValue = -123456789012345;
            e.MyChild1.MyLong = revisedValue;
            // use free in memory objects to revert
            e.FreeInMemoryObjects();
            e.MyChild1.MyLong.Should().Be(origValue);
            // now, make memory up to date, then free
            e.MyChild1.MyLong = revisedValue;
            e.EnsureLazinatorMemoryUpToDate();
            e.FreeInMemoryObjects();
            e.MyChild1.MyLong.Should().Be(revisedValue);
        }

        [Fact]
        public void FreeInMemoryObjects_RevisedValueSaved()
        {
            Example e = GetTypicalExample();
            var origValue = e.MyChild1.MyLong;
            const long revisedValue = -123456789012345;
            e.MyChild1.MyLong = revisedValue;
            e.EnsureLazinatorMemoryUpToDate();
            e.FreeInMemoryObjects();
            e.MyChild1.MyLong.Should().Be(revisedValue);
        }

        [Fact]
        public void FreeInMemoryObjects_LazinatorList()
        {
            var typical1 = GetTypicalExample();
            typical1.EnsureLazinatorMemoryUpToDate();
            var typical2 = GetTypicalExample();
            var origValue = typical2.MyChild1.MyLong;
            LazinatorList<Example> l = new LazinatorList<Example>() { typical1, typical2 };
            l.EnsureLazinatorMemoryUpToDate();
            l.LazinatorMemoryStorage.DisposeIndependently();
            l.FreeInMemoryObjects();
            typical2.MyChild1.MyLong = 46523496; // should not affect the list now
            l[0].MyChild1.MyLong.Should().Be(origValue);
            l[1].MyChild1.MyLong.Should().Be(origValue);

            const long revisedValue = -123456789012345;
            l[0].MyChild1.MyLong = revisedValue;
            l.EnsureLazinatorMemoryUpToDate();
            l.FreeInMemoryObjects();
            l[0].MyChild1.MyLong.Should().Be(revisedValue);
            l[1].MyChild1.MyLong.Should().Be(origValue);

        }
    }
}