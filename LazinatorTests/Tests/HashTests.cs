﻿using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Lazinator.Collections;
using LazinatorTests.Examples;
using LazinatorTests.Examples.Collections;
using Lazinator.Exceptions;
using Lazinator.Support;
using Lazinator.Buffers;
using Lazinator.Core;
using LazinatorTests.Examples.Tuples;
using Xunit;
using ExampleNonexclusiveInterfaceImplementer = LazinatorTests.Examples.ExampleNonexclusiveInterfaceImplementer;
using Lazinator.Wrappers;
using System.Buffers;
using System.Reflection;
using Lazinator.Spans;
using System.Collections;
using LazinatorTests.Examples.Abstract;
using LazinatorTests.Examples.Hierarchy;
using LazinatorTests.Examples.NonLazinator;
using LazinatorTests.Examples.Structs;
using LazinatorTests.Examples.Subclasses;
using LazinatorTests.Examples.NonAbstractGenerics;
using System.Text;

namespace LazinatorTests.Tests
{
    public class HashTests : SerializationDeserializationTestBase
    {
        [Theory]
        [InlineData("", 0xdc56d17au)]
        [InlineData("a", 0x3c973d4du)]
        [InlineData("ab", 0x417330fdu)]
        [InlineData("abc", 0x2f635ec7u)]
        [InlineData("abcd", 0x98b51e95u)]
        [InlineData("hi", 0xf2311502u)]
        [InlineData("abcde", 0xa3f366acu)]
        [InlineData("abcdef", 0x0f813aa4u)]
        [InlineData("abcdefg", 0x21deb6d7u)]
        [InlineData("abcdefgh", 0xfd7ec8b9u)]
        [InlineData("abcdefghi", 0x6f98dc86u)]
        [InlineData("abcdefghij", 0xf2669361u)]
        [InlineData("hello world", 0x19a7581au)]
        [InlineData("fred@example.com", 0x7acdc357u)]
        [InlineData("lee@lmmrtech.com", 0xaf0a30feu)]
        [InlineData("docklandsman@gmail.com", 0x5d8cdbf4u)]
        [InlineData("size:  a.out:  bad magic", 0xc6246b8du)]
        [InlineData("Go is a tool for managing Go source code.Usage:	go command [arguments]The commands are:    build       compile packages and dependencies    clean       remove object files    env         print Go environment information    fix         run go tool fix on packages    fmt         run gofmt on package sources    generate    generate Go files by processing source    get         download and install packages and dependencies    install     compile and install packages and dependencies    list        list packages    run         compile and run Go program    test        test packages    tool        run specified go tool    version     print Go version    vet         run go tool vet on packagesUse go help [command] for more information about a command.Additional help topics:    c           calling between Go and C    filetype    file types    gopath      GOPATH environment variable    importpath  import path syntax    packages    description of package lists    testflag    description of testing flags    testfunc    description of testing functionsUse go help [topic] for more information about that topic.", 0x9c8f96f3u)]
        [InlineData("Discard medicine more than two years old.", 0xe273108fu)]
        [InlineData("He who has a shady past knows that nice guys finish last.", 0xf585dfc4u)]
        [InlineData("I wouldn't marry him with a ten foot pole.", 0x363394d1u)]
        [InlineData("Free! Free!/A trip/to Mars/for 900/empty jars/Burma Shave", 0x7613810fu)]
        [InlineData("The days of the digital watch are numbered.  -Tom Stoppard", 0x2cc30bb7u)]
        [InlineData("Nepal premier won't resign.", 0x322984d9u)]
        [InlineData("For every action there is an equal and opposite government program.", 0xa5812ac8u)]
        [InlineData("His money is twice tainted: 'taint yours and 'taint mine.", 0x1090d244u)]
        [InlineData("There is no reason for any individual to have a computer in their home. -Ken Olsen, 1977", 0xff16c9e6u)]
        [InlineData("It's a tiny change to the code and not completely disgusting. - Bob Manchek", 0xcc3d0ff2u)]
        [InlineData("The major problem is with sendmail.  -Mark Horton", 0xd225e92eu)]
        [InlineData("Give me a rock, paper and scissors and I will move the world.  CCFestoon", 0x1b8db5d0u)]
        [InlineData("If the enemy is within range, then so are you.", 0x4fda5f07u)]
        [InlineData("It's well we cannot hear the screams/That we create in others' dreams.", 0x2e18e880u)]
        [InlineData("You remind me of a TV show, but that's all right: I watch it anyway.", 0xd07de88fu)]
        [InlineData("C is as portable as Stonehedge!!", 0x221694e4u)]
        [InlineData("Even if I could be Shakespeare, I think I should still choose to be Faraday. - A. Huxley", 0xe2053c2cu)]
        [InlineData("The fugacity of a constituent in a mixture of gases at a given temperature is proportional to its mole fraction.  Lewis-Randall Rule", 0x11c493bbu)]
        [InlineData("How can you write a big system without C++?  -Paul Glick", 0x0819a4e8u)]
        public void TestHash32(String str, uint expected)
        {
            var bytes = Encoding.ASCII.GetBytes(str);
            Span<byte> sp = bytes;
            Assert.Equal(FarmhashByteSpans.Hash32(sp), expected);
        }

