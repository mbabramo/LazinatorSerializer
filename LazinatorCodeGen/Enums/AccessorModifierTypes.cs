using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.CodeGeneration
{
    public enum AccessorModifierTypes : byte
    {
        PublicModifier,
        PrivateModifier,
        ProtectedModifier,
        InternalModifier,
        ProtectedInternalModifier, // protected or internal
        PrivateProtectedModifier // protected AND internal
    }
}
