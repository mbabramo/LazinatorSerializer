using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.Structs
{
    public partial class ContainerForExampleStructWithoutClass : IContainerForExampleStructWithoutClass
    {
        public
        ExampleStructWithoutClass ExampleStructWithoutClass
        { get; set; }
    }
}
