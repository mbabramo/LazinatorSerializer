using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using LazinatorTests.Examples;
using Lazinator.Core;
using Xunit;
using Lazinator.Wrappers;
using LazinatorTests.Examples.Hierarchy;
using LazinatorTests.Examples.Structs;
using LazinatorTests.Examples.NonAbstractGenerics;
using LazinatorCollections.Tuples;
using Lazinator.Buffers;
using LazinatorTests.Examples.ExampleHierarchy;
using System.Threading.Tasks;
using System.Diagnostics;

namespace LazinatorTests.Tests
{
    public class BasicTests : SerializationDeserializationTestBase
    {
        [Fact]
        public void BasicLazinatorSerializationWorks_WInt()
        {
            var original = (WInt32)17;
            var copy = original.CloneLazinatorTyped();
            copy.WrappedValue.Should().Be(17);
        }

        [Fact]
        public void BasicLazinatorSerializationWorks()
        {
            var original = GetTypicalExample();
            var copy = GetTypicalExample();
            var result = original.CloneLazinatorTyped();
            ExampleEqual(copy, result).Should().BeTrue();
        }

        [Fact]
        public async ValueTask AsynchronousSerializationWorks()
        {

            var original = GetTypicalExample();
            var copy = GetTypicalExample();
            var result = await original.CloneLazinatorTypedAsync(); 
            ExampleEqual(copy, result).Should().BeTrue();
        }

        private static LazinatorMemory GetLazinatorMemoryCopy(ILazinator e)
        {
            e.SerializeLazinator();
            var buffer = e.LazinatorMemoryStorage.GetConsolidatedMemory();
            BufferWriter b = new BufferWriter();
            b.Write(buffer.Span);
            return b.LazinatorMemory;
        }

        [Fact]
        public void ManualDeserializationWorks()
        {
            Example original = GetTypicalExample();
            LazinatorMemory serializedBytes = GetLazinatorMemoryCopy(original);
            Example copy = new Example(serializedBytes);
            var result = original.CloneLazinatorTyped();
            ExampleEqual(copy, result).Should().BeTrue();
        }

        [Fact]
        public void ManualDeserializationWorks_NullableEnabledContext()
        {
            NullableEnabledContext original = CloneNoBufferTests.GetNullableEnabledContext();
            LazinatorMemory serializedBytes = GetLazinatorMemoryCopy(original);
            NullableEnabledContext copy = new NullableEnabledContext(serializedBytes);
            LazinatorMemory serializedBytesCopy = GetLazinatorMemoryCopy(original);
            serializedBytes.Matches(serializedBytesCopy.InitialMemory.Span).Should().BeTrue();
        }

        [Fact]
        public void LazinatorSerializationCanSetChildToNull()
        {
            var original = GetTypicalExample();
            var result = original.CloneLazinatorTyped();
            result.MyChild1 = null;
            var result2 = result.CloneLazinatorTyped();
            result2.MyChild1.Should().Be(null);
        }

        [Fact]
        public void LazinatorSerializationVersionUpgradeWorks()
        {
            var original = GetTypicalExample();
            // Set to old version number. This should serialize as the old version number.
            original.LazinatorObjectVersion = 2;
            original.MyOldString = "Old string";
            var bytes = original.SerializeLazinator(new LazinatorSerializationOptions(IncludeChildrenMode.ExcludeAllChildren, false, false, false)); // serializes as version 3

            var upgraded = new Example(bytes);
            upgraded.LazinatorObjectVersion.Should().Be(3);
            upgraded.MyOldString.Should().Be(null);
            upgraded.MyNewString.Should().Be("NEW Old string");
        }

        [Fact]
        public void LazinatorSerializationVersionDowngradeWorks()
        {
            var original = GetTypicalExample();
            // Set to old version number. This should serialize as the old version number.
            original.LazinatorObjectVersion = 2;
            original.MyOldString = "Old string";
            var bytes = original.SerializeLazinator(new LazinatorSerializationOptions(IncludeChildrenMode.ExcludeAllChildren, false, false, false)); // serializes as version 3

            // Now, deserializing the version 3 bytes, but again setting to the old version.
            var stillOldVersion = new Example(bytes, null, IncludeChildrenMode.IncludeAllChildren, 2); // set back to version 2
            stillOldVersion.MyOldString.Should().Be("Old string");
            stillOldVersion.MyNewString.Should().Be(null);
        }


        [Fact]
        public void ExampleInterfaceContainerWorks()
        {
            ExampleInterfaceContainer c = new ExampleInterfaceContainer()
            {
                ExampleByInterface = GetExample(2),
            };

            var c2 = c.CloneLazinatorTyped();
            ExampleEqual((Example)c.ExampleByInterface, (Example)c2.ExampleByInterface).Should().BeTrue();
        }

