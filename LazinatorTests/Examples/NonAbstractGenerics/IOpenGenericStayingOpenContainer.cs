using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Wrappers;

namespace LazinatorTests.Examples.NonAbstractGenerics
{
    [Lazinator((int) ExampleUniqueIDs.OpenGenericStayingOpenContainer)]
    interface IOpenGenericStayingOpenContainer
    {
        OpenGeneric<LazinatorWrapperFloat> ClosedGeneric { get; set; }
    }
}
