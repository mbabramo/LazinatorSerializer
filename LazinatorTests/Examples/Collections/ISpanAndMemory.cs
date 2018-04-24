﻿using System;
using System.Collections.Generic;
using Lazinator.Attributes;

namespace LazinatorTests.Examples.Collections
{
    [Lazinator((int)ExampleUniqueIDs.SpanAndMemory)]
    public interface ISpanAndMemory
    {
        ReadOnlySpan<long> MyReadOnlySpanLong { get; set; }
        ReadOnlySpan<byte> MyReadOnlySpanByte { get; set; }
        ReadOnlySpan<DateTime> MyReadOnlySpanDateTime { get; set; }
        ReadOnlySpan<char> MyReadOnlySpanChar { get; set; }
        Memory<int> MyMemoryInt { get; set; }
        Memory<int>? MyNullableMemoryInt { get; set; }
    }
}