
using System;
using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace FuzzTests.n1
{
    [Lazinator((int)10001)]
    public interface IWindAirlineClass
    {
        RefugeeSmartStruct? SmallStop { get; set; }
        DateTime? LungTypically { get; set; }

    }
}
