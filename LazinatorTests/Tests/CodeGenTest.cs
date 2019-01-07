using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using FluentAssertions;
using Lazinator.CodeDescription;
using LazinatorTests.Examples;
using LazinatorTests.Examples.Collections;
using LazinatorTests.Examples.Tuples;
using Xunit;
using Lazinator.Collections;
using Lazinator.Wrappers;
using Microsoft.CodeAnalysis;
using System.Threading.Tasks;
using Lazinator.Collections.Avl;
using Lazinator.Collections.Dictionary;
using Lazinator.Spans;
using LazinatorAnalyzer.Settings;
using LazinatorCodeGen.Roslyn;
using LazinatorTests.Examples.NonAbstractGenerics;
using LazinatorTests.Support;
using Newtonsoft.Json;
using LazinatorListContainer = LazinatorTests.Examples.Collections.LazinatorListContainer;
using LazinatorTests.Examples.Abstract;
using LazinatorTests.Examples.Hierarchy;
using LazinatorTests.Examples.NonLazinator;
using LazinatorTests.Examples.Structs;
using LazinatorTests.Examples.Subclasses;
using Lazinator.Collections.OffsetList;
using Lazinator.Collections.Tree;
using Lazinator.Collections.Tuples;
using Lazinator.Collections.Factories;
using Lazinator.Examples.Structs;

namespace LazinatorTests.Tests
{
    public class CodeGenTest
    {
        [Fact]
        public void CodeStringBuilderWorks()
        {
            string source = @"
public class MyAltClass
{
public void MyAltMethod()
   {
        }
}
public class MyOtherClass
{
    public void MyAltMethod(int myAltN)
{
int x = myAltN + 1;
int y = x + 1;
}
}
";
            //string source = @"if (x == 2) {y=3; z= 2;}";
            string expected = @"
public class MyAltClass
{
    public void MyAltMethod()
    {
    }
}
public class MyOtherClass
{
    public void MyAltMethod(int myAltN)
    {
        int x = myAltN + 1;
        int y = x + 1;
    }
}
";
            var builder = new CodeStringBuilder();
            builder.Append(source);
            var result = builder.ToString();
            if (result != expected)
                IdentifyStringDifferencePoint(expected, result);
            result.Should().Be(expected);
        }

        private void IdentifyStringDifferencePoint(string s1, string s2)
        {
            for (int i = 0; i < Math.Min(s1.Length, s2.Length); i++)
            {
                if (s1[i] == s2[i])
                    Debug.Write(s1[i]);
                else
                {
                    Debug.WriteLine("PROBLEM AT " + i);
                    break;
                }
            }
        }

        [Fact]
        public void CanJsonDeserializeConfigFile()
        {
            string configString = @"
                {
                  ""InterchangeConverters"": {
                    ""MyNamespace.A"": ""AnotherNamespace.A_Interchange"",
                    ""MyNamespace.B"": ""AnotherNamespace.B_Interchange""
                  }
                }";
            LazinatorConfig config = new LazinatorConfig(null, configString);
            config.InterchangeConverters["MyNamespace.A"].Should().Be("AnotherNamespace.A_Interchange");
            config.InterchangeConverters["MyNamespace.B"].Should().Be("AnotherNamespace.B_Interchange");
        }

        [Fact]
        public void CanJsonDeserializeConfigFile2()
        {
            string configString = @"
                {
                  ""InterchangeConverters"": {
                    ""LazinatorTests.Examples.NonLazinatorInterchangeableClass"": ""LazinatorTests.Examples.NonLazinatorInterchangeClass""
                  }
                }";
            LazinatorConfig config = new LazinatorConfig(null, configString);
            config.InterchangeConverters["LazinatorTests.Examples.NonLazinatorInterchangeableClass"].Should().Be("LazinatorTests.Examples.NonLazinatorInterchangeClass");
        }

