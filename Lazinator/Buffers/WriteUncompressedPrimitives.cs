﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Lazinator.Exceptions;

namespace Lazinator.Buffers
{
    public static class WriteUncompressedPrimitives
    {
        public static byte WriteBool(BinaryBufferWriter writer, bool value)
        {
            writer.Write(value);
            return (byte) sizeof(byte);
        }

        public static byte WriteByte(BinaryBufferWriter writer, byte value)
        {
            writer.Write(value);
            return (byte)sizeof(byte);
        }

        public static byte WriteSByte(BinaryBufferWriter writer, sbyte value)
        {
            writer.Write(value);
            return (byte)sizeof(sbyte);
        }

        public static byte WriteShort(BinaryBufferWriter writer, short value)
        {
            writer.Write(value);
            return (byte)sizeof(short);
        }
        public static byte WriteUshort(BinaryBufferWriter writer, ushort value)
        {
            writer.Write(value);
            return (byte)sizeof(ushort);
        }
        public static byte WriteInt(BinaryBufferWriter writer, int value)
        {
            writer.Write(value);
            return (byte)sizeof(int);
        }
        public static byte WriteUint(BinaryBufferWriter writer, uint value)
        {
            writer.Write(value);
            return (byte)sizeof(uint);
        }
        public static byte WriteLong(BinaryBufferWriter writer, long value)
        {
            writer.Write(value);
            return (byte)sizeof(long);
        }
        public static byte WriteUlong(BinaryBufferWriter writer, ulong value)
        {
            writer.Write(value);
            return (byte)sizeof(ulong);
        }
        public static byte WriteSingle(BinaryBufferWriter writer, float value)
        {
            writer.Write(value);
            return (byte)sizeof(float);
        }
        public static byte WriteDouble(BinaryBufferWriter writer, double value)
        {
            writer.Write(value);
            return (byte)sizeof(double);
        }
        public static byte WriteGuid(BinaryBufferWriter writer, Guid value)
        {
            var bytes = value.ToByteArray();
            writer.Write(bytes); // TODO: when not using BinaryBufferWriter, we should be able to write directly to a destination span, using available extension methods
            return (byte)bytes.Length;
        }
        public static byte WriteNullableBool(BinaryBufferWriter writer, bool? value)
        {
            if (value == null)
            {
                writer.Write((byte) 1);
                return 1;
            }
            writer.Write((byte) 0);
            writer.Write(value.Value);
            return (byte)(sizeof(byte) + 1);
        }

        public static byte WriteNullableByte(BinaryBufferWriter writer, byte? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
                return 1;
            }
            writer.Write((byte)1);
            writer.Write(value.Value);
            return (byte)(sizeof(byte) + 1);
        }

        public static byte WriteNullableSByte(BinaryBufferWriter writer, byte? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
                return 1;
            }
            writer.Write((byte)1);
            writer.Write(value.Value);
            return (byte)sizeof(sbyte) + 1;
        }

        public static byte WriteNullableShort(BinaryBufferWriter writer, short? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
                return 1;
            }
            writer.Write((byte)1);
            writer.Write(value.Value);
            return (byte)sizeof(short) + 1;
        }
        public static byte WriteNullableUshort(BinaryBufferWriter writer, ushort? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
                return 1;
            }
            writer.Write((byte)1);
            writer.Write(value.Value);
            return (byte)sizeof(ushort) + 1;
        }
        public static byte WriteNullableInt(BinaryBufferWriter writer, int? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
                return 1;
            }
            writer.Write((byte)1);
            writer.Write(value.Value);
            return (byte)sizeof(int) + 1;
        }
        public static byte WriteNullableUint(BinaryBufferWriter writer, uint? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
                return 1;
            }
            writer.Write((byte)1);
            writer.Write(value.Value);
            return (byte)sizeof(uint) + 1;
        }
        public static byte WriteNullableLong(BinaryBufferWriter writer, long? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
                return 1;
            }
            writer.Write((byte)1);
            writer.Write(value.Value);
            return (byte)sizeof(long) + 1;
        }
        public static byte WriteNullableUlong(BinaryBufferWriter writer, ulong? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
                return 1;
            }
            writer.Write((byte)1);
            writer.Write(value.Value);
            return (byte)sizeof(ulong) + 1;
        }
        public static byte WriteNullableSingle(BinaryBufferWriter writer, float? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
                return 1;
            }
            writer.Write((byte)1);
            writer.Write(value.Value);
            return (byte)sizeof(float) + 1;
        }
        public static byte WriteNullableDouble(BinaryBufferWriter writer, double? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
                return 1;
            }
            writer.Write((byte)1);
            writer.Write(value.Value);
            return (byte)sizeof(double) + 1;
        }

        public static byte WriteNullableGuid(BinaryBufferWriter writer, Guid? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
                return 1;
            }
            writer.Write((byte)1);
            var bytes = value.Value.ToByteArray();
            writer.Write(bytes); // TODO: when not using BinaryBufferWriter, we should be able to write directly to a destination span, using available extension methods
            return (byte)(bytes.Length + 1);
        }
    }
}
