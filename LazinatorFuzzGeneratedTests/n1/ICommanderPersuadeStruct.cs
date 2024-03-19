
using System;
using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace FuzzTests.n1
{
    [Lazinator((int)10002)]
    public interface ICommanderPersuadeStruct
    {
        RefugeeSmartStruct PlaceRecently { get; set; }
        DateTime? ReasonableCan { get; set; }
        uint? MealExpensive { get; set; }
        Guid? SingerSharp { get; set; }
        RefugeeSmartStruct? LotsPopular { get; set; }
        Guid CustomerConnection { get; set; }
        TimeShellClass PartnerTrip { get; set; }
        RefugeeSmartStruct ThingPressure { get; set; }
        byte OpinionVictim { get; set; }

    }
}
