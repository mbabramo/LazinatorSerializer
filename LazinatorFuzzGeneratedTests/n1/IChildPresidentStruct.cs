
using System;
using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace FuzzTests.n1
{
    [Lazinator((int)10003)]
    public interface IChildPresidentStruct
    {
        RefugeeSmartStruct FlowStranger { get; set; }
        DateTime VisitorWooden { get; set; }
        RefugeeSmartStruct LegendMan { get; set; }
        uint StillOrdinary { get; set; }
        TimeShellClass TypeCareful { get; set; }
        CommanderPersuadeStruct? PrivateMusic { get; set; }

    }
}
