using Lazinator.Attributes;
using System;

namespace LazinatorTests.Examples
{
    [Lazinator((int)ExampleUniqueIDs.ExampleChild)]
    public interface IExampleChild
    {
        long MyLong { get; set; }
        short MyShort { get; set; }
        ReadOnlySpan<byte> ByteSpan { get; set; }
    }
}