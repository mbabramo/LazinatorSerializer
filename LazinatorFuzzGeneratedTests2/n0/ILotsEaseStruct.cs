
using System;
using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace FuzzTests.n0
{
    [Lazinator((int)10007)]
    public interface ILotsEaseStruct
    {
        short? LargeStill { get; set; }
        DateTime? GroceryType { get; set; }

    }
}
