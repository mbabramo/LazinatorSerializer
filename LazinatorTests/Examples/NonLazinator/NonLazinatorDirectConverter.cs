using Lazinator.Buffers;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples
{
    public static class NonLazinatorDirectConverter
    {
        public static NonLazinatorClass ConvertFromBytes_LazinatorTests_Examples_NonLazinatorClass(ReadOnlyMemory<byte> storage,
               DeserializationFactory deserializationFactory, LazinatorUtilities.InformParentOfDirtinessDelegate informParentOfDirtinessDelegate) =>
               Convert_NonLazinatorType.ConvertFromBytes_LazinatorTests_Examples_NonLazinatorClass(storage, deserializationFactory, informParentOfDirtinessDelegate);

        public static void ConvertToBytes_LazinatorTests_Examples_NonLazinatorClass(BinaryBufferWriter writer,
            NonLazinatorClass itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness) =>
            Convert_NonLazinatorType.ConvertToBytes_LazinatorTests_Examples_NonLazinatorClass(writer, itemToConvert,
                includeChildrenMode, verifyCleanness);


        public static NonLazinatorStruct ConvertFromBytes_LazinatorTests_Examples_NonLazinatorStruct(ReadOnlyMemory<byte> storage,
            DeserializationFactory deserializationFactory, LazinatorUtilities.InformParentOfDirtinessDelegate informParentOfDirtinessDelegate) =>
            Convert_NonLazinatorType.ConvertFromBytes_LazinatorTests_Examples_NonLazinatorStruct(storage, deserializationFactory, informParentOfDirtinessDelegate);

        public static void ConvertToBytes_LazinatorTests_Examples_NonLazinatorStruct(BinaryBufferWriter writer,
            NonLazinatorStruct itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness) =>
            Convert_NonLazinatorType.ConvertToBytes_LazinatorTests_Examples_NonLazinatorStruct(writer, itemToConvert,
                includeChildrenMode, verifyCleanness);
    }
}
