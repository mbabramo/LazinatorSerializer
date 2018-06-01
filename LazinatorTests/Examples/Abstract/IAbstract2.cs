using Lazinator.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Collections;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)ExampleUniqueIDs.Abstract2)]
    interface IAbstract2 : IAbstract1
    {
        string String2 { get; set; }
        List<int> IntList2 { get; set; }
        Example Example2 { get; set; }
    }
}
