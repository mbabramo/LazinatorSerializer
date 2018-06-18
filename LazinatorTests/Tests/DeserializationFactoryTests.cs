using System;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;
using LazinatorTests.Examples;
using Lazinator.Core;
using Lazinator.Wrappers;
using LazinatorTests.Examples.Abstract;
using Lazinator.Collections;
using System.Linq;

namespace LazinatorTests.Tests
{
    public class DeserializationFactoryTests
    {

        private static void ConfirmDeserializationFactoryWorks(DeserializationFactory df)
        {
            Example parent = new Example();
            ReadOnlyMemory<byte> serializedBytes = new ReadOnlyMemory<byte>();
            ILazinator selfSerialized =
                df.CreateKnownID((int) ExampleUniqueIDs.ExampleChild, serializedBytes,
                    parent);
            selfSerialized.Should().NotBeNull();
            selfSerialized.LazinatorObjectBytes.Should().Be(serializedBytes);
            selfSerialized.LazinatorParentClass.Should().Be(parent);
        }

        [Fact]
        public void CanGetTypeFromGenericTypeIDs_MaxOneTypeParameter()
        {
            List<int> IDs = new List<int>() { (int)ExampleUniqueIDs.GenericFromBase, (int)ExampleUniqueIDs.GenericFromBase, (int)Lazinator.Collections.LazinatorCollectionUniqueIDs.WInt };
            DeserializationFactory deserializationFactory = DeserializationFactory.Instance;
            (Type t, int numberIDsConsumed) = deserializationFactory.GetTypeBasedOnGenericIDType(new LazinatorGenericIDType(IDs));
            Type expectedType = typeof(GenericFromBase<GenericFromBase<WInt>>);
            t.Equals(expectedType).Should().BeTrue();
            var IDsForType = deserializationFactory.GetUniqueIDListForGenericType(t);
            IDs.SequenceEqual(IDsForType).Should().BeTrue();
        }

        [Fact]
        public void CanGetTypeFromGenericTypeIDs_MultipleTypeParameters()
        {
            List<int> IDs = new List<int>() { (int)Lazinator.Collections.LazinatorCollectionUniqueIDs.LazinatorTriple, (int)ExampleUniqueIDs.GenericFromBase, (int)Lazinator.Collections.LazinatorCollectionUniqueIDs.WInt, (int)Lazinator.Collections.LazinatorCollectionUniqueIDs.WLong, (int)ExampleUniqueIDs.GenericFromBase, (int)ExampleUniqueIDs.GenericFromBase, (int)Lazinator.Collections.LazinatorCollectionUniqueIDs.WByte };
            DeserializationFactory deserializationFactory = DeserializationFactory.Instance;
            (Type t, int numberIDsConsumed) = deserializationFactory.GetTypeBasedOnGenericIDType(new LazinatorGenericIDType(IDs));
            Type expectedType = typeof(LazinatorTriple<GenericFromBase<WInt>, WLong, GenericFromBase<GenericFromBase<WByte>>>);
            t.Equals(expectedType).Should().BeTrue();
            var IDsForType = deserializationFactory.GetUniqueIDListForGenericType(t);
            IDs.SequenceEqual(IDsForType).Should().BeTrue();
        }
    }
}
