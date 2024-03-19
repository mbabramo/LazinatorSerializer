
using System;
using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace FuzzTests.n1
{
    [Lazinator((int)10004)]
    public interface ICCrossTheaterClass : IIndependenceEmptyClass
    {
        RefugeeSmartStruct PersuadeSong { get; set; }
        WindAirlineClass PlaceRecently { get; set; }

    }
}
