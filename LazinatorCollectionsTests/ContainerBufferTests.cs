using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using LazinatorTests.Examples;
using Lazinator.Core;
using Xunit;
using Lazinator.Wrappers;
using Lazinator.Buffers;

namespace LazinatorCollectionsTests
{
    public class ContainerBufferTests
    {


        //[Fact]
        //public void CanCloneDictionaryAndThenEnsureUpToDate()
        //{
        //    // Note: This is a sequence that proved problematic in the next test
        //    var d = GetDictionary();
        //    var d2 = d.CloneLazinatorTyped();
        //    var d3 = d.CloneLazinatorTyped();
        //    d.UpdateStoredBuffer();
        //}

        //[Fact]
        //public void CanCloneListOfStructsAndThenEnsureUpToDate()
        //{
        //    // Note: This works because of IsStruct parameter in ReplaceBuffer. Without that parameter, the last call would lead to disposal of memory still needed.
        //    var l = new LazinatorList<WInt>() { 1 };
        //    var l2 = l.CloneLazinatorTyped();
        //    var l3 = l.CloneLazinatorTyped();
        //    l.UpdateStoredBuffer();
        //}

        //[Fact]
        //public void CanCloneListOfLazinatorsAndThenEnsureUpToDate()
        //{
        //    var l = new LazinatorList<Example>() { GetTypicalExample(), GetTypicalExample() };
        //    var l2 = l.CloneLazinatorTyped();
        //    var l3 = l.CloneLazinatorTyped();
        //    l.UpdateStoredBuffer();
        //}

        //[Fact]
        //public void ObjectDisposedExceptionThrownOnItemRemovedFromHierarchy()
        //{
        //    LazinatorDictionary<WInt, Example> d = GetDictionary();
        //    //same effect if both of the following lines are included
        //    //d.UpdateStoredBuffer();
        //    //d[0].MyChar = 'q';
        //    d[0].UpdateStoredBuffer(); // OwnedMemory has allocation ID of 0. 
        //    d.UpdateStoredBuffer(); // OwnedMemory for this and d[0] share allocation ID of 1
        //    Example e = d[0];
        //    d[0] = GetTypicalExample();
        //    d.LazinatorMemoryStorage.Dispose();
        //    Action a = () => { var x = e.MyChild1.LazinatorMemoryStorage.OwnedMemory.Memory; }; // note that error occurs only when looking at underlying memory
        //    a.Should().Throw<ObjectDisposedException>();
        //}

        //[Fact]
        //public void ObjectDisposedExceptionAvoidedByCloneToIndependentBuffer()
        //{
        //    LazinatorDictionary<WInt, Example> d = GetDictionary();
        //    d[0].UpdateStoredBuffer(); // OwnedMemory has allocation ID of 0. 
        //    d.UpdateStoredBuffer(); // OwnedMemory for this and d[0] share allocation ID of 1
        //    Example e = d[0].CloneLazinatorTyped(IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions.IndependentBuffers);
        //    d[0] = GetTypicalExample();
        //    d.UpdateStoredBuffer(); // allocation ID 1 disposed.
        //    Action a = () => { var x = e.MyChild1; };
        //    a.Should().NotThrow<ObjectDisposedException>();
        //}

        //[Fact]
        //public void CanAccessCopiedItemAfterEnsureUpToDate()
        //{
        //    LazinatorDictionary<WInt, Example> d = GetDictionary();
        //    d.UpdateStoredBuffer(); // OwnedMemory for this and d[0] share allocation ID of 0. As the original source, this will not be automatically disposed. 
        //    Example e = d[0];
        //    d[0] = GetTypicalExample();
        //    d.UpdateStoredBuffer(); // allocation ID 0 is not disposed.
        //    var x = e.MyChild1;
        //}
        //[Fact]
        //public void RemoveBufferWorks_LazinatorList()
        //{
        //    // when we remove the buffer from a lazinatorlist, it completely deserializes.

        //    LazinatorListContainer lazinator = new LazinatorListContainer()
        //    {
        //        MyStructList = new LazinatorList<WByte>()
        //        {
        //            3,
        //            4,
        //            5
        //        }
        //    };
        //    lazinator.UpdateStoredBuffer();
        //    lazinator.LazinatorMemoryStorage.IsEmpty.Should().BeFalse();
        //    var x = lazinator.MyStructList[0].WrappedValue;
        //    lazinator.MyStructList[0] = 6;
        //    lazinator.MyStructList[0].LazinatorMemoryStorage.IsEmpty.Should().BeTrue();

