using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Lazinator.Support;
using Lazinator.Buffers; 
using Lazinator.Core; 
using static Lazinator.Core.LazinatorUtilities;

namespace LazinatorTests.Examples
{
    public partial struct ExampleStruct : IExampleStruct
    {
        public static NonLazinatorClass ConvertFromBytes_LazinatorTests_Examples_NonLazinatorClass(ReadOnlyMemory<byte> storage,
            DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate) =>
            Convert_NonLazinatorType.ConvertFromBytes_LazinatorTests_Examples_NonLazinatorClass(storage, deserializationFactory, informParentOfDirtinessDelegate);

        public static void ConvertToBytes_LazinatorTests_Examples_NonLazinatorClass(BinaryBufferWriter writer,
            NonLazinatorClass itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness) =>
            Convert_NonLazinatorType.ConvertToBytes_LazinatorTests_Examples_NonLazinatorClass(writer, itemToConvert,
                includeChildrenMode, verifyCleanness);
    }
}
