using Lazinator.Attributes;
using System.Collections.Generic;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)ExampleUniqueIDs.Abstract1)]
    interface IAbstract1
    {
        string String1 { get; set; }
        List<int> IntList1 { get; set; }
        Example Example3 { get; set; }
    }
}
