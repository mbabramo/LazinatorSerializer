using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.AnotherNamespace
{
    public partial struct StructInAnotherNamespace : IStructInAnotherNamespace
    {
        public int MyInt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
