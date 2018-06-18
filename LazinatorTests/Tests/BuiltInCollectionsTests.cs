using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Lazinator.Collections;
using LazinatorTests.Examples;
using LazinatorTests.Examples.Collections;
using Lazinator.Core;
using LazinatorTests.Examples.Tuples;
using Xunit;
using Lazinator.Wrappers;
using LazinatorTests.Examples.Structs;

namespace LazinatorTests.Tests
{
    public class BuiltInCollectionsTests : SerializationDeserializationTestBase
    {
        [Fact]
        public void LazinatorTuple()
        {
            LazinatorTuple<WInt, WString> item =
                new LazinatorTuple<WInt, WString>(5, "hello");
            var clone = item.CloneLazinatorTyped();
            clone.Item1.WrappedValue.Should().Be(5);
            clone.Item2.WrappedValue.Should().Be("hello");
        }

        [Fact]
        public void LazinatorTupleWithLazinatorStruct()
        {

            void ConfirmSerializeAndDeserialize()
            {
                RegularTuple GetObject()
                {
                    return new RegularTuple()
                    {
                        MyTupleSerialized4 = new Tuple<int, ExampleStruct>(2, new ExampleStruct() { MyChar = '1' })
                    };
                }

                var original = GetObject();
                var result = original.CloneLazinatorTyped();
                result.MyTupleSerialized4.Item1.Should().Be(original.MyTupleSerialized4.Item1);
                result.MyTupleSerialized4.Item2.MyChar.Should().Be(original.MyTupleSerialized4.Item2.MyChar);
            }

            ConfirmSerializeAndDeserialize();
        }

        [Fact]
        public void LazinatorListWithObjectsWorks()
        {
            var original = new LazinatorListContainer()
            {
                MyList = new LazinatorList<ExampleChild>()
                {
                    new ExampleChild() { MyShort = 22 },
                    new ExampleChildInherited() { MyShort = 21, MyInt = 23 }
                }
            };
            var clone = original.CloneLazinatorTyped(); // second clone
            var list = clone.MyList;
            list.Should().NotBeNull();
            list[0].MyShort.Should().Be(22);
            var innerDerived = list[1] as ExampleChildInherited;
            innerDerived.Should().NotBeNull();
            innerDerived.MyShort.Should().Be(21);
            innerDerived.MyInt.Should().Be(23);
        }

        [Fact]
        public void LazinatorListWithStructsWorks()
        {
            LazinatorList<WInt> l = new LazinatorList<WInt>() { 3 };

            var c = l.CloneLazinatorTyped();
            c[0].WrappedValue.Should().Be(3);

            c = l.CloneLazinatorTyped();
            c.Insert(0, 2);
            var c2 = c.CloneLazinatorTyped();
            c2[0].WrappedValue.Should().Be(2);
            c2[1].WrappedValue.Should().Be(3);

            c = l.CloneLazinatorTyped();
            c.Add(4);
            var c3 = c.CloneLazinatorTyped();
            c3[0].WrappedValue.Should().Be(3);
            c3[1].WrappedValue.Should().Be(4);
            c3.Add(5);
            c3[2].Should().Be(5);

            c = c2.CloneLazinatorTyped();
            var accessed = c[1];
            var c2b = c.CloneLazinatorTyped();
            c2b[0].WrappedValue.Should().Be(2);
            c2b[1].WrappedValue.Should().Be(3);

        }

        [Fact]
        public void LazinatorListEnumeratorWorks()
        {
            LazinatorList<Example> l = new LazinatorList<Example>() { GetExample(1), GetExample(1) };
            var results = l.ToList();
            results[0].LazinatorParentClass.Should().Be(l);
            results[1].LazinatorParentClass.Should().Be(l);
            var c = l.CloneLazinatorTyped();
            results = c.ToList();
            results[0].LazinatorParentClass.Should().Be(c);
            results[1].LazinatorParentClass.Should().Be(c);
            c = l.CloneLazinatorTyped();
            foreach (var result in c)
                result.LazinatorParentClass.Should().Be(c);

        }