        [Theory]
        [InlineData("", 0x9ae16a3b2f90404fUL)]
        [InlineData("a", 0xb3454265b6df75e3UL)]
        [InlineData("ab", 0xaa8d6e5242ada51eUL)]
        [InlineData("abc", 0x24a5b3a074e7f369UL)]
        [InlineData("abcd", 0x1a5502de4a1f8101UL)]
        [InlineData("abcde", 0xc22f4663e54e04d4UL)]
        [InlineData("abcdef", 0xc329379e6a03c2cdUL)]
        [InlineData("abcdefg", 0x3c40c92b1ccb7355UL)]
        [InlineData("abcdefgh", 0xfee9d22990c82909UL)]
        [InlineData("abcdefghi", 0x332c8ed4dae5ba42UL)]
        [InlineData("abcdefghij", 0x8a3abb6a5f3fb7fbUL)]
        [InlineData("hi", 0x6a5d2fba44f012f8UL)]
        [InlineData("hello world", 0x588fb7478bd6b01bUL)]
        [InlineData("lee@lmmrtech.com", 0x61bec68db00fa2ffUL)]
        [InlineData("fred@example.com", 0x7fbbcd6191d8dce0UL)]
        [InlineData("size:  a.out:  bad magic", 0x80d73b843ba57db8UL)]
        [InlineData("docklandsman@gmail.com", 0xb678cf3842309f40UL)]
        [InlineData("Nepal premier won't resign.", 0x8eb3808d1ccfc779UL)]
        [InlineData("C is as portable as Stonehedge!!", 0xb944f8a16261e414UL)]
        [InlineData("Discard medicine more than two years old.", 0x2d072041b535155dUL)]
        [InlineData("He who has a shady past knows that nice guys finish last.", 0x9f9e3cdeb570f926UL)]
        [InlineData("I wouldn't marry him with a ten foot pole.", 0x361b79df08615cd6UL)]
        [InlineData("Free! Free!/A trip/to Mars/for 900/empty jars/Burma Shave", 0xdcfb73d4de1111c6UL)]
        [InlineData("The days of the digital watch are numbered.  -Tom Stoppard", 0xd71bdfedb6182a5dUL)]
        [InlineData("His money is twice tainted: 'taint yours and 'taint mine.", 0x3df4b8e109629602UL)]
        [InlineData("The major problem is with sendmail.  -Mark Horton", 0x1da6c1dfec23a597UL)]
        [InlineData("If the enemy is within range, then so are you.", 0x1f232f3375914f0aUL)]
        [InlineData("How can you write a big system without C++?  -Paul Glick", 0xa29944470950e8e4UL)]
        [InlineData("For every action there is an equal and opposite government program.", 0x8452fbb0c8f98c4fUL)]
        [InlineData("There is no reason for any individual to have a computer in their home. -Ken Olsen, 1977", 0x7fee06e367562d44UL)]
        [InlineData("It's a tiny change to the code and not completely disgusting. - Bob Manchek", 0x889b024bab17bf54UL)]
        [InlineData("Give me a rock, paper and scissors and I will move the world.  CCFestoon", 0xb8e2918a4398348dUL)]
        [InlineData("It's well we cannot hear the screams/That we create in others' dreams.", 0x796229f1faacec7eUL)]
        [InlineData("You remind me of a TV show, but that's all right: I watch it anyway.", 0x98d2fbd5131a5860UL)]
        [InlineData("Even if I could be Shakespeare, I think I should still choose to be Faraday. - A. Huxley", 0x4c349a4ff7ac0c89UL)]
        [InlineData("The fugacity of a constituent in a mixture of gases at a given temperature is proportional to its mole fraction.  Lewis-Randall Rule", 0x98eff6958c5e91aUL)]
        [InlineData("Go is a tool for managing Go source code.Usage: go command [arguments]The commands are:    build       compile packages and dependencies    clean       remove object files    env         print Go environment information    fix         run go tool fix on packages    fmt         run gofmt on package sources    generate    generate Go files by processing source    get         download and install packages and dependencies    install     compile and install packages and dependencies    list        list packages    run         compile and run Go program    test        test packages    tool        run specified go tool    version     print Go version    vet         run go tool vet on packagesUse go help [command] for more information about a command.Additional help topics:    c           calling between Go and C    filetype    file types    gopath      GOPATH environment variable    importpath  import path syntax    packages    description of package lists    testflag    description of testing flags    testfunc    description of testing functionsUse go help [topic] for more information about that topic.", 0x21609f6764c635edUL)]
        public void TestHash64(String str, ulong expected)
        {
            var bytes = Encoding.ASCII.GetBytes(str);
            Span<byte> sp = bytes;
            Assert.Equal(FarmhashByteSpans.Hash64(sp), expected);
        }

