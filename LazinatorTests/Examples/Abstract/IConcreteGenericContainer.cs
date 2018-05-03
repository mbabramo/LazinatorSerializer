using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)ExampleUniqueIDs.ConcreteGenericContainer)]
    interface IConcreteGenericContainer : IAbstractGenericContainer<int>
    {
    }
}
