
using System;
using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace FuzzTests.n1
{
    [Lazinator((int)10001)]
    public interface ITimeShellClass
    {
        char HappyRecall { get; set; }
        char? TalentSometimes { get; set; }
        RefugeeSmartStruct? PaleCross { get; set; }

    }
}
