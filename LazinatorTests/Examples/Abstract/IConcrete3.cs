using Lazinator.Attributes;
using System.Collections.Generic;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)ExampleUniqueIDs.Concrete3)]
    interface IConcrete3 : IAbstract2
    {
        string String3 { get; set; }
        List<int> IntList3 { get; set; }
    }
}