        [Fact]
        public void DerivedLazinatorListWithObjectsWorks()
        {
            var original = new LazinatorListContainer()
            {
                MyList = new DerivedLazinatorList<ExampleChild>()
                {
                    new ExampleChild() { MyShort = 22 },
                    new ExampleChildInherited() { MyShort = 21, MyInt = 23 }
                }
            };
            var clone = original.CloneLazinatorTyped(); // second clone
            var list = clone.MyList;
            list.Should().NotBeNull();
            list[0].MyShort.Should().Be(22);
            var innerDerived = list[1] as ExampleChildInherited;
            innerDerived.Should().NotBeNull();
            innerDerived.MyShort.Should().Be(21);
            innerDerived.MyInt.Should().Be(23);
        }
        [Fact]
        public void FastReadListIntWorks()
        {
            LazinatorFastReadList<int> r = new LazinatorFastReadList<int>();
            r.AsList = new List<int>() { 3, 4, 5 };
            r.IsDirty.Should().BeTrue();
            LazinatorFastReadList<int> r2 = r.CloneLazinatorTyped();
            r2[0].Should().Be(3);
            r2.IsDirty.Should().BeFalse();
            r2.AsList.Add(6);
            r2.IsDirty.Should().BeTrue();
            LazinatorFastReadList<int> r3 = r2.CloneLazinatorTyped();
            r3.AsList.Count().Should().Be(4);
        }

        [Fact]
        public void OffsetListWorks()
        {
            LazinatorOffsetList x = new LazinatorOffsetList();
            List<int> list = new List<int>() { 3, 4, 1600, 234234234, 234234345 };
            void CheckList()
            {
                for (int i = 0; i < list.Count; i++)
                    x[i].Should().Be(list[i]);
            }
            void CheckBeforeAndAfterSerialization()
            {
                CheckList();
                x = x.CloneLazinatorTyped();
                CheckList();
                x = x.CloneLazinatorTyped();
                CheckList();
            }
            // set the list
            foreach (int i in list)
                x.AddOffset(i);
            CheckBeforeAndAfterSerialization();
            // set the list a diferent way
            x.SetOffsets(list);
            CheckBeforeAndAfterSerialization();
            // change the list
            list.Add(234234346);
            x.AddOffset(234234346);
            CheckBeforeAndAfterSerialization();
            // change the list again
            x.SetOffsets(list);
            CheckBeforeAndAfterSerialization();
            // change the list again, with only large numbers
            list = new List<int>() { 234234234, 234234345, 999999999 };
            x.SetOffsets(list);
            CheckBeforeAndAfterSerialization();
            // change the list again, with only a small number
            list = new List<int>() { 0 };
            x.SetOffsets(list);
            CheckBeforeAndAfterSerialization();
            // change the list again, with a few small numbers
            list = new List<int>() { 2, 280, 9000 };
            x.SetOffsets(list);
            CheckBeforeAndAfterSerialization();
            // now try an empty list
            x = new LazinatorOffsetList();
            list = new List<int>();
            CheckBeforeAndAfterSerialization();
        }

        public enum ContainerForLazinatorList
        {
            GenericContainer,
            NonGenericContainer,
            NoContainer
        }

