using System;
using System.Collections.Generic;
using System.IO;
using Lazinator.Attributes;
using Lazinator.Support;
using Lazinator.Buffers; 
using Lazinator.Core;
using LazinatorTests.Examples;
using static Lazinator.Core.LazinatorUtilities;

namespace LazinatorTests.Examples.Collections
{
    public partial class DotNetList_Nested_NonSelfSerializable : IDotNetList_Nested_NonSelfSerializable
    {
        public DotNetList_Nested_NonSelfSerializable()
        {
        }

        public static NonLazinatorClass ConvertFromBytes_NonLazinatorClass(ReadOnlyMemory<byte> storage,
            DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate) => Convert_NonLazinatorType.ConvertFromBytes_NonLazinatorClass(storage, deserializationFactory, informParentOfDirtinessDelegate);

        public static void ConvertToBytes_NonLazinatorClass(BinaryBufferWriter writer,
            NonLazinatorClass itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness) => Convert_NonLazinatorType.ConvertToBytes_NonLazinatorClass(writer, itemToConvert,
            includeChildrenMode, verifyCleanness);

    }
}
