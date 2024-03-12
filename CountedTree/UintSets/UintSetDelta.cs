namespace CountedTree.UintSets
{
    public partial class UintSetDelta : IUintSetDelta
    { 
        public UintSetDelta(uint index, bool delete)
        {
            Index = index;
            Delete = delete;
        }

        public UintSetDelta Clone()
        {
            return new UintSetDelta(Index, Delete);
        }
    }
}
