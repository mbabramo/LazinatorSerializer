using Lazinator.Attributes;
using Lazinator.Core;
using LazinatorTests.Examples.Structs;
using System;

namespace LazinatorTests.Examples
{
    [Lazinator((int)ExampleUniqueIDs.ExampleChild)]
    [AsyncLazinatorMemory]
    public interface IExampleChild : ILazinator
    {
        long MyLong { get; set; }
        short MyShort { get; set; }
        ExampleGrandchild MyExampleGrandchild { get; set; }
        WrapperContainer MyWrapperContainer { get; set; }
        ReadOnlySpan<byte> ByteSpan { get; set; }
    }
}