using System;

namespace LazinatorCollections.OffsetList
{
    /// <summary>
    /// A Lazinator read-only list of integers
    /// </summary>
    public partial class LazinatorFastReadListInt32 : LazinatorFastReadList<Int32>, ILazinatorFastReadListInt32
    {
        public LazinatorFastReadListInt32()
        {
        }

        public override ReadOnlySpan<Int32> ReadOnly
        {
            get => Lazinator.Buffers.Spans.CastSpanToInt32(ReadOnlyBytes);
            set => ReadOnlyBytes = Lazinator.Buffers.Spans.CastSpanFromInt32(value);
        }
    }
}