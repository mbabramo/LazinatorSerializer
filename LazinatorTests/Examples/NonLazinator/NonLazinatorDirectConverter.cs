﻿using Lazinator.Buffers;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples
{
    public static class NonLazinatorDirectConverter
    {
        public static NonLazinatorClass ConvertFromBytes_NonLazinatorClass(ReadOnlyMemory<byte> storage, LazinatorUtilities.InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            // note that any serialization/deserialization method can be used here, as it's not autogenerated, and the order just needs to be consistent.
            // so, we'll do it a bit differently from autogen to demonstrate. We'll also change how strings are encoded.

            if (storage.Length == 0)
                return null;
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int myInt = span.ToDecompressedInt(ref bytesSoFar);

            string myString = span.ToString_VarIntLengthUtf8(ref bytesSoFar);

            return new NonLazinatorClass() { MyInt = myInt, MyString = myString };
        }

        public static void ConvertToBytes_NonLazinatorClass(BinaryBufferWriter writer, NonLazinatorClass itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == null)
                return;
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.MyInt);
            writer.WriteStringUtf8WithVarIntPrefix(itemToConvert.MyString);
        }



        public static NonLazinatorStruct ConvertFromBytes_NonLazinatorStruct(ReadOnlyMemory<byte> storage, LazinatorUtilities.InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            // note that any serialization/deserialization method can be used here, as it's not autogenerated, and the order just needs to be consistent.
            // so, we'll do it a bit differently from autogen to demonstrate. We'll also change how strings are encoded.

            if (storage.Length == 0)
                return default;
            ReadOnlySpan<byte> span = storage.Span;

            int bytesSoFar = 0;
            int myInt = span.ToDecompressedInt(ref bytesSoFar);

            string myString = span.ToString_VarIntLengthUtf8(ref bytesSoFar);

            return new NonLazinatorStruct() { MyInt = myInt, MyString = myString };
        }

        public static void ConvertToBytes_NonLazinatorStruct(BinaryBufferWriter writer, NonLazinatorStruct itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.MyInt);
            writer.WriteStringUtf8WithVarIntPrefix(itemToConvert.MyString);
        }
    }
}
