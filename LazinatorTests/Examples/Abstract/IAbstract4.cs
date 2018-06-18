using Lazinator.Attributes;
using System.Collections.Generic;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)ExampleUniqueIDs.Abstract4)]
    interface IAbstract4 : IConcrete3
    {
        string String4 { get; set; }
        List<int> IntList4 { get; set; }
    }
}
