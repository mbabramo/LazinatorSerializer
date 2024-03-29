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
using Lazinator.Collections.Dictionary;
using LazinatorGenerator.Settings;
using LazinatorTests.Examples.NonAbstractGenerics;
using CodeGenHelper;
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
using Lazinator.Collections.BitArray;
using Lazinator.Collections.ByteSpan;
using Lazinator.Collections.Remote;
using LazinatorTests.Examples.RemoteHierarchy;
using LazinatorTests.Examples.ExampleHierarchy;
using System.Security.Cryptography.X509Certificates;
using LazinatorTests.AnotherNamespace;
using Lazinator.Buffers;
using Lazinator.Persistence;
using Microsoft.CodeAnalysis.CSharp;
using LazinatorGenerator.Support;
using LazinatorGenerator.CodeDescription;

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
        public async Task CodeGenerationProducesActualCode_CoreCollections()
        {
            AdhocWorkspace ws = GetAdhocWorkspace();
            await CompleteGenerateCode(typeof(LazinatorBitArray), project: "Lazinator.Collections", mainFolder: "/BitArray/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorByteSpan), project: "Lazinator.Collections", mainFolder: "/ByteSpan/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorFastReadList<>), project: "Lazinator.Collections", mainFolder: "/OffsetList/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorFastReadListInt16), project: "Lazinator.Collections", mainFolder: "/OffsetList/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorFastReadListInt32), project: "Lazinator.Collections", mainFolder: "/OffsetList/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorOffsetList), project: "Lazinator.Collections", mainFolder: "/OffsetList/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(Remote<,>), project: "Lazinator.Collections", mainFolder: "/Remote/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorLinkedListNode<>), project: "Lazinator.Collections", mainFolder: "", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorLinkedList<>), project: "Lazinator.Collections", mainFolder: "", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorSortedLinkedList<>), project: "Lazinator.Collections", mainFolder: "", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorList<>), project: "Lazinator.Collections", mainFolder: "", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorSortedList<>), project: "Lazinator.Collections", mainFolder: "", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorArray<>), project: "Lazinator.Collections", mainFolder: "", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorQueue<>), project: "Lazinator.Collections", mainFolder: "", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorStack<>), project: "Lazinator.Collections", mainFolder: "", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorGeneralTree<>), project: "Lazinator.Collections", mainFolder: "/Tree/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorBinaryTreeNode<>), project: "Lazinator.Collections", mainFolder: "/Tree/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorBinaryTree<>), project: "Lazinator.Collections", mainFolder: "/Tree/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorLocationAwareTree<>), project: "Lazinator.Collections", mainFolder: "/Tree/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorLocationAwareTree<>), project: "Lazinator.Collections", mainFolder: "/Tree/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(DictionaryBucket<,>), project: "Lazinator.Collections", mainFolder: "/Dictionary/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorDictionary<,>), project: "Lazinator.Collections", mainFolder: "/Dictionary/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorTuple<,>), project: "Lazinator.Collections", mainFolder: "/Tuples/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorKeyValue<,>), project: "Lazinator.Collections", mainFolder: "/Tuples/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorComparableKeyValue<,>), project: "Lazinator.Collections", mainFolder: "/Tuples/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorTriple<,,>), project: "Lazinator.Collections", mainFolder: "/Tuples/", subfolder: "", ws);
        }


        [Fact]
        public async Task CodeGenerationProducesActualCode_BuffersAndPersistence()
        {

            AdhocWorkspace ws = GetAdhocWorkspace(); // must make sure that GetDirectories includes all folders here
            await CompleteGenerateCode(typeof(MemoryBlockLoadingInfo), project: "Lazinator", mainFolder: "/Buffers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(MemoryBlockInsetLoadingInfo), project: "Lazinator", mainFolder: "/Buffers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(MemoryBlockCollection), project: "Lazinator", mainFolder: "/Buffers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(MemoryRangeCollection), project: "Lazinator", mainFolder: "/Buffers/", subfolder: "", ws, c => (c ?? new LazinatorConfig()).WithDefaultAllowRecordLikeReadOnlyStructs(true));
            await CompleteGenerateCode(typeof(PersistentIndex), project: "Lazinator", mainFolder: "/Persistence/", subfolder: "", ws, c => (c ?? new LazinatorConfig()).WithDefaultAllowRecordLikeReadOnlyStructs(true));
        }

        [Fact]
        public async Task CodeGenerationProducesActualCode_Wrappers()
        {
            AdhocWorkspace ws = GetAdhocWorkspace();

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
            await CompleteGenerateCode(typeof(WInt16), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WUInt16), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WInt32), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WUInt32), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WInt64), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WUInt64), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
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
            await CompleteGenerateCode(typeof(WNullableInt16), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WNullableUInt16), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WNullableInt32), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WNullableUInt32), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WNullableInt64), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WNullableUInt64), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WNullableTimeSpan), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WNullableDateTime), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WDecimalArray), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WDoubleArray), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WFloatArray), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WInt32Array), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(WInt64Array), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
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
        public async Task CodeGenerationSingle()
        {
            AdhocWorkspace ws = GetAdhocWorkspace();
            // can put a single CompleteGenerateCode here if having trouble with that file
            // await CompleteGenerateCode(typeof(NullableContextEnabled), "LazinatorTests", "/Examples/", "ExampleHierarchy/", ws);
            //await CompleteGenerateCode(typeof(RecordLikeContainer), "LazinatorTests", "/Examples/", "Tuples/", ws);

            //await CompleteGenerateCode(typeof(RegularTuple), "LazinatorTests", "/Examples/", "Tuples/", ws);
            //await CompleteGenerateCode(typeof(ConcreteFromGenericFromBase), "LazinatorTests", "/Examples/", "Abstract/", ws); 

            // include some code so that we won't get a warning if not awaiting anything elsewhere
            Task GetT() { return Task.CompletedTask; };
            await GetT();
        }

        [Fact]
        public async Task CodeGenerationProducesActualCode_TestExamples()
        {
            AdhocWorkspace ws = GetAdhocWorkspace();
            //await CompleteGenerateCode(typeof(ExampleStructContainingStruct), "LazinatorTests", "/Examples/", "Structs/", ws); // Can change this to do a particular example -- then uncomment throw new Exception below
            // NOTE: Debugger seems to be running the test before allowing debugging of the test, so it's producing code prematurely. Thus, 
            // uncommenting the following will stop the run, so that we only run the one file that we want to focus on above.
            //throw new Exception(""); 

            await CompleteGenerateCode(typeof(Example), "LazinatorTests", "/Examples/", "ExampleHierarchy/", ws);
            await CompleteGenerateCode(typeof(TwoByteLengths), "LazinatorTests", "/Examples/", "ExampleHierarchy/", ws);
            await CompleteGenerateCode(typeof(EightByteLengths), "LazinatorTests", "/Examples/", "ExampleHierarchy/", ws);
            await CompleteGenerateCode(typeof(TwoByteLengthsContainer), "LazinatorTests", "/Examples/", "ExampleHierarchy/", ws);
            await CompleteGenerateCode(typeof(EightByteLengthsContainer), "LazinatorTests", "/Examples/", "ExampleHierarchy/", ws);
            await CompleteGenerateCode(typeof(ContainerForExampleWithDefault), "LazinatorTests", "/Examples/", "ExampleHierarchy/", ws);
            await CompleteGenerateCode(typeof(ContainerForEagerExample), "LazinatorTests", "/Examples/", "ExampleHierarchy/", ws);
            await CompleteGenerateCode(typeof(ExampleInterfaceContainer), "LazinatorTests", "/Examples/", "ExampleHierarchy/", ws);
            await CompleteGenerateCode(typeof(ExampleChild), "LazinatorTests", "/Examples/", "ExampleHierarchy/", ws);
            await CompleteGenerateCode(typeof(ExampleGrandchild), "LazinatorTests", "/Examples/", "ExampleHierarchy/", ws);
            await CompleteGenerateCode(typeof(ExampleChildInherited), "LazinatorTests", "/Examples/", "ExampleHierarchy/", ws);
            await CompleteGenerateCode(typeof(ExampleNonexclusiveInterfaceImplementer), "LazinatorTests", "/Examples/", "ExampleHierarchy/", ws);
            await CompleteGenerateCode(typeof(StructInAnotherNamespace), "LazinatorTests", "/Examples/", "ExampleHierarchy/AnotherNamespace/", ws);
            await CompleteGenerateCode(typeof(ContainerForNullableStructInAnotherNamespace), "LazinatorTests", "/Examples/", "ExampleHierarchy/", ws);
            await CompleteGenerateCode(typeof(ExampleStructContainingClasses), "LazinatorTests", "/Examples/", "Structs/", ws);
            await CompleteGenerateCode(typeof(ExampleContainerStructContainingClasses), "LazinatorTests", "/Examples/", "Structs/", ws);
            await CompleteGenerateCode(typeof(ExampleStructWithoutClass), "LazinatorTests", "/Examples/", "Structs/", ws);
            await CompleteGenerateCode(typeof(ContainerForExampleStructWithoutClass), "LazinatorTests", "/Examples/", "Structs/", ws);
            await CompleteGenerateCode(typeof(ExampleStructContainingStruct), "LazinatorTests", "/Examples/", "Structs/", ws);
            await CompleteGenerateCode(typeof(ExampleStructContainingStructContainer), "LazinatorTests", "/Examples/", "Structs/", ws);
            await CompleteGenerateCode(typeof(NullableContextEnabled), "LazinatorTests", "/Examples/", "ExampleHierarchy/", ws);
            await CompleteGenerateCode(typeof(NullableContextEnabledWithParameterlessConstructor), "LazinatorTests", "/Examples/", "ExampleHierarchy/", ws);
            await CompleteGenerateCode(typeof(SealedClass), "LazinatorTests", "/Examples/", "ExampleHierarchy/", ws);
            await CompleteGenerateCode(typeof(SingleParentClass), "LazinatorTests", "/Examples/", "ExampleHierarchy/", ws);
            await CompleteGenerateCode(typeof(UncompressedContainer), "LazinatorTests", "/Examples/", "ExampleHierarchy/", ws);
            await CompleteGenerateCode(typeof(Simplifiable), "LazinatorTests", "/Examples/", "ExampleHierarchy/", ws);
            await CompleteGenerateCode(typeof(RemoteHierarchy), "LazinatorTests", "/Examples/", "RemoteHierarchy/", ws);
            await CompleteGenerateCode(typeof(RemoteLevel1), "LazinatorTests", "/Examples/", "RemoteHierarchy/", ws);
            await CompleteGenerateCode(typeof(RemoteLevel2), "LazinatorTests", "/Examples/", "RemoteHierarchy/", ws);
            await CompleteGenerateCode(typeof(WrapperContainer), "LazinatorTests", "/Examples/", "Structs/", ws);
            await CompleteGenerateCode(typeof(SmallWrappersContainer), "LazinatorTests", "/Examples/", "Structs/", ws);
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
            await CompleteGenerateCode(typeof(ClosedGenericSealed), "LazinatorTests", "/Examples/", "NonAbstractGenerics/", ws);
            await CompleteGenerateCode(typeof(ClosedGenericWithGeneric), "LazinatorTests", "/Examples/", "NonAbstractGenerics/", ws);
            await CompleteGenerateCode(typeof(InheritingClosedGeneric), "LazinatorTests", "/Examples/", "NonAbstractGenerics/", ws);
            await CompleteGenerateCode(typeof(OpenGenericStayingOpenContainer), "LazinatorTests", "/Examples/", "NonAbstractGenerics/", ws);
            await CompleteGenerateCode(typeof(ClosedGenericWithoutBase), "LazinatorTests", "/Examples/", "NonAbstractGenerics/", ws);
            await CompleteGenerateCode(typeof(ConstrainedGeneric<,>), "LazinatorTests", "/Examples/", "NonAbstractGenerics/", ws);
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
            await CompleteGenerateCode(typeof(MultidimensionalArray), "LazinatorTests", "/Examples/", "Collections/", ws);
            await CompleteGenerateCode(typeof(LazinatorListContainer), "LazinatorTests", "/Examples/", "Collections/", ws);
            await CompleteGenerateCode(typeof(LazinatorListContainerGeneric<>), "LazinatorTests", "/Examples/", "Collections/", ws);
            await CompleteGenerateCode(typeof(StructTuple), "LazinatorTests", "/Examples/", "Tuples/", ws);
            await CompleteGenerateCode(typeof(RegularTuple), "LazinatorTests", "/Examples/", "Tuples/", ws);
            await CompleteGenerateCode(typeof(KeyValuePairTuple), "LazinatorTests", "/Examples/", "Tuples/", ws);
            await CompleteGenerateCode(typeof(NestedTuple), "LazinatorTests", "/Examples/", "Tuples/", ws);
            await CompleteGenerateCode(typeof(RecordLikeContainer), "LazinatorTests", "/Examples/", "Tuples/", ws, c => (c ?? new LazinatorConfig()).WithDefaultAllowRecordLikeStructs(true).WithDefaultAllowRecordLikeClasses(true).WithDefaultAllowRecordLikeReadOnlyStructs(true));
            await CompleteGenerateCode(typeof(RecordLikeCollections), "LazinatorTests", "/Examples/", "Tuples/", ws);
            await CompleteGenerateCode(typeof(ClassWithLocalEnum), "LazinatorTests", "/Examples/", "Subclasses/", ws);
            await CompleteGenerateCode(typeof(ClassWithForeignEnum), "LazinatorTests", "/Examples/", "Subclasses/", ws);
            await CompleteGenerateCode(typeof(ClassWithSubclass), "LazinatorTests", "/Examples/", "Subclasses/", ws);
            await CompleteGenerateCode(typeof(ClassWithSubclass.SubclassWithinClass), "LazinatorTests", "/Examples/", "Subclasses/", ws);
            await CompleteGenerateCode(typeof(LazinatorRecord), "LazinatorTests", "/Examples/", "Subclasses/", ws);
            await CompleteGenerateCode(typeof(LazinatorRecordSubclass), "LazinatorTests", "/Examples/", "Subclasses/", ws);
        }

        private static List<string> GetDirectories()
        {
            return new List<string>()
            {
                ReadCodeFile.GetCodeBasePath("Lazinator") + "/Attributes",
                ReadCodeFile.GetCodeBasePath("Lazinator.Collections"),
                ReadCodeFile.GetCodeBasePath("Lazinator.Collections") + "/ByteSpan",
                ReadCodeFile.GetCodeBasePath("Lazinator.Collections") + "/BitArray",
                ReadCodeFile.GetCodeBasePath("Lazinator.Collections") + "/Dictionary",
                ReadCodeFile.GetCodeBasePath("Lazinator.Collections") + "/Enumerators",
                ReadCodeFile.GetCodeBasePath("Lazinator.Collections") + "/OffsetList",
                ReadCodeFile.GetCodeBasePath("Lazinator") + "/Wrappers",
                ReadCodeFile.GetCodeBasePath("Lazinator") + "/Buffers",
                ReadCodeFile.GetCodeBasePath("Lazinator") + "/Persistence",
                ReadCodeFile.GetCodeBasePath("LazinatorTests") + "/Examples",
            };
        }

        private static async Task CompleteGenerateCode(Type existingType, string project, string mainFolder, string subfolder, AdhocWorkspace ws, Func<LazinatorConfig?, LazinatorConfig> modifyDefaultConfig = null)
        {
            bool completeTest = true; // Set to true to automatically update all test classes on the local development machine to a new format. This is useful as a way of updating the Lazinator code in the various Lazinator projects, which do not subscribe to the source generator. 
            bool automaticallyFixIfNoDiscrepancy = false; // if true, then the relevant code file is updated even if the contents other than the version and date matched; this can be used to update the version and the date. This should usually be set to false.
            bool automaticallyFixIfDiscrepancy = true; // if true, then if there is not a match, then the relevant code file is updated.

            if (!completeTest)
                return;
            
            if (mainFolder == "" && subfolder == "")
                mainFolder = "/";
            if (existingType.IsInterface)
                throw new Exception("Can complete generate code only on class implementing interface, not interface itself.");
            string projectPath = ReadCodeFile.GetCodeBasePath(project);
            string name = ReadCodeFile.GetNameOfType(existingType);
            ReadCodeFile.GetCodeInFile(projectPath, mainFolder, subfolder, name, ".g.cs", out string codeBehindPath, out string codeBehind);
            
            LazinatorConfig? config = FindConfigFileStartingFromSubfolder(mainFolder, subfolder, projectPath);
            if (config == null)
                config = new LazinatorConfig();
            // Note that if there is a config file already for the directory, that will take precedence over any modifications here
            if (modifyDefaultConfig != null)
            {
                config = modifyDefaultConfig(config); 
            }
            // uncomment to include tracing code -- also look at IncludeTracingCode in ObjectDescription.cs and the properties set in the Lazinator project file. 
            //config.IncludeTracingCode = true;

            Compilation compilation = await AdhocWorkspaceManager.GetCompilation(ws);
            LazinatorImplementingTypeInfo implementingTypeInfo = new LazinatorImplementingTypeInfo(compilation, existingType, config);
            
            var d = new LazinatorObjectDescription(implementingTypeInfo.ImplementingTypeSymbol, implementingTypeInfo, config, new RealDateTimeNow(), false);
            var result = d.GetCodeBehind();
            bool match = GeneratorTests.CompareExcludingLinesWithPrefix(codeBehind, result, "//     This code was generated by the Lazinator tool");
            
            if (automaticallyFixIfNoDiscrepancy || (automaticallyFixIfDiscrepancy && !match))
                File.WriteAllText(codeBehindPath, result);
            else
                match.Should().BeTrue();
        }

        private static LazinatorConfig? FindConfigFileStartingFromSubfolder(string mainFolder, string subfolder, string projectPath)
        {
            ReadCodeFile.GetCodeInFile(projectPath, mainFolder, subfolder, "LazinatorConfig", ".json", out string configPath, out string configText);
            if (configText == null || configText == "")
                ReadCodeFile.GetCodeInFile(projectPath, mainFolder, "/", "LazinatorConfig", ".json", out configPath, out configText);
            if (configText == null || configText == "")
                ReadCodeFile.GetCodeInFile(projectPath, "/", "", "LazinatorConfig", ".json", out configPath, out configText);
            LazinatorConfig? config = null;
            if (configText != null && configText != "")
                config = new LazinatorConfig(projectPath, configText);
            return config;
        }

        private AdhocWorkspace GetAdhocWorkspace()
        {
            List<string> directories = GetDirectories();
            return AdhocWorkspaceManager.CreateAdHocWorkspaceWithAllFilesInProjects(directories);
        }
    }
}