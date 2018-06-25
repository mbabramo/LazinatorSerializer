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

        public bool Any() => Parents != null && Parents.Any();

        public int Count => Parents.Sum(x => x.Value);

        public ILazinator First() => Parents.First().Key;

        public ILazinator FirstOrDefault() => Any() ? First() : null;

        public LazinatorParentsReference(ILazinator onlyParent)
        {
            Parents = new Dictionary<ILazinator, byte>();
            Parents[onlyParent] = 1;
        }

        public void Add(ILazinator parent)
        {
            if (Parents == null)
                Parents = new Dictionary<ILazinator, byte>();
            if (Parents.ContainsKey(parent))
                Parents[parent]++;
            else
                Parents[parent] = 1;

        }

        public void Remove(ILazinator parent)
        {
            if (Parents[parent] == 1)
                Parents.Remove(parent);
            else
                Parents[parent]--;
        }

        public void InformParentsOfDirtiness()
        {
            foreach (var parent in Parents)
            {
                parent.Key.DescendantIsDirty = true;
            }
        }
    }
}
