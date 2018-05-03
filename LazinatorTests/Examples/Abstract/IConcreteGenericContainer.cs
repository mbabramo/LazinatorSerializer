using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ConcreteGenericContainer)]
    interface IConcreteGenericContainer : IAbstractGenericContainer<int>
    {
    }
}
