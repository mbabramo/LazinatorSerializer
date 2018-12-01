using System.Collections.Generic;
using System.Linq;
using Lazinator.Core;

namespace Lazinator.Collections
{
    /// <summary>
    /// A list of progressively increasing integral values, used to store offsets into a stream. 
    /// </summary>
    public sealed partial class LazinatorOffsetList : ILazinator, ILazinatorOffsetList
    {
        //For space efficiency, we store these in two lists, one containing two-byte values and one containing four-byte values. (Adding one- and three-byte values would contain extra overhead.) More importantly, we don't want to deserialize these lists unnecessarily (that is, if we're not changing them). Thus, we deserialize the two lists into ReadOnlyMemory where we can access the offsets directly. We then create in-memory lists only if necessary.

        private int NumTwoByteItems => TwoByteItems?.Length ?? 0;
        private int NumFourByteItems => FourByteItems?.Length ?? 0;
        public int Count => NumTwoByteItems + NumFourByteItems;

        public void AddOffset(int offset)
        {
            if (offset <= short.MaxValue)
            {
                if (TwoByteItems == null)
                    TwoByteItems = new LazinatorFastReadListInt16();
                TwoByteItems.AsList.Add((short) offset);
            }
            else
            {
                if (FourByteItems == null)
                    FourByteItems = new LazinatorFastReadList<int>();
                FourByteItems.AsList.Add(offset);
            }
        }

        public void SetOffsets(List<int> offsets)
        {
            TwoByteItems.AsList = offsets.Where(x => x <= short.MaxValue).Select(x => (short) x).ToList();
            FourByteItems.AsList = offsets.Where(x => x > short.MaxValue).ToList();
        }

        public int this[int index]
        {
            get
            {
                if (index < NumTwoByteItems)
                    return TwoByteItems[index];
                return FourByteItems[index - NumTwoByteItems];
            }
        }
        
    }
}
