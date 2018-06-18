using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using LazinatorTests.Examples;
using LazinatorTests.Examples.Collections;
using Lazinator.Core;
using Xunit;
using LazinatorTests.Examples.Structs;

namespace LazinatorTests.Tests
{
    public class DirtinessTests : SerializationDeserializationTestBase
    {

        [Fact]
        public void DirtinessSetsCorrectly()
        {
            var hierarchy = GetHierarchy(0, 1, 2, 0, 0);
            hierarchy.IsDirty.Should().BeTrue();
            hierarchy = hierarchy.CloneLazinatorTyped();
            hierarchy.IsDirty.Should().BeFalse();
            hierarchy.MyDateTime = DateTime.Now - TimeSpan.FromHours(1);
            hierarchy.IsDirty.Should().BeTrue();
            hierarchy.MyChild1.IsDirty.Should().BeFalse();
            hierarchy = hierarchy.CloneLazinatorTyped();
            hierarchy.IsDirty.Should().BeFalse();
            hierarchy.MyChild1 = new ExampleChild() { MyLong = 232344 };
            hierarchy.IsDirty.Should().BeTrue();
        }

        [Fact]
        public void OnDirtyCalledWhereImplemented()
        {
            // The Example.cs method includes an _OnDirtyCalled nonserialized flag that is set to true when its OnDirty() method (not generally required) is called.
            var hierarchy = GetHierarchy(0, 1, 2, 0, 0);
            hierarchy._OnDirtyCalled.Should().BeTrue();
            hierarchy._OnDirtyCalled = false; // reset flag
            hierarchy = hierarchy.CloneLazinatorTyped();
            hierarchy.IsDirty.Should().BeFalse();
            hierarchy.MyDateTime = DateTime.Now - TimeSpan.FromHours(1);
            hierarchy.IsDirty.Should().BeTrue();
            hierarchy._OnDirtyCalled.Should().BeTrue();
            hierarchy._OnDirtyCalled = false; // reset flag
            hierarchy.MyChild1.IsDirty.Should().BeFalse();
            hierarchy = hierarchy.CloneLazinatorTyped();
            hierarchy.IsDirty.Should().BeFalse();
            hierarchy._OnDirtyCalled.Should().BeFalse();
            hierarchy.MyChild1 = new ExampleChild() { MyLong = 232344 };
            hierarchy.IsDirty.Should().BeTrue();
            hierarchy._OnDirtyCalled.Should().BeTrue();
        }

        [Fact]
        public void DescendantDirtinessSetsCorrectly()
        {
            var hierarchy = GetHierarchy(0, 1, 2, 0, 0);
            hierarchy.IsDirty.Should().BeTrue();
            hierarchy.MyChild1.MyShort = 5234;
            hierarchy.DescendantIsDirty.Should().BeTrue();
            hierarchy = hierarchy.CloneLazinatorTyped();
            hierarchy.IsDirty.Should().BeFalse();
            hierarchy.MyChild1.MyShort.Should().Be(5234);
            hierarchy.MyChild1.IsDirty.Should().BeFalse();
            hierarchy.MyChild1.MyLong = 987654;
            hierarchy.MyChild1.IsDirty.Should().BeTrue();
            hierarchy.IsDirty.Should().BeFalse();
            hierarchy.DescendantIsDirty.Should().BeTrue();
            hierarchy = hierarchy.CloneLazinatorTyped();
            hierarchy.MyChild1.MyLong.Should().Be(987654);
        }


        [Fact]
        public void DirtinessWorksAfterConvertToBytes()
        {
            var hierarchy = GetHierarchy(0, 1, 2, 0, 0);
            hierarchy.MyChild1.MyWrapperContainer = new WrapperContainer() { WrappedInt = 17 };
            hierarchy.MyChild1.IsDirty.Should().BeTrue();
            hierarchy.MyChild1.DescendantIsDirty.Should().BeTrue();
            hierarchy.MyChild1.MyWrapperContainer.LazinatorConvertToBytes();
            hierarchy.MyChild1.IsDirty.Should().BeTrue();
            hierarchy.MyChild1.DescendantIsDirty.Should().BeTrue();
            hierarchy.MyChild1.MyWrapperContainer.IsDirty.Should().BeFalse();
            hierarchy.MyChild1.MyWrapperContainer.WrappedInt = 18;
            hierarchy.MyChild1.MyWrapperContainer.IsDirty.Should().BeTrue();
            hierarchy.MyChild1.IsDirty.Should().BeTrue(); // hasn't changed
            hierarchy.MyChild1.DescendantIsDirty.Should().BeTrue(); // this time, wrapper's dirtiness change, so MyChild1 is notified
            var clone = hierarchy.CloneLazinatorTyped();
            // The following is the tricky part. We must make sure that LazinatorConvertToBytes doesn't cause MyChild1 to think that no serialization is necessary.
            clone.MyChild1.MyWrapperContainer.WrappedInt.Should().Be(18);
        }

        [Fact]
        public void DirtinessWorksAfterDotNetCollectionConverted()
        {
            DotNetHashSet_SelfSerialized l = new DotNetHashSet_SelfSerialized();
            l.MyHashSetSerialized = new HashSet<ExampleChild>();
            l.MyHashSetSerialized.Add(new ExampleChild());
            l.LazinatorConvertToBytes();
            l.IsDirty.Should().BeFalse();
            l.DescendantIsDirty.Should().BeFalse();
            var firstItem = l.MyHashSetSerialized.First();
            l.IsDirty.Should().BeTrue(); // should be true because .Net collection without special _Dirty property has been accessed
            firstItem.MyLong = 54321;
            var c = l.CloneLazinatorTyped();
            c.MyHashSetSerialized.First().MyLong.Should().Be(54321);
        }

    }
}
