using Lazinator.Buffers;
using Lazinator.Core;
using System;

namespace LazinatorTests.Examples
{
    public static class NonLazinatorDirectConverter
    {
        public static NonLazinatorClass ConvertFromBytes_NonLazinatorClass(LazinatorMemory storage)
        {
            // note that any serialization/deserialization method can be used here, as it's not autogenerated, and the order just needs to be consistent.
            // so, we'll do it a bit differently from autogen to demonstrate. We'll also change how strings are encoded.

            if (storage.Length == 0)
                return null;
            ReadOnlySpan<byte> span = storage.InitialReadOnlyMemory.Span;

            int bytesSoFar = 0;
            int myInt = span.ToDecompressedInt32(ref bytesSoFar);

            string myString = span.ToString_VarIntLengthUtf8(ref bytesSoFar);

            return new NonLazinatorClass() { MyInt = myInt, MyString = myString };
        }

        public static void ConvertToBytes_NonLazinatorClass(ref BufferWriter writer, NonLazinatorClass itemToConvert, LazinatorSerializationOptions options)
        {
            if (itemToConvert == null)
                return;
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.MyInt);
            writer.WriteStringUtf8WithVarIntPrefix(itemToConvert.MyString);
        }

        public static NonLazinatorClass CloneOrChange_NonLazinatorClass(NonLazinatorClass itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            return itemToClone == null ? null : new NonLazinatorClass() { MyInt = itemToClone.MyInt, MyString = itemToClone.MyString };
        }

        public static NonLazinatorStruct ConvertFromBytes_NonLazinatorStruct(LazinatorMemory storage)
        {
            // note that any serialization/deserialization method can be used here, as it's not autogenerated, and the order just needs to be consistent.
            // so, we'll do it a bit differently from autogen to demonstrate. We'll also change how strings are encoded.

            if (storage.Length == 0)
                return default;
            ReadOnlySpan<byte> span = storage.InitialReadOnlyMemory.Span;

            int bytesSoFar = 0;
            int myInt = span.ToDecompressedInt32(ref bytesSoFar);

            string myString = span.ToString_VarIntLengthUtf8(ref bytesSoFar);

            return new NonLazinatorStruct() { MyInt = myInt, MyString = myString };
        }

        public static void ConvertToBytes_NonLazinatorStruct(ref BufferWriter writer, NonLazinatorStruct itemToConvert, LazinatorSerializationOptions options)
        {
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.MyInt);
            writer.WriteStringUtf8WithVarIntPrefix(itemToConvert.MyString);
        }

        public static NonLazinatorStruct CloneOrChange_NonLazinatorStruct(NonLazinatorStruct itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            return itemToClone;
        }
    }
}
