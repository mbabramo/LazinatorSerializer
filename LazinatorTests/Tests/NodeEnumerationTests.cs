using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Lazinator.Collections;
using LazinatorTests.Examples;
using LazinatorTests.Examples.Collections;
using Lazinator.Core;
using Xunit;
using Lazinator.Wrappers;
using LazinatorTests.Examples.Structs;
using System.Dynamic;
using System;
using System.Text;

namespace LazinatorTests.Tests
{
    public class NodeEnumerationTests : SerializationDeserializationTestBase
    {


        [Fact]
        public void DirtinessEnumerationWorks()
        {
            var hierarchy = GetTypicalExample();
            var results = hierarchy.GetDirtyNodes(true).ToList();
            results.Count().Should().BeGreaterThan(1);

            var c = hierarchy.CloneLazinatorTyped();
            results = c.GetDirtyNodes(true).ToList();
            results.Count().Should().Be(0);

            var accessed = c.MyChild1;
            results = c.GetDirtyNodes(true).ToList();
            results.Count().Should().Be(0);

            c.MyUint = 12345;
            results = c.GetDirtyNodes(true).ToList();
            results.Count().Should().Be(1);
            results = c.GetDirtyNodes(true).ToList();
            results.Count().Should().Be(1); // still 1
            c.EnsureLazinatorMemoryUpToDate();
            results = c.GetDirtyNodes(true).ToList();
            results.Count().Should().Be(0); // nothing is dirty now

            c = c.CloneLazinatorTyped();
            c.MyChild1 = new ExampleChild();
            results = c.GetDirtyNodes(true).ToList();
            results.Count().Should().Be(2); // parent and MyChild1

            c = c.CloneLazinatorTyped();
            c.MyChild1.MyLong = 565656;
            results = c.GetDirtyNodes(true).ToList();
            results.Count().Should().Be(1); // child only is dirty
            (results[0] is ExampleChild).Should().BeTrue();

            c = c.CloneLazinatorTyped();
            c.MyChild1 = new ExampleChild();
            c = c.CloneLazinatorTyped();
            c.MyChild1 = new ExampleChild();
            c.MyChild1.MyLong = 7878;
            results = c.GetDirtyNodes(true).ToList();
            results.Count().Should().Be(2); // now, parent and child again

            c = c.CloneLazinatorTyped();
            c.MyChild1.MyWrapperContainer = new WrapperContainer() { WrappedInt = 8 };
            results = c.GetDirtyNodes(true).ToList();
            results.Count().Should().Be(3);
            (results[1] is WrapperContainer).Should().BeTrue();
            (results[2] is WInt).Should().BeTrue();

            c = c.CloneLazinatorTyped();
            c.MyChar = 'R';
            c.MyChild1.MyWrapperContainer.WrappedInt = 9;
            c.MyChild2 = new ExampleChild();
            results = c.GetDirtyNodes(true).ToList();
            results.Count().Should().Be(4);
            (results[0] is Example).Should().BeTrue();
            (results[1] is WrapperContainer).Should().BeTrue(); // wrapped int is a property of the container itself
            (results[2] is WInt).Should().BeTrue();
            (results[3] is ExampleChild).Should().BeTrue(); // i.e., MyChild2
            results = c.GetDirtyNodes(true).ToList();
            results.Count().Should().Be(4); // no change

            results = c.EnumerateLazinatorNodes(null, true, null, true, false).ToList();
            results.Count().Should().Be(1); // only highest dirty returned 
            (results[0] is Example).Should().BeTrue();

            results = c.EnumerateLazinatorNodes(null, false, x => !(x is WrapperContainer), true, false).ToList();
            results.Count().Should().Be(4); // WrapperContainer is yielded but not further explored; dirtiness is not a consideration

            results = c.EnumerateLazinatorNodes(x => x is WInt, false, null, true, false).ToList();
            results.Count().Should().Be(1); // full exploration, but only WInt returned 
            (results[0] is WInt).Should().BeTrue();
        }

        [Fact]
        public void LazinatorListDirtinessEnumerationWorks()
        {
            LazinatorListContainer nonGenericContainer = new LazinatorListContainer()
            {
            };
            nonGenericContainer.MyList = new LazinatorList<ExampleChild>();

            var v2 = nonGenericContainer.CloneLazinatorTyped();
            v2.MyList.Add(GetExampleChild(1));
            v2.MyList.Add(GetExampleChild(1));
            v2.MyList.Add(GetExampleChild(1));

            var results = v2.GetDirtyNodes(true);
            results.Count().Should().Be(7);

            var v5 = v2.CloneLazinatorTyped();
            v5.MyList[1].MyLong = -98765;
            results = v5.GetDirtyNodes(true);
            results.Count().Should().Be(1);
        }

        [Fact]
        public void ViewLazinatorChildrenWorks()
        {
            int GetCount(ExpandoObject eo)
            {
                return ((IDictionary<string, object>)eo).Count;
            }

            Example e = GetTypicalExample();
            dynamic deserializedOnly = e.ViewLazinatorChildren(true);
            dynamic all = e.ViewLazinatorChildren();
            (GetCount(all) > GetCount(deserializedOnly)).Should().BeTrue();
            (GetCount(deserializedOnly) > 1).Should().BeTrue();
            e = e.CloneLazinatorTyped();
            var loaded = e.MyChild1;
            deserializedOnly = e.ViewLazinatorChildren(true);
            GetCount(deserializedOnly).Should().Be(1);
            all = e.ViewLazinatorChildren();
            (GetCount(all) > 1).Should().BeTrue();
        }

