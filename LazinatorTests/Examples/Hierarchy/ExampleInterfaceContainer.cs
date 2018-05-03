using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.Hierarchy
{
    class ExampleInterfaceContainer : IExampleInterfaceContainer
    {
        public IExample ExampleByInterface { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
