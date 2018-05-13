using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.NonAbstractGenerics
{
    public partial class ClosedGeneric : OpenGeneric<ExampleChild>, IClosedGeneric
    {
    }
}
