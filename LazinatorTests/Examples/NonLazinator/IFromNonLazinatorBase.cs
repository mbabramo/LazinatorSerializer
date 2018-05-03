using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace LazinatorTests.Examples.NonLazinator
{
    [Lazinator((int)ExampleUniqueIDs.FromNonLazinatorBase)]
    interface IFromNonLazinatorBase
    {
        [DerivationKeyword("override")]
        int MyInt { get; set; }
    }
}
