using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.CodeDescription
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