        //    lazinator.RemoveBufferInHierarchy();
        //    lazinator.LazinatorMemoryStorage.IsEmpty.Should().BeTrue();
        //    lazinator.MyStructList[0].LazinatorMemoryStorage.IsEmpty.Should().BeTrue();
        //    lazinator.MyStructList[0].WrappedValue.Should().Be(6);

        //    lazinator.UpdateStoredBuffer();
        //    lazinator.MyStructList[0].LazinatorMemoryStorage.IsEmpty.Should().BeTrue();
        //    lazinator.MyStructList[0].WrappedValue.Should().Be(6);

        //    lazinator.UpdateStoredBuffer();
        //    lazinator.MyStructList[0].LazinatorMemoryStorage.IsEmpty.Should().BeTrue();
        //    lazinator.MyStructList[0].WrappedValue.Should().Be(6);

        //    lazinator.MyStructList[0] = 7;
        //    lazinator.UpdateStoredBuffer();
        //    lazinator.MyStructList[0].LazinatorMemoryStorage.IsEmpty.Should().BeTrue();
        //    lazinator.MyStructList[0].WrappedValue.Should().Be(7);

        //    WByte w = new WByte(8).CloneLazinatorTyped(); // make 
        //    lazinator.MyStructList[0] = w;
        //    lazinator.MyStructList[0].LazinatorMemoryStorage.IsEmpty.Should().BeFalse();
        //    lazinator.RemoveBufferInHierarchy();
        //    lazinator.MyStructList[0].LazinatorMemoryStorage.IsEmpty.Should().BeTrue();

        //    lazinator.MyStructList[1] = w;
        //    lazinator.MyStructList[1].LazinatorMemoryStorage.IsEmpty.Should().BeFalse();
        //    lazinator.UpdateStoredBuffer();
        //    lazinator.MyStructList[1].LazinatorMemoryStorage.IsEmpty.Should().BeTrue();

        //    lazinator = new LazinatorListContainer()
        //    {
        //        MyStructList = new LazinatorList<WByte>()
        //        {
        //            3,
        //            4,
        //            5
        //        }
        //    };
        //    lazinator = lazinator.CloneLazinatorTyped();
        //    var list = lazinator.MyStructList;
        //    lazinator.RemoveBufferInHierarchy();
        //    lazinator.LazinatorMemoryStorage.IsEmpty.Should().BeTrue();
        //    lazinator.MyStructList.LazinatorMemoryStorage.IsEmpty.Should().BeTrue();
        //    lazinator.MyStructList[0].WrappedValue.Should().Be(3);
        //}
        //[Fact]
        //public void UpdateBufferForDeserialized_LazinatorList()
        //{
        //    LazinatorListContainer c = GetLazinatorListContainer();
        //    c.MyList[0].MyExampleGrandchild.MyInt = 200;
        //    UpdateStoredBufferFromExisting(c);
        //    var item = c.MyList[0].CloneLazinatorTyped();
        //    var c2 = c.CloneLazinatorTyped();
        //    c.MyList[0].MyExampleGrandchild.MyInt.Should().Be(200);
        //}


        //[Fact]
        //public void UpdateBufferForDeserialized_LazinatorList_Struct()
        //{
        //    LazinatorListContainer c = new LazinatorListContainer() { MyStructList = new LazinatorList<WByte>() };
        //    c.MyStructList.Add(3);
        //    c.MyStructList.Add(4);
        //    c = c.CloneLazinatorTyped();
        //    var x = c.MyStructList[0];
        //    c.MyInt = -234;
        //    UpdateStoredBufferFromExisting(c);
        //    var storageOverall = c.LazinatorMemoryStorage.OwnedMemory as ExpandableBytes;
        //    var storageItem = c.MyStructList[0].LazinatorMemoryStorage.OwnedMemory as ExpandableBytes;
        //    storageOverall.AllocationID.Should().Be(storageItem.AllocationID);
        //    var item = c.MyStructList[0].CloneLazinatorTyped();
        //    var c2 = c.CloneLazinatorTyped();
        //    item.WrappedValue.Should().Be(3);
        //}
        //[Fact]
        //public void BuffersUpdateInTandem_LazinatorList()
        //{
        //    LazinatorListContainer e = GetLazinatorListContainer();
        //    e.MyList[1].MyExampleGrandchild.MyInt = 6;
        //    e.MyList[1].UpdateStoredBuffer(); // generate a new buffer in a list member
        //    ConfirmBuffersUpdateInTandem(e);
        //    e.MyInt = 17; // keep list clean while making container dirty
        //    ConfirmBuffersUpdateInTandem(e);
        //}

