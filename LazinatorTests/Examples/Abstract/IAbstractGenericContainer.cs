using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Collections;
using Lazinator.Core;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)ExampleUniqueIDs.AbstractGenericContainer)]
    public interface IAbstractGenericContainer<T>
    {
        IAbstractGeneric1<T> Item { get; set; }
    }
}
