
using System;
using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace FuzzTests.n0
{
    [Lazinator((int)10000)]
    public interface IRefugeeSmartClass
    {
        long? TicketImage { get; set; }
        short? InteractionPassion { get; set; }
        bool SufferYear { get; set; }

    }
}
