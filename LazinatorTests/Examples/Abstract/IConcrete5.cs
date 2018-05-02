using Lazinator.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Collections;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.Concrete5)]
    interface IConcrete5 : IAbstract4
    {
        string String5 { get; set; }
    }
}
