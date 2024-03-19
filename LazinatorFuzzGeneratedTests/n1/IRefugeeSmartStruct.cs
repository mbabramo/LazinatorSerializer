
using System;
using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace FuzzTests.n1
{
    [Lazinator((int)10000)]
    public interface IRefugeeSmartStruct
    {
        long? TicketImage { get; set; }
        short? InteractionPassion { get; set; }

    }
}
