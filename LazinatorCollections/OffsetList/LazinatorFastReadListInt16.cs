using System;
using System.Collections.Generic;
using System.Text;
using Lazinator;

namespace LazinatorCollections.OffsetList
{
    public partial class LazinatorFastReadListInt16 : LazinatorFastReadList<Int16>, ILazinatorFastReadListInt16
    {
        public override ReadOnlySpan<Int16> ReadOnly
        {
            get => Lazinator.Buffers.Spans.CastSpanToInt16(ReadOnlyBytes);
            set => ReadOnlyBytes = Lazinator.Buffers.Spans.CastSpanFromInt16(value);
        }
    }
}