        [Fact]
        public void LazinatorListEnumerateNodesWorks()
        {
            LazinatorList<Example> l = new LazinatorList<Example>() { GetExample(1), GetExample(1) };
            var c = l.CloneLazinatorTyped();
            var results2 = c.EnumerateAllNodes().ToList();
            results2[0].Should().Be(c);
            results2[1].LazinatorParents.LastAdded.Should().Be(c);
        }

        [Fact]
        public void LazinatorListForEachLazinatorWorks()
        {
            LazinatorList<WString> l = new LazinatorList<WString>() { "hello", "world" };
            StringBuilder sb = new StringBuilder();
            var c = l.ForEachLazinator(x =>
            {
                if (x is WString ws)
                    sb.Append(ws.WrappedValue);
                return x;
            }, true);
            sb.ToString().Should().Be("helloworld");
            l = l.CloneLazinatorTyped();
            sb = new StringBuilder();
            c = l.ForEachLazinator(x =>
            {
                if (x is WString ws)
                    sb.Append(ws.WrappedValue);
                return x;
            }, true);
            sb.ToString().Should().Be("");
            c = l.ForEachLazinator(x =>
            {
                if (x is WString ws)
                    sb.Append(ws.WrappedValue);
                return x;
            }, false); // now deserialize
            sb.ToString().Should().Be("helloworld");

        }

        [Fact]
        public void HierarchyTreeWorks_TwoLevel()
        {
            var hierarchy = GetTypicalExample();
            HierarchyTree tree = new HierarchyTree(hierarchy);
            string result = tree.ToString();
            string expected = 
$@"LazinatorTests.Examples.Example
    MyNullableDouble: 4.2
    MyBool: False
    MyChar: ⍂
    MyDateTime: 10/22/1972 5:36:00 PM
    MyNewString: NULL
    MyNullableDecimal: NULL
    MyNullableTimeSpan: 4.00:00:00
    MyOldString: NULL
    MyString: NULL
    MyStringBrotli: 
    MyTestEnum: MyTestValue3
    MyTestEnumByteNullable: MyTestValue
    MyUint: 1235
    MyNonLazinatorChild: LazinatorTests.Examples.NonLazinatorClass
    IncludableChild: NULL
    MyChild1: LazinatorTests.Examples.ExampleChild
        MyLong: 123123
        MyShort: 543
        ByteSpan: System.ReadOnlySpan<Byte>[0]
        MyExampleGrandchild: LazinatorTests.Examples.ExampleGrandchild
            AString: hello
            MyInt: 123
        MyWrapperContainer: NULL
    MyChild2: LazinatorTests.Examples.ExampleChild
        MyLong: 123123
        MyShort: 543
        ByteSpan: System.ReadOnlySpan<Byte>[0]
        MyExampleGrandchild: LazinatorTests.Examples.ExampleGrandchild
            AString: hello
            MyInt: 123
        MyWrapperContainer: NULL
    MyChild2Previous: NULL
    MyInterfaceImplementer: NULL
    WrappedInt: 2
        WrappedValue: 2
    ExcludableChild: NULL
";
            result.Should().Be(expected);
        }

        [Fact]
        public void HierarchyTreeWorks_Larger()
        {
            var hierarchy = GetHierarchy(0, 1, 2, 0, 0);
            hierarchy.MyChild1.MyWrapperContainer = new WrapperContainer() { WrappedInt = 17 };
            HierarchyTree tree = new HierarchyTree(hierarchy);
            string result = tree.ToString();
            string expected = 
$@"LazinatorTests.Examples.Example
    MyNullableDouble: 3.5
    MyBool: True
    MyChar: b
    MyDateTime: 1/1/2000 12:00:00 AM
    MyNewString: NULL
    MyNullableDecimal: -2341.5212352
    MyNullableTimeSpan: 03:00:00
    MyOldString: NULL
    MyString: this is a very long way of saying hello, world
    MyStringBrotli: this is a very long way of saying hello, world
    MyTestEnum: MyTestValue2
    MyTestEnumByteNullable: NULL
    MyUint: 2342343242
    MyNonLazinatorChild: NULL
    IncludableChild: NULL
    MyChild1: LazinatorTests.Examples.ExampleChild
        MyLong: 123123
        MyShort: 543
        ByteSpan: System.ReadOnlySpan<Byte>[0]
        MyExampleGrandchild: LazinatorTests.Examples.ExampleGrandchild
            AString: hello
            MyInt: 123
        MyWrapperContainer: LazinatorTests.Examples.Structs.WrapperContainer
            WrappedInt: 17
                WrappedValue: 17
    MyChild2: LazinatorTests.Examples.ExampleChild
        MyLong: 999888
        MyShort: -23
        ByteSpan: System.ReadOnlySpan<Byte>[0]
        MyExampleGrandchild: LazinatorTests.Examples.ExampleGrandchild
            AString: x
            MyInt: 3456345
        MyWrapperContainer: NULL
    MyChild2Previous: NULL
    MyInterfaceImplementer: NULL
    WrappedInt: 5
        WrappedValue: 5
    ExcludableChild: NULL
";
            result.Should().Be(expected);
        }
    }
}
