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
        AbstractGeneric1<T>.EnumWithinAbstractGeneric MyEnumWithinAbstractGeneric { get; set; }
        [FullyQualify]
        AbstractGeneric1<int>.EnumWithinAbstractGeneric MyEnumWithinAbstractGeneric2 { get; set; }
    }
}
