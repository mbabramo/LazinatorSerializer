using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Collections;
using Lazinator.Core;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)ExampleUniqueIDs.AbstractGeneric1)]
    public interface IAbstractGeneric1<T> : ILazinator
    {
        T MyT { get; set; }
    }
}
