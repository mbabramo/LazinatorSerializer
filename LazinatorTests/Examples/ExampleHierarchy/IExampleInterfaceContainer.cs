using System.Collections.Generic;
using Lazinator.Attributes;

namespace LazinatorTests.Examples.Hierarchy
{
    [Lazinator((int)ExampleUniqueIDs.ExampleInterfaceContainer)]
    interface IExampleInterfaceContainer
    {
        [IncludableChild] // for purpose of IncludeChildrenModeWorks_UnaccessedProperties test
        IExample ExampleByInterface { get; set; }
        List<IExample> ExampleListByInterface { get; set; }
    }
}