        [Fact]
        public void BinaryHashCodesWork()
        {
            var example = GetHierarchy(1, 1, 1, 1, 0);
            var hash32 = example.GetBinaryHashCode32();
            var hash64 = example.GetBinaryHashCode64();
            var clone = example.CloneLazinatorTyped();
            clone.GetBinaryHashCode32().Should().Be(hash32);
            clone.GetBinaryHashCode64().Should().Be(hash64);
            var anotherExample = GetHierarchy(1, 1, 1, 1, 0);
            anotherExample.GetBinaryHashCode64().Should().Be(hash64);

            example.MyBool = !example.MyBool;
            var hash32b = example.GetBinaryHashCode32();
            var hash64b = example.GetBinaryHashCode64();
            hash32b.Should().NotBe(hash32);
            hash64b.Should().NotBe(hash64);
            var clone2 = example.CloneLazinatorTyped();
            clone2.GetBinaryHashCode32().Should().Be(hash32b);
            clone2.GetBinaryHashCode64().Should().Be(hash64b);

            example.MyChild1.MyShort = (short)9999;
            example.MyChild1.MyLong = 987654987654; // TODO: this doesn't seem to work if long isn't changed. 
            var hash32c = example.GetBinaryHashCode32();
            var hash64c = example.GetBinaryHashCode64();
            hash32c.Should().NotBe(hash32);
            hash64c.Should().NotBe(hash64);
            hash32c.Should().NotBe(hash32b);
            hash64c.Should().NotBe(hash64b);
            var clone3 = example.CloneLazinatorTyped();
            clone3.GetBinaryHashCode32().Should().Be(hash32c);
            clone3.GetBinaryHashCode64().Should().Be(hash64c);
        }

        [Fact]
        void BinaryHashInList()
        {
            var wrapped = new WInt(1);
            var wrapped2 = new WInt(1);
            LazinatorList<WInt> x = new LazinatorList<WInt>();
            x.Add(wrapped2);
            x.GetListMemberHash32(0).Should().Be(wrapped.GetBinaryHashCode32());
            var clone = x.CloneLazinatorTyped();
            clone.GetListMemberHash32(0).Should().Be(wrapped.GetBinaryHashCode32());
        }

        [Fact]
        void BinaryHashCanBeAssignedToPropertyOfItem()
        {
            // the challenge here is that the call to GetBinaryHashCode results in a ConvertToBytes. Meanwhile, the object may have been accessed already on the left side of the assignment. We want to make sure this doesn't cause any problems.
            Example e = GetHierarchy(1, 1, 1, 1, 0);
            e.MyChild1.MyLong = (long)e.MyChild1.GetBinaryHashCode64();
            e.MyChild1.Should().NotBeNull();
            e.MyChild1.MyLong.Should().NotBe(0);
            var c = e.CloneLazinatorTyped();
            c.MyChild1.MyLong = 0;
            c.MyChild1.MyLong = (long)e.MyChild1.GetBinaryHashCode64();
            c.MyChild1.Should().NotBeNull();
            c.MyChild1.MyLong.Should().NotBe(0);
            Example e2 = GetHierarchy(1, 1, 1, 1, 0);
            e2.MyChild1 = new ExampleChild();
            e2.MyChild1.MyLong = 0;
            e2.MyChild1.MyLong = (long)e.MyChild1.GetBinaryHashCode64();
            e2.MyChild1.Should().NotBeNull();
            e2.MyChild1.MyLong.Should().NotBe(0);
        }
    }
}
