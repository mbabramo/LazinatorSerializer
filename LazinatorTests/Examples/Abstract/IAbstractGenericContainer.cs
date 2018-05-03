using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.AbstractGenericContainer)]
    public interface IAbstractGenericContainer<T>
    {
        IAbstractGeneric1<T> Item { get; set; }
    }
}
