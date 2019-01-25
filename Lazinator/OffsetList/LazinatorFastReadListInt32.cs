using System;

namespace Lazinator.OffsetList
{
    public partial class LazinatorFastReadListInt32 : LazinatorFastReadList<Int32>, ILazinatorFastReadListInt32
    {
        public override ReadOnlySpan<Int32> ReadOnly
        {
            get => Lazinator.Buffers.Spans.CastSpanToInt32(ReadOnlyBytes);
            set => ReadOnlyBytes = Lazinator.Buffers.Spans.CastSpanFromInt32(value);
        }
    }
}