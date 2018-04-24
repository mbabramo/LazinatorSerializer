using System;
using System.IO;
using Lazinator.Support;
using Lazinator.Buffers; 
using Lazinator.Core; 
using static Lazinator.Core.LazinatorUtilities;

namespace LazinatorTests.Examples.Tuples
{
    public partial class NestedTuple : INestedTuple
    {
        public NestedTuple()
        {
        }

        public static NonLazinatorClass ConvertFromBytes_NonLazinatorClass(ReadOnlyMemory<byte> storage,
            DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate) => Convert_NonLazinatorType.ConvertFromBytes_NonLazinatorClass(storage, deserializationFactory, informParentOfDirtinessDelegate);

        public static void ConvertToBytes_NonLazinatorClass(BinaryBufferWriter writer,
            NonLazinatorClass itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness) => Convert_NonLazinatorType.ConvertToBytes_NonLazinatorClass(writer, itemToConvert,
            includeChildrenMode, verifyCleanness);
    }
}