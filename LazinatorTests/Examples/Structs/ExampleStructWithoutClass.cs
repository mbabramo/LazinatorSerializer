using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.Structs
{
    public partial class ExampleStructWithoutClass : IExampleStructWithoutClass
    {
        public int MyInt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
