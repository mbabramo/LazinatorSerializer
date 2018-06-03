using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FluentAssertions;
using Lazinator.Support;
using LazinatorTests.Examples;
using Lazinator.Buffers; 
using Lazinator.Core; 
using static Lazinator.Core.LazinatorUtilities;

namespace LazinatorTests.Tests
{
    public class DeserializationFactoryTests
    {
        [Fact]
        public void CanSetupDeserializationFactoryType()
        {
            DeserializationFactory df = new DeserializationFactory(new Type[] { typeof(ExampleChild) }, false);
            ConfirmDeserializationFactoryWorks(df);
        }

        [Fact]
        public void CanSetupDeserializationFactoryTypeFromAnotherType()
        {
            DeserializationFactory df = new DeserializationFactory(new Type[] { typeof(Example) }, true);
            ConfirmDeserializationFactoryWorks(df);
        }

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
            selfSerialized.DeserializationFactory.Should().Be(df);
        }
    }
}
