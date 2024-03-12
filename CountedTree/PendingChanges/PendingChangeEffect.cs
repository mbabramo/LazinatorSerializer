namespace CountedTree.PendingChanges
{
    public partial struct PendingChangeEffect : IPendingChangeEffect
    {

        public PendingChangeEffect(uint id, byte childIndex, bool delete) : this()
        {
            ID = id;
            ChildIndex = childIndex;
            Delete = delete;
        }
    }
}
