using Lazinator.Attributes;
using Lazinator.Collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.Generics
{
    [Lazinator((int) ExampleUniqueIDs.LazinatorListContainer)]
    public interface ILazinatorListContainer
    {
        LazinatorList<ExampleChild> MyList { get; set; }
    }
}
