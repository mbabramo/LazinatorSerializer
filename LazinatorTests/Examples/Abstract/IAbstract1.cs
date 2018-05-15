using Lazinator.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Collections;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)ExampleUniqueIDs.Abstract1)]
    interface IAbstract1
    {
        string String1 { get; set; }
        List<int> IntList1 { get; set; }
    }
}
