using System;
using System.Collections.Generic;
using System.Text;
using static Lazinator.Core.LazinatorUtilities;

namespace Lazinator.Core
{
    public struct LazinatorParentsReference
    {
        // This class allows a single Lazinator object to be present in multiple hierarchies or in multiple parts of a single hierarchy.
        // It does this by keeping track of the object's parents (assuming they are classes).
        // Thus, on lazinator class X, when we set property A from value A1 to value A2, we would call A1.Remove(this), and A2.Add(this).

        // Further complication: 

        private List<ILazinator> Parents;

        public void Add(ILazinator parent)
        {
            if (Parents == null)
                Parents = new List<ILazinator>();
            Parents.Add(parent);
        }

        public void Remove(ILazinator parent)
        {
            Parents.Remove(parent);
        }
    }
}
