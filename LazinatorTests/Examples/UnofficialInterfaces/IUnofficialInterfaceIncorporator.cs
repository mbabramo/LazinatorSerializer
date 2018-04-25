﻿using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;

namespace LazinatorTests.Examples
{
    [UnofficiallyIncorporateInterface("LazinatorTests.Examples.IUnofficialInterface", "private")]
    [Lazinator((int) ExampleUniqueIDs.UnofficialInterfaceIncorporator)]
    public interface IUnofficialInterfaceIncorporator
    {
        long MyLong { get; set; }
    }
}
