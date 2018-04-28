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
        OpenGenericStayingOpen<LazinatorWrapperFloat> ClosedGeneric { get; set; }
    }
}
