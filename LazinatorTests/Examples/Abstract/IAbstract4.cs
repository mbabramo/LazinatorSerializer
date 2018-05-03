using Lazinator.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Collections;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)ExampleUniqueIDs.Abstract4)]
    interface IAbstract4 : IConcrete3
    {
        string String4 { get; set; }
    }
}
