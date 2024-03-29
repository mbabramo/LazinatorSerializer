﻿using LazinatorFuzzTestWriter;

namespace LazinatorFuzzTestWriter
{ 

    public class PrimitiveObject : IObjectContents
    {
        public ISupportedType TheType => ThePrimitiveType;
        public PrimitiveType ThePrimitiveType { get; set; }
        public object? Value { get; set; }

        public PrimitiveObject(PrimitiveEnum primitiveType, object? value)
        {
            ThePrimitiveType = new PrimitiveType() { PrimitiveEnum = primitiveType };
            Value = value;
        }

        public PrimitiveObject(PrimitiveEnum primitiveEnum, bool nullable, Random r)
        {
            ThePrimitiveType = new PrimitiveType() { PrimitiveEnum = primitiveEnum };
            Value = GetRandomNonNullableValue(r);
        }

        public PrimitiveObject(Random r)
        {
            PrimitiveEnum primitiveEnum = (PrimitiveEnum)r.Next(0, (int) PrimitiveEnum.Decimal);
            bool nullable = r.Next(2) == 0;
            ThePrimitiveType = new PrimitiveType() { PrimitiveEnum = primitiveEnum };
            Value = GetRandomNonNullableValue(r);
        }

        public string CodeToGetValue => GetValidCSharpRepresentationOfValue(Value);
        public string CodeToTestValue => $"Value == {CodeToGetValue}";

        private string GetValidCSharpRepresentationOfValue(object? value)
        {
            if (value == null)
                return "null";
            switch (ThePrimitiveType.PrimitiveEnum)
            {
                case PrimitiveEnum.Bool:
                    return (bool)value == true ? "true" : "false";
                case PrimitiveEnum.Byte:
                    return ((byte)value).ToString();
                case PrimitiveEnum.SByte:
                    return ((sbyte)value).ToString();
                case PrimitiveEnum.Char:
                    return ((char)value).ToString();
                case PrimitiveEnum.Short:
                    return ((short)value).ToString();
                case PrimitiveEnum.UShort:
                    return ((ushort)value).ToString();
                case PrimitiveEnum.Int:
                    return ((int)value).ToString();
                case PrimitiveEnum.UInt:
                    return ((uint)value).ToString();
                case PrimitiveEnum.Long:
                    return ((long)value).ToString();
                case PrimitiveEnum.ULong:
                    return ((ulong)value).ToString();
                case PrimitiveEnum.String:
                    return "\"" + value.ToString() + "\"";
                case PrimitiveEnum.DateTime:
                    return "new DateTime(" + ((DateTime)value).Ticks + ")";
                case PrimitiveEnum.TimeSpan:
                    return "new TimeSpan(" + ((TimeSpan)value).Ticks + ")";
                case PrimitiveEnum.Guid:
                    return "new Guid(\"" + value.ToString() + "\")";
                case PrimitiveEnum.Float:
                    return ((float)value).ToString() + "f";
                case PrimitiveEnum.Double:
                    return ((double)value).ToString();
                case PrimitiveEnum.Decimal:
                    return ((decimal)value).ToString() + "m";
                default:
                    throw new NotImplementedException();
            }
        }

        public object GetRandomNonNullableValue(Random r)
        {
            switch (ThePrimitiveType.PrimitiveEnum)
            {
                case PrimitiveEnum.Bool:
                    return r.Next(2) == 0;
                case PrimitiveEnum.Byte:
                    return (byte)r.Next(256);
                case PrimitiveEnum.SByte:
                    return (sbyte)r.Next(256);
                case PrimitiveEnum.Char:
                    return (char)r.Next(256);
                case PrimitiveEnum.Short:
                    if (r.Next(2) == 0)
                        return (short)r.Next(-300, 300);
                    else
                        return (short)r.Next(short.MinValue, short.MaxValue);
                case PrimitiveEnum.UShort:
                    if (r.Next(2) == 0)
                        return (ushort)r.Next(0, 300);
                    else
                        return (ushort)r.Next(ushort.MinValue, ushort.MaxValue);
                case PrimitiveEnum.Int:
                    if (r.Next(2) == 0)
                        return (int)r.Next(-300, 300);
                    else
                        return r.Next(int.MinValue, int.MaxValue);
                case PrimitiveEnum.UInt:
                    if (r.Next(2) == 0)
                        return (uint)r.Next(-0, 300);
                    else
                        return (uint) (r.Next(0, int.MaxValue) + (uint)Int32.MaxValue);
                case PrimitiveEnum.Long:
                    if (r.Next(2) == 0)
                        return (long)r.Next(-300, 300);
                    else
                        return r.NextInt64(long.MinValue, long.MaxValue);
                case PrimitiveEnum.ULong:
                    if (r.Next(2) == 0)
                        return (ulong)r.Next(-300, 300);
                    else
                    {
                        byte[] buffer = new byte[8];
                        r.NextBytes(buffer); // Fill buffer with random bytes
                        return BitConverter.ToUInt64(buffer, 0);
                    }
                case PrimitiveEnum.String:
                    int stringLength = r.Next(10);
                    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*(),./;'[]\\|é";
                    return new string(Enumerable.Repeat(chars, stringLength)
                      .Select(s => s[r.Next(s.Length)]).ToArray());
                case PrimitiveEnum.DateTime:
                    return new DateTime(r.NextInt64(long.MinValue, long.MaxValue));
                case PrimitiveEnum.TimeSpan:
                    return new TimeSpan(r.NextInt64(long.MinValue, long.MaxValue));
                case PrimitiveEnum.Guid:
                    return Guid.NewGuid();
                case PrimitiveEnum.Float:
                    return (float)r.NextDouble();
                case PrimitiveEnum.Double:
                    return r.NextDouble();
                case PrimitiveEnum.Decimal:
                    return (decimal)r.NextDouble();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
