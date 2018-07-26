using System;

namespace LazinatorTests.Examples.Collections
{
    public partial class SpanAndMemory : ISpanAndMemory
    {
        public Memory<char> MyReadOnlyMemoryChar { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
