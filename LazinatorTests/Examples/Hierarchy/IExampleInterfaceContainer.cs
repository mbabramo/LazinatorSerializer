using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;

namespace LazinatorTests.Examples.Hierarchy
{
    [Lazinator((int)ExampleUniqueIDs.ExampleInterfaceContainer)]
    interface IExampleInterfaceContainer
    {
        IExample ExampleByInterface { get; set; }
    }
}