        [InlineData(ContainerForLazinatorList.GenericContainer)]
        [InlineData(ContainerForLazinatorList.NonGenericContainer)]
        [InlineData(ContainerForLazinatorList.NoContainer)]
        [Theory]
        public void LazinatorListWorks(ContainerForLazinatorList containerOption)
        {
            LazinatorListContainer nonGenericContainer = new LazinatorListContainer()
            {
            };
            LazinatorListContainerGeneric<ExampleChild> genericContainer = new LazinatorListContainerGeneric<ExampleChild>()
            {
            };
            LazinatorList<ExampleChild> withoutContainer = null;
            LazinatorList<ExampleChild> GetList()
            {
                switch (containerOption)
                {
                    case ContainerForLazinatorList.NoContainer:
                        return withoutContainer;
                    case ContainerForLazinatorList.NonGenericContainer:
                        return nonGenericContainer.MyList;
                    case ContainerForLazinatorList.GenericContainer:
                        return genericContainer.MyList;
                }
                throw new NotImplementedException();
            }
            void SetList(LazinatorList<ExampleChild> value)
            {
                switch (containerOption)
                {
                    case ContainerForLazinatorList.NoContainer:
                        withoutContainer = value;
                        break;
                    case ContainerForLazinatorList.NonGenericContainer:
                        nonGenericContainer.MyList = value;
                        break;
                    case ContainerForLazinatorList.GenericContainer:
                        genericContainer.MyList = value;
                        break;
                }
            }

            SetList(new LazinatorList<ExampleChild>()
            {
            });

            LazinatorList<ExampleChild> list = new LazinatorList<ExampleChild>();
            for (int i = 0; i <= 3; i++)
            {
                AddItem(i);
                list.IsDirty.Should().BeTrue();

            }
            void AddItem(int i, int? insertIndex = null)
            {
                if (insertIndex is int insertIndexInt)
                {
                    GetList().Insert(insertIndexInt, GetExampleChild(i));
                    list.Insert(insertIndexInt, GetExampleChild(i));
                }
                else
                {
                    GetList().Add(GetExampleChild(i));
                    list.Add(GetExampleChild(i));
                }
            }
            void RemoveItem(int i, bool useRemoveAt)
            {
                if (useRemoveAt)
                {
                    GetList().RemoveAt(i);
                    list.RemoveAt(i);
                }
                else
                {
                    var item = GetList()[i];
                    GetList().Remove(item);
                    list.RemoveAt(i); // not testing this, so can just do this
                }
            }
            void Clear()
            {
                GetList().Clear();
                list.Clear();
                list.IsDirty.Should().BeTrue();
            }
            void CheckList()
            {
                for (int i = 0; i < list.Count; i++)
                {
                    var currentListItem = GetList()[i];
                    if (currentListItem == null)
                        list[i].Should().Be(null);
                    else
                        ExampleChildEqual(currentListItem, list[i]).Should().BeTrue();
                }
                // now check another way, using enumerables
                var zipped = GetList().Zip(list, (a, b) => (a, b));
                foreach (var zip in zipped)
                    ExampleChildEqual(zip.a, zip.b).Should().BeTrue();
            }
            void CheckBeforeAndAfterSerialization()
            {
                CheckList();

                switch (containerOption)
                {
                    case ContainerForLazinatorList.NoContainer:
                        SetList(GetList().CloneLazinatorTyped());
                        break;
                    case ContainerForLazinatorList.NonGenericContainer:
                        nonGenericContainer = nonGenericContainer.CloneLazinatorTyped();
                        break;
                    case ContainerForLazinatorList.GenericContainer:
                        genericContainer = genericContainer.CloneLazinatorTyped();
                        break;
                }

                CheckList();
            }

            CheckBeforeAndAfterSerialization();
            CheckBeforeAndAfterSerialization(); // do it again
            AddItem(1);
            CheckBeforeAndAfterSerialization();
            AddItem(0);
            CheckBeforeAndAfterSerialization();
            AddItem(2, 1);
            CheckBeforeAndAfterSerialization();
            AddItem(0, 0);
            CheckBeforeAndAfterSerialization();
            RemoveItem(3, true);
            CheckBeforeAndAfterSerialization();
            RemoveItem(3, false);
            CheckBeforeAndAfterSerialization();
            RemoveItem(0, false);
            CheckBeforeAndAfterSerialization();
            RemoveItem(0, true);
            CheckBeforeAndAfterSerialization();
            Clear();
            CheckBeforeAndAfterSerialization();
        }

        [Fact]
        public void LazinatorListWorksWithPartialAccessAfterChange()
        {
            LazinatorList<WInt> l = new LazinatorList<WInt>();
            l.Add(3);
            l.Add(4);
            var c = l.CloneLazinatorTyped();
            c[0] = 999999;
            var c2 = c.CloneLazinatorTyped();
            c2[1].WrappedValue.Should().Be(4);
        }

        [Fact]
        public void LazinatorListEmptyWorks()
        {
            LazinatorListContainer nonGenericContainer = new LazinatorListContainer()
            {
            };
            nonGenericContainer.MyList = new LazinatorList<ExampleChild>();
            var clone = nonGenericContainer.CloneLazinatorTyped();
            var listInClone = clone.MyList;
            listInClone.Should().NotBeNull();

            // and again
            clone = nonGenericContainer.CloneLazinatorTyped();
            listInClone = clone.MyList;
            listInClone.Should().NotBeNull();
        }

        [Fact]
        public void LazinatorListNullWorks()
        {
            LazinatorListContainer nonGenericContainer = new LazinatorListContainer()
            {
            };
            nonGenericContainer.MyList = null;
            var clone = nonGenericContainer.CloneLazinatorTyped();
            var listInClone = clone.MyList;
            listInClone.Should().BeNull();

            // and again
            clone = nonGenericContainer.CloneLazinatorTyped();
            listInClone = clone.MyList;
            listInClone.Should().BeNull();
        }

        [Fact]
        public void EmptyLazinatorListWorks()
        {
            LazinatorList<WInt> l = new LazinatorList<WInt>();
            var c = l.CloneLazinatorTyped();
            c.Count().Should().Be(0);
        }

        [Fact]
        public void ShortLazinatorListWorks()
        {
            LazinatorList<WInt> l = new LazinatorList<WInt>();
            l.Add(3);
            var c = l.CloneLazinatorTyped();
            var result = c[0];
            result.Should().Be(3);
        }

