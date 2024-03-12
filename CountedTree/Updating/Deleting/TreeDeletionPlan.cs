using System;

namespace CountedTree.Updating
{
    public partial class TreeDeletionPlan : DeletionPlan, ITreeDeletionPlan
    {
        public TreeDeletionPlan(DateTime deletionTime, long rootID) : base(deletionTime)
        {
            RootID = rootID;
        }
    }
}