        [Fact]
        public void RecursiveExampleWorks()
        {
            // the Recursive example allows us to build a tree
            RecursiveExample r = new RecursiveExample()
            {
                RecursiveClass = new RecursiveExample(),
                RecursiveInterface = new RecursiveExample()
                {
                    RecursiveClass = new RecursiveExample()
                },
            };
            var r2 = r.CloneLazinatorTyped();
            r2.RecursiveClass.Should().NotBeNull();
            r2.RecursiveInterface.Should().NotBeNull();
            r2.RecursiveInterface.RecursiveClass.Should().NotBeNull();
        }

        [Fact]
        public void TwoByteAndEightByteLengthsWork()
        {
            TwoByteLengthsContainer container2 = new TwoByteLengthsContainer()
            {
                Contents = new TwoByteLengths()
                {
                    Example = GetExample(0)
                }
            };
            container2.SerializeLazinator();
            EightByteLengthsContainer container8 = new EightByteLengthsContainer()
            {
                Contents = new EightByteLengths()
                {
                    Example = GetExample(0)
                }
            };
            container8.SerializeLazinator();
            container8.LazinatorMemoryStorage.Length.Should().Be(container2.LazinatorMemoryStorage.Length + 6);

            container2 = container2.CloneLazinatorTyped();
            container8 = container8.CloneLazinatorTyped();
            container8.LazinatorMemoryStorage.Length.Should().Be(container2.LazinatorMemoryStorage.Length + 6);
        }


        [Fact]
        public void DistantPropertiesSerialized()
        {
            var hierarchy = GetHierarchy(0, 1, 2, 0, 0);
            hierarchy.MyChild1.MyWrapperContainer = new WrapperContainer() { WrappedInt = 17 };
            hierarchy = hierarchy.CloneLazinatorTyped();
            hierarchy.MyChild1.MyWrapperContainer.WrappedInt.Should().Be(17);
            hierarchy.MyChild1.MyWrapperContainer.WrappedInt = 19;
            hierarchy = hierarchy.CloneLazinatorTyped();
            hierarchy.MyChild1.MyWrapperContainer.WrappedInt.Should().Be(19);
        }

        [Fact]
        public void PropertiesWorkAfterConvertToBytes()
        {
            // Suppose we have two properties, property A and property B. After deserialization, the byte points are set. Suppose that property A is accessed, but property B is not. If we then convert to bytes but continue to use the old object, then the old byte points will be out of date. Property A will be good; since it has been accessed, it still has the object. But unless we update the byte points, property B will now be pointing to a spot on the old set of bytes.

            LazinatorTriple<WString, WString, WString> x = new LazinatorTriple<WString, WString, WString>("one", "andtwo", "andthree");
            var c3 = x.CloneLazinatorTyped(); // c3 has LazinatorMemoryStorage set
            c3.Item1 = "another";
            var c4 = c3.CloneLazinatorTyped(); // LazinatorMemoryStorage is now updated in C3
            c3.Item1.Should().Be("another");
            c3.Item2.Should().Be("andtwo"); // this will cause a problem if the byte indides are not set correctly
        }


        [Fact]
        public void LazinatorSerializationRecognizesUpdates()
        {
            var hierarchy = GetHierarchy(0, 1, 2, 0, 0);
            var original = GetHierarchy(0, 1, 2, 0, 0);
            var withChildrenReversed = GetHierarchy(0, 2, 1, 0, 0);

            foreach (bool explicitlyCallConvertFromBytes in new bool[] { true, false })
                foreach (bool verifyCleanness in new bool[] { true, false })
                    hierarchy = ReverseChildrenAndBack(hierarchy, original, withChildrenReversed, explicitlyCallConvertFromBytes, verifyCleanness);
        }

        private Example ReverseChildrenAndBack(Example hierarchy, Example original, Example withChildrenReversed, bool explicitlyCallConvertFromBytes, bool verifyCleanness)
        {
            ReverseChildrenMembers(hierarchy);
            ExampleEqual(hierarchy, withChildrenReversed).Should().Be(true);
            hierarchy = CloneWithOptionalVerification(hierarchy, true, verifyCleanness);
            ExampleEqual(hierarchy, withChildrenReversed).Should().Be(true);
            ReverseChildrenMembers(hierarchy);
            hierarchy = CloneWithOptionalVerification(hierarchy, true, verifyCleanness);
            ExampleEqual(hierarchy, original).Should().Be(true);
            return hierarchy;
        }

        private void ReverseChildrenMembers(Example original)
        {
            long myLong1 = original.MyChild1.MyLong;
            short myShort1 = original.MyChild1.MyShort;
            long myLong2 = original.MyChild2.MyLong;
            short myShort2 = original.MyChild2.MyShort;
            original.MyChild1.MyLong = myLong2;
            original.MyChild1.MyShort = myShort2;
            original.MyChild2.MyLong = myLong1;
            original.MyChild2.MyShort = myShort1;
        }

