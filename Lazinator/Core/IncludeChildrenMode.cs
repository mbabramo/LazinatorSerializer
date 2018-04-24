using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Core
{
    public enum IncludeChildrenMode : byte
    {
        IncludeAllChildren,
        ExcludeAllChildren,
        ExcludeOnlyExcludableChildren
    }
}
