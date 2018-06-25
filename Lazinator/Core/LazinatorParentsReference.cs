using System;
using System.Collections.Generic;
using System.Text;
using static Lazinator.Core.LazinatorUtilities;

namespace Lazinator.Core
{
    public struct LazinatorParentsReference
    {
        // DEBUG
        // Alternative: Set property A from value A1 to value A2. 
        // Assume this is a class.
        // Then, if A1.LazinatorParentClass is not null, we call A1.LazinatorParentClass.ChildHasMoved = true; which also sets IsDirty = true;
        // When serializing, if ChildHasMoved, we throw an exception.
        // Then, A2.LazinatorParentClass can be set to the correct value.
        // Assume this is a struct.
        // Then, A1 would need to call another delegate to inform its parent that the child has moved. And then we would set the delegate on A2.

        // This class allows a single Lazinator object to be present in multiple hierarchies or in multiple parts of a single hierarchy.
        // It does this by keeping track of the object's parents (in the case of classes) or delegates that can inform parents (in the case of structs).
        // Thus, on lazinator class X, when we set property A from value A1 to value A2, we would call A1.Remove(this), and A2.Add(this).
        // 



        private List<ILazinator> ClassParents;
        private List<InformParentOfDirtinessDelegate> StructParents;

        public void Add(ILazinator parent)
        {
            if (ClassParents == null)
                ClassParents = new List<ILazinator>();
            ClassParents.Add(parent);
        }

        public void Remove(ILazinator parent)
        {
            ClassParents.Remove(parent);
        }

        public void Add(InformParentOfDirtinessDelegate informParent)
        {
            if (StructParents == null)
                StructParents = new List<InformParentOfDirtinessDelegate>();
            StructParents.Add(informParent);
        }

        public void Remove(InformParentOfDirtinessDelegate informParent)
        {
            StructParents.Remove(informParent);
        }
    }
}
