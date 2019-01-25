using FluentAssertions;
using LazinatorTests.Examples;
using Xunit;

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
            e.UpdateStoredBuffer();
            var origValue = e.MyChild1.MyLong;
            const long revisedValue = -123456789012345;
            e.MyChild1.MyLong = revisedValue;
            // use free in memory objects to revert
            e.FreeInMemoryObjects();
            e.MyChild1.MyLong.Should().Be(origValue);
            // now, make memory up to date, then free
            e.MyChild1.MyLong = revisedValue;
            e.UpdateStoredBuffer();
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
            e.UpdateStoredBuffer();
            e.FreeInMemoryObjects();
            e.MyChild1.MyLong.Should().Be(revisedValue);
        }
    }
}