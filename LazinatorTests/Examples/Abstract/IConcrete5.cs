using Lazinator.Attributes;
using System.Collections.Generic;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)ExampleUniqueIDs.Concrete5)]
    interface IConcrete5
    {
        string String5 { get; set; }
        List<int> IntList5 { get; set; }
    }
}
