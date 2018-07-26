using System;

namespace LazinatorTests.Examples.Collections
{
    public partial class SpanAndMemory : ISpanAndMemory
    {
        public ReadOnlyMemory<int>? MyNullableReadOnlyMemoryInt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
