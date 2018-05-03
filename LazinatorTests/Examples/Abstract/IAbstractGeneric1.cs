using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)ExampleUniqueIDs.AbstractGeneric1)]
    public interface IAbstractGeneric1<T>
    {
        T MyT { get; set; }
    }
}
