using Lazinator.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Collections;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.Concrete3)]
    interface IConcrete3 : IAbstract2
    {
        string String3 { get; set; }
    }
}
