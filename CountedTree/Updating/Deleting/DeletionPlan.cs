using System;

namespace CountedTree.Updating
{
    public partial class DeletionPlan : IDeletionPlan
    {

        public DeletionPlan(DateTime deletionTime)
        {
            DeletionTime = deletionTime;
        }
    }
}
