using Lazinator.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.NonAbstractGenerics
{
    [Lazinator((int)ExampleUniqueIDs.InheritingClosedGeneric)]
    interface IInheritingClosedGeneric : IClosedGeneric
    {
        int YetAnotherInt { get; set; }
    }
}
