﻿using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Wrappers;

namespace LazinatorTests.Examples.Generics
{
    [Lazinator((int) ExampleUniqueIDs.OpenGenericStayingOpenContainer)]
    interface IOpenGenericStayingOpenContainer
    {
        IOpenGenericStayingOpen<LazinatorWrapperFloat> ClosedGeneric { get; set; }
    }
}
