using System.Collections.Generic;
using FluentAssertions;
using LazinatorTests.Examples;
using Lazinator.Core;
using Xunit;
using ExampleNonexclusiveInterfaceImplementer = LazinatorTests.Examples.ExampleNonexclusiveInterfaceImplementer;
using Lazinator.Wrappers;
using LazinatorTests.Examples.Abstract;
using LazinatorTests.Examples.NonAbstractGenerics;

namespace LazinatorTests.Tests
{
    public class GenericTests : SerializationDeserializationTestBase
    {

        [Fact]
        public void OpenGenericWithConcreteTypeSerializes()
        {
            OpenGenericStayingOpenContainer x = new OpenGenericStayingOpenContainer()
            {
                ClosedGenericFloat = new OpenGeneric<WFloat>()
                {
                    MyT = new WFloat(3.4F)
                }
            };
            var c = x.CloneLazinatorTyped();
            c.ClosedGenericFloat.MyT.WrappedValue.Should().Be(3.4F);
        }

        [Fact]
        public void OpenGenericWithInterfaceTypeSerializes()
        {
            OpenGenericStayingOpenContainer x = new OpenGenericStayingOpenContainer()
            {
                ClosedGenericInterface = new OpenGeneric<IExampleChild>()
                {
                    MyT = new ExampleChild() { MyShort = 45 }
                }
            };
            var c = x.CloneLazinatorTyped();
            ((ExampleChild)c.ClosedGenericInterface.MyT).MyShort.Should().Be(45);

            x = new OpenGenericStayingOpenContainer()
            {
                ClosedGenericInterface = new OpenGeneric<IExampleChild>()
                {
                    MyT = new ExampleChildInherited() { MyShort = 45, MyInt = -75 }
                }
            };
            c = x.CloneLazinatorTyped();
            ((ExampleChildInherited)c.ClosedGenericInterface.MyT).MyShort.Should().Be(45);
            ((ExampleChildInherited)c.ClosedGenericInterface.MyT).MyInt.Should().Be(-75);
        }

        [Fact]
        public void OpenGenericWithNonexclusiveInterfaceTypeSerializes()
        {
            OpenGenericStayingOpenContainer x = new OpenGenericStayingOpenContainer()
            {
                ClosedGenericNonexclusiveInterface = new OpenGeneric<IExampleNonexclusiveInterface>()
                {
                    MyT = new ExampleNonexclusiveInterfaceImplementer() { MyInt = 45 }
                }
            };
            var c = x.CloneLazinatorTyped();
            ((IExampleNonexclusiveInterface)c.ClosedGenericNonexclusiveInterface.MyT).MyInt.Should().Be(45);
            ((ExampleNonexclusiveInterfaceImplementer)c.ClosedGenericNonexclusiveInterface.MyT).MyInt.Should().Be(45);

        }

        [Fact]
        public void ClosedGenericSerializes()
        {
            ClosedGeneric g = new ClosedGeneric() { MyT = new ExampleChild() { MyLong = 1 }, MyListT = new List<ExampleChild> { new ExampleChild() { MyLong = 2 }, new ExampleChild() { MyLong = 3 } } };
            var c = g.CloneLazinatorTyped();
            c.MyT.MyLong.Should().Be(1);
            c.MyListT[0].MyLong.Should().Be(2);
            c.MyListT[1].MyLong.Should().Be(3);
        }

        [Fact]
        public void ConcreteFromGenericFromBaseSerializes()
        {
            ConcreteFromGenericFromBase f = new ConcreteFromGenericFromBase()
            {
                MyT = 3.4M
            };
            var c = f.CloneLazinatorTyped();
            c.MyT.Should().Be(3.4M);
        }

        [Fact]
        public void ConcreteGenericClassesSerialize()
        {
            ConcreteGeneric2a cg2a = new ConcreteGeneric2a()
            {
                AnotherProperty = "hi",
                MyT = 5, // now is an int
                LazinatorExample = GetExample(2),
            };
            var cg2a_clone = cg2a.CloneLazinatorTyped();
            cg2a_clone.AnotherProperty.Should().Be("hi");
            cg2a_clone.MyT.Should().Be(5);
            cg2a_clone.LazinatorExample.Should().NotBeNull();

            ConcreteGeneric2b cg2b = new ConcreteGeneric2b()
            {
                AnotherProperty = "hi",
                MyT = GetExample(2),
                LazinatorExample = GetExample(1),
            };
            var cg2b_clone = cg2b.CloneLazinatorTyped();
            cg2b_clone.AnotherProperty.Should().Be("hi");
            cg2b_clone.MyT.Should().NotBeNull();
            cg2b_clone.LazinatorExample.Should().NotBeNull();
        }

        [Fact]
        public void ConcreteGenericContainerWorks()
        {
            ConcreteGenericContainer c = new ConcreteGenericContainer()
            {
                Item = new ConcreteGeneric2a()
                {
                    AnotherProperty = "hi",
                    MyT = 5, // now is an int
                    LazinatorExample = GetExample(2),
                },
            };
            var c2 = c.CloneLazinatorTyped();
            ExampleEqual(((ConcreteGeneric2a)c.Item).LazinatorExample, ((ConcreteGeneric2a)c2.Item).LazinatorExample)
                .Should().BeTrue();
        }

        [Fact]
        public void ConcreteGenericContainerWithDerivedGeneric()
        {
            DerivedGenericContainer<WInt> c = new DerivedGenericContainer<WInt>()
            {
                Item = new DerivedGeneric2c<WInt>()
                {
                    MyT = 5 // now is a wrapped int -- note that Item is defined as being IAbstract<T>
                },
            };
            var c2 = c.CloneLazinatorTyped();
            var item = c2.Item;
            ((DerivedGeneric2c<WInt>)item).MyT.WrappedValue.Should().Be(5);
        }

        [Fact]
        public void GenericFromBase()
        {
            GenericFromBase<WInt> c = new GenericFromBase<WInt>()
            {
                MyT = 5
            };
            var c2 = c.CloneLazinatorTyped();
            var item = c2.MyT;
            item.WrappedValue.Should().Be(5);
        }

        [Fact]
        public void GenericFromBaseInContainer()
        {
            GenericFromBase<WInt> g = new GenericFromBase<WInt>()
            {
                MyT = 5
            };
            BaseContainer c = new BaseContainer()
            {
                MyBase = g
            };
            var c2 = c.CloneLazinatorTyped();
            var item = ((GenericFromBase<WInt>)c2.MyBase);
            item.MyT.WrappedValue.Should().Be(5);
        }
    }
}
