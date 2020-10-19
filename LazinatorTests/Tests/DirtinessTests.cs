using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using LazinatorTests.Examples;
using LazinatorTests.Examples.Collections;
using Lazinator.Core;
using Xunit;
using LazinatorTests.Examples.Structs;
using Lazinator.Wrappers;
using LazinatorCollections.Tuples;
using LazinatorTests.Examples.ExampleHierarchy;
using System.Reflection;

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
            hierarchy._OnDescendantIsDirtyCalled.Should().BeFalse();
            hierarchy.MyChild2.MyLong = -345334;
            hierarchy._OnDescendantIsDirtyCalled.Should().BeTrue();
            hierarchy._OnDirtyCalled = false; // reset flag
            hierarchy._OnDescendantIsDirtyCalled = false; // reset flag
            hierarchy.MyChild1.IsDirty.Should().BeFalse();
            hierarchy = hierarchy.CloneLazinatorTyped();
            hierarchy.IsDirty.Should().BeFalse();
            hierarchy._OnDirtyCalled.Should().BeFalse();
            hierarchy.MyChild1 = new ExampleChild() { MyLong = 232344 };
            hierarchy.IsDirty.Should().BeTrue();
            hierarchy._OnDirtyCalled.Should().BeTrue();
        }

        [Fact]
        public void CloningShouldntChangeDirtiness()
        {
            DotNetList_Values v = new DotNetList_Values() { MyListInt2 = new List<int>() { 3, 4 } };
            v.SerializeLazinator();
            var c = v.CloneLazinatorTyped();
            c.IsDirty.Should().BeFalse();
            var list = c.MyListInt2; // note that this doesn't have a _Dirty property
            c.IsDirty.Should().BeTrue(); // as a result of access of nonlazinator
            c.HasChanged.Should().BeTrue();
            var ctemp = c.CloneLazinatorTyped();
            c.IsDirty.Should().BeTrue(); // still true since the original hasn't changed
            c.HasChanged.Should().BeTrue();
            c.SerializeLazinator();
            var c2 = c.CloneLazinatorTyped();
            c.IsDirty.Should().BeFalse();
            c.HasChanged.Should().BeTrue();
            c2.IsDirty.Should().BeFalse();
            c2.HasChanged.Should().BeFalse();

            c.MarkHierarchyUnchanged(); // reset HasChanged to false
            c.HasChanged.Should().BeFalse();
            c.DescendantIsDirty = true; // force serialization to test whether the serialization itself causes dirtiness
            c.SerializeLazinator();
            var c3 = c.CloneLazinatorTyped();
            c.IsDirty.Should().BeFalse(); // this is easy because SerializeToExistingBuffer resets Dirty
            c.HasChanged.Should().BeFalse(); // this is harder -- for it to work, CloneLazinatorTyped must not temporarily cause c to become dirty
            c3.IsDirty.Should().BeFalse();
            c3.HasChanged.Should().BeFalse();
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
        public void DirtinessWorksAfterEnsureUpToDate()
        {
            var hierarchy = GetHierarchy(0, 1, 2, 0, 0);
            hierarchy.MyChild1.MyWrapperContainer = new WrapperContainer() { WrappedInt = 17 };
            hierarchy.MyChild1.IsDirty.Should().BeTrue();
            hierarchy.MyChild1.DescendantIsDirty.Should().BeTrue();
            hierarchy.MyChild1.HasChanged.Should().BeTrue();
            hierarchy.MyChild1.DescendantHasChanged.Should().BeTrue();
            hierarchy.MyChild1.MyWrapperContainer.SerializeLazinator();
            hierarchy.MyChild1.IsDirty.Should().BeTrue();
            hierarchy.MyChild1.DescendantIsDirty.Should().BeTrue();
            hierarchy.MyChild1.HasChanged.Should().BeTrue();
            hierarchy.MyChild1.DescendantHasChanged.Should().BeTrue();
            hierarchy.MyChild1.MyWrapperContainer.IsDirty.Should().BeFalse();
            hierarchy.MyChild1.MyWrapperContainer.HasChanged.Should().BeTrue(); // here is the key difference -- it will stay in HasChanged until it is manually changed.
            hierarchy.DescendantHasChanged.Should().BeTrue();
            hierarchy.MyChild1.MyWrapperContainer.WrappedInt = 18;
            hierarchy.MyChild1.MyWrapperContainer.IsDirty.Should().BeTrue();
            hierarchy.MyChild1.MyWrapperContainer.HasChanged.Should().BeTrue();
            hierarchy.MyChild1.IsDirty.Should().BeTrue(); // hasn't changed
            hierarchy.MyChild1.HasChanged.Should().BeTrue();
            hierarchy.MyChild1.DescendantIsDirty.Should().BeTrue(); // this time, wrapper's dirtiness change, so MyChild1 is notified
            hierarchy.MyChild1.DescendantHasChanged.Should().BeTrue();
            
            var clone = hierarchy.CloneLazinatorTyped();
            // The following is the tricky part. We must make sure that UpdateStoredBuffer doesn't cause MyChild1 to think that no serialization is necessary.
            clone.MyChild1.MyWrapperContainer.WrappedInt.Should().Be(18);
            clone.MyChild1.IsDirty.Should().BeFalse();
            clone.MyChild1.HasChanged.Should().BeFalse();
            clone.MyChild1.DescendantIsDirty.Should().BeFalse();
            clone.MyChild1.DescendantHasChanged.Should().BeFalse();
            clone.DescendantHasChanged.Should().BeFalse();

            clone.MyChild1.MyWrapperContainer.WrappedInt = 16;
            clone.DescendantHasChanged.Should().BeTrue();
            clone.MyChild1.MyWrapperContainer.DescendantHasChanged.Should().BeTrue();
            clone.MyChild1.MyWrapperContainer.WrappedInt.HasChanged.Should().BeTrue();
            clone.MarkHierarchyClean();
            clone.DescendantHasChanged.Should().BeFalse();
            clone.MyChild1.MyWrapperContainer.DescendantHasChanged.Should().BeFalse();
            clone.MyChild1.MyWrapperContainer.WrappedInt.HasChanged.Should().BeFalse();

            var clone2 = hierarchy.CloneLazinatorTyped();
            clone2.MyChild1.MyWrapperContainer.WrappedInt = 25;
            clone2.MyChild1.MyWrapperContainer.SerializeLazinator();
            var clone3 = clone2.CloneLazinatorTyped();
            clone3.MyChild1.MyWrapperContainer.WrappedInt.Should().Be(25);

        }

        [Fact]
        public void DirtinessWorksAfterDotNetCollectionConverted()
        {
            DotNetHashSet_Lazinator l = new DotNetHashSet_Lazinator();
            l.MyHashSetSerialized = new HashSet<ExampleChild>();
            l.MyHashSetSerialized.Add(new ExampleChild());
            l.SerializeLazinator();
            l.IsDirty.Should().BeFalse();
            l.DescendantIsDirty.Should().BeFalse();
            var firstItem = l.MyHashSetSerialized.First();
            l.IsDirty.Should().BeTrue(); // should be true because .Net collection without special _Dirty property has been accessed
            firstItem.MyLong = 54321;
            var c = l.CloneLazinatorTyped();
            c.MyHashSetSerialized.First().MyLong.Should().Be(54321);
        }

        [Fact]
        public void DirtinessWorksForClassInStruct()
        {
            ExampleStructContainingClasses s = new ExampleStructContainingClasses()
            {
                MyChild1 = new ExampleChild()
            };
            var c = s.CloneLazinatorTyped();
            c.DescendantIsDirty.Should().BeFalse();
            c.MyChild1.MyLong = 23451243;
            c.DescendantIsDirty.Should().BeTrue();
        }

        [Fact]
        public void DirtinessNotificationOccursWithStructAsGeneric_ChangingFromDefault()
        {
            LazinatorTuple<ExampleStructContainingClasses, ExampleStructContainingClasses> e = new LazinatorTuple<ExampleStructContainingClasses, ExampleStructContainingClasses>();
            var c = e.CloneLazinatorTyped();
            c.Item1 = new ExampleStructContainingClasses() { MyChar = 'A' }; // because Item1 is set, IsDirty notification occurs
            c.IsDirty.Should().BeTrue();

            // The following code is invalid and thus doesn't present a problem.
            //e = new LazinatorTuple<ExampleStructContainingClasses, ExampleStructContainingClasses>();
            //c = e.CloneLazinatorTyped();
            //c.Item1.MyChar = 'A' ; // INVALID -- can't change MyChar b/c Item1 is a struct. Instead, we would do c.Item1 = c.Item1 { MyChar = 'A' }, which works as above
            //c.IsDirty.Should().BeTrue();
        }

        [Fact]
        public void DirtinessNotificationOccursWithStructAsGeneric()
        {
            LazinatorTuple<ExampleStructContainingClasses, ExampleStructContainingClasses> e = new LazinatorTuple<ExampleStructContainingClasses, ExampleStructContainingClasses>
            (
                new ExampleStructContainingClasses() { MyChar = 'B' },
                new ExampleStructContainingClasses() { MyChar = 'C' }
            );
            var c = e.CloneLazinatorTyped();
            c.Item1 = new ExampleStructContainingClasses() { MyChar = 'A' };
            c.IsDirty.Should().BeTrue();
        }

        [Fact]
        public void DirtinessNotificationOccursWithClassAsGeneric()
        {
            LazinatorTuple<ExampleChild, ExampleChild> e = new LazinatorTuple<ExampleChild, ExampleChild>
            (
                new ExampleChild() { MyShort = 23 },
                new ExampleChild() { MyShort = 24 }
            );
            var c = e.CloneLazinatorTyped();
            c.Item1 = new ExampleChild() { MyShort = 25 };
            c.IsDirty.Should().BeTrue();

            e = new LazinatorTuple<ExampleChild, ExampleChild>
            (
                new ExampleChild() { MyShort = 23 },
                new ExampleChild() { MyShort = 24 }
            );
            c = e.CloneLazinatorTyped();
            c.IsDirty.Should().BeFalse();

            e = new LazinatorTuple<ExampleChild, ExampleChild>
            (
                new ExampleChild() { MyShort = 23 },
                new ExampleChild() { MyShort = 24 }
            );
            c = e.CloneLazinatorTyped();
            c.Item1.MyShort = 25;
            c.IsDirty.Should().BeFalse();
            c.DescendantIsDirty.Should().BeTrue();
        }

        [Fact]
        public void DirtinessWithDefaultValueInConstructor()
        {
            ContainerForExampleWithDefault o = new ContainerForExampleWithDefault();
            var c = o.CloneLazinatorTyped();
            c.IsDirty.Should().BeFalse();
            c.DescendantIsDirty.Should().BeFalse();
            c.HasChanged.Should().BeFalse();
            c.DescendantHasChanged.Should().BeFalse();
            c.Example.MyChar.Should().Be('D');
        }

        [Fact]
        public void DirtinessWithEager()
        {
            ContainerForEagerExample o = new ContainerForEagerExample() { EagerExample = new Example() { MyChar = 'J' } };
            var c = o.CloneLazinatorTyped();
            c.IsDirty.Should().BeFalse(); // even though accessed, should not be dirty
            c.DescendantIsDirty.Should().BeFalse();
            c.HasChanged.Should().BeFalse();
            c.DescendantHasChanged.Should().BeFalse();
            FieldInfo privateFieldInfoForEagerChild = typeof(ContainerForEagerExample).GetField("_EagerExample_Accessed", BindingFlags.NonPublic | BindingFlags.Instance);
            bool accessed = (bool) privateFieldInfoForEagerChild.GetValue(c);
            accessed.Should().BeTrue();
            c.EagerExample.MyChar.Should().Be('J');
        }

        [Fact]
        public void ConstructorSetsProperty()
        {
            ContainerForExampleWithDefault o = new ContainerForExampleWithDefault();
            o.IsDirty.Should().BeTrue();
            o.Example.MyChar.Should().Be('D');
        }

        [Fact]
        public void DirtinessWithStructChild()
        {
            WrapperContainer e = new WrapperContainer()
            {
                WrappedInt = 3
            };
            e.WrappedInt.IsDirty.Should().BeTrue();
            e.DescendantIsDirty.Should().BeTrue();

            e.SerializeLazinator();
            var c = e.CloneLazinatorTyped();
            // consider original, which should be clean
            e.IsDirty.Should().BeFalse();
            e.DescendantIsDirty.Should().BeFalse();
            e.WrappedInt.IsDirty.Should().BeFalse();
            // now consider clone
            c.IsDirty.Should().BeFalse();
            c.DescendantIsDirty.Should().BeFalse();
            c.WrappedInt.IsDirty.Should().BeFalse();
        }

        [Fact]
        public void DirtinessWithOpenGenericStructChild()
        {
            LazinatorTuple<WInt32, WInt32> e = new LazinatorTuple<WInt32, WInt32>
            {
                Item1 = 3, 
                Item2 = 4
            };
            e.Item1.IsDirty.Should().BeTrue();
            e.DescendantIsDirty.Should().BeTrue();

            e.SerializeLazinator();
            var c = e.CloneLazinatorTyped();
            // consider original, which should be clean
            e.IsDirty.Should().BeFalse();
            e.DescendantIsDirty.Should().BeFalse();
            e.Item1.IsDirty.Should().BeFalse();
            // now consider clone
            c.IsDirty.Should().BeFalse();
            c.DescendantIsDirty.Should().BeFalse();
            c.Item1.IsDirty.Should().BeFalse();
        }

        [Fact]
        public void DirtinessWithNestedStructs()
        {
            ExampleStructContainingStructContainer e = new ExampleStructContainingStructContainer()
            {
                Subcontainer = new ExampleStructContainingStruct() { MyExampleStructContainingClasses = new ExampleStructContainingClasses() { MyChar = 'Z' } }
            };
            e.Subcontainer.IsDirty.Should().BeTrue();
            e.Subcontainer.MyExampleStructContainingClasses.IsDirty.Should().BeTrue();
            e.DescendantIsDirty.Should().BeTrue();

            e.SerializeLazinator();
            var c = e.CloneLazinatorTyped();
            // consider original, which should be clean
            e.IsDirty.Should().BeFalse();
            e.DescendantIsDirty.Should().BeFalse();
            e.Subcontainer.IsDirty.Should().BeFalse();
            e.Subcontainer.MyExampleStructContainingClasses.IsDirty.Should().BeFalse();
            // now consider clone
            c.IsDirty.Should().BeFalse();
            c.DescendantIsDirty.Should().BeFalse();
            c.Subcontainer.IsDirty.Should().BeFalse();
            c.Subcontainer.MyExampleStructContainingClasses.IsDirty.Should().BeFalse();
        }

        [Fact]
        public void MarkHierarchyClean_Example()
        {
            Example e = GetTypicalExample();
            e.SerializeLazinator();
            e.LazinatorMemoryStorage.IsEmpty.Should().BeFalse();
            e.MyChild1.MyExampleGrandchild.MyInt++;
            e.MyChild1.MyExampleGrandchild.IsDirty.Should().BeTrue();
            e.MyChild1.MyExampleGrandchild.HasChanged.Should().BeTrue();
            e.DescendantIsDirty.Should().BeTrue();
            e.DescendantHasChanged.Should().BeTrue();
            e.MarkHierarchyClean();
            e.MyChild1.MyExampleGrandchild.IsDirty.Should().BeFalse();
            e.MyChild1.MyExampleGrandchild.HasChanged.Should().BeFalse();
            e.DescendantIsDirty.Should().BeFalse();
            e.DescendantHasChanged.Should().BeFalse();
        }

        [Fact]
        public void MarkHierarchyClean_ExampleStructContainer()
        {
            var e = new ExampleContainerContainingClassesStructContainingClasses();
            e.SerializeLazinator();
            e.LazinatorMemoryStorage.IsEmpty.Should().BeFalse();
            e.IntWrapper++;
            e.IntWrapper.IsDirty.Should().BeTrue();
            e.IntWrapper.HasChanged.Should().BeTrue();
            e.DescendantIsDirty.Should().BeTrue();
            e.DescendantHasChanged.Should().BeTrue();
            e.MarkHierarchyClean();
            e.IntWrapper.IsDirty.Should().BeFalse();
            e.IntWrapper.HasChanged.Should().BeFalse();
            e.DescendantIsDirty.Should().BeFalse();
            e.DescendantHasChanged.Should().BeFalse();
        }

    }
}
