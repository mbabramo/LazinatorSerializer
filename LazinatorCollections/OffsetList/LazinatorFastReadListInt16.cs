using System;

namespace LazinatorCollections.OffsetList
{
    /// <summary>
    /// A Lazinator read-only list of shorts
    /// </summary>
    public partial class LazinatorFastReadListInt16 : LazinatorFastReadList<Int16>, ILazinatorFastReadListInt16
    {
        public LazinatorFastReadListInt16()
        {
        }

        public override ReadOnlySpan<Int16> ReadOnly
        {
            get => Lazinator.Buffers.Spans.CastSpanToInt16(ReadOnlyBytes);
            set => ReadOnlyBytes = Lazinator.Buffers.Spans.CastSpanFromInt16(value);
        }
    }
}
