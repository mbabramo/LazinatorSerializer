using Lazinator.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)ExampleUniqueIDs.AbstractGeneric1Unofficial)]
    interface IAbstractGeneric1Unofficial
    {
        int MyUnofficialInt { get; set; }
    }
}
