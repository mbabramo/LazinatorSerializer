
using System;
using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace FuzzTests.n0
{
    [Lazinator((int)10002)]
    public interface ITimeShellClass : IRefugeeSmartClass
    {
        char HappyRecall { get; set; }

    }
}