        //[Fact]
        //public void BuffersUpdateInTandem_LazinatorList_Struct()
        //{
        //    LazinatorListContainer e = GetLazinatorListContainer();
        //    e.MyStructList[1] = 6;
        //    e.MyStructList[1] = e.MyStructList[1].CloneLazinatorTyped(IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions.IndependentBuffers); // generate a new buffer in a list member
        //    ConfirmBuffersUpdateInTandem(e);
        //    e.MyInt = 17; // keep list clean while making container dirty
        //    ConfirmBuffersUpdateInTandem(e);
        //}
        //private LazinatorListContainer GetLazinatorListContainer()
        //{
        //    LazinatorListContainer container = new LazinatorListContainer();
        //    container.MyList = new LazinatorList<ExampleChild>();
        //    container.MyList.Add(GetExampleChild(1));
        //    container.MyList[0].MyExampleGrandchild = new ExampleGrandchild() { MyInt = 5 };
        //    container.MyList.Add(GetExampleChild(1));
        //    container.MyList[1].MyExampleGrandchild = new ExampleGrandchild() { MyInt = 5 };
        //    container.MyStructList = new LazinatorList<WByte>();
        //    container.MyStructList.Add(1);
        //    container.MyStructList.Add(2);
        //    container = container.CloneLazinatorTyped();
        //    return container;
        //}


        //private LazinatorDictionary<WInt, Example> GetDictionary()
        //{
        //    LazinatorDictionary<WInt, Example> d = new LazinatorDictionary<WInt, Example>();
        //    for (int i = 0; i < dictsize; i++)
        //    {
        //        d[i] = GetTypicalExample();
        //    }
        //    return d;
        //}
        //[Fact]
        //public void CloneWithoutBuffer_LazinatorArray_Example()
        //{
        //    LazinatorArray<Example> GetArray()
        //    {
        //        LazinatorArray<Example> l = new LazinatorArray<Example>(3);
        //        l[0] = GetExample(1);
        //        l[1] = GetExample(1);
        //        l[2] = null;
        //        return l;
        //    }
        //    VerifyCloningEquivalence(() => GetArray());
        //}

        //[Fact]
        //public void CloneWithoutBuffer_LazinatorDictionary()
        //{
        //    LazinatorDictionary<WInt, Example> GetDictionary()
        //    {
        //        LazinatorDictionary<WInt, Example> d = new LazinatorDictionary<WInt, Example>();
        //        d[23] = GetExample(1);
        //        d[0] = GetExample(2);
        //        return d;
        //    }
        //    VerifyCloningEquivalence(() => GetDictionary());
        //}


        //[Fact]
        //public void FreeInMemoryObjects_LazinatorList()
        //{
        //    var typical1 = GetTypicalExample();
        //    typical1.UpdateStoredBuffer();
        //    var typical2 = GetTypicalExample();
        //    var origValue = typical2.MyChild1.MyLong;
        //    LazinatorList<Example> l = new LazinatorList<Example>() { typical1, typical2 };
        //    l.UpdateStoredBuffer();
        //    l.FreeInMemoryObjects();
        //    typical2.MyChild1.MyLong = 46523496; // should not affect the list now
        //    l[0].MyChild1.MyLong.Should().Be(origValue);
        //    l[1].MyChild1.MyLong.Should().Be(origValue);

        //    const long revisedValue = -123456789012345;
        //    l[0].MyChild1.MyLong = revisedValue;
        //    l.UpdateStoredBuffer();
        //    l.FreeInMemoryObjects();
        //    l[0].MyChild1.MyLong.Should().Be(revisedValue);
        //    l[1].MyChild1.MyLong.Should().Be(origValue);

        //}



        //[Fact]
        //public void CloneWithoutBuffer_LazinatorList_Example()
        //{
        //    VerifyCloningEquivalence(() => new LazinatorList<Example>() { GetExample(1), GetExample(1) });
        //}

        //[Fact]
        //public void CloneWithoutBuffer_LazinatorList_WInt()
        //{
        //    VerifyCloningEquivalence(() => new LazinatorList<WInt>() { 3 });
        //}
    }
}
