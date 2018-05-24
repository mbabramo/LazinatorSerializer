using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.Hierarchy
{
    public partial class ExampleInterfaceContainer : IExampleInterfaceContainer
    {
        public List<IExample> ExampleListByInterface { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
