using Lazinator.Attributes;
using System;

namespace LazinatorTests.Examples.ExampleHierarchy
{
    [Lazinator((int) ExampleUniqueIDs.UncompressedContainer)]
    public interface IUncompressedContainer
    {
        [Uncompressed]
        decimal MyUncompressedDecimal { get; set; }
        [Uncompressed]
        short MyUncompressedShort { get; set; }
        [Uncompressed]
        ushort MyUncompressedUShort { get; set; }
        [Uncompressed]
        int MyUncompressedInt { get; set; }
        [Uncompressed]
        uint MyUncompressedUInt { get; set; }
        [Uncompressed]
        long MyUncompressedLong { get; set; }
        [Uncompressed]
        ulong MyUncompressedULong { get; set; }
        [Uncompressed]
        TimeSpan MyUncompressedTimeSpan { get; set; }
        [Uncompressed]
        DateTime MyUncompressedDateTime { get; set; }
        [Uncompressed]
        string MyUncompressed { get; set; }
        [Uncompressed]
        decimal? MyUncompressedNullableDecimal { get; set; }
        [Uncompressed]
        short? MyUncompressedNullableShort { get; set; }
        [Uncompressed]
        ushort? MyUncompressedNullableUShort { get; set; }
        [Uncompressed]
        int? MyUncompressedNullableInt { get; set; }
        [Uncompressed]
        uint? MyUncompressedNullableUInt { get; set; }
        [Uncompressed]
        long? MyUncompressedNullableLong { get; set; }
        [Uncompressed]
        ulong? MyUncompressedNullableULong { get; set; }
        [Uncompressed]
        TimeSpan? MyUncompressedNullableTimeSpan { get; set; }
        [Uncompressed]
        DateTime? MyUncompressedNullableDateTime { get; set; }
    }
}