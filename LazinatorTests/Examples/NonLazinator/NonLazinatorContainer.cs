using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Lazinator.Support;
using Lazinator.Buffers; 
using Lazinator.Core;
using LazinatorTests.Examples;
using static Lazinator.Core.LazinatorUtilities;

namespace LazinatorTests.Examples
{
    public partial struct NonLazinatorContainer : INonLazinatorContainer
    {

        public static NonLazinatorClass ConvertFromBytes_NonLazinatorClass(ReadOnlyMemory<byte> storage,
            DeserializationFactory deserializationFactory, LazinatorUtilities.InformParentOfDirtinessDelegate informParentOfDirtinessDelegate) =>
            Convert_NonLazinatorType.ConvertFromBytes_NonLazinatorClass(storage, deserializationFactory, informParentOfDirtinessDelegate);

        public static void ConvertToBytes_NonLazinatorClass(BinaryBufferWriter writer,
            NonLazinatorClass itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness) =>
            Convert_NonLazinatorType.ConvertToBytes_NonLazinatorClass(writer, itemToConvert,
                includeChildrenMode, verifyCleanness);


        public static NonLazinatorStruct ConvertFromBytes_NonLazinatorStruct(ReadOnlyMemory<byte> storage,
            DeserializationFactory deserializationFactory, LazinatorUtilities.InformParentOfDirtinessDelegate informParentOfDirtinessDelegate) =>
            Convert_NonLazinatorType.ConvertFromBytes_NonLazinatorStruct(storage, deserializationFactory, informParentOfDirtinessDelegate);

        public static void ConvertToBytes_NonLazinatorStruct(BinaryBufferWriter writer,
            NonLazinatorStruct itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness) =>
            Convert_NonLazinatorType.ConvertToBytes_NonLazinatorStruct(writer, itemToConvert,
                includeChildrenMode, verifyCleanness);
    }
}
