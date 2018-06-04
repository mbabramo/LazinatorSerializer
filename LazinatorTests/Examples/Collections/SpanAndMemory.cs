using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.Collections
{
    public partial class SpanAndMemory : ISpanAndMemory
    {
        public ReadOnlyMemory<byte> MyReadOnlyMemoryByte { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
