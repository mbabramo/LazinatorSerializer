using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ConcreteGeneric2a)]
    interface IConcreteGeneric2a : IAbstractGeneric1<int>
    {
        string AnotherProperty { get; set; }
        Example LazinatorExample { get; set; }
    }
}
