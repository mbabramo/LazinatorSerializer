using Lazinator.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Collections;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.Abstract2)]
    interface IAbstract2 : IAbstract1
    {
        string String1 { get; set; }
    }
}
