using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Lazinator.Core.LazinatorUtilities;

namespace Lazinator.Core
{
    /// <summary>
    /// Tracks the classes that are parents of a Lazinator class.
    /// It can be used to add or remove a parent, or to notify all parents when the child becomes dirty.
    /// When the parent is a struct, the parent is not tracked. The parent struct's value can be changed only 
    /// when setting the value of the struct, so that will generate the notification of dirtiness.
    /// </summary>
    public readonly struct LazinatorParentsCollection
    {
        // A complication is that an object can have the same parent more than once (that is, two childs of an
        // object can be the same), so we have to keep track of the number of times a parent is a parent.

        private readonly Dictionary<ILazinator, byte> OtherParents;

        public readonly ILazinator LastAdded;

        public bool Any() => LastAdded != null;

        public int Count => LastAdded == null ? 0 : 1 + (OtherParents?.Sum(x => x.Value) ?? 0);

        public LazinatorParentsCollection(ILazinator lastAdded, Dictionary<ILazinator, byte> otherParents = null)
        {
            LastAdded = lastAdded;
            OtherParents = otherParents;
        }

        public LazinatorParentsCollection WithAdded(ILazinator parent)
        {
            if (LastAdded == null)
            {
                return new LazinatorParentsCollection(parent, OtherParents);
            }
            var otherParents = OtherParents;
            if (otherParents == null)
                otherParents = new Dictionary<ILazinator, byte>();
            if (otherParents.ContainsKey(parent))
                otherParents[parent]++;
            else
                otherParents[parent] = 1;
            return new LazinatorParentsCollection(LastAdded, otherParents);
        }

        public LazinatorParentsCollection WithRemoved(ILazinator parent)
        {
            if (LastAdded == parent)
            {
                if (Count == 1)
                {
                    return new LazinatorParentsCollection();
                }
                else
                    return new LazinatorParentsCollection(null, OtherParents);
            }
            var otherParents = OtherParents;
            if (otherParents[parent] == 1)
                otherParents.Remove(parent);
            else
                otherParents[parent]--;
            return new LazinatorParentsCollection(LastAdded, otherParents);
        }

        public void InformParentsOfDirtiness()
        {
            if (LastAdded != null)
                LastAdded.DescendantIsDirty = true;
            if (OtherParents != null)
                foreach (var parent in OtherParents)
                {
                    if (parent.Key != LastAdded)
                        parent.Key.DescendantIsDirty = true;
                }
        }
    }
}
