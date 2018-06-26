using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Lazinator.Core.LazinatorUtilities;

namespace Lazinator.Core
{
    public struct LazinatorParentsReference
    {
        // This class allows a single Lazinator object to be present in multiple hierarchies or in multiple parts of a single hierarchy.
        // It does this by keeping track of the object's parents (assuming they are classes).
        // A complication is that an object can have the same parent more than once (that is, two childs of an
        // object can be the same), so we have to keep track of the number of times a parent is a parent.

        private Dictionary<ILazinator, byte> OtherParents;

        public ILazinator LastAdded { get; private set; }

        public bool Any() => LastAdded != null;

        public int Count => LastAdded == null ? 0 : OtherParents.Sum(x => x.Value);

        public LazinatorParentsReference(ILazinator lastAdded, Dictionary<ILazinator, byte> otherParents = null)
        {
            LastAdded = lastAdded;
            OtherParents = otherParents;
        }

        public LazinatorParentsReference WithAdded(ILazinator parent)
        {
            if (LastAdded == null)
            {
                return new LazinatorParentsReference(parent, OtherParents);
            }
            var otherParents = OtherParents;
            if (otherParents == null)
                otherParents = new Dictionary<ILazinator, byte>();
            if (otherParents.ContainsKey(parent))
                otherParents[parent]++;
            else
                otherParents[parent] = 1;
            return new LazinatorParentsReference(LastAdded, otherParents);
        }

        public LazinatorParentsReference WithRemoved(ILazinator parent)
        {
            if (LastAdded == parent)
            {
                if (Count == 1)
                {
                    return new LazinatorParentsReference();
                }
                else
                    return new LazinatorParentsReference(null, OtherParents);
            }
            var otherParents = OtherParents;
            if (otherParents[parent] == 1)
                otherParents.Remove(parent);
            else
                otherParents[parent]--;
            return new LazinatorParentsReference(LastAdded, otherParents);
        }


        public void Add(ILazinator parent)
        {
            if (LastAdded == null)
            {
                LastAdded = parent;
                return;
            }
            if (OtherParents == null)
                OtherParents = new Dictionary<ILazinator, byte>();
            if (OtherParents.ContainsKey(parent))
                OtherParents[parent]++;
            else
                OtherParents[parent] = 1;
        }

        public void Remove(ILazinator parent)
        {
            if (LastAdded == parent)
            {
                if (Count == 1)
                {
                    LastAdded = null;
                    OtherParents = null;
                }
                else
                    LastAdded = null;
                return;
            }
            if (OtherParents[parent] == 1)
                OtherParents.Remove(parent);
            else
                OtherParents[parent]--;
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
