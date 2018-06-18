using System.Collections.Generic;
using Lazinator.Attributes;

namespace LazinatorTests.Examples.Hierarchy
{
    [Lazinator((int)ExampleUniqueIDs.ExampleInterfaceContainer)]
    interface IExampleInterfaceContainer
    {
        IExample ExampleByInterface { get; set; }
        List<IExample> ExampleListByInterface { get; set; }
    }
}
