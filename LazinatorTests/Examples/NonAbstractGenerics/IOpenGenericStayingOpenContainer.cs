﻿using Lazinator.Attributes;
using Lazinator.Wrappers;
using LazinatorTests.Examples.Abstract;

namespace LazinatorTests.Examples.NonAbstractGenerics
{
    [Lazinator((int) ExampleUniqueIDs.OpenGenericStayingOpenContainer)]
    interface IOpenGenericStayingOpenContainer
    {
        // The reason this is more challenging than some other examples is that we haven't defined a new type, e.g. OpenGenericFloat, and then had ClosedGeneric be of that type. We are thus dependent on recording the type of the open generic.
        OpenGeneric<WFloat> ClosedGenericFloat { get; set; }
        OpenGeneric<IExampleChild> ClosedGenericInterface { get; set; }
        OpenGeneric<IExampleNonexclusiveInterface> ClosedGenericNonexclusiveInterface { get; set; }
        OpenGeneric<Base> ClosedGenericBase { get; set; } 
        GenericFromBase<Base> ClosedGenericFromBaseWithBase { get; set; }
    }
}
