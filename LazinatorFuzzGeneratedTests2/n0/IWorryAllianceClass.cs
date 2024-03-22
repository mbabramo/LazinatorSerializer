
using System;
using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace FuzzTests.n0
{
    [Lazinator((int)10001)]
    public interface IWorryAllianceClass
    {
        RefugeeSmartClass? MentalBasketball { get; set; }
        short WoodPersonality { get; set; }
        int? FavoriteVisible { get; set; }

    }
}
