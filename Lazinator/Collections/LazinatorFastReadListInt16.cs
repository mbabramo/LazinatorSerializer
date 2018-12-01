using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
    public partial class LazinatorFastReadListInt16 : LazinatorFastReadList<Int16>, ILazinatorFastReadListInt16
    {
        public override ReadOnlySpan<Int16> ReadOnly
        {
            get => Lazinator.Buffers.Spans.CastSpanToInt16(ReadOnlyBytes);
            set => ReadOnlyBytes = Buffers.Spans.CastSpanFromInt16(value);
        }
    }
}