        [Fact]
        public async Task CodeGenerationProducesActualCode_LazinatorBuiltIns()
        {
            AdhocWorkspace ws = GetAdhocWorkspace();
            await CompleteGenerateCode(typeof(LazinatorByteSpan), project: "Lazinator", mainFolder: "/Spans/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorBitArray), project: "Lazinator", mainFolder: "/Spans/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorFastReadList<>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "OffsetList/", ws);
            await CompleteGenerateCode(typeof(LazinatorFastReadListInt16), project: "Lazinator", mainFolder: "/Collections/", subfolder: "OffsetList/", ws);
            await CompleteGenerateCode(typeof(LazinatorFastReadListInt32), project: "Lazinator", mainFolder: "/Collections/", subfolder: "OffsetList/", ws);
            await CompleteGenerateCode(typeof(LazinatorOffsetList), project: "Lazinator", mainFolder: "/Collections/", subfolder: "OffsetList/", ws);
            await CompleteGenerateCode(typeof(LazinatorLinkedListNode<>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorLinkedList<>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(SortedLazinatorLinkedList<>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorList<>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(SortedLazinatorList<>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorArray<>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorQueue<>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorStack<>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorGeneralTree<>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "Tree/", ws);
            await CompleteGenerateCode(typeof(LazinatorLocationAwareTree<>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "Tree/", ws);
            await CompleteGenerateCode(typeof(LazinatorLocationAwareTree<>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "Tree/", ws);
            await CompleteGenerateCode(typeof(BinaryTree<>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "Tree/", ws);
            await CompleteGenerateCode(typeof(BinaryNode<>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "Tree/", ws);
            await CompleteGenerateCode(typeof(DictionaryBucket<,>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "Dictionary/", ws);
            await CompleteGenerateCode(typeof(LazinatorDictionary<,>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "Dictionary/", ws);
            await CompleteGenerateCode(typeof(LazinatorTuple<,>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "Tuples/", ws);
            await CompleteGenerateCode(typeof(LazinatorKeyValue<,>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "Tuples/", ws);
            await CompleteGenerateCode(typeof(LazinatorTriple<,,>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "Tuples/", ws);

            await CompleteGenerateCode(typeof(AvlNode<>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "Avl/", ws);
            await CompleteGenerateCode(typeof(AvlCountedNode<>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "Avl/", ws);
            await CompleteGenerateCode(typeof(AvlTree<>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "Avl/", ws);
            await CompleteGenerateCode(typeof(AvlSortedTree<>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "Avl/", ws);
            await CompleteGenerateCode(typeof(AvlIndexableTree<>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "Avl/", ws);
            await CompleteGenerateCode(typeof(AvlSortedIndexableTree<>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "Avl/", ws);
            await CompleteGenerateCode(typeof(AvlList<>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "Avl/", ws);
            await CompleteGenerateCode(typeof(AvlSortedList<>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "Avl/", ws);
            //await CompleteGenerateCode(typeof(AvlOldNode<,>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "Avl/", ws);
            //await CompleteGenerateCode(typeof(AvlOldTree<,>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "Avl/", ws);
            //await CompleteGenerateCode(typeof(AvlDictionary<,>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "Avl/", ws);
            //await CompleteGenerateCode(typeof(AvlSortedDictionary<,>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "Avl/", ws);
            //await CompleteGenerateCode(typeof(AvlListNodeContents<,>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "Avl/", ws);
            //await CompleteGenerateCode(typeof(AvlListNodeTree<,>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "Avl/", ws);

            await CompleteGenerateCode(typeof(AvlIndexableTreeFactory<>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "Factories/", ws);
            await CompleteGenerateCode(typeof(AvlListFactory<>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "Factories/", ws);
            await CompleteGenerateCode(typeof(AvlSortedIndexableTreeFactory<>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "Factories/", ws);
            await CompleteGenerateCode(typeof(AvlSortedListFactory<>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "Factories/", ws);
            //await CompleteGenerateCode(typeof(AvlSortedDictionaryFactory<,>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "Factories/", ws);
            //await CompleteGenerateCode(typeof(AvlSortedListFactory<>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "Factories/", ws);
            await CompleteGenerateCode(typeof(AvlKeyValueTreeFactory<,>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "Factories/", ws);
            await CompleteGenerateCode(typeof(LazinatorLinkedListFactory<>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "Factories/", ws);
            await CompleteGenerateCode(typeof(LazinatorListFactory<>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "Factories/", ws);
            await CompleteGenerateCode(typeof(SortedLazinatorLinkedListFactory<>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "Factories/", ws);
            await CompleteGenerateCode(typeof(SortedLazinatorListFactory<>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "Factories/", ws);
            //await CompleteGenerateCode(typeof(AvlListNodeTreeFactory<,>), project: "Lazinator", mainFolder: "/Collections/", subfolder: "Factories/", ws);

            await CompleteGenerateCode(typeof(WAbstract<>), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WNullableStruct<>), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(Placeholder), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WBool), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WByte), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WSByte), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WChar), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WDecimal), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WFloat), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WDouble), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WShort), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WUshort), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WInt), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WUint), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WLong), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WUlong), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WTimeSpan), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WDateTime), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WGuid), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WString), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WNullableBool), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WNullableByte), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WNullableSByte), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WNullableChar), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WNullableDecimal), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WNullableFloat), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WNullableDouble), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WNullableShort), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WNullableUshort), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WNullableInt), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WNullableUint), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WNullableLong), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WNullableUlong), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WNullableTimeSpan), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WNullableDateTime), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WDecimalArray), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WDoubleArray), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WFloatArray), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WIntArray), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WLongArray), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WNullableGuid), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WReadOnlySpanChar), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
        }

        //[Fact]
        //public void CodeGenerationProducesActualCode_MainLazinatorClasses()
        //{
        //    // The commented out ones can't be auto-generated because they use T : struct
        //    // await CompleteGenerateCode(typeof(LazinatorFastReadList<>), subfolder: "", project: "Lazinator", mainFolder: "/Collections/"); 
        //    // await CompleteGenerateCode(typeof(LazinatorOffsetList), subfolder: "", project: "Lazinator", mainFolder: "/Collections/");
        //    // And this one can't be done because we override a lot of the default logic. We could redesign it so that's not necessary.
        //    // await CompleteGenerateCode(typeof(LazinatorList<>), subfolder: "", project: "Lazinator", mainFolder: "/Collections/");
        //}

        [Fact]
        public async Task CodeGenerationProducesActualCode_TestExamples()
        {
            AdhocWorkspace ws = GetAdhocWorkspace();
            await CompleteGenerateCode(typeof(Example), "LazinatorTests", "/Examples/", "ExampleHierarchy/", ws);
            await CompleteGenerateCode(typeof(ContainerForExampleWithDefault), "LazinatorTests", "/Examples/", "ExampleHierarchy/", ws);
            await CompleteGenerateCode(typeof(ExampleInterfaceContainer), "LazinatorTests", "/Examples/", "ExampleHierarchy/", ws);
            await CompleteGenerateCode(typeof(ExampleChild), "LazinatorTests", "/Examples/", "ExampleHierarchy/", ws);
            await CompleteGenerateCode(typeof(ExampleGrandchild), "LazinatorTests", "/Examples/", "ExampleHierarchy/", ws);
            await CompleteGenerateCode(typeof(ExampleChildInherited), "LazinatorTests", "/Examples/", "ExampleHierarchy/", ws);
            await CompleteGenerateCode(typeof(ExampleNonexclusiveInterfaceImplementer), "LazinatorTests", "/Examples/", "ExampleHierarchy/", ws);
            await CompleteGenerateCode(typeof(ExampleStructContainingClasses), "LazinatorTests", "/Examples/", "Structs/", ws);
            await CompleteGenerateCode(typeof(ExampleContainerContainingClassesStructContainingClasses), "LazinatorTests", "/Examples/", "Structs/", ws);
            await CompleteGenerateCode(typeof(ExampleStructWithoutClass), "LazinatorTests", "/Examples/", "Structs/", ws);
            await CompleteGenerateCode(typeof(ContainerForExampleStructWithoutClass), "LazinatorTests", "/Examples/", "Structs/", ws);
            await CompleteGenerateCode(typeof(ExampleStructContainingStruct), "LazinatorTests", "/Examples/", "Structs/", ws);
            await CompleteGenerateCode(typeof(ExampleStructContainingStructContainer), "LazinatorTests", "/Examples/", "Structs/", ws);
            await CompleteGenerateCode(typeof(Simplifiable), "LazinatorTests", "/Examples/", "ExampleHierarchy/", ws);
            await CompleteGenerateCode(typeof(WrapperContainer), "LazinatorTests", "/Examples/", "Structs/", ws);
            await CompleteGenerateCode(typeof(SmallWrappersContainer), "LazinatorTests", "/Examples/", "Structs/", ws);
            await CompleteGenerateCode(typeof(StructWithBadHashFunction), "LazinatorTests", "/Examples/", "Structs/", ws);
            await CompleteGenerateCode(typeof(NonComparableWrapper), "LazinatorTests", "/Examples/", "Structs/", ws);
            await CompleteGenerateCode(typeof(NonComparableWrapperString), "LazinatorTests", "/Examples/", "Structs/", ws);
            await CompleteGenerateCode(typeof(RecursiveExample), "LazinatorTests", "/Examples/", "ExampleHierarchy/", ws);
            await CompleteGenerateCode(typeof(Abstract1), "LazinatorTests", "/Examples/", "Abstract/", ws);
            await CompleteGenerateCode(typeof(ContainerWithAbstract1), "LazinatorTests", "/Examples/", "Abstract/", ws);
            await CompleteGenerateCode(typeof(Abstract2), "LazinatorTests", "/Examples/", "Abstract/", ws);
            await CompleteGenerateCode(typeof(Concrete3), "LazinatorTests", "/Examples/", "Abstract/", ws);
            await CompleteGenerateCode(typeof(Abstract4), "LazinatorTests", "/Examples/", "Abstract/", ws);
            await CompleteGenerateCode(typeof(Concrete5), "LazinatorTests", "/Examples/", "Abstract/", ws);
            await CompleteGenerateCode(typeof(Concrete6), "LazinatorTests", "/Examples/", "Abstract/", ws);
            await CompleteGenerateCode(typeof(Base), "LazinatorTests", "/Examples/", "Abstract/", ws);
            await CompleteGenerateCode(typeof(ConcreteFromBase), "LazinatorTests", "/Examples/", "Abstract/", ws);
            await CompleteGenerateCode(typeof(GenericFromBase<>), "LazinatorTests", "/Examples/", "Abstract/", ws);
            await CompleteGenerateCode(typeof(ConcreteFromGenericFromBase), "LazinatorTests", "/Examples/", "Abstract/", ws);
            await CompleteGenerateCode(typeof(BaseContainer), "LazinatorTests", "/Examples/", "Abstract/", ws);
            await CompleteGenerateCode(typeof(AbstractGeneric1<>), "LazinatorTests", "/Examples/", "Abstract/", ws);
            await CompleteGenerateCode(typeof(ConcreteGeneric2b), "LazinatorTests", "/Examples/", "Abstract/", ws);
            await CompleteGenerateCode(typeof(ConcreteGeneric2a), "LazinatorTests", "/Examples/", "Abstract/", ws);
            await CompleteGenerateCode(typeof(DerivedGeneric2c<>), "LazinatorTests", "/Examples/", "Abstract/", ws);
            await CompleteGenerateCode(typeof(DerivedGenericContainer<>), "LazinatorTests", "/Examples/", "Abstract/", ws);
            await CompleteGenerateCode(typeof(AbstractGenericContainer<>), "LazinatorTests", "/Examples/", "Abstract/", ws);
            await CompleteGenerateCode(typeof(ConcreteGenericContainer), "LazinatorTests", "/Examples/", "Abstract/", ws);
            await CompleteGenerateCode(typeof(DerivedLazinatorList<>), "LazinatorTests", "/Examples/", "NonAbstractGenerics/", ws);
            await CompleteGenerateCode(typeof(ClosedGenericWithoutBase), "LazinatorTests", "/Examples/", "NonAbstractGenerics/", ws);
            await CompleteGenerateCode(typeof(OpenGeneric<>), "LazinatorTests", "/Examples/", "NonAbstractGenerics/", ws);
            await CompleteGenerateCode(typeof(ClosedGeneric), "LazinatorTests", "/Examples/", "NonAbstractGenerics/", ws);
            await CompleteGenerateCode(typeof(ClosedGenericWithGeneric), "LazinatorTests", "/Examples/", "NonAbstractGenerics/", ws);
            await CompleteGenerateCode(typeof(InheritingClosedGeneric), "LazinatorTests", "/Examples/", "NonAbstractGenerics/", ws);
            await CompleteGenerateCode(typeof(OpenGenericStayingOpenContainer), "LazinatorTests", "/Examples/", "NonAbstractGenerics/", ws);
            await CompleteGenerateCode(typeof(ClosedGenericWithoutBase), "LazinatorTests", "/Examples/", "NonAbstractGenerics/", ws);
            await CompleteGenerateCode(typeof(NonLazinatorContainer), "LazinatorTests", "/Examples/", "NonLazinator/", ws);
            await CompleteGenerateCode(typeof(NonLazinatorInterchangeClass), "LazinatorTests", "/Examples/", "NonLazinator/", ws);
            await CompleteGenerateCode(typeof(NonLazinatorInterchangeStruct), "LazinatorTests", "/Examples/", "NonLazinator/", ws);
            await CompleteGenerateCode(typeof(FromNonLazinatorBase), "LazinatorTests", "/Examples/", "NonLazinator/", ws);
            await CompleteGenerateCode(typeof(UnofficialInterfaceIncorporator), "LazinatorTests", "/Examples/", "UnofficialInterfaces/", ws);
            await CompleteGenerateCode(typeof(Dictionary_Values_Lazinator), "LazinatorTests", "/Examples/", "Collections/", ws);
            await CompleteGenerateCode(typeof(DotNetHashSet_Lazinator), "LazinatorTests", "/Examples/", "Collections/", ws);
            await CompleteGenerateCode(typeof(DotNetList_Nested_NonLazinator), "LazinatorTests", "/Examples/", "Collections/", ws);
            await CompleteGenerateCode(typeof(Derived_DotNetList_Nested_NonLazinator), "LazinatorTests", "/Examples/", "Collections/", ws);
            await CompleteGenerateCode(typeof(DotNetList_NonLazinator), "LazinatorTests", "/Examples/", "Collections/", ws);
            await CompleteGenerateCode(typeof(DotNetList_Lazinator), "LazinatorTests", "/Examples/", "Collections/", ws);
            await CompleteGenerateCode(typeof(DotNetList_Values), "LazinatorTests", "/Examples/", "Collections/", ws);
            await CompleteGenerateCode(typeof(DotNetList_Wrapper), "LazinatorTests", "/Examples/", "Collections/", ws);
            await CompleteGenerateCode(typeof(DotNetQueue_Values), "LazinatorTests", "/Examples/", "Collections/", ws);
            await CompleteGenerateCode(typeof(DotNetQueue_Lazinator), "LazinatorTests", "/Examples/", "Collections/", ws);
            await CompleteGenerateCode(typeof(DotNetStack_Values), "LazinatorTests", "/Examples/", "Collections/", ws);
            await CompleteGenerateCode(typeof(SpanAndMemory), "LazinatorTests", "/Examples/", "Collections/", ws);
            await CompleteGenerateCode(typeof(SpanInDotNetList), "LazinatorTests", "/Examples/", "Collections/", ws);
            await CompleteGenerateCode(typeof(Array_Values), "LazinatorTests", "/Examples/", "Collections/", ws);
            await CompleteGenerateCode(typeof(DerivedArray_Values), "LazinatorTests", "/Examples/", "Collections/", ws);
            await CompleteGenerateCode(typeof(ArrayMultidimensional_Values), "LazinatorTests", "/Examples/", "Collections/", ws);
            await CompleteGenerateCode(typeof(LazinatorListContainer), "LazinatorTests", "/Examples/", "Collections/", ws);
            await CompleteGenerateCode(typeof(LazinatorListContainerGeneric<>), "LazinatorTests", "/Examples/", "Collections/", ws);
            await CompleteGenerateCode(typeof(StructTuple), "LazinatorTests", "/Examples/", "Tuples/", ws);
            await CompleteGenerateCode(typeof(RegularTuple), "LazinatorTests", "/Examples/", "Tuples/", ws);
            await CompleteGenerateCode(typeof(KeyValuePairTuple), "LazinatorTests", "/Examples/", "Tuples/", ws);
            await CompleteGenerateCode(typeof(NestedTuple), "LazinatorTests", "/Examples/", "Tuples/", ws);
            await CompleteGenerateCode(typeof(RecordLikeContainer), "LazinatorTests", "/Examples/", "Tuples/", ws);
            await CompleteGenerateCode(typeof(RecordLikeCollections), "LazinatorTests", "/Examples/", "Tuples/", ws);
            await CompleteGenerateCode(typeof(ClassWithLocalEnum), "LazinatorTests", "/Examples/", "Subclasses/", ws);
            await CompleteGenerateCode(typeof(ClassWithForeignEnum), "LazinatorTests", "/Examples/", "Subclasses/", ws);
            await CompleteGenerateCode(typeof(ClassWithSubclass), "LazinatorTests", "/Examples/", "Subclasses/", ws);
            await CompleteGenerateCode(typeof(ClassWithSubclass.SubclassWithinClass), "LazinatorTests", "/Examples/", "Subclasses/", ws);
        }

        private static List<string> GetDirectories()
        {
            return new List<string>()
            {
                ReadCodeFile.GetCodeBasePath("Lazinator") + "/Attributes",
                ReadCodeFile.GetCodeBasePath("Lazinator") + "/Collections",
                ReadCodeFile.GetCodeBasePath("Lazinator") + "/Collections/Avl",
                ReadCodeFile.GetCodeBasePath("Lazinator") + "/Spans",
                ReadCodeFile.GetCodeBasePath("Lazinator") + "/Wrappers",
                ReadCodeFile.GetCodeBasePath("LazinatorTests") + "/Examples",
            };
        }

        private static async Task CompleteGenerateCode(Type existingType, string project, string mainFolder, string subfolder, AdhocWorkspace ws)
        {
            if (existingType.IsInterface)
                throw new Exception("Can complete generate code only on class implementing interface, not interface itself.");
            string projectPath = ReadCodeFile.GetCodeBasePath(project);
            string name = ReadCodeFile.GetNameOfType(existingType);
            ReadCodeFile.GetCodeInFile(projectPath, mainFolder, subfolder, name, ".g.cs", out string codeBehindPath, out string codeBehind);
            LazinatorConfig config = FindConfigFileStartingFromSubfolder(mainFolder, subfolder, projectPath);

            // uncomment to include tracing code
            //if (config == null)
            //    config = new LazinatorConfig();
            //config.IncludeTracingCode = true;

            var compilation = await AdhocWorkspaceManager.GetCompilation(ws);
            LazinatorCompilation lazinatorCompilation = new LazinatorCompilation(compilation, existingType, config);

            var d = new ObjectDescription(lazinatorCompilation.ImplementingTypeSymbol, lazinatorCompilation, codeBehindPath, true);
            var result = d.GetCodeBehind();
            bool match = codeBehind == result;

            // return; // uncomment this to prevent any changes to classes during testing if automaticallyFix is true

            bool automaticallyFix = true; // Set to true to automatically update all test classes on the local development machine to a new format. This will trivially result in the test passing. Before doing this, submit all changes. After doing this, if code compiles, run all tests. Then set this back to false. 
            if (automaticallyFix && !match)
                File.WriteAllText(codeBehindPath, result);
            else
                match.Should().BeTrue();
        }

        private static LazinatorConfig FindConfigFileStartingFromSubfolder(string mainFolder, string subfolder, string projectPath)
        {
            ReadCodeFile.GetCodeInFile(projectPath, mainFolder, subfolder, "LazinatorConfig", ".json", out string configPath, out string configText);
            if (configText == null)
                ReadCodeFile.GetCodeInFile(projectPath, mainFolder, "/", "LazinatorConfig", ".json", out configPath, out configText);
            if (configText == null)
                ReadCodeFile.GetCodeInFile(projectPath, "/", "", "LazinatorConfig", ".json", out configPath, out configText);
            LazinatorConfig config = null;
            if (configText != null)
                config = JsonConvert.DeserializeObject<LazinatorConfig>(configText);
            return config;
        }

        private AdhocWorkspace GetAdhocWorkspace()
        {
            List<string> directories = GetDirectories();
            return AdhocWorkspaceManager.CreateAdHocWorkspaceWithAllFilesInProjects(directories);
        }
    }
}