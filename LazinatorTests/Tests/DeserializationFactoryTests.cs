using System;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;
using LazinatorTests.Examples;
using Lazinator.Core;
using Lazinator.Wrappers;
using LazinatorTests.Examples.Abstract;
using System.Linq;
using Lazinator.Buffers;
using LazinatorCollections.Tuples;

namespace LazinatorTests.Tests
{
    public class DeserializationFactoryTests
    {

        private static void ConfirmDeserializationFactoryWorks(DeserializationFactory df)
        {
            Example parent = new Example();
            LazinatorMemory serializedBytes = new LazinatorMemory(new Memory<byte>());
            ILazinator lazinatorObject =
                df.CreateKnownID((int) ExampleUniqueIDs.ExampleChild, serializedBytes,
                    parent);
            lazinatorObject.Should().NotBeNull();
            lazinatorObject.LazinatorMemoryStorage.Should().Be(serializedBytes);
            lazinatorObject.LazinatorParents.Should().Be(parent);
        }

        [Fact]
        public void CanGetTypeFromGenericTypeIDs_MaxOneTypeParameter()
        {
            List<int> IDs = new List<int>() { (int)ExampleUniqueIDs.GenericFromBase, (int)ExampleUniqueIDs.GenericFromBase, (int)Lazinator.LazinatorCoreUniqueIDs.IWInt32 };
            DeserializationFactory deserializationFactory = DeserializationFactory.Instance;
            (Type t, int numberIDsConsumed) = deserializationFactory.GetTypeBasedOnGenericIDType(new LazinatorGenericIDType(IDs));
            Type expectedType = typeof(GenericFromBase<GenericFromBase<WInt32>>);
            t.Equals(expectedType).Should().BeTrue();
            var IDsForType = deserializationFactory.GetUniqueIDListForGenericType(t);
            IDs.SequenceEqual(IDsForType).Should().BeTrue();
        }

        [Fact]
        public void CanGetTypeFromGenericTypeIDs_MultipleTypeParameters()
        {
            List<int> IDs = new List<int>() { (int)LazinatorCollections.LazinatorCollectionUniqueIDs.ILazinatorTriple, (int)ExampleUniqueIDs.GenericFromBase, (int)Lazinator.LazinatorCoreUniqueIDs.IWInt32, (int)Lazinator.LazinatorCoreUniqueIDs.IWInt64, (int)ExampleUniqueIDs.GenericFromBase, (int)ExampleUniqueIDs.GenericFromBase, (int)Lazinator.LazinatorCoreUniqueIDs.IWByte };
            DeserializationFactory deserializationFactory = DeserializationFactory.Instance;
            (Type t, int numberIDsConsumed) = deserializationFactory.GetTypeBasedOnGenericIDType(new LazinatorGenericIDType(IDs));
            Type expectedType = typeof(LazinatorTriple<GenericFromBase<WInt32>, WInt64, GenericFromBase<GenericFromBase<WByte>>>);
            t.Equals(expectedType).Should().BeTrue();
            var IDsForType = deserializationFactory.GetUniqueIDListForGenericType(t);
            IDs.SequenceEqual(IDsForType).Should().BeTrue();
        }
    }
}
