using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Collections;
using Lazinator.Core;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.AbstractGenericContainer)]
    public interface IAbstractGenericContainer<T> : ILazinator
    {
        IAbstractGeneric1<T> Item { get; set; }
    }
}