        [Fact]
        public void LazinatorListItemParentSet()
        {
            LazinatorList<ExampleChild> l = new LazinatorList<ExampleChild>()
            {
                new ExampleChild(),
                new ExampleChild()
            };
            l[0].LazinatorParentClass.Should().Be(l);
            l[1].LazinatorParentClass.Should().Be(l);
            var c = l.CloneLazinatorTyped();
            c[0].LazinatorParentClass.Should().Be(c);
            c[1].LazinatorParentClass.Should().Be(c);
            var c2 = l.CloneLazinatorTyped();
            c2.Insert(0, new ExampleChild());
            c2[1].LazinatorParentClass.Should().Be(c2);
            var c3 = l.CloneLazinatorTyped();
            var lc3 = c3.ToList();
            lc3[1].LazinatorParentClass.Should().Be(c3);
            var c4 = l.CloneLazinatorTyped();
            var lc4 = c4.AsEnumerable().ToList();
            lc4[1].LazinatorParentClass.Should().Be(c4);
            var c5 = l.CloneLazinatorTyped();
            c5[0] = new ExampleChild();
            c5[0].LazinatorParentClass.Should().Be(c5);
        }

        [Fact]
        public void LazinatorListDirtinessWorks()
        {
            LazinatorListContainer nonGenericContainer = new LazinatorListContainer()
            {
            };
            nonGenericContainer.MyList = new LazinatorList<ExampleChild>();
            nonGenericContainer.MyList.IsDirty.Should().BeTrue();
            nonGenericContainer.IsDirty.Should().BeTrue();
            nonGenericContainer.DescendantIsDirty.Should().BeTrue();

            var v2 = nonGenericContainer.CloneLazinatorTyped();
            v2.IsDirty.Should().BeFalse();
            v2.DescendantIsDirty.Should().BeFalse();
            v2.MyList.IsDirty.Should().BeFalse();
            v2.MyList.Add(GetExampleChild(1));
            v2.MyList.IsDirty.Should().BeTrue();
            v2.IsDirty.Should().BeFalse();
            v2.DescendantIsDirty.Should().BeTrue();

            var v3 = v2.CloneLazinatorTyped();
            v3.MyList.IsDirty.Should().BeFalse();
            v3.MyList.DescendantIsDirty.Should().BeFalse();
            v3.MyList[0].MyLong = 987654321;
            v3.MyList.IsDirty.Should().BeFalse();
            v3.MyList.DescendantIsDirty.Should().BeTrue();
            var v4 = v3.CloneLazinatorTyped();
            v4.MyList[0].MyLong.Should().Be(987654321);

            // now, back to prior list
            v2.MyList.Add(GetExampleChild(1));
            v2.MyList.Add(GetExampleChild(1));
            v2.MyList.Add(GetExampleChild(1));
            var v5 = v2.CloneLazinatorTyped();
            v5.IsDirty.Should().BeFalse();
            v5.DescendantIsDirty.Should().BeFalse();
            v5.MyList.IsDirty.Should().BeFalse();
            var x = v5.MyList[2];
            v5.MyList.IsDirty.Should().BeFalse();
            v5.MyList.DescendantIsDirty.Should().BeFalse();
            x.MyLong = 25;
            v5.MyList.IsDirty.Should().BeFalse();
            v5.MyList.DescendantIsDirty.Should().BeTrue();
            v5.IsDirty.Should().BeFalse();
            v5.DescendantIsDirty.Should().BeTrue();

            // attaching dirty item
            var v6 = v5.CloneLazinatorTyped();
            var v7 = v5.CloneLazinatorTyped();
            var v6item = v6.MyList[0];
            v6item.MyLong = 987654321;
            v6.DescendantIsDirty.Should().BeTrue();
            v6item.IsDirty.Should().BeTrue();
            v7.MyList[1] = v6item;
            v7.MyList.IsDirty.Should().BeTrue();
            v7.MyList.DescendantIsDirty.Should().BeTrue();
            v7.DescendantIsDirty.Should().BeTrue();

            // attaching item with dirty descendant
            v5.MyList[0].MyWrapperContainer = new WrapperContainer() { WrappedInt = 4 };
            v6 = v5.CloneLazinatorTyped();
            v7 = v5.CloneLazinatorTyped();
            v6item = v6.MyList[0];
            v6item.MyWrapperContainer.WrappedInt = 5;
            v6item.IsDirty.Should().BeFalse();
            v6item.DescendantIsDirty.Should().BeTrue();
            v6.DescendantIsDirty.Should().BeTrue();
            v7.MyList[1] = v6item;
            v7.MyList[1].IsDirty.Should().BeFalse();
            v7.MyList[1].DescendantIsDirty.Should().BeTrue();
            v7.MyList.IsDirty.Should().BeTrue();
            v7.MyList.DescendantIsDirty.Should().BeTrue();
            v7.DescendantIsDirty.Should().BeTrue();
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

            var results = v2.GetDirtyNodes();
            results.Count().Should().Be(4);

            var v5 = v2.CloneLazinatorTyped();
            v5.MyList[1].MyLong = -98765;
            results = v5.GetDirtyNodes();
            results.Count().Should().Be(1);
        }
    }
}
