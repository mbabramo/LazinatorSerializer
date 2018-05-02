using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.AbstractGeneric1)]
    interface IAbstractGeneric1<T>
    {
        T MyT { get; set; }
    }
}
