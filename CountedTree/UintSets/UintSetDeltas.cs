using Lazinator.Wrappers;
using System.Collections.Generic;
using System.Linq;
using Lazinator.Core;

namespace CountedTree.UintSets
{
    /// <summary>
    /// An immutable data structure representing changes to a UintSet.
    /// </summary>
    public partial class UintSetDeltas : IUintSetDeltas
    {

        public UintSetDeltas()
        {
            Changes = new List<UintSetDelta>();
        }

        public UintSetDeltas(UintSetDelta[] changes)
        {
            Changes = changes.Select(x => x.CloneLazinatorTyped()).ToList();
        }

        public UintSetDeltas(List<UintSetDelta> changes)
        {
            Changes = changes;
        }

        public UintSetDeltas Update(IEnumerable<UintSetDelta> changes)
        {
            var revisedChanges = Changes.ToList();
            foreach (var change in changes)
                revisedChanges.Add(change);
            return new UintSetDeltas(revisedChanges);
        }

        public UintSetDeltas Update(List<WUInt32> indicesToRemove, List<WUInt32> indicesToAdd)
        {
            List<UintSetDelta> changes = new List<UintSetDelta>();
            foreach (uint i in indicesToRemove)
                changes.Add(new UintSetDelta(i, true));
            foreach (uint i in indicesToAdd)
                changes.Add(new UintSetDelta(i, false));
            return Update(changes);
        }

        public void IntegrateIntoUintSet(UintSet u)
        {
            foreach (UintSetDelta d in Changes)
            {
                if (d.Delete)
                    u.RemoveUint(d.Index);
                else
                    u.AddUint(d.Index);
            }
        }

        public int Count()
        {
            return Changes.Count();
        }
    }
}
