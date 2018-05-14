using Lazinator.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)ExampleUniqueIDs.ContainerWithAbstract1)]
    interface IContainerWithAbstract1 
    {
        Abstract1 AbstractProperty { get; set; }
    }
}
