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

        private Dictionary<ILazinator, byte> Parents;

        public ILazinator LastAdded { get; private set; }

        public bool Any() => LastAdded != null;

        public int Count => LastAdded == null ? 0 : Parents.Sum(x => x.Value);

        public LazinatorParentsReference(ILazinator onlyParent)
        {
            LastAdded = onlyParent;
            Parents = null;
        }

        public void Add(ILazinator parent)
        {
            if (LastAdded == null)
            {
                LastAdded = parent;
                return;
            }
            if (Parents == null)
                Parents = new Dictionary<ILazinator, byte>();
            if (Parents.ContainsKey(parent))
                Parents[parent]++;
            else
                Parents[parent] = 1;
        }

        public void Remove(ILazinator parent)
        {
            if (LastAdded == parent)
            {
                if (Count == 1)
                {
                    LastAdded = null;
                    Parents = null;
                }
                else
                    LastAdded = null;
                return;
            }
            if (Parents[parent] == 1)
                Parents.Remove(parent);
            else
                Parents[parent]--;
        }

        public void InformParentsOfDirtiness()
        {
            if (LastAdded == null)
                LastAdded.DescendantIsDirty = true;
            if (Parents != null)
                foreach (var parent in Parents)
                {
                    if (parent.Key != LastAdded)
                        parent.Key.DescendantIsDirty = true;
                }
        }
    }
}
