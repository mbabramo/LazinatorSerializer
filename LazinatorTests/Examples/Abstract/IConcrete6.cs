using Lazinator.Attributes;
using System.Collections.Generic;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)ExampleUniqueIDs.Concrete6)]
    interface IConcrete6 : IConcrete5
    {
        List<int> IntList6 { get; set; }
    }
}
