
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
        bool SufferYear { get; set; }
        Guid? StopWorry { get; set; }
        long TypicallyProducer { get; set; }
        sbyte? IndependenceEmpty { get; set; }
        TimeSpan? AirlineGallery { get; set; }
        long ReaderBury { get; set; }

    }
}
