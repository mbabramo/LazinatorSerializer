using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Buffers;
using Lazinator.Core;

namespace LazinatorTests.Examples.Abstract
{
    public partial class ConcreteGeneric2a : AbstractGeneric1<int>, IConcreteGeneric2a
    {
        public override ILazinator LazinatorParentClass { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override bool IsDirty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override LazinatorUtilities.InformParentOfDirtinessDelegate InformParentOfDirtinessDelegate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override bool DescendantIsDirty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override DeserializationFactory DeserializationFactory { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override MemoryInBuffer HierarchyBytes { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override ReadOnlyMemory<byte> LazinatorObjectBytes { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override int MyT { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override int LazinatorUniqueID => throw new NotImplementedException();

        public override int LazinatorObjectVersion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string AnotherProperty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Example LazinatorExample { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override ILazinator CloneLazinator()
        {
            throw new NotImplementedException();
        }

        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode)
        {
            throw new NotImplementedException();
        }

        public override void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            throw new NotImplementedException();
        }

        public override void Deserialize()
        {
            throw new NotImplementedException();
        }

        public override void InformParentOfDirtiness()
        {
            throw new NotImplementedException();
        }

        public override void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            throw new NotImplementedException();
        }

        public override MemoryInBuffer SerializeNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            throw new NotImplementedException();
        }

        internal override MemoryInBuffer EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            throw new NotImplementedException();
        }
    }
}
