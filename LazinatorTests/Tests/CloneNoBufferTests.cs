using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Lazinator.Collections;
using LazinatorTests.Examples;
using Lazinator.Core;
using Xunit;
using Lazinator.Wrappers;
using LazinatorTests.Examples.Hierarchy;
using LazinatorTests.Examples.Structs;
using LazinatorTests.Examples.NonAbstractGenerics;
using LazinatorTests.Examples.Collections;

namespace LazinatorTests.Tests
{
    public class CloneNoBufferTests : SerializationDeserializationTestBase
    {
        private void VerifyCloningEquivalence(ILazinator lazinator)
        {
            VerifyCloningEquivalence(lazinator, IncludeChildrenMode.ExcludeOnlyExcludableChildren); // DEBUG -- fix order
            VerifyCloningEquivalence(lazinator, IncludeChildrenMode.ExcludeAllChildren);
            VerifyCloningEquivalence(lazinator, IncludeChildrenMode.IncludeAllChildren);
            VerifyCloningEquivalence(lazinator, IncludeChildrenMode.IncludeOnlyIncludableChildren);
        }

        private void VerifyCloningEquivalence(ILazinator lazinator, IncludeChildrenMode includeChildrenMode)
        {
            var clonedWithBuffer = lazinator.CloneLazinator(includeChildrenMode, CloneBufferOptions.LinkedBuffer);
            var clonedNoBuffer = lazinator.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
            var clonedWithBufferString = new HierarchyTree(clonedWithBuffer).ToString();
            var clonedNoBufferString = new HierarchyTree(clonedNoBuffer).ToString();
            LazinatorUtilities.ConfirmHierarchiesEqual(clonedWithBuffer, clonedNoBuffer);
        }

        [Fact]
        public void CloneWithoutBuffer_Example()
        {
            VerifyCloningEquivalence(GetTypicalExample());
        }

        [Fact]
        public void CloneWithoutBuffer_SpanAndMemory()
        {
            SpanAndMemory s = LazinatorSpanTests.GetSpanAndMemory(false);
            VerifyCloningEquivalence(s);
        }

        [Fact]
        public void CloneWithoutBuffer_SpanAndMemory_Empty()
        {
            SpanAndMemory s = LazinatorSpanTests.GetSpanAndMemory(true);
            VerifyCloningEquivalence(s);
        }

        [Fact]
        public void CloneWithoutBuffer_DotNetListValues()
        {
            DotNetList_Values d = new DotNetList_Values()
                {
                    MyListInt = new List<int>() { 3, 4, 5 }
                };
            VerifyCloningEquivalence(d);
        }

        [Fact]
        public void CloneWithoutBuffer_DotNetHashSet()
        {
            DotNetHashSet_Lazinator d = new DotNetHashSet_Lazinator()
            {
                MyHashSetSerialized = new HashSet<ExampleChild>()
                    {
                        GetExampleChild(1),
                        GetExampleChild(3), 
                        null // null item
                    }
            };
            VerifyCloningEquivalence(d);
        }

        [Fact]
        public void CloneWithoutBuffer_DotNetList_Lazinator()
        {
            DotNetList_Lazinator d = new DotNetList_Lazinator()
            {
                MyListSerialized = new List<ExampleChild>()
                    {
                        GetExampleChild(1),
                        GetExampleChild(3), // inherited item
                        null // null item
                    },

            };
            VerifyCloningEquivalence(d);
        }

        [Fact]
        public void CloneWithoutBuffer_DotNetList_Wrapper()
        {
            DotNetList_Wrapper d = new DotNetList_Wrapper()
            {
                MyListNullableByte = new List<WNullableByte>() { 3, 4, 249, null },
                MyListNullableInt = new List<WNullableInt>() { 3, 16000, 249, null, 1000000000 }
            };
            VerifyCloningEquivalence(d);
        }
    }
}
