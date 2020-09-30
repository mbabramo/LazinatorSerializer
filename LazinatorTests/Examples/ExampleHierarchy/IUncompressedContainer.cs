using Lazinator.Attributes;
using System;

namespace LazinatorTests.Examples.ExampleHierarchy
{
    [Lazinator((int) ExampleUniqueIDs.UncompressedContainer)]
    public interface IUncompressedContainer
    {
        decimal MyUncompressedDecimal { get; set; }
        short MyUncompressedShort { get; set; }
        ushort MyUncompressedUShort { get; set; }
        int MyUncompressedInt { get; set; }
        uint MyUncompressedUInt { get; set; }
        long MyUncompressedLong { get; set; }
        ulong MyUncompressedULong { get; set; }
        TimeSpan MyUncompressedTimeSpan { get; set; }
        DateTime MyUncompressedDateTime { get; set; }
        string MyUncompressed { get; set; }
        decimal? MyUncompressedNullableDecimal { get; set; }
        short? MyUncompressedNullableShort { get; set; }
        ushort? MyUncompressedNullableUShort { get; set; }
        int? MyUncompressedNullableInt { get; set; }
        uint? MyUncompressedNullableUInt { get; set; }
        long? MyUncompressedNullableLong { get; set; }
        ulong? MyUncompressedNullableULong { get; set; }
        TimeSpan? MyUncompressedNullableTimeSpan { get; set; }
        DateTime? MyUncompressedNullableDateTime { get; set; }
    }
}