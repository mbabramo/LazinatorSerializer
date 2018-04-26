﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.CodeDescription
{
    public static class PrimitiveReadWriteMethodNames
    {
        public static readonly Dictionary<string, string> ReadNames = new Dictionary<string, string>()
        {
            { "bool", $"ToBoolean" },
            { "byte", $"ToByte" },
            { "sbyte", $"ToSByte" },
            { "char", $"ToChar" },
            { "decimal", $"ToDecompressedDecimal" },
            { "float", $"ToSingle" },
            { "double", $"ToDouble" },
            { "short", $"ToDecompressedShort" },
            { "ushort", $"ToDecompressedUshort" },
            { "int", $"ToDecompressedInt" },
            { "uint", $"ToDecompressedUint" },
            { "long", $"ToDecompressedLong" },
            { "ulong", $"ToDecompressedUlong" },
            { "TimeSpan", $"ToDecompressedTimeSpan" },
            { "DateTime", $"ToDecompressedDateTime" },
            { "Guid", $"ToGuid" },
            { "string", $"ToString_VarIntLength" },
            { "bool?", $"ToDecompressedNullableBool" },
            { "byte?", $"ToDecompressedNullableByte" },
            { "sbyte?", $"ToDecompressedNullableSByte" },
            { "char?", $"ToNullableChar" },
            { "decimal?", $"ToDecompressedNullableDecimal" },
            { "float?", $"ToNullableSingle" },
            { "double?", $"ToNullableDouble" },
            { "short?", $"ToDecompressedNullableShort" },
            { "ushort?", $"ToDecompressedNullableUshort" },
            { "int?", $"ToDecompressedNullableInt" },
            { "uint?", $"ToDecompressedNullableUint" },
            { "long?", $"ToDecompressedNullableLong" },
            { "ulong?", $"ToDecompressedNullableUlong" },
            { "TimeSpan?", $"ToDecompressedNullableTimeSpan" },
            { "DateTime?", $"ToDecompressedNullableDateTime" },
            { "Guid?", $"ToNullableGuid" },
        };


        public static readonly Dictionary<string, string> WriteNames = new Dictionary<string, string>()
        {
            { "bool", $"WriteUncompressedPrimitives.WriteBool" },
            { "byte", $"WriteUncompressedPrimitives.WriteByte" },
            { "sbyte", $"WriteUncompressedPrimitives.WriteSByte" },
            { "char", $"EncodeCharAndString.WriteCharInTwoBytes" },
            { "decimal", $"CompressedDecimal.WriteCompressedDecimal" },
            { "float", $"WriteUncompressedPrimitives.WriteSingle" },
            { "double", $"WriteUncompressedPrimitives.WriteDouble" },
            { "short", $"CompressedIntegralTypes.WriteCompressedShort" },
            { "ushort", $"CompressedIntegralTypes.WriteCompressedUshort" },
            { "int", $"CompressedIntegralTypes.WriteCompressedInt" },
            { "uint", $"CompressedIntegralTypes.WriteCompressedUint" },
            { "long", $"CompressedIntegralTypes.WriteCompressedLong" },
            { "ulong", $"CompressedIntegralTypes.WriteCompressedUlong" },
            { "TimeSpan", $"CompressedIntegralTypes.WriteCompressedTimeSpan" },
            { "DateTime", $"CompressedIntegralTypes.WriteCompressedDateTime" },
            { "Guid", $"WriteUncompressedPrimitives.WriteGuid" },
            { "string", $"EncodeCharAndString.WriteStringWithVarIntPrefix" },
            { "bool?", $"CompressedIntegralTypes.WriteCompressedNullableBool" },
            { "byte?", $"CompressedIntegralTypes.WriteCompressedNullableByte" },
            { "sbyte?", $"CompressedIntegralTypes.WriteCompressedNullableSByte" },
            { "char?", $"EncodeCharAndString.WriteNullableChar" },
            { "decimal?", $"CompressedDecimal.WriteCompressedNullableDecimal" },
            { "float?", $"WriteUncompressedPrimitives.WriteNullableSingle" },
            { "double?", $"WriteUncompressedPrimitives.WriteNullableDouble" },
            { "short?", $"CompressedIntegralTypes.WriteCompressedNullableShort" },
            { "ushort?", $"CompressedIntegralTypes.WriteCompressedNullableUshort" },
            { "int?", $"CompressedIntegralTypes.WriteCompressedNullableInt" },
            { "uint?", $"CompressedIntegralTypes.WriteCompressedNullableUint" },
            { "long?", $"CompressedIntegralTypes.WriteCompressedNullableLong" },
            { "ulong?", $"CompressedIntegralTypes.WriteCompressedNullableUlong" },
            { "TimeSpan?", $"CompressedIntegralTypes.WriteCompressedNullableTimeSpan" },
            { "DateTime?", $"CompressedIntegralTypes.WriteCompressedNullableDateTime" },
            { "Guid?", $"WriteUncompressedPrimitives.WriteNullableGuid" },
        };
    }
}
