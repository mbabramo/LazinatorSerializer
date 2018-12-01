using System;

namespace Lazinator.Collections
{
    public partial class LazinatorFastReadListInt32 : LazinatorFastReadList<Int32>
    {
        public override ReadOnlySpan<Int32> ReadOnly
        {
            get => Lazinator.Buffers.Spans.CastSpanToInt32(ReadOnlyBytes);
            set => ReadOnlyBytes = Buffers.Spans.CastSpanFromInt32(value);
        }
    }
}