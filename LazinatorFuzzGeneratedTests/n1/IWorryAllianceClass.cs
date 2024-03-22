
using System;
using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace FuzzTests.n1
{
    [Lazinator((int)10001)]
    public interface IWorryAllianceClass
    {
        RefugeeSmartClass MentalBasketball { get; set; }
        short WoodPersonality { get; set; }

    }
}
