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
            VerifyCloningEquivalence(lazinator, IncludeChildrenMode.ExcludeAllChildren);
            VerifyCloningEquivalence(lazinator, IncludeChildrenMode.IncludeAllChildren);
            VerifyCloningEquivalence(lazinator, IncludeChildrenMode.ExcludeOnlyExcludableChildren);
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
    }
}
