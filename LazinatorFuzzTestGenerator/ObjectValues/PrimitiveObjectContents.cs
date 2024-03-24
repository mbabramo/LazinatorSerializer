using LazinatorFuzzTestGenerator.Interfaces;
using LazinatorFuzzTestGenerator.ObjectTypes;
using System;
using System.Diagnostics;

namespace LazinatorFuzzTestGenerator.ObjectValues
{

    public class PrimitiveObjectContents : IObjectContents
    {
        public ISupportedType TheType => ThePrimitiveType;
        public PrimitiveType ThePrimitiveType { get; set; }
        private object? Value { get; set; }
        public bool IsNull => Value == null;

        public PrimitiveObjectContents(PrimitiveEnum primitiveEnum, object? value)
        {
            ThePrimitiveType = new PrimitiveType(primitiveEnum);
            Value = value;
        }

        public PrimitiveObjectContents(PrimitiveEnum primitiveEnum, Random r, int? inverseProbabilityOfNull)
        {
            ThePrimitiveType = new PrimitiveType(primitiveEnum);
            SetToRandom(r, inverseProbabilityOfNull);
        }

        public string CodeToGetValue => GetValidCSharpRepresentationOfValue(Value);
        public string CodeToTestValue(string containerName) => $"{containerName} == {CodeToGetValue}";

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
                    return "'" + ((char)value).ToString() + "'";
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

        private void SetToRandom(Random r, int? inverseProbabilityOfNull)
        {
            if (inverseProbabilityOfNull == null || r.Next((int)inverseProbabilityOfNull) != 0)
                Initialize(r);
            else
                Value = null;
        }

        public void Initialize(Random r)
        {
            Value = GetRandomNonNullableValue(r);
        }

        public (string codeForMutation, (IObjectContents objectContents, string objectName)? additionalObject) MutateAndReturnCodeForMutation(Random r, string varName)
        {
            Initialize(r);
            return ($@"{varName} = {CodeToGetValue};", null);
        }

        private object GetRandomNonNullableValue(Random r)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*(),./;[]|é";
            switch (ThePrimitiveType.PrimitiveEnum)
            {
                case PrimitiveEnum.Bool:
                    return r.Next(2) == 0;
                case PrimitiveEnum.Byte:
                    return (byte)r.Next(256);
                case PrimitiveEnum.SByte:
                    return (sbyte)r.Next(256);
                case PrimitiveEnum.Char:
                    return (char)chars[r.Next(chars.Length)];
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
                        return r.Next(-300, 300);
                    else
                        return r.Next(int.MinValue, int.MaxValue);
                case PrimitiveEnum.UInt:
                    if (r.Next(2) == 0)
                        return (uint)r.Next(-0, 300);
                    else
                        return (uint)(r.Next(0, int.MaxValue) + (uint)int.MaxValue);
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
                    return new string(Enumerable.Repeat(chars, stringLength)
                      .Select(s => s[r.Next(s.Length)]).ToArray());
                case PrimitiveEnum.DateTime:
                    return new DateTime(r.NextInt64((DateTime.MinValue).Ticks, (DateTime.MaxValue).Ticks));
                case PrimitiveEnum.TimeSpan:
                    return new TimeSpan(r.NextInt64((TimeSpan.MinValue).Ticks, (TimeSpan.MaxValue).Ticks));
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