        [Fact]
        public void DeserializeMultipleWorks()
        {
            Example e = GetTypicalExample();
            OpenGeneric<WFloat> o = new OpenGeneric<WFloat>()
            {
                MyT = 3.0F,
                MyListT = new List<WFloat>()
                    {
                        1.0F,
                        2.0F
                    }
            };
            e.SerializeLazinator();
            o.SerializeLazinator();
            int lengthExample = (int) e.CloneLazinator().LazinatorMemoryStorage.Length;
            int lengthOpenGeneric = (int)o.CloneLazinator().LazinatorMemoryStorage.Length;
            int lengthSoFar = 0;
            const int numPairs = 5;
            Memory<byte> memory = new Memory<byte>(new byte[numPairs * (lengthExample + lengthOpenGeneric)]);
            for (int i = 0; i < numPairs; i++)
            {
                e.LazinatorMemoryStorage.OnlyMemory.CopyTo(memory.Slice(lengthSoFar));
                lengthSoFar += lengthExample;
                o.LazinatorMemoryStorage.OnlyMemory.CopyTo(memory.Slice(lengthSoFar));
                lengthSoFar += lengthOpenGeneric;
            }
            var results = DeserializationFactory.Instance
                .CreateMultiple(memory, null)
                .Select(x => (ILazinator)x)
                .Where(x => x != null).ToList();
            results.Count().Should().Be(10);
            for (int i = 0; i < numPairs; i++)
            {
                (results[2 * i] as Example).Should().NotBeNull();
                (results[2 * i + 1] as OpenGeneric<WFloat>).Should().NotBeNull();
                (results[2 * i + 1] as OpenGeneric<WInt32>).Should().BeNull();
            }
        }

        [Fact]
        public void VerifyHierarchyWorks()
        {
            var original = GetTypicalExample();
            var another = GetTypicalExample();
            LazinatorUtilities.ConfirmHierarchiesEqual(original, another);
            var clone = original.CloneLazinatorTyped();
            LazinatorUtilities.ConfirmHierarchiesEqual(original, clone);
        }
        
        public static IEnumerable<object[]> ConfirmHierarchySerializationData()
        {
            for (int i = 0; i <= 2; i++)
                for (int i2 = 0; i2 <= 3; i2++)
                    for (int i3 = 0; i3 <= 3; i3++)
                        for (int i4 = 0; i4 <= 3; i4++)
                            for (int i5 = 0; i5 <= 2; i5++)
                            {
                                yield return new object[] { i, i2, i3, i4, i5 };
                            }
        }

        [Theory]
        [MemberData(nameof(ConfirmHierarchySerializationData))]
        public  void ConfirmHierarchySerialization(int indexUpTo2, int indexUpTo3a, int indexUpTo3b,
            int indexUpTo3c, int indexUpTo2b)
        {
            var original = GetHierarchy(indexUpTo2, indexUpTo3a, indexUpTo3b, indexUpTo3c, indexUpTo2b);
            var copy = GetHierarchy(indexUpTo2, indexUpTo3a, indexUpTo3b, indexUpTo3c, indexUpTo2b);
            var anotherCopyForReference = GetHierarchy(indexUpTo2, indexUpTo3a, indexUpTo3b, indexUpTo3c, indexUpTo2b);
            // do an initial serialization / deserialization cycle
            var result = original.CloneLazinatorTyped();
            ExampleEqual(copy, result).Should().BeTrue();
            result.MyChild1?.LazinatorParents.EnumerateParents().Any(x => x == result).Should().BeTrue();
            result.MyChild2?.LazinatorParents.EnumerateParents().Any(x => x == result).Should().BeTrue();
            // repeat the cycle
            var result2 = result.CloneLazinatorTyped();
            ExampleEqual(copy, result2).Should().BeTrue();
            // and now again, verifying cleanness
            var result3 = CloneWithOptionalVerification(result2, true, true);
            ExampleEqual(copy, result3).Should().BeTrue();
            ExampleEqual(original, result3).Should().BeTrue();
        }

        [Fact]
        public void RefPropertiesWork()
        {
            void ChangeStuff(ref char c, ref string s)
            {
                c = 'k';
                s = "changed string";
            }

            Example e = GetTypicalExample();
            e = e.CloneLazinatorTyped();
            e.IsDirty.Should().BeFalse();
            ChangeStuff(ref e.MyChar_Ref, ref e.MyString_Ref);
            e.IsDirty.Should().BeTrue();
            e.MyChar.Should().Be('k');
            e.MyString.Should().Be("changed string");
        }

        [Fact]
        public void CloneLazinatorTyped_NullEnabledContext()
        {
            var nec = CloneNoBufferTests.GetNullableEnabledContext();
            var result = nec.CloneLazinatorTyped(IncludeChildrenMode.IncludeAllChildren);
        }

    }
}
