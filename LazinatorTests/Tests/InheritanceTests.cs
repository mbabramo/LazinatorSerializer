using System;
using FluentAssertions;
using Lazinator.Core;
using Xunit;
using LazinatorTests.Examples.Abstract;
using LazinatorTests.Examples.NonLazinator;
using LazinatorTests.Examples.Subclasses;

namespace LazinatorTests.Tests
{
    public class InheritanceTests : SerializationDeserializationTestBase
    {

        [Fact]
        public void LazinatorInheritanceWorks()
        {
            var original = GetHierarchy(1, 1, 3, 1, 2);
            var copy = GetHierarchy(1, 1, 3, 1, 2);
            var result = original.CloneLazinatorTyped();
            ExampleEqual(copy, result).Should().BeTrue();
        }

        [Fact]
        public void SubclassesWork()
        {
            ClassWithSubclass outer = new ClassWithSubclass()
            {
                IntWithinSuperclass = 5,
                SubclassInstance1 = new ClassWithSubclass.SubclassWithinClass()
                {
                    StringWithinSubclass = "within1"
                },
                SubclassInstance2 = new ClassWithSubclass.SubclassWithinClass()
                {
                    StringWithinSubclass = "within2"
                }
            };
            var clone = outer.CloneLazinatorTyped();
            clone.IntWithinSuperclass.Should().Be(5);
            clone.SubclassInstance1.StringWithinSubclass.Should().Be("within1");
            clone.SubclassInstance2.StringWithinSubclass.Should().Be("within2");
        }

        [Fact]
        public void ChildOfLazinatorWithoutAttributeWorks()
        {
            ChildOfLazinatorWithoutAttribute b = new ChildOfLazinatorWithoutAttribute()
            {
                MyInt = 3 // property defined virtually in base class
            };
            Action a = () =>
            {
                var typedClone = b.CloneLazinatorTyped();
            };
            a.Should().Throw<Exception>(); // we can't return an item of same type as ChildOfLazinatorWithoutAttribute, since Lazinator doesn't know about that type

            ILazinator untypedClone = b.CloneLazinator();
            (untypedClone as ChildOfLazinatorWithoutAttribute).Should().BeNull();
            (untypedClone as FromNonLazinatorBase).MyInt.Should().Be(3);
        }

        [Fact]
        public void ConcreteClassesInheritingFromAbstractWorks()
        {
            // serialize the concrete classes inheriting from the abstract ones
            Concrete5 c = new Concrete5()
            {
                String1 = "1",
                String2 = "2",
                String3 = "3",
                String4 = "4",
                String5 = "5"
            };
            var c2 = c.CloneLazinatorTyped();
            c2.String1.Should().Be("1");
            c2.String2.Should().Be("2");
            c2.String3.Should().Be("3");
            c2.String4.Should().Be("4");
            c2.String5.Should().Be("5");
        }
        [Fact]
        public void AbstractPropertySerializes()
        {
            ContainerWithAbstract1 c = new ContainerWithAbstract1()
            {
                AbstractProperty = new Concrete3() { String1 = "1", String2 = "2", String3 = "3" }
            };
            var c2 = c.CloneLazinatorTyped();
            var c2_abstractProperty = (c2.AbstractProperty as Concrete3);
            c2_abstractProperty.String1.Should().Be("1");
            c2_abstractProperty.String2.Should().Be("2");
            c2_abstractProperty.String3.Should().Be("3");
        }

        [Fact]
        public void PropertyChangedInConstructorSerializes()
        {
            // Concrete3's constructor sets Example2 and Example3 to null. This means that their _Accessed fields will be true.
            // When we deserialize ContainerWithAbstract1, we also deserialize Concrete3. If we did not call FreeInMemoryObjects,
            // then when we access Example2, it would appear that no deserialization is necessary, and Example2 will stay at its null value.
            // Both of the following examples fail without FreeInMemoryObjects().

            var concrete = new Concrete3()
            {
                Example2 = GetTypicalExample(),
                Example3 = GetTypicalExample()
            };
            var concrete2 = concrete.CloneLazinatorTyped();
            concrete2.Example2.Should().NotBeNull();
            concrete2.Example3.Should().NotBeNull();

            ContainerWithAbstract1 c = new ContainerWithAbstract1()
            {
                AbstractProperty = new Concrete3() { Example2 = GetTypicalExample(), Example3 = GetTypicalExample() }
            };
            var c2 = c.CloneLazinatorTyped();
            var c2_abstractProperty = (c2.AbstractProperty as Concrete3);
            c2_abstractProperty.Example2.Should().NotBeNull();
            c2_abstractProperty.Example3.Should().NotBeNull();
        }

        [Fact]
        public void LazinatorRecords()
        {
            LazinatorRecord r = new LazinatorRecord()
            {
                MyInt = 17,
                MyString = "hello"
            };
            var r2 = r.CloneLazinatorTyped();
            r2.MyInt.Should().Be(17);
            r2.MyString.Should().Be("hello");
        }

        [Fact]
        public void LazinatorRecordSubclass()
        {
            LazinatorRecordSubclass r = new LazinatorRecordSubclass()
            {
                MyInt = 17,
                MyString = "hello",
                MySubclassInt = 18,
            };
            var r2 = r.CloneLazinatorTyped();
            r2.MyInt.Should().Be(17);
            r2.MyString.Should().Be("hello");
            r2.MySubclassInt.Should().Be(18);
        }
    }
}
