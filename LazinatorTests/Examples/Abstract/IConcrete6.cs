using Lazinator.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)ExampleUniqueIDs.Concrete6)]
    interface IConcrete6
    {
        List<int> IntList6 { get; set; }
    }
}
