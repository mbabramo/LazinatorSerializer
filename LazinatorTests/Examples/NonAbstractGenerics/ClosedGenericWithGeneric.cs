using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.NonAbstractGenerics
{
    public partial class ClosedGenericWithGeneric : OpenGeneric<OpenGeneric<ExampleChild>>, IClosedGenericWithGeneric
    {
        // we are closing the generic, but we are closing it with something generic (which in this case happens to be the same type as the open generic)
    }
}
