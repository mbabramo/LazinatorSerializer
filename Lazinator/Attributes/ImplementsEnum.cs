using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Attributes
{
    public enum ImplementsEnum
    {
        LazinatorObjectVersionUpgrade,
        PreSerialization,
        PostDeserialization,
        OnDirty,
        ConvertFromBytesAfterHeader,
        WritePropertiesIntoBuffer,
        EnumerateLazinatorNodes_Helper
    }
}
