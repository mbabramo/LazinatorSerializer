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
using System.Diagnostics;

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
                        MyTupleSerialized4 = new Tuple<int, ExampleStructContainingClasses>(2, new ExampleStructContainingClasses() { MyChar = '1' })
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
            results[0].LazinatorParents.LastAdded.Should().Be(l);
            results[1].LazinatorParents.LastAdded.Should().Be(l);
            var c = l.CloneLazinatorTyped();
            results = c.ToList();
            results[0].LazinatorParents.LastAdded.Should().Be(c);
            results[1].LazinatorParents.LastAdded.Should().Be(c);
            c = l.CloneLazinatorTyped();
            foreach (var result in c)
                result.LazinatorParents.LastAdded.Should().Be(c);
        }

        [Fact]
        public void LazinatorListCountWorks()
        {
            LazinatorList<Example> l = new LazinatorList<Example>() { GetExample(1), GetExample(1) };
            l.Count.Should().Be(2);
            l.Add(null);
            l.Count.Should().Be(3);
            l.RemoveAt(1);
            l.Count.Should().Be(2);
            var c = l.CloneLazinatorTyped();
            c.Count.Should().Be(2);
            c.Insert(0, GetExample(1));
            c.Count.Should().Be(3);
            c.Add(GetExample(2));
            c.Count.Should().Be(4);
            c.RemoveAll(x => true);
            c.Count.Should().Be(0);
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
            LazinatorFastReadListInt32 r = new LazinatorFastReadListInt32();
            r.AsList = new List<int>() { 3, 4, 5 };
            r.IsDirty.Should().BeTrue();
            LazinatorFastReadListInt32 r2 = r.CloneLazinatorTyped();
            r2[0].Should().Be(3);
            r2.IsDirty.Should().BeFalse();
            r2.AsList.Add(6);
            r2.IsDirty.Should().BeTrue();
            LazinatorFastReadListInt32 r3 = r2.CloneLazinatorTyped();
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

        [InlineData(ContainerForLazinatorList.GenericContainer, true)]
        [InlineData(ContainerForLazinatorList.NonGenericContainer, true)]
        [InlineData(ContainerForLazinatorList.NoContainer, true)]
        [InlineData(ContainerForLazinatorList.GenericContainer, false)]
        [InlineData(ContainerForLazinatorList.NonGenericContainer, false)]
        [InlineData(ContainerForLazinatorList.NoContainer, false)]
        [Theory]
        public void LazinatorListWorks(ContainerForLazinatorList containerOption, bool cloneAfterEachStep)
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
            void CloneList()
            {

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
            }
            void CheckBeforeAndAfterSerialization()
            {
                CheckList();
                CloneList();
                CheckList();
                if (cloneAfterEachStep)
                    CloneList();
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
        public void LazinatorListRemoveAllWorks()
        {

            LazinatorList<WInt> l = new LazinatorList<WInt>() { 2, 3, 4, 5, 6 };
            l.RemoveAll(x => x % 2 == 0);
            var results = l.ToList().Select(x => x.WrappedValue);
            results.SequenceEqual(new int[] { 3, 5 }).Should().BeTrue();

            l = new LazinatorList<WInt>() { 2, 3, 4, 5, 6 };
            l.RemoveAll(x => x % 2 == 1);
            results = l.ToList().Select(x => x.WrappedValue);
            results.SequenceEqual(new int[] { 2, 4, 6 }).Should().BeTrue();

            l = new LazinatorList<WInt>() { 2, 3, 4, 5, 6 };
            l.RemoveAll(x => x > 6);
            results = l.ToList().Select(x => x.WrappedValue);
            results.SequenceEqual(new int[] { 2, 3, 4, 5, 6 }).Should().BeTrue();

            l = new LazinatorList<WInt>() { 2, 3, 4, 5, 6 };
            l.RemoveAll(x => x <= 6);
            results = l.ToList().Select(x => x.WrappedValue);
            results.SequenceEqual(new int[] { }).Should().BeTrue();

            l = new LazinatorList<WInt>() { };
            l.RemoveAll(x => x <= 6);
            results = l.ToList().Select(x => x.WrappedValue);
            results.SequenceEqual(new int[] { }).Should().BeTrue();
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
        public void LazinatorArrayWorks()
        {
            LazinatorArray<WInt> l = new LazinatorArray<WInt>(3);
            l[0] = 10;
            l[2] = 12;
            var c = l.CloneLazinatorTyped();
            c[0].WrappedValue.Should().Be(10);
            c[1].WrappedValue.Should().Be(0); // Default value
            c[2].WrappedValue.Should().Be(12);
        }

        [Fact]
        public void LazinatorListItemParentSet()
        {
            LazinatorList<ExampleChild> l = new LazinatorList<ExampleChild>()
            {
                new ExampleChild(),
                new ExampleChild()
            };
            l[0].LazinatorParents.LastAdded.Should().Be(l);
            l[1].LazinatorParents.LastAdded.Should().Be(l);
            var c = l.CloneLazinatorTyped();
            c[0].LazinatorParents.LastAdded.Should().Be(c);
            c[1].LazinatorParents.LastAdded.Should().Be(c);
            var c2 = l.CloneLazinatorTyped();
            c2.Insert(0, new ExampleChild());
            c2[1].LazinatorParents.LastAdded.Should().Be(c2);
            var c3 = l.CloneLazinatorTyped();
            var lc3 = c3.ToList();
            lc3[1].LazinatorParents.LastAdded.Should().Be(c3);
            var c4 = l.CloneLazinatorTyped();
            var lc4 = c4.AsEnumerable().ToList();
            lc4[1].LazinatorParents.LastAdded.Should().Be(c4);
            var c5 = l.CloneLazinatorTyped();
            c5[0] = new ExampleChild();
            c5[0].LazinatorParents.LastAdded.Should().Be(c5);
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
            LazinatorUtilities.ConfirmMatch(v6.LazinatorMemoryStorage.Memory, v7.LazinatorMemoryStorage.Memory);
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

            // attaching item with dirty descendant after fully deserializing
            v5.MyList[0].MyWrapperContainer = new WrapperContainer() { WrappedInt = 4 };
            v6 = v5.CloneLazinatorTyped();
            v7 = v5.CloneLazinatorTyped();
            v6item = v6.MyList[0];
            v6item.MyWrapperContainer.WrappedInt = 5;
            v6item.IsDirty.Should().BeFalse();
            v6item.DescendantIsDirty.Should().BeTrue();
            v6.DescendantIsDirty.Should().BeTrue();
            v7.MyList.Insert(0, null);
            v7.MyList.UpdateStoredBuffer();
            v7.MyList.IsDirty.Should().BeFalse();
            v7.MyList.DescendantIsDirty.Should().BeFalse();
            v7.MyList[1] = v6item;
            v7.MyList[1].IsDirty.Should().BeFalse();
            v7.MyList[1].DescendantIsDirty.Should().BeTrue();
            v7.MyList.DescendantIsDirty.Should().BeTrue();
            v7.DescendantIsDirty.Should().BeTrue();
        }

        [Fact]
        public void LazinatorListDirtinessWithStructs()
        {
            LazinatorList<WInt> l = new LazinatorList<WInt>()
            {
                new WInt(3)
            };
            l.UpdateStoredBuffer();
            var c = l.CloneLazinatorTyped();
            // consider original list, which should be clean
            l.IsDirty.Should().BeFalse();
            l.DescendantIsDirty.Should().BeFalse(); 
            l[0].IsDirty.Should().BeFalse();
            // now consider clone
            c.IsDirty.Should().BeFalse();
            c.DescendantIsDirty.Should().BeFalse();
            c[0].IsDirty.Should().BeFalse();
        }

        [Fact]
        public void LazinatorListDirtinessWithNestedStructs()
        {
            LazinatorList<ExampleStructContainingStruct> l = new LazinatorList<ExampleStructContainingStruct>()
            {
                new ExampleStructContainingStruct() { MyExampleStructContainingClasses = new ExampleStructContainingClasses() { MyChar = 'Q'} }
            };
            l.UpdateStoredBuffer();
            var c = l.CloneLazinatorTyped();
            // consider original list, which should be clean
            l.IsDirty.Should().BeFalse();
            l.DescendantIsDirty.Should().BeFalse();
            l[0].IsDirty.Should().BeFalse();
            l[0].MyExampleStructContainingClasses.IsDirty.Should().BeFalse();
            // now consider clone
            c.IsDirty.Should().BeFalse();
            c.DescendantIsDirty.Should().BeFalse();
            c[0].IsDirty.Should().BeFalse();
            c[0].MyExampleStructContainingClasses.IsDirty.Should().BeFalse();
            c[0].MyExampleStructContainingClasses.MyChar.Should().Be('Q');
        }

        [Fact]
        public void ChangeToObjectAppearingTwiceInLazinatorListAffectsBoth()
        {
            LazinatorList<ExampleChild> e = new LazinatorList<ExampleChild>()
            {
                new ExampleChild(),
                new ExampleChild() { MyLong = -123456 }
            };
            var c = e.CloneLazinatorTyped();
            c[0] = c[1];
            c[0].MyLong = -987;
            var c2 = c.CloneLazinatorTyped();
            c2[0].MyLong.Should().Be(-987);
            c2[1].MyLong.Should().Be(-987);
        }

        [Fact]
        public void LazinatorListParentItemsWorks()
        {
            LazinatorList<ExampleChild> e = new LazinatorList<ExampleChild>();
            var child = new ExampleChild();
            e.Add(child);
            child.LazinatorParents.LastAdded.Should().Be(e);
            e.Add(child);
            child.LazinatorParents.LastAdded.Should().Be(e);
            e[0] = null;
            child.LazinatorParents.LastAdded.Should().Be(e);
            e[1] = null;
            child.LazinatorParents.Count.Should().Be(0);

            LazinatorList<ExampleChild> e2 = new LazinatorList<ExampleChild>();
            e.Add(child);
            e2.Add(child);
            child.LazinatorParents.Count.Should().Be(2);
            e2[0] = null;
            child.LazinatorParents.Count.Should().Be(1);
        }

        [Fact]
        public void LazinatorStackWorks()
        {
            LazinatorStack<WInt> s = new LazinatorStack<WInt>();
            s.Push(3);
            s.Any().Should().BeTrue();
            var r = s.Pop();
            r.WrappedValue.Should().Be(3);
            s.Any().Should().BeFalse();
            s.Push(3);
            s.Push(4);
            r = s.Peek();
            r.WrappedValue.Should().Be(4);
            r = s.Pop();
            r.WrappedValue.Should().Be(4);
            s.Push(4);
            s.Push(5);
            r = s.Pop();
            r.WrappedValue.Should().Be(5);
            r = s.Pop();
            r.WrappedValue.Should().Be(4);
            r = s.Pop();
            r.WrappedValue.Should().Be(3);
            s.Any().Should().BeFalse();
        }

        [Fact]
        public void LazinatorStack_PopAfterDeserialize()
        {
            LazinatorStack<WInt> s = new LazinatorStack<WInt>();
            s.Push(1);
            s.Push(2);
            s.Push(3);
            s.Count.Should().Be(3);
            s = s.CloneLazinatorTyped();
            s.Pop();
            s.Pop();
            s = s.CloneLazinatorTyped();
            s.Count.Should().Be(1);
            s[0].WrappedValue.Should().Be(1);
            s.Push(2);
            s.Push(3);
            s.Pop();
            s = s.CloneLazinatorTyped();
            s.Count.Should().Be(2);
            s.Pop();
            s.Pop();
            s = s.CloneLazinatorTyped();
            s.Count.Should().Be(0);
        }

        [Fact]
        public void LazinatorQueueWorks()
        {
            LazinatorQueue<WInt> s = new LazinatorQueue<WInt>();
            s.Enqueue(3);
            s.Any().Should().BeTrue();
            var r = s.Dequeue();
            r.WrappedValue.Should().Be(3);
            s.Any().Should().BeFalse();
            s.Enqueue(3);
            s.Enqueue(4);
            r = s.Peek();
            r.WrappedValue.Should().Be(3);
            r = s.Dequeue();
            r.WrappedValue.Should().Be(3);
            s.Enqueue(3);
            s.Enqueue(5);
            r = s.Dequeue();
            r.WrappedValue.Should().Be(4);
            r = s.Dequeue();
            r.WrappedValue.Should().Be(3);
            r = s.Dequeue();
            r.WrappedValue.Should().Be(5);
            s.Any().Should().BeFalse();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SortedLazinatorListWorks(bool allowDuplicates)
        {
            const int numOperations = 100;
            const int maxValue = 60;
            Random r = new Random();
            SortedLazinatorList<WInt> s = new SortedLazinatorList<WInt>()
            {
                AllowDuplicates = allowDuplicates
            };
            List<int> basic = new List<int>();
            for (int i = 0; i < numOperations; i++)
            {
                if (r.Next(3) == 0 && basic.Any())
                { // remove item
                    int j = r.Next(basic.Count());
                    int n = basic[j];
                    basic.Remove(n);
                    s.RemoveSorted(n);
                }
                else
                { // add item
                    int n = r.Next(maxValue);
                    if (!basic.Contains(n) || allowDuplicates)
                        basic.Add(n);
                    s.Insert(n);
                }
            }
            List<int> sortedResult = s.Select(x => x.WrappedValue).ToList();
            basic.OrderBy(x => x).SequenceEqual(sortedResult).Should().BeTrue();
        }
        
    }
}
