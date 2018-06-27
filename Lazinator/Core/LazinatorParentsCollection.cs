using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Lazinator.Core.LazinatorUtilities;

namespace Lazinator.Core
{
    /// <summary>
    /// Tracks the classes that are parents of a Lazinator class or struct.
    /// It can be used to add or remove a parent, or to notify all parents when the child becomes dirty.
    /// When the parent is a struct, the parent is not tracked. In that case, the parent struct's value can be changed only 
    /// when setting the value of the struct, so that will generate the notification of dirtiness.
    /// </summary>
    public readonly struct LazinatorParentsCollection
    {
        // A complication is that an object can have the same parent more than once (that is, two childs of an
        // object can be the same), so we have to keep track of the number of times a parent is a parent.
        // Since usually this will only have one or two items at most, we use a list rather than a dictionary.
        // TODO: LinkedList?
        private readonly List<(ILazinator parent, int count)> OtherParents;

        /// <summary>
        /// The last Lazinator parent added. If the last added is subsequently removed, this will 
        /// be null, even if some other parents were added earlier.
        /// </summary>
        public readonly ILazinator LastAdded;

        private readonly int LastAddedCount;

        public bool Any() => LastAdded != null || (OtherParents != null && OtherParents.Any());

        /// <summary>
        /// The number of parents stored. A parent is counted only once even if stored more often.
        /// </summary>
        public int Count => (LastAdded == null ? 0 : 1) + (OtherParents?.Count() ?? 0);

        public LazinatorParentsCollection(ILazinator lastAdded, List<(ILazinator parent, int count)> otherParents = null)
        {
            LastAdded = lastAdded;
            LastAddedCount = (lastAdded == null) ? 0 : 1;
            OtherParents = otherParents;
        }

        public LazinatorParentsCollection(ILazinator lastAdded, int lastAddedCount, List<(ILazinator parent, int count)> otherParents = null)
        {
            LastAdded = lastAdded;
            LastAddedCount = lastAddedCount;
            OtherParents = otherParents;
        }



        private int? GetIndexInOtherParents(ILazinator parent)
        {
            if (OtherParents == null)
                return null;
            for (int i = 0; i < OtherParents.Count; i++)
                if (OtherParents[i].parent == parent)
                    return i;
            return null;
        }

        public LazinatorParentsCollection WithAdded(ILazinator parent, Action<LazinatorParentsCollection, ILazinator> multipleParentsAction = null)
        {
            if (multipleParentsAction != null && Count > 0)
                multipleParentsAction(this, parent);

            if (parent == null)
                return this;

            if (LastAdded == null)
            {
                if (OtherParents == null)
                    return new LazinatorParentsCollection(parent);
                int? indexInOtherParents = GetIndexInOtherParents(parent);
                if (indexInOtherParents != null)
                {   
                    var otherParentsWithRemoval = OtherParents;
                    int revisedCount = OtherParents[(int) indexInOtherParents].count + 1;
                    otherParentsWithRemoval.RemoveAt((int) indexInOtherParents);
                    return new LazinatorParentsCollection(parent, revisedCount, otherParentsWithRemoval);
                }
                return new LazinatorParentsCollection(parent, OtherParents);
            }

            if (parent == LastAdded)
                return new LazinatorParentsCollection(parent, LastAddedCount + 1, OtherParents);

            // move LastAdded to OtherParents
            var otherParentsWithAddition = OtherParents;
            if (otherParentsWithAddition == null)
                otherParentsWithAddition = new List<(ILazinator parent, int count)>();
            otherParentsWithAddition.Add((LastAdded, LastAddedCount));
            return new LazinatorParentsCollection(parent, otherParentsWithAddition);
        }

        public LazinatorParentsCollection WithRemoved(ILazinator parent)
        {
            if (LastAdded == parent)
            { 
                if (LastAddedCount > 1)
                {
                    return new LazinatorParentsCollection(parent, LastAddedCount - 1, OtherParents);
                }
                else
                    return new LazinatorParentsCollection(null, OtherParents);
            }
            int? indexInOtherParents = GetIndexInOtherParents(parent);
            if (indexInOtherParents == null)
                return this; // nothing to remove
            var otherParents = OtherParents;
            if (otherParents[(int) indexInOtherParents].count == 1)
                otherParents.RemoveAt((int) indexInOtherParents);
            else
                otherParents[(int)indexInOtherParents] = (otherParents[(int)indexInOtherParents].parent, otherParents[(int)indexInOtherParents].count - 1);
            return new LazinatorParentsCollection(LastAdded, otherParents);
        }

        public void InformParentsOfDirtiness()
        {
            if (LastAdded != null)
                LastAdded.DescendantIsDirty = true;
            if (OtherParents != null)
                foreach (var parent in OtherParents)
                {
                    if (parent.parent != LastAdded)
                        parent.parent.DescendantIsDirty = true;
                }
        }

        public IEnumerable<ILazinator> EnumerateParents()
        {
            if (LastAdded != null)
                yield return LastAdded;
            if (OtherParents != null)
                foreach (var parent in OtherParents)
                {
                    if (parent.parent != LastAdded)
                        yield return parent.parent;
                }
        }
    }
}
