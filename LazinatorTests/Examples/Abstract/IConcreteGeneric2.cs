using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ConcreteGeneric2)]
    interface IConcreteGeneric2 : IAbstractGeneric1<int>
    {
        string AnotherProperty { get; set; }
    }
}
